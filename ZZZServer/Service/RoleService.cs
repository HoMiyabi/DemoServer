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
            Pos = new Vector3d(16.183698654174805, -1.6450014114379883, -21.747297286987305),
            Rot = new Vector3d()
        };
    }
}