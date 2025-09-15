using Newtonsoft.Json;
using Serilog;

namespace ZZZServer.Anim;

public static class ActionMgr
{
    public static Dictionary<string, Action> Actions { get; private set; }

    public static void Init()
    {
        string dataPath = AppConfigMgr.Config.ActionDataPath;
        Log.Debug("ActionMgr 初始化路径: {0}", dataPath);
        Actions = new Dictionary<string, Action>();

        string[] paths = Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories);
        foreach (string path in paths)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            if (Actions.ContainsKey(name))
            {
                Log.Warning("AnimRootMotion名字重复, name: {0}, path: {1}", name, path);
                continue;
            }
            string text = File.ReadAllText(path);
            var action = JsonConvert.DeserializeObject<Action>(text);
            Actions.Add(name, action);
        }
    }
}