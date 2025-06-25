using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Friend;

public class ReqGetFriendInfos_Handler : RpcHandler<ReqGetFriendInfos, RspGetFriendInfos>
{
    protected override void Run(Session session, ReqGetFriendInfos req, RspGetFriendInfos rsp, Action reply)
    {
        var player = (Player)session.Data;
        rsp.OtherPlayerInfos.Add(
            player.FriendUids
                .Select(PlayerService.GetPlayerByUid)
                .Select(it => it.NetOther()
                )
        );
    }
}