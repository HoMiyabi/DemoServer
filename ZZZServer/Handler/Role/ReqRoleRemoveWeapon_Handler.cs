using Kirara.Network;
using ZZZServer.MongoDocEntity;
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
            rsp.Result.Code = 1;
            rsp.Result.Msg = "角色不存在";
            return;
        }

        if (role.WeaponId == null)
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "该角色没有装备武器";
            return;
        }

        var weapon = player.Weapons.Find(x => x.Id == role.WeaponId);
        if (weapon == null)
        {
            rsp.Result.Code = 3;
            rsp.Result.Msg = "武器不存在";
            return;
        }
        weapon.RoleId = null;
        role.WeaponId = null;
    }
}