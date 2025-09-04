namespace ZZZServer.Utils;

public class Component
{
    public readonly Node node;

    public Component(Node node)
    {
        this.node = node;
    }

    public virtual void Update(float dt)
    {
    }
}