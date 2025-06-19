using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler.Account;

public class ReqModifyAvatar_Handler : RpcHandler<ReqModifyAvatar, RspModifyAvatar>
{
    protected override void Run(Session session, ReqModifyAvatar req, RspModifyAvatar rsp, Action reply)
    {
        var player = (Player)session.Data;

        if (req.AvatarCid < 1 || req.AvatarCid >= 10)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "头像ID不合法";
            return;
        }
        player.AvatarCid = req.AvatarCid;
        rsp.Result.Msg = "修改成功";
    }
}