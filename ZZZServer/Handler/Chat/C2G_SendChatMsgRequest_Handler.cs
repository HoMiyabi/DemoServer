using ZZZServer.DbEntity;
using Kirara.Network;
using Microsoft.IdentityModel.Tokens;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler.Chat;

public class ReqSendChatMsg_Handler : RpcHandler<ReqSendChatMsg, RspSendChatMsg>
{
    protected override void Run(Session session, ReqSendChatMsg req, RspSendChatMsg rsp, Action reply)
    {
        var player = (Player)session.Data;

        var msg = req.ChatMsgItem;
        if (!msg.Text.IsNullOrEmpty())
        {
            // 为文本消息
            msg.StickerCid = 0;
        }
        else if (msg.StickerCid >= 1)
        {
            // 为贴纸消息
            msg.Text = null;
        }
        else
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "错误的发送内容";
            return;
        }

        var receiverUid = req.ReceiverUid;
        bool isFriend = player.FriendUids.Contains(receiverUid);
        if (!isFriend)
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "对方不是你的好友";
            return;
        }

        var senderUid = player.Uid;

        long unixTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if (PlayerService.UidToPlayer.TryGetValue(receiverUid, out var receiver))
        {
            receiver.Session.Send(new NotifyReceiveChatMsg
            {
                ChatMsgRecord = new NChatMsgRecord
                {
                    SenderUid = senderUid,
                    UnixTimeMs = unixTimeMs,
                    ChatMsgItem = msg,
                }
            });
        }

        int smallUId = senderUid;
        int bigUId = receiverUId;
        if (senderUid > receiverUId)
        {
            (smallUId, bigUId) = (bigUId, smallUId);
        }

        DbHelper.Db.CopyNew().Insertable(new ChatMsgRecord
        {
            SmallUId = smallUId,
            BigUId = bigUId,
            SenderUId = senderUid,
            EpochMs = unixTimeMs,
            Text = msg.Text,
            StickerConfigId = msg.StickerConfigId
        }).ExecuteCommand();

        rsp.UnixTimeMs = unixTimeMs;
    }
}