using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Account;

public class ReqModifyAvatar_Handler : RpcHandler<ReqModifyAvatar, RspModifyAvatar>
{
    protected override void Run(Session session, ReqModifyAvatar req, RspModifyAvatar rsp, Action reply)
    {
        var player = (Player)session.Data;

        if (req.AvatarCid < 1 || req.AvatarCid >= 10)
        {
            rsp.Result = new Result { Code = 1, Msg = "头像ID不合法" };
            return;
        }
        player.AvatarCid = req.AvatarCid;
        rsp.Result = new Result { Code = 0, Msg = "修改成功" };
    }
}