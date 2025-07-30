using System.Numerics;

namespace ZZZServer.Utils
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static readonly Vector3 zero = new(0, 0, 0);

        public static readonly Vector3 one = new(1, 1, 1);

        public static readonly Vector3 forward = new(0, 0, 1);

        public static readonly Vector3 back = new(0, 0, -1);

        public static readonly Vector3 up = new(0, 1, 0);

        public static readonly Vector3 down = new(0, -1, 0);

        public static readonly Vector3 left = new(-1, 0, 0);

        public static readonly Vector3 right = new(1, 0, 0);

        public static readonly Vector3 positiveInfinity =
            new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        public static readonly Vector3 negativeInfinity =
            new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        public float magnitude => (float)Math.Sqrt(x * x + y * y + z * z);

        public Vector3 normalized => this / magnitude;

        // 距离
        public static float Distance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float dz = a.z - b.z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        // 点乘
        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        #region 运算符重载

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.x, -a.y, -a.z);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(float a, Vector3 b)
        {
            return new Vector3(a * b.x, a * b.y, a * b.z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 operator /(Vector3 a, float b)
        {
            return new Vector3(a.x / b, a.y / b, a.z / b);
        }

        #endregion
    }
}