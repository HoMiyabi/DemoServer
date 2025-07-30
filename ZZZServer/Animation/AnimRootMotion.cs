using System.Numerics;
using Vector3 = ZZZServer.Utils.Vector3;

namespace ZZZServer.Animation;

public class AnimRootMotion
{
    public float length;
    public string name;
    public List<float> tx = new();
    public List<float> ty = new();
    public List<float> tz = new();
    public List<float> qx = new();
    public List<float> qy = new();
    public List<float> qz = new();
    public List<float> qw = new();

    public Vector3 EvalT(float time)
    {
        const float frameRate = 60f;
        float frame = time * frameRate;
        frame = Math.Clamp(frame, 0, tx.Count - 1);
        int frameIndex = (int)frame;
        return new Vector3(tx[frameIndex], ty[frameIndex], tz[frameIndex]);
    }

    public Quaternion EvalQ(float time)
    {
        const float frameRate = 60f;
        float frame = time * frameRate;
        frame = Math.Clamp(frame, 0, qx.Count - 1);
        int frameIndex = (int)frame;
        return new Quaternion(qx[frameIndex], qy[frameIndex], qz[frameIndex], qw[frameIndex]);
    }
}