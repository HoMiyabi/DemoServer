using System.Collections.Concurrent;
using MongoDB.Driver;
using ZZZServer.MongoDocEntity;

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

    public static Player GetPlayer(string uid, out bool isOnline)
    {
        isOnline = UidToPlayer.TryGetValue(uid, out var player);
        if (isOnline)
        {
            return player;
        }
        var db = DbHelper.Database;
        var players = db.GetCollection<Player>("player");
        return players.Find(Builders<Player>.Filter.Eq(x => x.Uid, uid)).FirstOrDefault();
    }
}