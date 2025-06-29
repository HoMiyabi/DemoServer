using System.Runtime.CompilerServices;

namespace ZZZServer.Utils;

public struct Vector3
{
    public const float kEpsilon = 0.00001F;

    public float x;

    public float X
    {
        get => x;
        set => x = value;
    }

    public float y;

    public float Y
    {
        get => y;
        set => y = value;
    }

    public float z;

    public float Z
    {
        get => z;
        set => z = value;
    }

    public NFloat3 Net => new()
    {
        X = X,
        Y = Y,
        Z = Z
    };

    public Vector3 Set(NFloat3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public void Set(float newX, float newY, float newZ)
    {
        x = newX;
        y = newY;
        z = newZ;
    }

    public float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            switch (index)
            {
                case 0: return x;
                case 1: return y;
                case 2: return z;
                default:
                    throw new IndexOutOfRangeException("Invalid Vector3 index!");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            switch (index)
            {
                case 0: x = value; break;
                case 1: y = value; break;
                case 2: z = value; break;
                default:
                    throw new IndexOutOfRangeException("Invalid Vector3 index!");
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 a)
    {
        return new Vector3(-a.x, -a.y, -a.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 a, float d)
    {
        return new Vector3(a.x * d, a.y * d, a.z * d);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(float d, Vector3 a)
    {
        return new Vector3(a.x * d, a.y * d, a.z * d);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 a, float d)
    {
        return new Vector3(a.x / d, a.y / d, a.z / d);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3 lhs, Vector3 rhs)
    {
        float diff_x = lhs.x - rhs.x;
        float diff_y = lhs.y - rhs.y;
        float diff_z = lhs.z - rhs.z;
        float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
        return sqrmag < kEpsilon * kEpsilon;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3 lhs, Vector3 rhs)
    {
        return !(lhs == rhs);
    }
}