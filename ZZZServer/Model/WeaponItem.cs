using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.Model;

public class WeaponItem
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int Cid { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public string RoleId { get; set; }
    public bool Locked { get; set; }
    public int RefineLevel { get; set; }

    // 属性
    public WeaponAttr BaseAttr { get; set; }
    public WeaponAttr AdvancedAttr { get; set; }

    public NWeaponItem Net()
    {
        return new NWeaponItem
        {
            Id = Id,
            Cid = Cid,
            Level = Level,
            Exp = Exp,
            RoleId = RoleId,
            Locked = Locked,
            RefineLevel = RefineLevel,
            BaseAttr = BaseAttr.Net(),
            AdvancedAttr = AdvancedAttr.Net()
        };
    }
}