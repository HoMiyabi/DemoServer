using Kirara.Network;
using ZZZServer.MongoDocEntity;

namespace ZZZServer.Handler.Friend;

public class ReqRefuseAddFriend_Handler : RpcHandler<ReqRefuseAddFriend, RspRefuseAddFriend>
{
    protected override void Run(Session session, ReqRefuseAddFriend req, RspRefuseAddFriend rsp, Action reply)
    {
        var player = (Player)session.Data;

        // 好友请求发送者
        string senderUid = req.SenderUid;

        if (!player.FriendUids.Remove(senderUid))
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "好友请求不存在";
            return;
        }

        rsp.Result.Msg = "好友请求拒绝成功";
    }
}