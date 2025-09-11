using System.Diagnostics;
using cfg.main;
using Mathd;
using Serilog;
using ZZZServer.Anim;
using ZZZServer.Model;
using ZZZServer.Service;
using ZZZServer.Utils;
using Action = System.Action;

namespace ZZZServer;

public class Monster : Node
{
    private readonly MonsterConfig config;
    public Room room;
    public readonly int monsterId;
    public double hp;

    private readonly ActionPlayer actionPlayer;
    private readonly GravityComponent gravityComponent;

    public Monster(int cid, Room room, int monsterId, Vector3d position, Quaterniond rotation)
    {
        this.position = position;
        this.rotation = rotation;

        actionPlayer = new ActionPlayer(this);
        actionPlayer.OnActionPlayerMove += OnActionPlayerMove;

        gravityComponent = new GravityComponent(this);
        gravityComponent.PlaneY = -1.65;

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
        Move(deltaPosition);
    }

    private void Move(Vector3d deltaPosition)
    {
        position += deltaPosition;
    }

    public void Update(float dt)
    {
        actionPlayer.Update(dt);
        UpdateState(dt);
        // Log.Debug("RoomId: {0} MonsterId: {1} State: {2} pos: {3}, rot: {4}",
        //     room.id, monsterId, _state, pos, rot);
        gravityComponent.Update(dt);
        UpdateCollisionDetection();
    }

    private bool DetectCollision(Vector3d pos1, double radius1, Vector3d pos2, double radius2, out double dist)
    {
        pos1.y = 0;
        pos2.y = 0;
        dist = Vector3d.Distance(pos1, pos2);
        return dist < radius1 + radius2;
    }

    private List<Role> DetectCollisionRoles(Vector3d pos, double radius)
    {
        var roles = new List<Role>();
        float radius1 = 0.3f;
        foreach (var player in room.players)
        {
            var frontRole = player.Roles.Find(x => x.Id == player.FrontRoleId);
            if (frontRole == null) continue;
            var pos1 = frontRole.Pos;
            // Log.Debug("pos1: {0}, radius1: {1}, pos: {2}, radius: {3}", pos1, radius1, pos, radius);
            if (DetectCollision(pos1, radius1, pos, radius, out double dist))
            {
                roles.Add(frontRole);
            }
        }
        return roles;
    }

    private void UpdateCollisionDetection()
    {
        float radius1 = 0.3f;
        float radius2 = 0.55f;
        foreach (var player in room.players)
        {
            var frontRole = player.Roles.Find(x => x.Id == player.FrontRoleId);
            if (frontRole == null) continue;
            if (DetectCollision(frontRole.Pos, radius1, position, radius2, out double dist))
            {
                var v = position - frontRole.Pos;
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
                var role = room.ClosestFrontRole(position, out double dis);
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
                var role = room.ClosestFrontRole(position, out double dis);
                if (role == null)
                {
                    EnterState(State.Idle);
                }
                else
                {
                    var v = role.Pos - position;
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
                var role = room.ClosestFrontRole(position, out double dis);
                if (role != null)
                {
                    var v = role.Pos - position;
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

    public void BoxBegin(BoxNotifyState box)
    {
        if (box.boxType == EBoxType.HitBox)
        {
            if (box.boxShape == EBoxShape.Sphere)
            {
                var pos = box.center;
                pos = rotation * pos + position;
                var roles = DetectCollisionRoles(pos, box.radius);
                Log.Debug("Detect roles.Count: {0}", roles.Count);
                if (roles.Count > 0)
                {
                    bool parried = roles.Any(x => x.Parrying);
                    if (parried)
                    {
                        Log.Debug("parried");
                    }
                    else
                    {
                        var notify = new NotifyRoleTakeDamage()
                        {
                            Damage = 100
                        };
                        foreach (var role in roles)
                        {
                            notify.RoleId = role.Id;
                            room.Broadcast(notify);
                        }
                    }
                }
            }
            else
            {
                Log.Warning("BoxShape: {0} 不支持", box.boxShape);
            }
        }
    }

    public void TakeDamage(Player player, MsgMonsterTakeDamage msg)
    {
        Vector3d hitFrom;
        if (msg.HitGatherDist != 0f)
        {
            hitFrom = position - msg.CenterPos.Native();
        }
        else
        {
            hitFrom = msg.RolePos.Native() - position;
        }
        EnterState(Monster.State.Hit, hitFrom);

        hp = Math.Max(0, hp - msg.Damage);
        var notify = new NotifyMonsterTakeDamage
        {
            MonsterId = msg.MonsterId,
            Damage = msg.Damage,
            IsCrit = msg.IsCrit,
            CurrHp = hp
        };
        room.Broadcast(notify);

        // 聚怪效果
        if (msg.HitGatherDist != 0f)
        {
            var worldCenter = msg.CenterPos.Native();

            // 移动向量的水平投影，最长不能超过v
            var v = (worldCenter - position);
            v.y = 0f;

            double dist = Math.Min(msg.HitGatherDist, v.magnitude); // 不能越过中心
            var dir = v.normalized; // 方向
            position += dir * dist;
        }

        if (hp <= 0)
        {
            room.monsters.Remove(this);
            var notifyMonsterDie = new NotifyMonsterDie
            {
                MonsterId = msg.MonsterId
            };
            room.Broadcast(notifyMonsterDie);
        }
    }
}