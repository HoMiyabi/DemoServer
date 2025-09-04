using Mathd;
using Vector3 = ZZZServer.Utils.Vector3;

namespace ZZZServer.Anim;

public class AnimRootMotion
{
    public float length;
    public int frameRate;
    public List<float> tx;
    public List<float> ty;
    public List<float> tz;
    public List<float> qx;
    public List<float> qy;
    public List<float> qz;
    public List<float> qw;

    public Vector3 EvalT(float time)
    {
        const float frameRate = 60f;
        float frame = time * frameRate;
        frame = Math.Clamp(frame, 0, tx.Count - 1);
        int frameIndex = (int)frame;
        return new Vector3(tx[frameIndex], ty[frameIndex], tz[frameIndex]);
    }

    public Quaterniond EvalQ(float time)
    {
        const float frameRate = 60f;
        float frame = time * frameRate;
        frame = Math.Clamp(frame, 0, qx.Count - 1);
        int frameIndex = (int)frame;
        return new Quaterniond(qx[frameIndex], qy[frameIndex], qz[frameIndex], qw[frameIndex]);
    }
}