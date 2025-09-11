using Mathd;
using MongoDB.Bson;
using ZZZServer.Model;

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
            Pos = new Vector3d(),
            Rot = new Vector3d()
        };
    }
}