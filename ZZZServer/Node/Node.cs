using Mathd;

namespace ZZZServer;

public class Node
{
    public Vector3d position = Vector3d.zero;
    public Quaterniond rotation = Quaterniond.identity;

    public Vector3d TransformPoint(Vector3d point)
    {
        return rotation * point + position;
    }
}