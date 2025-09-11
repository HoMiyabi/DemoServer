using Mathd;

namespace ZZZServer.Anim;

public class BoxNotifyState : ActionNotifyState
{
    public float start;
    public float length;
    public EBoxType boxType;
    public EBoxShape boxShape;
    public Vector3d center;
    public float radius;
    public Vector3d size;
    public EHitStrength hitStrength;
    public int hitId;
    public bool setParticleRot;
    public float rotValue;
    public float rotMaxValue;
    public float hitGatherDist;

    public override void NotifyBegin(ActionPlayer player)
    {
        base.NotifyBegin(player);
        if (player.node is Monster monster)
        {
            monster.BoxBegin(this);
        }
    }
}