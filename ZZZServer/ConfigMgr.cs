using Newtonsoft.Json.Linq;
using Serilog;

namespace ZZZServer;

public static class ConfigMgr
{
    public static cfg.Tables tb { get; private set; }

    public static void Init()
    {
        Log.Debug("加载配置表");
        tb = new cfg.Tables(JsonLoader);
    }

    private static JArray JsonLoader(string fileName)
    {
        string path = AppConfigMgr.Config.ConfigTableDataPath;
        string json = File.ReadAllText(Path.Join(path, fileName) + ".json");
        return JArray.Parse(json);
    }
}