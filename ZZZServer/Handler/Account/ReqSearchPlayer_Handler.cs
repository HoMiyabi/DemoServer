using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Account;

public class ReqSearchPlayer_Handler : RpcHandler<ReqSearchPlayer, RspSearchPlayer>
{
    protected override void Run(Session session, ReqSearchPlayer req, RspSearchPlayer rsp, Action reply)
    {
        var player = (Player)session.Data;
        if (req.Username == player.Username)
        {
            rsp.Result = new Result { Code = 2, Msg = "不能添加自己" };
            return;
        }

        var target = PlayerService.GetPlayerByUsername(req.Username);
        if (target == null)
        {
            rsp.Result = new Result { Code = 1, Msg = "找不到" };
            return;
        }

        rsp.SocialPlayers = new NSocialPlayer
        {
            Uid = target.Uid,
            Username = target.Username,
            Signature = target.Signature,
            AvatarCid = target.AvatarCid,
            IsOnline = target.IsOnline
        };
        rsp.Result = new Result { Code = 0, Msg = "查询成功" };
    }
}