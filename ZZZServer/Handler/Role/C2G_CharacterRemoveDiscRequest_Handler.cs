using Kirara.Network;
using ZZZServer.Model;
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
            rsp.Result = new Result { Code = 1, Msg = "驱动盘位置参数错误" };
            return;
        }

        var role = player.Roles.Find(x => x.Id == req.RoleId);
        if (role == null)
        {
            rsp.Result = new Result { Code = 2, Msg = "角色不存在" };
            return;
        }

        var discId = role.DiscIds[req.DiscPos - 1];
        if (string.IsNullOrEmpty(discId))
        {
            rsp.Result = new Result { Code = 3, Msg = "角色该位置没有驱动盘" };
            return;
        }

        var disc = player.Discs.Find(x => x.Id == discId);
        if (disc == null)
        {
            rsp.Result = new Result { Code = 4, Msg = "驱动盘不存在" };
            return;
        }
        disc.RoleId = "";
        role.DiscIds[req.DiscPos - 1] = "";
    }
}