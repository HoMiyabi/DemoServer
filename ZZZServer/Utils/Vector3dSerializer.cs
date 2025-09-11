using Mathd;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ZZZServer.Utils;

public class Vector3dSerializer : StructSerializerBase<Vector3d>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Vector3d value)
    {
        var writer = context.Writer;
        writer.WriteStartDocument();

        writer.WriteName("x");
        writer.WriteDouble(value.x);

        writer.WriteName("y");
        writer.WriteDouble(value.y);

        writer.WriteName("z");
        writer.WriteDouble(value.z);

        writer.WriteEndDocument();
    }

    public override Vector3d Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var reader = context.Reader;

        double x = 0;
        double y = 0;
        double z = 0;

        reader.ReadStartDocument();
        while (reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            string name = reader.ReadName(Utf8NameDecoder.Instance);
            switch (name)
            {
                case "x":
                    x = reader.ReadDouble();
                    break;
                case "y":
                    y = reader.ReadDouble();
                    break;
                case "z":
                    z = reader.ReadDouble();
                    break;
            }
        }
        reader.ReadEndDocument();
        return new Vector3d(x, y, z);
    }
}