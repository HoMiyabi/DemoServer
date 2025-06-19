using Kirara.Network;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ZZZServer.Service;

namespace ZZZServer.MongoDocEntity;

public class Player
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Uid { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int AvatarCid { get; set; }
    public string Signature { get; set; }
    // 好友
    public List<string> FriendUids { get; set; }
    public List<string> FriendRequestUids { get; set; }
    public List<MaterialItem> Materials { get; set; }
    public List<CurrencyItem> Currencies { get; set; }
    public List<WeaponItem> Weapons { get; set; }
    public List<DiscItem> Discs { get; set; }
    public List<Role> Roles { get; set;} // 角色
    public List<string> TeamRoleIds { get; set; } // 队伍内角色
    public string FrontRoleId { get; set; } // 前台角色

    [BsonIgnore]
    public Session Session { get; set; }
    [BsonIgnore]
    public Room Room { get; set; }

    public NPlayer Net()
    {
        return new NPlayer
        {
            Uid = Uid,
            Username = Username,
            AvatarCid = AvatarCid,
            Signature = Signature,
            FriendUids = {FriendUids},
            FriendRequestUids = {FriendRequestUids},
            Materials = {Materials.Select(it => it.Net())},
            Currencies = {Currencies.Select(it => it.Net())},
            Weapons = {Weapons.Select(it => it.Net())},
            Discs = {Discs.Select(it => it.Net())},
            Roles = {Roles.Select(it => it.Net())},
            TeamRoleIds = {TeamRoleIds},
            FrontRoleId = FrontRoleId,
        };
    }
}