using MongoDB.Driver;
using Tomlyn.Model;
using ZZZServer.Model;

namespace ZZZServer;

public static class DbHelper
{
    public static MongoClient Client { get; private set; }
    public static IMongoDatabase Database { get; private set; }
    public static IMongoCollection<Player> Players => Database.GetCollection<Player>("player");
    public static IMongoCollection<ChatMsg> ChatMsgs => Database.GetCollection<ChatMsg>("chat_msg");

    public static void Init()
    {
        string connectionString = ((TomlTable)Configuration.Config["Database"])["ConnectionString"] as string;
        string databaseName = ((TomlTable)Configuration.Config["Database"])["DatabaseName"] as string;

        Client = new MongoClient(connectionString);
        Database = Client.GetDatabase(databaseName);
    }
}