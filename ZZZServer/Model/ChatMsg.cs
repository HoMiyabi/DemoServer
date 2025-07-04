﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.Model;

public class ChatMsg
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SenderUid { get; set; }
    public string ReceiverUid { get; set; }
    public long UnixTimeMs { get; set; }
    public int MsgType { get; set; }
    public string Text { get; set; }
    public int StickerCid { get; set; }

    public NChatMsg Net => new()
    {
        SenderUid = SenderUid,
        ReceiverUid = ReceiverUid,
        UnixTimeMs = UnixTimeMs,
        MsgType = MsgType,
        Text = Text,
        StickerCid = StickerCid
    };
}