using ZZZServer.DbEntity;
using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqAcceptAddFriend_Handler : RpcHandler<ReqAcceptAddFriend, RspAcceptAddFriend>
{
    protected override void Run(Session session, ReqAcceptAddFriend req, RspAcceptAddFriend rsp, Action reply)
    {
        var player = (Player)session.Data;

        // 好友请求发送者
        var senderUid = req.SenderUid;

        if (!player.FriendUids.Remove(senderUid))
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "好友请求不存在";
            return;
        }

        // 双向添加
        player.FriendUids.Add(senderUid);
        if (PlayerService.UidToPlayer.TryGetValue(senderUid, out var data))
        {
            data.dbPlayer.FriendUIds.Add(player.UId);
        }
        else
        {
            var otherPlayer = DbHelper.Db.CopyNew().Queryable<DbPlayer>().InSingle(senderUid);
            otherPlayer.FriendUIds ??= [];
            otherPlayer.FriendUIds.Add(player.UId);
            DbHelper.Db.CopyNew().Updateable(otherPlayer).ExecuteCommand();
        }

        rsp.Result.Code = 0;
        rsp.Result.Msg = "好友请求同意成功";
    }
}