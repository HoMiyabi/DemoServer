using ZZZServer.Animation;
using ZZZServer.Service;
using ZZZServer.Utils;
using Quaternion = System.Numerics.Quaternion;
using Vector3 = ZZZServer.Utils.Vector3;

namespace ZZZServer.SVEntity;

public class Monster
{
    public Room room;
    public int monsterId;
    public float hp;
    public Vector3 pos;
    public Quaternion rot;

    public Monster(Room room, int monsterId, float hp)
    {
        this.room = room;
        this.monsterId = monsterId;
        this.hp = hp;
    }

    public NSyncMonster NSyncMonster => new()
    {
        MonsterId = monsterId,
        Pos = pos.Net(),
        Rot = rot.Net(),
    };

    public enum State
    {
        Idle,
        Chase, // 追击
        Attack,
        Hit, // 受击
        Stun, // 硬直
    }

    public State _state;

    public float maxIdleColdTime = 2f;
    public float idleColdTime;

    private AnimRootMotion Idle = AnimMgr.AnimRootMotions["Idle"];
    private AnimRootMotion Run = AnimMgr.AnimRootMotions["Run"];
    private AnimRootMotion Die = AnimMgr.AnimRootMotions["Die"];

    private List<(Vector3 hitFrom, AnimRootMotion anim)> HitAnims =
    [
        (new Vector3(0, 0, 1), AnimMgr.AnimRootMotions["Damage_Front"]),
        (new Vector3(0, 0, -1), AnimMgr.AnimRootMotions["Damage_Back"]),
        (new Vector3(1, 0, 0), AnimMgr.AnimRootMotions["Damage_Right"]),
        (new Vector3(-1, 0, 0), AnimMgr.AnimRootMotions["Damage_Left"])
    ];

    private AnimRootMotion[] AttackAnims =
    [
        AnimMgr.AnimRootMotions["Skill_A"],
        AnimMgr.AnimRootMotions["Skill_B"],
        AnimMgr.AnimRootMotions["Skill_C"],
    ];

    public float attackDistance;
    public float chaseDistance;

    public void Update(float dt)
    {
        UpdateAnimPlayer(dt);
        UpdateState(dt);
    }

    private void UpdateState(float dt)
    {
        switch (_state)
        {
            case State.Idle:
            {
                idleColdTime += dt;
                if (idleColdTime < maxIdleColdTime) return;

                var role = room.ClosestFrontRole(pos, out float dis);
                if (dis < attackDistance)
                {
                    EnterState(State.Attack);
                }
                else if (dis < chaseDistance)
                {
                    EnterState(State.Chase);
                }
                break;
            }
            case State.Chase:
            {
                var role = room.ClosestFrontRole(pos, out float dis);
                if (role == null)
                {
                    EnterState(State.Idle);
                }
                else
                {
                    LookAt(role.Pos);
                    if (dis < attackDistance)
                    {
                        EnterState(State.Attack);
                    }
                    else if (dis > chaseDistance)
                    {
                        EnterState(State.Idle);
                    }
                }
                break;
            }
        }
    }

    public void EnterState(State state, object arg = null)
    {
        _state = state;
        switch (state)
        {
            case State.Idle:
            {
                idleColdTime = 0f;
                Play(Idle);
                break;
            }
            case State.Chase:
            {
                Play(Run);
                break;
            }
            case State.Attack:
            {
                var role = room.ClosestFrontRole(pos, out float dis);
                if (role != null)
                {
                    LookAt(role.Pos);
                    int i = Random.Shared.Next(0, AttackAnims.Length);
                    Play(AttackAnims[i], () =>
                    {
                        EnterState(State.Idle);
                    });
                }
                break;
            }
            case State.Hit:
            {
                var hitFrom = (Vector3)arg;
                var hitAction = ChooseHitAction(hitFrom);
                Play(hitAction, () =>
                {
                    EnterState(State.Idle);
                });
                break;
            }
            case State.Stun:
            {
                // curAniState.Speed = -2f;
                // curAniState.Events.Clear();
                // curAniState.Events.Add(0f, () =>
                // {
                //     EnterState(State.Idle);
                // });
                break;
            }
        }
    }

    private AnimRootMotion ChooseHitAction(Vector3 hitFrom)
    {
        hitFrom = hitFrom.normalized;
        AnimRootMotion result = null;
        float min = float.MaxValue;
        foreach (var item in HitAnims)
        {
            float dot = Vector3.Dot(item.hitFrom, hitFrom);
            if (dot < min)
            {
                min = dot;
                result = item.anim;
            }
        }
        return result;
    }

    private void LookAt(Vector3 target)
    {
        var dir = target - pos;
        dir.y = 0;
        dir = dir.normalized;
        float theta = (float)Math.Atan2(dir.x, dir.z);
        var q = Quaternion.CreateFromAxisAngle(new System.Numerics.Vector3(0, 1, 0), theta);
        rot = q;
    }

    #region AnimPlayer

    private AnimRootMotion _currMotion;
    private float _currTime;
    private Action _onFinish;
    private void Play(AnimRootMotion motion, Action onFinish = null)
    {
        var notify = new NotifyMonsterPlayAction
        {
            MonsterId = monsterId,
            ActionName = motion.name
        };
        room.SendAllPlayers(notify);

        _currMotion = motion;
        _currTime = 0f;
        _onFinish = onFinish;
    }

    private void UpdateAnimPlayer(float dt)
    {
        if (_currMotion == null) return;

        var pos1 = _currMotion.EvalT(_currTime);
        var rot1 = _currMotion.EvalQ(_currTime);
        _currTime += dt;
        var pos2 = _currMotion.EvalT(_currTime);
        var rot2 = _currMotion.EvalQ(_currTime);

        var deltaPosition = pos2 - pos1;
        deltaPosition = rot.Mul(deltaPosition);
        var deltaRotation = rot2 * Quaternion.Inverse(rot1);

        OnAnimPlayerMove(deltaPosition, deltaRotation);
        if (_currTime >= _currMotion.length)
        {
            _currMotion = null;
            _currTime = 0;
            _onFinish?.Invoke();
        }
    }

    private void OnAnimPlayerMove(Vector3 deltaPosition, Quaternion deltaRotation)
    {
        pos += deltaPosition;
        rot *= deltaRotation;
    }

    #endregion
}