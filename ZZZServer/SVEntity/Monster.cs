using cfg.main;
using Mathd;
using Serilog;
using ZZZServer.Anim;
using ZZZServer.Service;
using ZZZServer.Utils;
using Action = System.Action;

namespace ZZZServer.SVEntity;

public class Monster : Node
{
    private readonly MonsterConfig config;
    public Room room;
    public readonly int monsterId;
    public double hp;

    private readonly ActionPlayer actionPlayer;

    public Monster(int cid, Room room, int monsterId)
    {
        actionPlayer = new ActionPlayer(this);
        actionPlayer.OnActionPlayerMove += OnActionPlayerMove;

        config = ConfigMgr.tb.TbMonsterConfig[cid];
        this.room = room;
        this.monsterId = monsterId;
        hp = config.Hp;
        Log.Debug("Monster, id: {0}, hp: {1}", monsterId, hp);

        EnterState(State.Idle);
    }

    public NSyncMonster NSyncMonster => new()
    {
        MonsterCid = config.Id,
        MonsterId = monsterId,
        Pos = position.Net(),
        Rot = rotation.Net(),
        Hp = hp,
        ActionName = actionPlayer.Action.name
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

    public ZZZServer.Anim.Action Idle = ActionMgr.Actions["Monster1_Idle"];
    public ZZZServer.Anim.Action Run = ActionMgr.Actions["Monster1_Run"];
    public ZZZServer.Anim.Action Die = ActionMgr.Actions["Monster1_Die"];

    private List<(Vector3d hitFrom, ZZZServer.Anim.Action action)> HitActions =
    [
        (new Vector3d(0, 0, 1), ActionMgr.Actions["Monster1_Damage_Front"]),
        (new Vector3d(0, 0, -1), ActionMgr.Actions["Monster1_Damage_Back"]),
        (new Vector3d(1, 0, 0), ActionMgr.Actions["Monster1_Damage_Right"]),
        (new Vector3d(-1, 0, 0), ActionMgr.Actions["Monster1_Damage_Left"])
    ];

    private ZZZServer.Anim.Action[] AttackActions =
    [
        ActionMgr.Actions["Monster1_Skill_A"],
        ActionMgr.Actions["Monster1_Skill_B"],
        ActionMgr.Actions["Monster1_Skill_C"],
    ];

    public float attackDistance = 5f;
    public float chaseDistance = 10f;

    private void OnActionPlayerMove(Vector3d deltaPosition, Quaterniond deltaRotation)
    {
        position += deltaPosition;
    }

    public void Update(float dt)
    {
        actionPlayer.Update(dt);
        UpdateState(dt);
        // Log.Debug("RoomId: {0} MonsterId: {1} State: {2} pos: {3}, rot: {4}",
        //     room.id, monsterId, _state, pos, rot);
        UpdateGravity(dt);
        UpdateCollisionDetect();
    }

    private void UpdateGravity(float dt)
    {
        position.y -= 9.8 * dt;
        if (position.y < 0)
        {
            position.y = 0;
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
            double dist = Vector3d.Distance(frontRole.Pos.ToDouble(), position);
            if (dist < radius1 + radius2)
            {
                var v = position - frontRole.Pos.ToDouble();
                v.y = 0f;
                position += v.normalized * (radius1 + radius2 - dist);
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
                var role = room.ClosestFrontRole(position.ToSingle(), out float dis);
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
                var role = room.ClosestFrontRole(position.ToSingle(), out float dis);
                if (role == null)
                {
                    EnterState(State.Idle);
                }
                else
                {
                    var v = role.Pos.ToDouble() - position;
                    v.y = 0f;
                    rotation.SetLookRotation(v);
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
        Log.Debug("RoomId: {0} MonsterId: {1} EnterState: {2}",
            room.id, monsterId, state);
        switch (state)
        {
            case State.Idle:
            {
                idleColdTime = 0f;
                PlayAction(Idle);
                break;
            }
            case State.Chase:
            {
                PlayAction(Run);
                break;
            }
            case State.Attack:
            {
                var role = room.ClosestFrontRole(position.ToSingle(), out float dis);
                if (role != null)
                {
                    var v = role.Pos.ToDouble() - position;
                    v.y = 0;
                    rotation.SetLookRotation(v);
                    int i = Random.Shared.Next(0, AttackActions.Length);
                    PlayAction(AttackActions[i], () =>
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
                PlayAction(hitAction, () =>
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

    private void PlayAction(Anim.Action action, Action onFinish = null)
    {
        actionPlayer.Play(action, onFinish);
        Log.Debug("RoomId: {0}, MonsterId: {1}, Play: {2}",
            room.id, monsterId, action.name);
        var notify = new NotifyMonsterPlayAction
        {
            MonsterId = monsterId,
            ActionName = action.name
        };
        room.Broadcast(notify);
    }

    private Anim.Action ChooseHitAction(Vector3d hitFrom)
    {
        hitFrom = hitFrom.normalized;
        Anim.Action result = null;
        double min = double.MaxValue;
        foreach (var item in HitActions)
        {
            double dot = Vector3d.Dot(item.hitFrom, hitFrom);
            if (dot < min)
            {
                min = dot;
                result = item.action;
            }
        }
        return result;
    }
}