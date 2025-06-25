using Kirara.Network;
using Microsoft.IdentityModel.Tokens;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Chat;

public class ReqSendChatMsg_Handler : RpcHandler<ReqSendChatMsg, RspSendChatMsg>
{
    protected override void Run(Session session, ReqSendChatMsg req, RspSendChatMsg rsp, Action reply)
    {
        var player = (Player)session.Data;

        var msg = req.ChatMsg;
        if (msg.MsgType == 0)
        {
            // 为文本消息
            if (string.IsNullOrEmpty(msg.Text))
            {
                rsp.Result.Code = 1;
                rsp.Result.Msg = "错误的发送内容";
                return;
            }
            msg.StickerCid = 0;
        }
        else if (msg.MsgType == 1)
        {
            // 为贴纸消息
            msg.Text = null;
        }
        else
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "错误的消息类型";
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
            if (receiver.IsOnline)
            {
                receiver.Session.Send(new NotifyReceiveChatMsg
                {
                    ChatMsgRecord = new NChatMsgRecord
                    {
                        SenderUid = senderUid,
                        UnixTimeMs = unixTimeMs,
                        ChatMsg = msg,
                    }
                });
            }
        }

        DbHelper.ChatMsgRecords.InsertOne(new ChatMsgRecord
        {
            SenderUid = senderUid,
            ReceiverUid = receiverUid,
            UnixTimeMs = unixTimeMs,
            MsgType = msg.MsgType,
            Text = msg.Text,
            StickerCid = msg.StickerCid
        });

        rsp.UnixTimeMs = unixTimeMs;
    }
}