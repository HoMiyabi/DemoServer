using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqGetFriendInfos_Handler : RpcHandler<ReqGetFriendInfos, RspGetFriendInfos>
{
    protected override void Run(Session session, ReqGetFriendInfos req, RspGetFriendInfos rsp, Action reply)
    {
        var player = (Player)session.Data;
        rsp.OtherPlayerInfos.Add(player.FriendUIds.Select(friendUId =>
        {
            DbPlayer friend;
            bool isOnline;
            if (PlayerService.UidToPlayer.TryGetValue(friendUId, out var data))
            {
                friend = data.dbPlayer;
                isOnline = true;
            }
            else
            {
                friend = DbHelper.Db.CopyNew().Queryable<DbPlayer>().InSingle(friendUId);
                isOnline = false;
            }
            return new NOtherPlayerInfo
            {
                UId = friend.UId,
                Username = friend.Username,
                Signature = friend.Signature,
                AvatarConfigId = friend.AvatarConfigId,
                IsOnline = isOnline,
            };
        }));
    }
}