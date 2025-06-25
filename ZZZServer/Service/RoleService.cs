using ZZZServer.Model;

namespace ZZZServer.Service;

public static class RoleService
{
    public static Role CreateRole(int cid)
    {
        return new Role
        {
            Cid = cid,
            Level = 60,
            Exp = 0,
            WeaponId = null,
            DiscIds = [null, null, null, null, null, null]
        };
    }
}