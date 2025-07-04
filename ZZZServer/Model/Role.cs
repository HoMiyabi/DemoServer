﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ZZZServer.Utils;

namespace ZZZServer.Model;

public class Role
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int Cid { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public string WeaponId { get; set; }
    public List<string> DiscIds { get; set; }
    public Vector3 Pos { get; set; }
    public Vector3 Rot { get; set; }

    public NRole NRole => new()
    {
        Id = Id,
        Cid = Cid,
        Level = Level,
        Exp = Exp,
        WeaponId = WeaponId,
        DiscIds = {DiscIds},
        Pos = Pos.Net,
        Rot = Rot.Net
    };

    public NSimRole NSim => new()
    {
        Id = Id,
        Movement = NMovement,
        Cid = Cid
    };

    public NSyncRole NSyncRole => new()
    {
        Id = Id,
        Movement = NMovement,
    };

    public NMovement NMovement => new()
    {
        Pos = Pos.Net,
        Rot = Rot.Net
    };
}