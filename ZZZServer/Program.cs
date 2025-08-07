using System.Net;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Kirara.Network;
using MongoDB.Driver;
using Serilog;
using ZZZServer.Animation;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer;

internal static class Program
{
    private static void Main()
    {
        // 配置
        Configuration.Init("Application.toml");

        // 日志
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();

        // 配置表
        ConfigMgr.Init();

        // 动画
        AnimMgr.Init("ConfigAnimData");

        // 数据库
        DbHelper.Init();

        KiraraNetwork.Init(new MsgMeta(), typeof(Program).Assembly);

        var server = new Server();
        server.AddMsgProcessorUpdate("RoomService", RoomService.Update);
        server.Run(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23434));
    }
}