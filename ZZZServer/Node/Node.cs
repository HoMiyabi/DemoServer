using Mathd;

namespace ZZZServer;

public class Node
{
    public Vector3d position;
    public Quaterniond rotation;

    public Vector3d TransformPoint(Vector3d point)
    {
        return rotation * point + position;
    }
}