using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.MongoDocEntity;

public class DiscItem
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int Cid { get; set; }

    public int Level { get; set; }
    public int Exp { get; set; }
    public string RoleId { get; set; }
    public bool Locked { get; set; }
    public int Pos { get; set; }

    // 属性
    public DiscAttr MainAttr { get; set; }
    public List<DiscAttr> SubAttrs { get; set; }

    public NDiscItem Net()
    {
        return new NDiscItem
        {
            Id = Id,
            Cid = ConfigId,
            Level = Level,
            Exp = Exp,
            WearerId = WearerId,
            Locked = Locked,
            Pos = Pos,
            MainAttr = MainAttr.Net(),
            SubAttrs = SubAttrs.Select(it => it.Net()).ToList()
        };
    }
}