using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler.Account;

public class ReqGetPlayerData_Handler : RpcHandler<ReqGetPlayerData, RspGetPlayerData>
{
    protected override void Run(Session session, ReqGetPlayerData req, RspGetPlayerData rsp, Action reply)
    {
        var player = (Player)session.Data;
        rsp.PlayerData = player.Net;
    }
}