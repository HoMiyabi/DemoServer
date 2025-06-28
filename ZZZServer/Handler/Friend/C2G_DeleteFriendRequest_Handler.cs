using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqDeleteFriend_Handler : RpcHandler<ReqDeleteFriend, RspDeleteFriend>
{
    protected override void Run(Session session, ReqDeleteFriend req, RspDeleteFriend rsp, Action reply)
    {
        var player = (Player)session.Data;

        if (!player.FriendUids.Remove(req.FriendUid))
        {
            rsp.Result = new Result { Code = 1, Msg = "不是你的好友" };
            return;
        }

        var other = PlayerService.GetPlayerByUid(req.FriendUid);
        other.FriendUids.Remove(player.Uid);
    }
}