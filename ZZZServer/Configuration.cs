using Tomlyn;
using Tomlyn.Model;

namespace ZZZServer;

public static class Configuration
{
    public static TomlTable Config { get; private set; }

    public static void Init()
    {
        string tomlContent = File.ReadAllText("Application.toml");
        Config = Toml.ToModel(tomlContent);
    }
}