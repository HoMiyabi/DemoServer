using MongoDB.Driver;
using ZZZServer.Model;

namespace ZZZServer.Service;

public static class ChatService
{
    public static List<ChatMsg> GetChatMsgs(string uid1, string uid2)
    {
        var f1 = Builders<ChatMsg>.Filter.Eq(x => x.SenderUid, uid1) &
                 Builders<ChatMsg>.Filter.Eq(x => x.ReceiverUid, uid2);

        var f2 = Builders<ChatMsg>.Filter.Eq(x => x.SenderUid, uid2) &
                 Builders<ChatMsg>.Filter.Eq(x => x.ReceiverUid, uid1);

        var f = f1 | f2;

        var msgs = DbHelper.ChatMsgs
            .Find(f)
            .SortBy(x => x.UnixTimeMs)
            .ToList();
        return msgs;
    }
}