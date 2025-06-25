using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.Model;

public class ChatMsgRecord
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SenderUid { get; set; }
    public string ReceiverUid { get; set; }
    public long UnixTimeMs { get; set; }
    public int MsgType { get; set; }
    public string Text { get; set; }
    public int StickerCid { get; set; }

    public NChatMsgRecord Net()
    {
        return new NChatMsgRecord
        {
            SenderUid = SenderUid,
            UnixTimeMs = UnixTimeMs,
            ChatMsg = new NChatMsg
            {
                MsgType = MsgType,
                Text = Text,
                StickerCid = StickerCid
            }
        };
    }
}