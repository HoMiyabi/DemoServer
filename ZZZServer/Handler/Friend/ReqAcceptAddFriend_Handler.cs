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
            rsp.Result = new Result { Code = 1, Msg = "好友请求不存在" };
            return;
        }
        player.Session.Send(new NotifyFriendRequestsRemove
        {
            Uid = senderUid
        });

        // 双向添加
        var sender = PlayerService.GetPlayerByUid(senderUid);

        player.FriendUids.Add(senderUid);
        player.Session.Send(new NotifyFriendsAdd
        {
            Player = sender.NSocial
        });

        sender.FriendUids.Add(player.Uid);
        sender.Session?.Send(new NotifyFriendsAdd
        {
            Player = player.NSocial
        });
    }
}