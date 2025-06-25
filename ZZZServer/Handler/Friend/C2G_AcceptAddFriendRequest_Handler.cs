using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqAcceptAddFriend_Handler : RpcHandler<ReqAcceptAddFriend, RspAcceptAddFriend>
{
    protected override void Run(Session session, ReqAcceptAddFriend req, RspAcceptAddFriend rsp, Action reply)
    {
        var player = (Player)session.Data;

        // 好友请求发送者
        var senderUid = req.SenderUid;

        if (!player.FriendRequestUids.Remove(senderUid))
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "好友请求不存在";
            return;
        }

        // 双向添加
        player.FriendUids.Add(senderUid);
        var sender = PlayerService.GetPlayerByUid(senderUid);
        sender.FriendUids.Add(player.Uid);
    }
}