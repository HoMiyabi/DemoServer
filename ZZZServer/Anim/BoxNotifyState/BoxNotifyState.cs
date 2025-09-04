using Mathd;

namespace ZZZServer.Anim;

public class BoxNotifyState
{
    public float start;
    public float length;
    public EBoxType boxType;
    public Vector3d center;
    public float radius;
    public Vector3d size;
    public EHitStrength hitStrength;
    public int hitId;
    public bool setRot;
    public float rotValue;
    public float rotMaxValue;
    public float hitGatherDist;
}