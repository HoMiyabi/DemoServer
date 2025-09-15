namespace ZZZServer.Utils;

public class Singleton<T> where T : Singleton<T>, new()
{
    private static volatile T _instance;

    private static readonly object _lockObject = new();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}