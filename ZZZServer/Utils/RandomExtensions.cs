namespace ZZZServer.Utils;

public static class RandomExtensions
{
    public static T RandomItem<T>(this List<T> array)
    {
        if (array.Count == 0)
        {
            return default;
        }
        return array[Random.Shared.Next(0, array.Count)];
    }
}