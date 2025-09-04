using Mathd;

namespace ZZZServer.Utils;

public class ActionPlayer : Component
{
    public delegate void OnActionPlayerMoveDel(Vector3d deltaPosition, Quaterniond deltaRotation);

    public ActionPlayer(Node node) : base(node)
    {
    }

    public event OnActionPlayerMoveDel OnActionPlayerMove;

    private Anim.Action _action;

    public Anim.Action Action
    {
        get => _action;
    }
    public float Time { get; private set; }

    private Action _onFinish;

    public void Play(Anim.Action motion, Action onFinish = null)
    {
        _action = motion;
        Time = 0f;
        _onFinish = onFinish;
    }

    public override void Update(float dt)
    {
        if (_action == null) return;

        var motion = _action.rootMotion;
        var pos1 = motion.EvalT(Time);
        var rot1 = motion.EvalQ(Time);
        Time += dt;
        var pos2 = motion.EvalT(Time);
        var rot2 = motion.EvalQ(Time);

        var deltaPosition = (pos2 - pos1).ToDouble();
        deltaPosition = node.rotation * deltaPosition;
        var deltaRotation = rot2 * Quaterniond.Inverse(rot1);

        OnActionPlayerMove?.Invoke(deltaPosition, deltaRotation);
        if (Time >= motion.length)
        {
            if (_action.isLoop)
            {
                Time = 0;
            }
            else
            {
                _action = null;
                Time = 0;
                _onFinish?.Invoke();
            }
        }
    }
}