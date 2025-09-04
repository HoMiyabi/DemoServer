using ZZZServer.Utils;

namespace ZZZServer.Anim;

public class ActionNotifyState
{
    public virtual void NotifyBegin(ActionPlayer player) {}
    public virtual void NotifyTick(ActionPlayer player, float time) {}
    public virtual void NotifyEnd(ActionPlayer player) {}
}