using ZZZServer.DbEntity;
using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

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

        var rawItems = DbHelper.Db.CopyNew()
            .Queryable<ChatMsgRecord>()
            .Where(x => x.SmallUId == smallUId && x.BigUId == bigUId)
            .OrderBy(x => x.EpochMs)
            .ToList();

        var items = rawItems
            .Select(it => new NChatMsgRecord
        {
            SenderUId = it.SenderUId,
            EpochMs = it.EpochMs,
            ChatMsgItem = new NChatMsgItem
            {
                Text = it.Text,
                StickerConfigId = it.StickerConfigId
            }
        });

        rsp.Result.ChatMsgRecordItems.Add(items);
    }
}