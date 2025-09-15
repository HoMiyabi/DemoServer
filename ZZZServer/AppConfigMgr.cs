using Tomlyn;

namespace ZZZServer;

public static class AppConfigMgr
{
    public static AppConfig Config { get; private set; }

    public static void Init(string path)
    {
        string text = File.ReadAllText(path);
        Config = Toml.ToModel<AppConfig>(text, path, new TomlModelOptions()
        {
            ConvertPropertyName = x => x,
            ConvertFieldName = x => x
        });
    }
}

public class AppConfig
{
    public string ActionDataPath { get; set; }
    public string NavigationDataPath { get; set; }
    public string ConfigTableDataPath { get; set; }
    public DatabaseConfig Database { get; set; }
}

public class DatabaseConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}