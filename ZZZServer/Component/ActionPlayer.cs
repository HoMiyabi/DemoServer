using Mathd;
using ZZZServer.Utils;

namespace ZZZServer;

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
    private int _notifyStatesFront = 0;

    public void Play(Anim.Action motion, Action onFinish = null)
    {
        _action = motion;
        Time = 0f;
        _onFinish = onFinish;
        _notifyStatesFront = 0;
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

        var deltaPosition = pos2 - pos1;
        deltaPosition = node.rotation * deltaPosition;
        var deltaRotation = rot2 * Quaterniond.Inverse(rot1);

        ProcessNotifies();

        OnActionPlayerMove?.Invoke(deltaPosition, deltaRotation);
        if (Time >= motion.length)
        {
            if (_action.isLoop)
            {
                Time = 0;
                _notifyStatesFront = 0;
            }
            else
            {
                _action = null;
                Time = 0;
                _onFinish?.Invoke();
                _onFinish = null;
                _notifyStatesFront = 0;
            }
        }
    }

    private void ProcessNotifies()
    {
        while (_notifyStatesFront < _action.boxes.Count && _action.boxes[_notifyStatesFront].start <= Time)
        {
            var state = _action.boxes[_notifyStatesFront];
            state.NotifyBegin(this);
            _notifyStatesFront++;
        }
    }
}