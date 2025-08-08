using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqSendAddFriend_Handler : RpcHandler<ReqSendAddFriend, RspSendAddFriend>
{
    protected override void Run(Session session, ReqSendAddFriend req, RspSendAddFriend rsp, Action reply)
    {
        var player = (Player)session.Data;
        string targetUid = req.TargetUid;

        // 不能添加自己为好友
        if (targetUid == player.Uid)
        {
            rsp.Result = new Result { Code = 1, Msg = "不能添加自己为好友" };
            return;
        }

        // 已经是好友，不能重复添加
        if (player.FriendUids.Contains(targetUid))
        {
            rsp.Result = new Result { Code = 2, Msg = "已经是好友" };
            return;
        }

        // 如果对方已发送好友请求给自己，只能接受，不能给对方发
        if (player.FriendRequestUids.Contains(targetUid))
        {
            rsp.Result = new Result { Code = 3, Msg = "对方已发送好友请求给自己，不能再给对方发" };
            return;
        }

        var target = PlayerService.GetPlayerByUid(targetUid);

        // 对方不存在
        if (target == null)
        {
            rsp.Result = new Result { Code = 4, Msg = "对方不存在" };
            return;
        }

        if (!target.FriendRequestUids.Contains(player.Uid))
        {
            target.FriendRequestUids.Add(player.Uid);
            target.Session?.Send(new NotifyFriendRequestsAdd
            {
                Player = player.NSocial
            });
        }
    }
}