using cfg.main;
using Mathd;
using Serilog;
using ZZZServer.Animation;
using ZZZServer.Service;
using ZZZServer.Utils;
using Vector3 = ZZZServer.Utils.Vector3;

namespace ZZZServer.SVEntity;

public class Monster
{
    private readonly MonsterConfig config;
    public Room room;
    public readonly int monsterId;
    public double hp;
    public Vector3d pos;
    public Quaterniond rot;

    public Monster(int cid, Room room, int monsterId)
    {
        config = ConfigMgr.tb.TbMonsterConfig[cid];
        this.room = room;
        this.monsterId = monsterId;
        hp = config.Hp;
        Log.Debug("Monster, id: {0}, hp: {1}", monsterId, hp);
        Play(Idle, () => EnterState(State.Idle));
    }

    public NSyncMonster NSyncMonster => new()
    {
        MonsterCid = config.Id,
        MonsterId = monsterId,
        Pos = pos.Net(),
        Rot = rot.Net(),
        Hp = hp,
        ActionName = _currMotion?.name ?? "",
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

    public AnimRootMotion Idle = AnimMgr.AnimRootMotions["Idle"];
    public AnimRootMotion Run = AnimMgr.AnimRootMotions["Run"];
    public AnimRootMotion Die = AnimMgr.AnimRootMotions["Die"];

    private List<(Vector3d hitFrom, AnimRootMotion anim)> HitAnims =
    [
        (new Vector3d(0, 0, 1), AnimMgr.AnimRootMotions["Damage_Front"]),
        (new Vector3d(0, 0, -1), AnimMgr.AnimRootMotions["Damage_Back"]),
        (new Vector3d(1, 0, 0), AnimMgr.AnimRootMotions["Damage_Right"]),
        (new Vector3d(-1, 0, 0), AnimMgr.AnimRootMotions["Damage_Left"])
    ];

    private AnimRootMotion[] AttackAnims =
    [
        AnimMgr.AnimRootMotions["Skill_A"],
        AnimMgr.AnimRootMotions["Skill_B"],
        AnimMgr.AnimRootMotions["Skill_C"],
    ];

    public float attackDistance = 5f;
    public float chaseDistance = 10f;

    public void Update(float dt)
    {
        UpdateAnimPlayer(dt);
        UpdateState(dt);
        // Log.Debug("RoomId: {0} MonsterId: {1} State: {2} pos: {3}, rot: {4}",
        //     room.id, monsterId, _state, pos, rot);
        UpdateGravity(dt);
        UpdateCollisionDetect();
    }

    private void UpdateGravity(float dt)
    {
        pos.y -= 9.8 * dt;
        if (pos.y < 0)
        {
            pos.y = 0;
        }
    }

    private void UpdateCollisionDetect()
    {
        float radius1 = 0.3f;
        float radius2 = 0.55f;
        foreach (var player in room.players)
        {
            var frontRole = player.Roles.Find(x => x.Id == player.FrontRoleId);
            if (frontRole == null) continue;
            double dist = Vector3d.Distance(frontRole.Pos.ToDouble(), pos);
            if (dist < radius1 + radius2)
            {
                var v = pos - frontRole.Pos.ToDouble();
                v.y = 0f;
                pos += v.normalized * (radius1 + radius2 - dist);
            }
        }
    }

    private void UpdateState(float dt)
    {
        switch (_state)
        {
            case State.Idle:
            {
                idleColdTime += dt;
                if (idleColdTime < maxIdleColdTime) return;
                idleColdTime = 0f;
                var role = room.ClosestFrontRole(pos.ToSingle(), out float dis);
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
                var role = room.ClosestFrontRole(pos.ToSingle(), out float dis);
                if (role == null)
                {
                    EnterState(State.Idle, () => Play(Idle));
                }
                else
                {
                    var v = role.Pos.ToDouble() - pos;
                    v.y = 0f;
                    rot.SetLookRotation(v);
                    if (dis < attackDistance)
                    {
                        EnterState(State.Attack);
                    }
                    else if (dis > chaseDistance)
                    {
                        EnterState(State.Idle, () => Play(Idle));
                    }
                }
                break;
            }
        }
    }

    public void EnterState(State state, object arg = null)
    {
        _state = state;
        Log.Debug("RoomId: {0} MonsterId: {1} EnterState: {2}",
            room.id, monsterId, state);
        switch (state)
        {
            case State.Idle:
            {
                idleColdTime = 0f;
                Play(Idle, () => Play(Idle));
                break;
            }
            case State.Chase:
            {
                Play(Run, () => Play(Run));
                break;
            }
            case State.Attack:
            {
                var role = room.ClosestFrontRole(pos.ToSingle(), out float dis);
                if (role != null)
                {
                    var v = role.Pos.ToDouble() - pos;
                    v.y = 0;
                    rot.SetLookRotation(v);
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
                var hitFrom = (Vector3d)arg;
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

    private AnimRootMotion ChooseHitAction(Vector3d hitFrom)
    {
        hitFrom = hitFrom.normalized;
        AnimRootMotion result = null;
        double min = double.MaxValue;
        foreach (var item in HitAnims)
        {
            double dot = Vector3d.Dot(item.hitFrom, hitFrom);
            if (dot < min)
            {
                min = dot;
                result = item.anim;
            }
        }
        return result;
    }

    #region AnimPlayer

    private AnimRootMotion _currMotion;
    private float _currTime;
    private Action _onFinish;
    public void Play(AnimRootMotion motion, Action onFinish = null)
    {
        Log.Debug("RoomId: {0}, MonsterId: {1}, Play: {2}",
            room.id, monsterId, motion.name);
        var notify = new NotifyMonsterPlayAction
        {
            MonsterId = monsterId,
            ActionName = motion.name
        };
        room.Broadcast(notify);

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

        var deltaPosition = (pos2 - pos1).ToDouble();
        deltaPosition = rot * deltaPosition;
        var deltaRotation = rot2 * Quaterniond.Inverse(rot1);

        OnAnimPlayerMove(deltaPosition, deltaRotation);
        if (_currTime >= _currMotion.length)
        {
            Log.Debug("RoomId: {0}, MonsterId: {1}, Finish: {2}, Length: {3}",
                room.id, monsterId, _currMotion.name, _currMotion.length);
            _currMotion = null;
            _currTime = 0;
            _onFinish?.Invoke();
        }
    }

    private void OnAnimPlayerMove(Vector3d deltaPosition, Quaterniond deltaRotation)
    {
        pos += deltaPosition;
        // rot *= deltaRotation;
    }

    #endregion
}