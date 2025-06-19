using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class ReqRoleRemoveDisc_Handler : RpcHandler<ReqRoleRemoveDisc, RspRoleRemoveDisc>
{
    protected override void Run(Session session, ReqRoleRemoveDisc req, RspRoleRemoveDisc rsp,
        Action reply)
    {
        var player = (Player)session.Data;

        if (req.DiscPos < 1 || req.DiscPos > 6)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "驱动盘位置参数错误";
            return;
        }

        var role = player.Roles.Find(x => x.Id == req.RoleId);
        if (role == null)
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "角色不存在";
            return;
        }

        var discId = role.DiscIds[req.DiscPos - 1];
        if (discId == null)
        {
            rsp.Result.Code = 3;
            rsp.Result.Msg = "角色该位置没有驱动盘";
            return;
        }

        var disc = player.Discs.Find(x => x.Id == discId);
        if (disc == null)
        {
            rsp.Result.Code = 4;
            rsp.Result.Msg = "驱动盘不存在";
            return;
        }
        disc.RoleId = null;
        role.DiscIds[req.DiscPos - 1] = null;
    }
}