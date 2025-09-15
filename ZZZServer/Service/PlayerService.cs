using System.Collections.Concurrent;
using MongoDB.Driver;
using Serilog;
using ZZZServer.Model;

namespace ZZZServer.Service;

public static class PlayerService
{
    private static ConcurrentDictionary<string, Player> UidToPlayer { get; } = new();

    public static Player CreatePlayer(string username, string password)
    {
        var p = new Player
        {
            Username = username,
            Password = password,
            AvatarCid = 1,
            Signature = "",
            FriendUids = [],
            FriendRequestUids = [],
            Materials = [],
            Currencies = [],
            Weapons = [],
            Discs = [],
            Roles =
            [
                RoleService.CreateRole(1),
                RoleService.CreateRole(2),
                RoleService.CreateRole(3)
            ],
            TeamRoleIds = [],
        };
        p.TeamRoleIds.Add(p.Roles[0].Id);
        p.TeamRoleIds.Add(p.Roles[1].Id);
        p.TeamRoleIds.Add(p.Roles[2].Id);
        p.FrontRoleId = p.Roles[0].Id;
        return p;
    }

    public static void SaveAllPlayers()
    {
        foreach (var player in UidToPlayer.Values)
        {
            SavePlayer(player);
        }
    }

    public static void SavePlayer(Player player)
    {
        Log.Debug("保存玩家 Uid: {PlayerUid}", player.Uid);
        var db = DbMgr.Database;
        var players = db.GetCollection<Player>("player");
        players.ReplaceOne(
            Builders<Player>.Filter.Eq(x => x.Uid, player.Uid),
            player, new ReplaceOptions() {IsUpsert = true}); // Update Insert
    }

    public static Player GetPlayerByUsername(string username)
    {
        var players = DbMgr.Players;
        var player = players.Find(Builders<Player>.Filter.Eq(x => x.Username, username)).FirstOrDefault();

        if (player == null) return null;

        return UidToPlayer.GetOrAdd(player.Uid, key =>
        {
            Log.Debug("加载玩家 Uid: {PlayerUid}", player.Uid);
            return player;
        });
    }

    public static Player GetPlayerByUid(string uid)
    {
        return UidToPlayer.GetOrAdd(uid, key =>
        {
            var players = DbMgr.Players;
            var player = players.Find(Builders<Player>.Filter.Eq(x => x.Uid, key)).FirstOrDefault();
            if (player != null)
            {
                Log.Debug("加载玩家 Uid: {PlayerUid}", player.Uid);
            }
            return player;
        });
    }
}