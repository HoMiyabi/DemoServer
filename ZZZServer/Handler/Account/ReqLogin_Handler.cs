using Kirara.Network;
using Serilog;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class ReqLogin_Handler : RpcHandler<ReqLogin, RspLogin>
{
    protected override void Run(Session session, ReqLogin req, RspLogin rsp, Action reply)
    {
        var player = PlayerService.GetPlayerByUsername(req.Username);

        if (player == null || player.Password != req.Password)
        {
            rsp.Result = new Result {Code = 1, Msg = "用户名或密码错误"};
            return;
        }

        if (player.IsOnline)
        {
            rsp.Result = new Result { Code = 2, Msg = "用户已登录" };
            return;
        }

        session.Data = player;
        player.Session = session;
        player.IsOnline = true;

        session.OnDisconnected += () =>
        {
            NetMsgProcessor.Instance.EnqueueTask(() =>
            {
                session.Data = null;
                player.Session = null;
                player.IsOnline = false;

                PlayerService.SavePlayer(player);
            });
        };
    }
}