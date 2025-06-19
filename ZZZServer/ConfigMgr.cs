using Newtonsoft.Json.Linq;

namespace ZZZServer;

public static class ConfigMgr
{
    public static cfg.Tables tb { get; private set; }

    public static void Init()
    {
        tb = new cfg.Tables(LoadJson);
    }

    private static JArray LoadJson(string fileName)
    {
        string json = File.ReadAllText(Path.Join("ConfigTableData", fileName) + ".json");
        return JArray.Parse(json);
    }
}