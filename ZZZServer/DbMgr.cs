using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ZZZServer.Model;
using ZZZServer.Utils;

namespace ZZZServer;

public static class DbMgr
{
    public static MongoClient Client { get; private set; }
    public static IMongoDatabase Database { get; private set; }
    public static IMongoCollection<Player> Players => Database.GetCollection<Player>("player");
    public static IMongoCollection<ChatMsg> ChatMsgs => Database.GetCollection<ChatMsg>("chat_msg");

    public static void Init()
    {
        string connectionString = AppConfigMgr.Config.Database.ConnectionString;
        string databaseName = AppConfigMgr.Config.Database.DatabaseName;

        BsonSerializer.RegisterSerializer(new Vector3dSerializer());

        Client = new MongoClient(connectionString);
        Database = Client.GetDatabase(databaseName);
    }
}