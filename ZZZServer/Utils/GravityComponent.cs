namespace ZZZServer.Utils;

public class GravityComponent : Component
{
    public GravityComponent(Node node) : base(node)
    {
    }

    public override void Update(float dt)
    {
        node.position.y -= 9.8 * dt;
        if (node.position.y < 0)
        {
            node.position.y = 0;
        }
    }
}