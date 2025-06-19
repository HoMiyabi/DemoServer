using Google.Protobuf;
using Kirara.Network;
using Serilog;
using ZZZServer.MongoDocEntity;
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

    public void AddPlayer(Player player)
    {
        var session = player.Session;

        // 更新自己的位置
        var ntfSelf = new NotifyUpdateSelf()
        {
            PosRot = new NPosRot()
            {
                Pos = new NFloat3().CopyPosFrom(player),
                Rot = new NFloat3().CopyRotFrom(player)
            }
        };
        session.Send(ntfSelf);

        // 更新别人的位置
        var ntfOthersEnter = new NotifyOthersEnterRoom();
        ntfOthersEnter.PlayerInfos.Add(players.Select(it => it.dbPlayer.NRoomSimPlayerInfo()));
        session.Send(ntfOthersEnter);

        // 将自己的位置发给别人
        ntfOthersEnter = new NotifyOthersEnterRoom();
        ntfOthersEnter.PlayerInfos.Add(player.NRoomSimPlayerInfo());

        SendAllPlayers(ntfOthersEnter);

        players.Add(player);
        player.Room = this;

        Log.Debug($"房间{id}数量：{players.Count}");

        session.OnDisconnected += () =>
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

        var msg = new NotifyOtherLeaveRoom()
        {
            Uid = player.Uid
        };

        SendAllPlayers(msg, player);
    }

    public void PlayerChPlayAction(Player player, string actionName)
    {
        var msg = new NotifyOtherPlayAction()
        {
            Uid = player.Uid,
            ActionName = actionName
        };

        SendAllPlayers(msg, player);
    }

    public void SyncEntityFromAutonomous(Player player, NPosRot posRot)
    {
        player.CopyPosFrom(posRot.Pos);
        player.CopyRotFrom(posRot.Rot);

        var msg = new NotifyUpdateOther()
        {
            Uid = player.Uid,
            PosRot = posRot
        };

        SendAllPlayers(msg, player);
    }

    public void SpawnMonster(int monsterCid, NPosRot posRot)
    {
        var config = ConfigMgr.tb.TbMonsterConfig[monsterCid];
        var monster = new Monster(this, NextMonsterId, config.Hp);
        monsters.Add(monster);
        var msg = new NotifySpawnMonster
        {
            MonsterCid = monsterCid,
            MonsterId = monster.monsterId,
            PosRot = posRot
        };
        SendAllPlayers(msg);
    }

    public void MonsterTakeDamage(Player player, int monsterId, float damage)
    {
        var monster = monsters.Find(it => it.monsterId == monsterId);
        if (monster == null)
        {
            Log.Debug("找不到monster");
            return;
        }

        monster.hp -= damage;
        var msg = new NotifySyncMonsterTakeDamage
        {
            MonsterId = monsterId,
            Damage = damage,
        };
        SendAllPlayers(msg, player);

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

    public void PlayerSwitchRole(Player player, string frontRoleId, bool next)
    {
        player.FrontRoleId = frontRoleId;

        var msg = new NotifySwitchRole
        {
            Uid = player.Uid,
            FrontRoleId = frontRoleId,
            Next = next,
        };
        SendAllPlayers(msg, player);
    }

    public void SendAllPlayers(IMsg msg)
    {
        foreach (var player in players)
        {
            player.Session.Send(msg);
        }
    }

    public void SendAllPlayers(IMsg msg, Player except)
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