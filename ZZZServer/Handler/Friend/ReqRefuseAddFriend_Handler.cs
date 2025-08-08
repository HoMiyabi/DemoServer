using Kirara.Network;
using ZZZServer.Model;

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
            rsp.Result = new Result { Code = 1, Msg = "好友请求不存在" };
            return;
        }

        rsp.Result = new Result { Code = 0, Msg = "好友请求拒绝成功" };
    }
}