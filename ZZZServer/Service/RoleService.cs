using MongoDB.Bson;
using ZZZServer.Model;
using ZZZServer.Utils;

namespace ZZZServer.Service;

public static class RoleService
{
    public static Role CreateRole(int cid)
    {
        return new Role
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Cid = cid,
            Level = 60,
            Exp = 0,
            WeaponId = "",
            DiscIds =
            [
                "",
                "",
                "",
                "",
                "",
                ""
            ],
            Pos = new Float3(),
            Rot = new Float3()
        };
    }
}