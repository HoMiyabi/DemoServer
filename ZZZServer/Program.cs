using System.Net;
using Kirara.Network;
using Serilog;
using ZZZServer.Anim;
using ZZZServer.Navigation;
using ZZZServer.Service;

namespace ZZZServer;

internal static class Program
{
    private static void Main()
    {
        // 程序配置
        AppConfigMgr.Init("Data/App.toml");

        // 日志
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();

        // 配置表
        ConfigMgr.Init();

        // 动画
        ActionMgr.Init();

        // 导航
        NavigationMgr.Instance.Init();

        // 数据库
        DbMgr.Init();

        // 注册消息
        KiraraNetwork.Init(new MsgMeta().Init(), typeof(Program).Assembly);

        var server = new Server();
        server.AddMsgProcessorUpdate("RoomService", RoomService.Update);
        server.OnBeforeClose += PlayerService.SaveAllPlayers;
        server.Run(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23434));
    }
}