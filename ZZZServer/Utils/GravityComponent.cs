namespace ZZZServer.Utils;

public class GravityComponent : Component
{
    public double PlaneY { get; set; }

    public GravityComponent(Node node) : base(node)
    {
    }

    public override void Update(float dt)
    {
        node.position.y -= 9.8 * dt;
        if (node.position.y < PlaneY)
        {
            node.position.y = PlaneY;
        }
    }
}