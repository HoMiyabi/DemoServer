using Mathd;
using Serilog;
using ZZZServer.Utils;

namespace ZZZServer.Navigation;

public class NavigationMgr : Singleton<NavigationMgr>, IDisposable
{
    private string navMeshName = "Data/NavigationData/solo_navmesh.bin";

    public void Init()
    {
        RecastInterface.Init();

        if (!RecastInterface.LoadMap(1, navMeshName.ToCharArray()))
        {
            Log.Error("加载导航网格失败: {0}", navMeshName);
        }
    }

    public void SearchPath(int mapId, Vector3d start, Vector3d end, List<Vector3d> path)
    {
        if (RecastInterface.FindPath(mapId, ref start, ref end))
        {
            RecastInterface.Smooth(mapId, 2f, 0.5f);
            {
                int smoothCount = 0;
                float[] smooths = RecastInterface.GetPathSmooth(mapId, out smoothCount);
                for (int i = 0; i < smoothCount; ++i)
                {
                    var node = new Vector3d(smooths[i * 3], smooths[i * 3 + 1], smooths[i * 3 + 2]);
                    path.Add(node);
                    //Log.Info($"路径点：{node}");
                }
            }
        }
        else
        {
            Log.Error("寻路失败, id: {0}, start: {1}, end: {2}", mapId, start, end);
        }
    }

    public void Dispose()
    {
        RecastInterface.Fini();
    }
}