using System.Collections.Concurrent;
using MongoDB.Driver;
using ZZZServer.Model;

namespace ZZZServer.Service;

public static class PlayerService
{
    public static readonly ConcurrentDictionary<string, Player> UidToPlayer = new();

    public static Player CreatePlayer(string username, string password)
    {
        return new Player
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
            Roles = [
                RoleService.CreateRole(1),
                RoleService.CreateRole(2),
                RoleService.CreateRole(3)
            ]
        };
    }

    public static void SavePlayer(Player player)
    {
        var db = DbHelper.Database;
        var players = db.GetCollection<Player>("player");
        players.ReplaceOne(
            Builders<Player>.Filter.Eq(x => x.Uid, player.Uid),
            player, new ReplaceOptions() { IsUpsert = true});
    }

    public static Player GetPlayerByUsername(string username)
    {
        var players = DbHelper.Players;
        var player = players.Find(Builders<Player>.Filter.Eq(x => x.Username, username)).FirstOrDefault();

        if (player == null) return null;

        return UidToPlayer.GetOrAdd(player.Uid, player);
    }

    public static Player GetPlayerByUid(string uid)
    {
        if (UidToPlayer.TryGetValue(uid, out var player)) return player;

        var players = DbHelper.Players;
        player = players.Find(Builders<Player>.Filter.Eq(x => x.Uid, uid)).FirstOrDefault();

        if (player == null) return null;

        return UidToPlayer.GetOrAdd(uid, player);
    }
}