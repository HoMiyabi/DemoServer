using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqRemoveFriend_Handler : RpcHandler<ReqRemoveFriend, RspRemoveFriend>
{
    protected override void Run(Session session, ReqRemoveFriend req, RspRemoveFriend rsp, Action reply)
    {
        var player = (Player)session.Data;

        if (!player.FriendUids.Remove(req.FriendUid))
        {
            rsp.Result = new Result { Code = 1, Msg = "不是你的好友" };
            return;
        }
        session.Send(new NotifyFriendsRemove
        {
            Uid = req.FriendUid
        });

        var other = PlayerService.GetPlayerByUid(req.FriendUid);
        other.FriendUids.Remove(player.Uid);
        other.Session?.Send(new NotifyFriendsRemove
        {
            Uid = player.Uid
        });
    }
}