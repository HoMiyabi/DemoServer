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

    public static NQuaternion Net(this System.Numerics.Quaternion q)
    {
        return new NQuaternion
        {
            X = q.X,
            Y = q.Y,
            Z = q.Z,
            W = q.W
        };
    }

    public static Vector3 Mul(this System.Numerics.Quaternion rotation, Vector3 point)
    {
        float x = rotation.X * 2F;
        float y = rotation.Y * 2F;
        float z = rotation.Z * 2F;
        float xx = rotation.X * x;
        float yy = rotation.Y * y;
        float zz = rotation.Z * z;
        float xy = rotation.X * y;
        float xz = rotation.X * z;
        float yz = rotation.Y * z;
        float wx = rotation.W * x;
        float wy = rotation.W * y;
        float wz = rotation.W * z;

        Vector3 res;
        res.x = (1F - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
        res.y = (xy + wz) * point.x + (1F - (xx + zz)) * point.y + (yz - wx) * point.z;
        res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1F - (xx + yy)) * point.z;
        return res;
    }
}