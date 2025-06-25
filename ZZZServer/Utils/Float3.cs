namespace ZZZServer.Utils;

public class Float3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public NFloat3 Net => new()
    {
        X = X,
        Y = Y,
        Z = Z
    };

    public Float3 Set(NFloat3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
        return this;
    }
}