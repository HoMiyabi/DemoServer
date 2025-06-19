using MongoDB.Driver;

namespace ZZZServer;

public static class DbHelper
{
    private static string MongoConnectionString = "mongodb://localhost:27017";
    private static string DatabaseName = "game";

    public static MongoClient Client { get; private set; }
    public static IMongoDatabase Database { get; private set; }

    public static void Init()
    {
        Client = new MongoClient(MongoConnectionString);
        Database = Client.GetDatabase(DatabaseName);
    }
}