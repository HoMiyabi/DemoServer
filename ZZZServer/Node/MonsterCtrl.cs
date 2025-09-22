using cfg.main;
using Mathd;
using Serilog;
using ZZZServer.Anim;
using ZZZServer.Model;
using ZZZServer.Navigation;
using ZZZServer.Service;
using ZZZServer.Utils;
using Action = System.Action;

namespace ZZZServer;

public class MonsterCtrl : Node
{
    private readonly MonsterConfig config;
    public Room room;
    public readonly int monsterId;
    public double hp;

    private readonly ActionPlayer actionPlayer;
    private readonly GravityComponent gravityComponent;

    private readonly List<Role> roles = [];

    // 添加一个受击次数记录，没达到之前受击就会进入Hit受击状态，达到之后不受影响照样发动攻击，发动攻击后应该清0。
    private int getHitCount = 0;
    private int maxCanEnterHitGetHitCount = 20;

    public MonsterCtrl(int cid, Room room, int monsterId, Vector3d position, Quaterniond rotation)
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
        Idle, // 待机
        Chase, // 追击
        Attack, // 攻击
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
    public float chaseDistance = 15f;

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

    private void UpdateCollisionDetection()
    {
        double roleRadius = 0.55;
        double monsterRadius = 0.4;

        // 检测和角色碰撞
        foreach (var player in room.Players)
        {
            var frontRole = player.Roles.Find(x => x.Id == player.FrontRoleId);
            if (frontRole == null) continue;

            if (Room.DetectCollision(frontRole.Pos, roleRadius, position, monsterRadius, out double dist))
            {
                var dir = position - frontRole.Pos;
                dir.y = 0f;
                dir.Normalize();
                var delta = dir * (roleRadius + monsterRadius - dist);
                position += delta;
                // Log.Debug("碰撞检测, delta: {0}", delta);
            }
        }

        // 检测墙壁碰撞
        double xMin = 10.5;
        double zMin = -40.35;

        double xMax = 28.5;
        double zMax = -2;
        position.x = Math.Clamp(position.x, xMin + monsterRadius, xMax - monsterRadius);
        position.z = Math.Clamp(position.z, zMin + monsterRadius, zMax - monsterRadius);
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
                    timeToUpdatePath -= dt;
                    if (timeToUpdatePath < 0f)
                    {
                        timeToUpdatePath = pathUpdateInterval;
                        Path.Clear();
                        NavigationMgr.Instance.SearchPath(1, position, role.Pos, Path);
                        Log.Debug("导航路径. start: {0}, end: {1}, path: {2}",
                            position, role.Pos, string.Join(", ", Path));

                        BroadcastPath();
                    }
                    if (Path.Count > 0)
                    {
                        var v = Path[0] - position;
                        v.y = 0f;
                        rotation = Quaterniond.Lerp(rotation, Quaterniond.LookRotation(v, Vector3d.up), dt * 20f);
                        if (Vector3d.Distance(Path[0], position) < 0.4f)
                        {
                            Path.RemoveAt(0);
                            BroadcastPath();
                        }
                    }
                    if (dis < attackDistance)
                    {
                        Path.Clear();
                        BroadcastPath();
                        EnterState(State.Attack);
                    }
                    else if (dis > chaseDistance)
                    {
                        Path.Clear();
                        BroadcastPath();
                        EnterState(State.Idle);
                    }
                }
                break;
            }
        }
    }

    private void BroadcastPath()
    {
        room.Broadcast(new NotifyMonsterPath
        {
            MonsterId = monsterId,
            Path = {Path.Select(x => x.Net())}
        });
    }

    private List<Vector3d> Path { get; } = [];
    private float pathUpdateInterval = 1f;
    private float timeToUpdatePath;

    public void EnterState(State state, object arg = null)
    {
        _state = state;
        // Log.Debug("RoomId: {0} MonsterId: {1} EnterState: {2}",
        //     room.id, monsterId, state);
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
                var role = room.ClosestFrontRole(position, out double dis);
                if (role != null)
                {
                    timeToUpdatePath = pathUpdateInterval;
                    Path.Clear();
                    NavigationMgr.Instance.SearchPath(1, position, role.Pos, Path);
                    Log.Debug("导航路径. start: {0}, end: {1}, path: {2}",
                        position, role.Pos, string.Join(", ", Path));
                }
                PlayAction(Run);
                break;
            }
            case State.Attack:
            {
                getHitCount = 0;
                var role = room.ClosestFrontRole(position, out double dis);
                if (role != null)
                {
                    var v = role.Pos - position;
                    v.y = 0;
                    rotation.SetLookRotation(v);
                    int i = Random.Shared.Next(0, AttackActions.Length);
                    if (i == 2)
                    {
                        PlayAction(AttackActions[2], () =>
                        {
                            PlayAction(AttackActions[1], () =>
                            {
                                EnterState(State.Idle);
                            });
                        });
                    }
                    else
                    {
                        PlayAction(AttackActions[i], () =>
                        {
                            EnterState(State.Idle);
                        });
                    }
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
        Log.Debug("PlayAction {0}", action.name);
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
                var worldPos = TransformPoint(box.center);
                room.DetectCollisionRoles(worldPos, box.radius, roles);
                Role parryingRole = null;
                if (roles.Count > 0)
                {
                    parryingRole = roles.FirstOrDefault(x => x.Parrying);
                    if (parryingRole != null)
                    {
                        var notify = new NotifyMonsterAttackRole()
                        {
                            MonsterId = monsterId,
                            RoleId = parryingRole.Id,
                            Damage = 0,
                            Parried = true
                        };
                        room.Broadcast(notify);

                        // 进入被格挡状态，动作暂停0.5秒
                        actionPlayer.PauseDuration += 0.5f;
                    }
                    else
                    {
                        var notify = new NotifyMonsterAttackRole()
                        {
                            Damage = 100,
                            Parried = false
                        };
                        foreach (var role in roles)
                        {
                            if (role.Dodging)
                            {
                                Log.Debug("Attack, Dodging Role {0}", role.Id);
                            }
                            else
                            {
                                if (role.ActionName.EndsWith("Attack_Ex_Special"))
                                {
                                    // 强化特殊技有霸体和无敌，理应做到技能里面，但现在只能特判一下，唉
                                    // 除非把角色的技能属性也放到服务器跑
                                    Log.Debug("Role {0} Attack_Ex_Special", role.Id);
                                }
                                else
                                {
                                    Log.Debug("[Attack], Hit Role {0}", role.Id);
                                    notify.RoleId = role.Id;
                                    room.Broadcast(notify);
                                }
                            }
                        }
                    }
                }
                Log.Debug("[Attack], Count: {0}, Parrying role: {1}", roles.Count, parryingRole?.Id);
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
        getHitCount++;

        // 放技能不能打断，这里特判一下
        if (getHitCount <= maxCanEnterHitGetHitCount &&
            actionPlayer.Action.name != "Monster1_Skill_A" &&
            actionPlayer.Action.name != "Monster1_Skill_B" &&
            actionPlayer.Action.name != "Monster1_Skill_C")
        {
            EnterState(MonsterCtrl.State.Hit, hitFrom);
        }

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
            room.Monsters.Remove(this);
            var notifyMonsterDie = new NotifyMonsterDie
            {
                MonsterId = msg.MonsterId
            };
            room.Broadcast(notifyMonsterDie);
        }
    }
}