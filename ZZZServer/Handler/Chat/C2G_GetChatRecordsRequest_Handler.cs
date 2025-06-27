/*using Kirara.Network;
using MongoDB.Driver;
using ZZZServer.Model;

namespace ZZZServer.Handler.Chat;

public class ReqGetChatRecords_Handler : RpcHandler<ReqGetChatRecords, RspGetChatRecords>
{
    protected override void Run(Session session, ReqGetChatRecords req, RspGetChatRecords rsp, Action reply)
    {
        var player = (Player)session.Data;

        var friendUid = req.FriendUid;
        bool isFriend = player.FriendUids.Contains(friendUid);
        if (!isFriend)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "不是好友";
            return;
        }

        var f1 = Builders<ChatMsgRecord>.Filter.Eq(x => x.SenderUid, player.Uid) &
                     Builders<ChatMsgRecord>.Filter.Eq(x => x.ReceiverUid, friendUid);

        var f2 = Builders<ChatMsgRecord>.Filter.Eq(x => x.SenderUid, friendUid) &
                     Builders<ChatMsgRecord>.Filter.Eq(x => x.ReceiverUid, player.Uid);

        var filter = f1 | f2;

        var records = DbHelper.ChatMsgRecords
            .Find(filter)
            .SortBy(x => x.UnixTimeMs)
            .ToList();

        rsp.ChatMsgRecords.AddRange(records.Select(it => it.Net()));
    }
}*/