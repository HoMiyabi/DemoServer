using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZZZServer.Model;

public class CurrencyItem
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int Cid { get; set; }
    public int Count { get; set; }

    public NCurrencyItem Net()
    {
        return new NCurrencyItem
        {
            Cid = Cid,
            Count = Count
        };
    }
}