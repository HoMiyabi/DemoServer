namespace ZZZServer.Utils;

public class Float3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public NFloat3 Net()
    {
        return new NFloat3
        {
            X = X,
            Y = Y,
            Z = Z
        };
    }
}