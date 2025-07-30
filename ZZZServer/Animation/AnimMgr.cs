using Newtonsoft.Json;
using Serilog;

namespace ZZZServer.Animation;

public static class AnimMgr
{
    public static Dictionary<string, AnimRootMotion> AnimRootMotions { get; private set; }

    public static void Init(string dataPath)
    {
        Log.Debug("AnimMgr 初始化路径: {0}", dataPath);
        AnimRootMotions = new Dictionary<string, AnimRootMotion>();

        string[] paths = Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories);
        foreach (string path in paths)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            if (AnimRootMotions.ContainsKey(name))
            {
                Log.Warning("AnimRootMotion名字重复, name: {0}, path: {1}", name, path);
                continue;
            }
            string text = File.ReadAllText(path);
            var motion = JsonConvert.DeserializeObject<AnimRootMotion>(text);
            motion.name = name;
            AnimRootMotions.Add(name, motion);
        }
    }
}