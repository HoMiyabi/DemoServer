using Google.Protobuf;
using Kirara.Network;
using Mathd;
using Serilog;
using ZZZServer.Model;
using ZZZServer.SVEntity;
using ZZZServer.Utils;

namespace ZZZServer.Service;

public class Room
{
    public int id;
    public readonly List<Player> players = [];
    public readonly List<Monster> monsters = [];

    private int _monsterId = 0;
    private int NextMonsterId => Interlocked.Increment(ref _monsterId);

    public Room(int id)
    {
        this.id = id;
    }

    public void Update(float dt)
    {
        foreach (var monster in monsters)
        {
            monster.Update(dt);
        }

        var notifyUpdateFromAuthority = new NotifyUpdateFromAuthority()
        {
            Players = {players.Select(it => it.NSync)}
        };
        Broadcast(notifyUpdateFromAuthority);

        var notifySyncMonster = new NotifyUpdateMonster()
        {
            Monsters = {monsters.Select(x => x.NSyncMonster)}
        };
        Broadcast(notifySyncMonster);
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
        player.Room = this;

        Log.Debug($"房间{id}数量：{players.Count}");

        player.Session.OnDisconnected += () =>
        {
            NetMsgProcessor.Instance.EnqueueTask(() => RemovePlayer(player));
        };
    }

    public void RemovePlayer(Player player)
    {
        if (!players.Remove(player))
        {
            Log.Warning("离开但没有player {0}", player.Uid);
            return;
        }
        Log.Debug($"房间{id}数量：{players.Count}");
        player.Room = null;

        var msg = new NotifyRemoveSimulatedPlayers
        {
            Uids = {player.Uid}
        };

        Broadcast(msg);
    }

    public void SpawnMonster(int monsterCid, NMovement movement)
    {
        Log.Debug("SpawnMonster, monsterCid: {0}", monsterCid);
        var monster = new Monster(monsterCid, this, NextMonsterId);
        monsters.Add(monster);
    }

    public void Broadcast(IMessage msg)
    {
        foreach (var player in players)
        {
            player.Session.Send(msg);
        }
    }

    public void BroadcastExcept(IMessage msg, Player except)
    {
        foreach (var player in players)
        {
            if (player.Session != except.Session)
            {
                player.Session.Send(msg);
            }
        }
    }

    public Role ClosestFrontRole(Vector3 pos, out float distance)
    {
        Role role = null;
        float min = float.MaxValue;
        foreach (var player in players)
        {
            var frontRole = player.Roles.Find(x => x.Id == player.FrontRoleId);
            if (frontRole == null) continue;
            float dist = Vector3.Distance(pos, frontRole.Pos);
            if (dist < min)
            {
                min = dist;
                role = frontRole;
            }
        }
        distance = min;
        return role;
    }
}