using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.Model;

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

    public NDiscItem Net => new()
    {
        Id = Id,
        Cid = Cid,
        Level = Level,
        Exp = Exp,
        RoleId = RoleId,
        Locked = Locked,
        Pos = Pos,
        MainAttr = MainAttr.Net,
        SubAttrs = {SubAttrs.Select(it => it.Net)}
    };
}