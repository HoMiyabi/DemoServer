using Tomlyn;
using Tomlyn.Model;

namespace ZZZServer;

public static class Configuration
{
    public static TomlTable Config { get; private set; }

    public static void Init(string path)
    {
        string tomlContent = File.ReadAllText(path);
        Config = Toml.ToModel(tomlContent);
    }
}