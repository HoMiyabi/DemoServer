using Google.Protobuf;
using Serilog;
using ZZZServer.Model;
using ZZZServer.SVEntity;

namespace ZZZServer.Service;

public class Room
{
    public int id;
    private readonly List<Player> players = [];
    private readonly List<Monster> monsters = [];

    private int _monsterId = 0;
    public int NextMonsterId => Interlocked.Increment(ref _monsterId);

    public Room(int id)
    {
        this.id = id;
    }

    public void Tick()
    {

    }

    public void AddPlayer(Player player)
    {
        // 新加入的玩家添加其他玩家
        var notifyAdd = new NotifyAddSimulatedPlayers
        {
            Players = {players.Select(it => it.NSim)}
        };
        player.Session.Send(notifyAdd);

        // 其他玩家添加新加入的玩家
        notifyAdd = new NotifyAddSimulatedPlayers
        {
            Players = {player.NSim}
        };

        SendAllPlayersExcept(notifyAdd, player);

        players.Add(player);
        player.Room = this;

        Log.Debug($"房间{id}数量：{players.Count}");

        player.Session.OnDisconnected += () =>
        {
            RemovePlayer(player);
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

        var msg = new NotifyRemoveSimulatedPlayers
        {
            Uids = {player.Uid}
        };

        SendAllPlayersExcept(msg, player);
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
            role.Pos.Set(syncRole.Movement.Pos);
            role.Rot.Set(syncRole.Movement.Rot);
        }

        var msg = new NotifyUpdateFromAuthority
        {
            Player = syncPlayer
        };

        SendAllPlayersExcept(msg, player);
    }

    public void SpawnMonster(int monsterCid, NMovement movement)
    {
        var config = ConfigMgr.tb.TbMonsterConfig[monsterCid];
        var monster = new Monster(this, NextMonsterId, config.Hp);
        monsters.Add(monster);
        var msg = new NotifySpawnMonster
        {
            MonsterCid = monsterCid,
            MonsterId = monster.monsterId,
            Movement = movement
        };
        SendAllPlayers(msg);
    }

    public void MonsterTakeDamage(Player player, int monsterId, float damage)
    {
        var monster = monsters.Find(it => it.monsterId == monsterId);
        if (monster == null)
        {
            Log.Debug("找不到Monster, MonsterId: {0}", monsterId);
            return;
        }

        monster.hp -= damage;
        var msg = new NotifyMonsterTakeDamage
        {
            MonsterId = monsterId,
            Damage = damage,
        };
        SendAllPlayersExcept(msg, player);

        if (monster.hp <= 0f)
        {
            monsters.Remove(monster);
            var dieMsg = new NotifyMonsterDie
            {
                MonsterId = monsterId
            };
            SendAllPlayers(dieMsg);
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
}