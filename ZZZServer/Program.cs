using System.Net;
using Kirara.Network;
using MongoDB.Driver;
using Serilog;
using ZZZServer.MongoDocEntity;

namespace ZZZServer;

internal static class Program
{
    private static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();

        string connectionString = "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase("game");
        var collection = db.GetCollection<Player>("player");
        // var player = new Player()
        // {
        //     Username = "kirara",
        //     Password = "123456"
        // };
        // collection.InsertOne(player);
        var player = collection.Find(Builders<Player>.Filter.Eq(x => x.Username, "kirara")).First();
        Log.Debug($"{player.Uid} {player.Username} {player.Password}");
        player.Signature = "hello";
        player.Weapons = [];
        collection.ReplaceOne(Builders<Player>.Filter.Eq(x => x.Uid, player.Uid), player);


        ConfigMgr.Init();
        DbHelper.Init();

        KiraraNetwork.Init(new MsgMeta(), typeof(Program).Assembly);

        var server = new Server();
        server.Run(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23434));
    }
}