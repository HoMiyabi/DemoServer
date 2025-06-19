using Kirara.Network;
using ZZZServer.MongoDocEntity;
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
            rsp.Result.Code = 1;
            rsp.Result.Msg = "不能添加自己为好友";
            return;
        }

        // 已经是好友，不能重复添加
        if (player.FriendUids.Contains(targetUid))
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "已经是好友";
            return;
        }

        // 如果对方已发送好友请求给自己，只能接受，不能给对方发
        if (player.FriendRequestUids.Contains(targetUid))
        {
            rsp.Result.Code = 3;
            rsp.Result.Msg = "对方已发送好友请求给自己，不能再给对方发";
            return;
        }

        var target = PlayerService.GetPlayer(targetUid, out bool isOnline);

        // 对方不存在
        if (target == null)
        {
            rsp.Result.Code = 4;
            rsp.Result.Msg = "对方不存在";
            return;
        }

        if (!target.FriendUids.Contains(player.Uid))
        {
            target.FriendUids.Add(player.Uid);
        }

        //todo)) 保存数据库
        if (fromDb)
        {
            PlayerService.Save(target);
        }

        rsp.Result.Code = 0;
        rsp.Result.Msg = "发送好友请求成功";
    }
}