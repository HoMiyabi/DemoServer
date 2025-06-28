using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Role;

public class ReqRoleRemoveWeapon_Handler : RpcHandler<ReqRoleRemoveWeapon, RspRoleRemoveWeapon>
{
    protected override void Run(Session session, ReqRoleRemoveWeapon req, RspRoleRemoveWeapon rsp,
        Action reply)
    {
        var player = (Player)session.Data;

        var role = player.Roles.Find(x => x.Id == req.RoleId);
        if (role == null)
        {
            rsp.Result = new Result { Code = 1, Msg = "角色不存在" };
            return;
        }

        if (string.IsNullOrEmpty(role.WeaponId))
        {
            rsp.Result = new Result { Code = 2, Msg = "该角色没有装备武器" };
            return;
        }

        var weapon = player.Weapons.Find(x => x.Id == role.WeaponId);
        if (weapon == null)
        {
            rsp.Result = new Result { Code = 3, Msg = "武器不存在" };
            return;
        }
        weapon.RoleId = "";
        role.WeaponId = "";
    }
}