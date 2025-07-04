﻿using Kirara.Network;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ZZZServer.Service;

namespace ZZZServer.Model;

public class Player
{
    [BsonIgnore]
    public bool IsOnline { get; set; }

    [BsonIgnore]
    public Session Session { get; set; }

    [BsonIgnore]
    public Room Room { get; set; }

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

    public NPlayer Net
    {
        get
        {
            var player = new NPlayer
            {
                Uid = Uid,
                Username = Username,
                AvatarCid = AvatarCid,
                Signature = Signature,
                Materials = {Materials.Select(it => it.Net())},
                Currencies = {Currencies.Select(it => it.Net())},
                Weapons = {Weapons.Select(it => it.Net())},
                Discs = {Discs.Select(it => it.Net)},
                Roles = {Roles.Select(it => it.NRole)},
                TeamRoleIds = {TeamRoleIds},
                FrontRoleId = FrontRoleId,
            };
            foreach (var friendUid in FriendUids)
            {
                var p = PlayerService.GetPlayerByUid(friendUid);
                var socialPlayer = p.NSocial;
                socialPlayer.ChatMsgs.AddRange(ChatService.GetChatMsgs(Uid, p.Uid).Select(x => x.Net));
                player.Friends.Add(socialPlayer);
            }
            foreach (var friendRequestUid in FriendRequestUids)
            {
                var p = PlayerService.GetPlayerByUid(friendRequestUid);
                var socialPlayer = p.NSocial;
                player.FriendRequests.Add(socialPlayer);
            }
            return player;
        }
    }

    public NSocialPlayer NSocial => new()
    {
        Uid = Uid,
        Username = Username,
        Signature = Signature,
        AvatarCid = AvatarCid,
        IsOnline = IsOnline
    };

    public NSimPlayer NSim => new()
    {
        Uid = Uid,
        Roles = {Roles.Select(x => x.NSim)}
    };

    public NSyncPlayer NSync => new()
    {
        Uid = Uid,
        Roles = {Roles.Select(x => x.NSyncRole)},
    };
}