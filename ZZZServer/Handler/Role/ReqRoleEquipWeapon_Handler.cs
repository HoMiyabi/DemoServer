using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler.Role;

public class ReqRoleEquipWeapon_Handler : RpcHandler<ReqRoleEquipWeapon, RspRoleEquipWeapon>
{
    protected override void Run(Session session, ReqRoleEquipWeapon req, RspRoleEquipWeapon rsp, Action reply)
    {
        var player = (Player)session.Data;

        var role = player.Roles.Find(x => x.Id == req.RoleId);
        if (role == null)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "角色不存在";
            return;
        }

        if (role.WeaponId != null)
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "该角色已经装备武器";
            return;
        }

        var weapon = player.Weapons.Find(x => x.Id == req.NewWeaponId);
        if (weapon == null)
        {
            rsp.Result.Code = 3;
            rsp.Result.Msg = "武器不存在";
            return;
        }

        if (weapon.RoleId != null)
        {
            rsp.Result.Code = 4;
            rsp.Result.Msg = "武器已被装备";
            return;
        }

        weapon.RoleId = role.Id;
        role.WeaponId = weapon.Id;
    }
}