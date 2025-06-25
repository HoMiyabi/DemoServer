using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.Model;

public class MaterialItem
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int Cid { get; set; }
    public int Count { get; set; }

    public NMaterialItem Net()
    {
        return new NMaterialItem
        {
            Cid = Cid,
            Count = Count
        };
    }
}