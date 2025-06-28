using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler;

public class ReqRoleEquipDisc_Handler : RpcHandler<ReqRoleEquipDisc, RspRoleEquipDisc>
{
    protected override void Run(Session session, ReqRoleEquipDisc req, RspRoleEquipDisc rsp,
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

        if (!string.IsNullOrEmpty(role.DiscIds[req.DiscPos - 1]))
        {
            rsp.Result = new Result { Code = 3, Msg = "角色该位置已装备驱动盘" };
            return;
        }

        var disc = player.Discs.Find(x => x.Id == req.NewDiscId);
        if (disc == null)
        {
            rsp.Result = new Result { Code = 4, Msg = "驱动盘不存在" };
            return;
        }
        if (!string.IsNullOrEmpty(disc.RoleId))
        {
            rsp.Result = new Result { Code = 5, Msg = "驱动盘已被装备" };
            return;
        }

        disc.RoleId = role.Id;
        role.DiscIds[req.DiscPos - 1] = disc.Id;
    }
}