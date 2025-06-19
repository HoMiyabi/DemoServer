using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqDeleteFriend_Handler : RpcHandler<ReqDeleteFriend, RspDeleteFriend>
{
    protected override void Run(Session session, ReqDeleteFriend req, RspDeleteFriend rsp, Action reply)
    {
        var player = (Player)session.Data;

        if (!player.FriendUids.Remove(req.FriendUid))
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "不是你的好友";
            return;
        }
        if (PlayerService.UidToPlayer.TryGetValue(friendUId, out var data))
        {
            data.dbPlayer.FriendUIds.Remove(player.UId);
        }
        else
        {
            var otherPlayer = DbHelper.Db.CopyNew().Queryable<DbPlayer>().InSingle(friendUId);
            otherPlayer.FriendUIds.Remove(player.UId);
            DbHelper.Db.CopyNew().Updateable(otherPlayer).ExecuteCommand();
        }

        rsp.Result.Code = 0;
        rsp.Result.Msg = "删除好友成功";
    }
}