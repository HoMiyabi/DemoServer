using Mathd;

namespace ZZZServer.Utils;

public static class Extensions
{
    public static NVector3 Net(this Vector3 v)
    {
        return new NVector3
        {
            X = v.x,
            Y = v.y,
            Z = v.z
        };
    }

    public static Vector3 Native(this NVector3 v)
    {
        return new Vector3(v.X, v.Y, v.Z);
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

    public static Vector3d ToDouble(this NVector3 v)
    {
        return new Vector3d(v.X, v.Y, v.Z);
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

    public static Vector3d ToDouble(this Vector3 v)
    {
        return new Vector3d(v.x, v.y, v.z);
    }

    public static Vector3 ToSingle(this Vector3d v)
    {
        return new Vector3((float)v.x, (float)v.y, (float)v.z);
    }
}