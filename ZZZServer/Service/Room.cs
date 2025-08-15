using Google.Protobuf;
using Kirara.Network;
using Serilog;
using ZZZServer.Model;
using ZZZServer.SVEntity;
using ZZZServer.Utils;

namespace ZZZServer.Service;

public class Room
{
    public int id;
    public readonly List<Player> players = [];
    private readonly List<Monster> monsters = [];

    private int _monsterId = 0;
    public int NextMonsterId => Interlocked.Increment(ref _monsterId);

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
        SendAllPlayers(notifyUpdateFromAuthority);

        var notifySyncMonster = new NotifyUpdateMonster()
        {
            Monsters = {monsters.Select(x => x.NSyncMonster)}
        };
        SendAllPlayers(notifySyncMonster);
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

        SendAllPlayers(msg);
    }

    public void PlayerRolePlayAction(Player player, string roleId, string actionName)
    {
        var msg = new NotifyOtherRolePlayAction()
        {
            Uid = player.Uid,
            RoleId = roleId,
            ActionName = actionName
        };

        SendAllPlayersExcept(msg, player);
    }

    public void UpdateFromAutonomous(Player player, NSyncPlayer syncPlayer)
    {
        foreach (var syncRole in syncPlayer.Roles)
        {
            var role = player.Roles.Find(x => x.Id == syncRole.Id);
            if (role == null)
            {
                Log.Warning("Role不存在 syncRole.Id: {0}", syncRole.Id);
                return;
            }
            role.Pos = syncRole.Movement.Pos.Native();
            role.Rot = syncRole.Movement.Rot.Native();
        }
    }

    public void SpawnMonster(int monsterCid, NMovement movement)
    {
        Log.Debug("SpawnMonster, monsterCid: {0}", monsterCid);
        var monster = new Monster(monsterCid, this, NextMonsterId);
        monsters.Add(monster);
    }

    public void MonsterTakeDamage(Player player, MsgMonsterTakeDamage msg)
    {
        var monster = monsters.Find(it => it.monsterId == msg.MonsterId);
        if (monster == null)
        {
            Log.Debug("找不到Monster, MonsterId: {0}", msg.MonsterId);
            return;
        }

        monster.hp = Math.Max(0f, monster.hp - msg.Damage);
        var notify = new NotifyMonsterTakeDamage
        {
            MonsterId = msg.MonsterId,
            Damage = msg.Damage,
            IsCrit = msg.IsCrit,
            CurrHp = monster.hp
        };
        SendAllPlayers(notify);

        // 聚怪效果
        if (msg.HitGatherDist != 0f)
        {
            var worldCenter = msg.CenterPos.ToDouble();

            // 移动向量的水平投影，最长不能超过v
            var v = (worldCenter - monster.pos);
            v.y = 0f;

            double dist = Math.Min(msg.HitGatherDist, v.magnitude); // 不能越过中心
            var dir = v.normalized; // 方向
            monster.pos += dir * dist;
        }

        if (monster.hp <= 0f)
        {
            monsters.Remove(monster);
            var notifyMonsterDie = new NotifyMonsterDie
            {
                MonsterId = msg.MonsterId
            };
            SendAllPlayers(notifyMonsterDie);
        }
    }

    public void PlayerSwitchRole(Player player, string frontRoleId)
    {
        player.FrontRoleId = frontRoleId;
    }

    public void SendAllPlayers(IMessage msg)
    {
        foreach (var player in players)
        {
            player.Session.Send(msg);
        }
    }

    public void SendAllPlayersExcept(IMessage msg, Player except)
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