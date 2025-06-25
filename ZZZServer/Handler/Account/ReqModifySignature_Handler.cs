using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Account;

public class ReqModifySignature_Handler : RpcHandler<ReqModifySignature, RspModifySignature>
{
    protected override void Run(Session session, ReqModifySignature req, RspModifySignature rsp, Action reply)
    {
        var player = (Player)session.Data;
        // todo)) 签名格式校验
        player.Signature = req.Signature;
        rsp.Result.Msg = "修改成功";
    }
}