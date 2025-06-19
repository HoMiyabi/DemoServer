using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.MongoDocEntity;

public class DiscAttr
{
    public int AttrEntryId { get; set; }
    public int AttrTypeId { get; set; }
    public float Value { get; set; }
    public int UpgradeTimes { get; set; }
}