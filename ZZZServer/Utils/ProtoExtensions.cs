using Mathd;

namespace ZZZServer.Utils;

public static class ProtoExtensions
{
    public static Vector3d Native(this NVector3 v)
    {
        return new Vector3d(v.X, v.Y, v.Z);
    }

    public static NVector3 Net(this Vector3d v)
    {
        return new NVector3
        {
            X = (float)v.x,
            Y = (float)v.y,
            Z = (float)v.z
        };
    }

    public static Quaterniond Native(this NQuaternion q)
    {
        return new Quaterniond(q.X, q.Y, q.Z, q.W);
    }

    public static NQuaternion Net(this Quaterniond q)
    {
        return new NQuaternion
        {
            X = (float)q.x,
            Y = (float)q.y,
            Z = (float)q.z,
            W = (float)q.w
        };
    }
}