namespace ZZZServer.Utils;

public class Component
{
    public Node node;

    public Component(Node node)
    {
        this.node = node;
    }

    public virtual void Update(float dt)
    {
    }
}