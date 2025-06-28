using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class ReqUpgradeDisc_Handler : RpcHandler<ReqUpgradeDisc, RspUpgradeDisc>
{
    protected override void Run(Session session, ReqUpgradeDisc req, RspUpgradeDisc rsp, Action reply)
    {
        var player = (Player)session.Data;
        var disc = player.Discs.Find(it => it.Id == req.DiscId);
        if (disc == null)
        {
            rsp.Result = new Result { Code = 1, Msg = "驱动盘不存在" };
            return;
        }

        var matConfig = ConfigMgr.tb.TbMaterialItemConfig[req.MaterialCid];
        int exp = matConfig.Exp;
        int count = req.Count;
        int total = exp * count;
        DiscService.UpgradeDisc(disc, total);
        rsp.SubAttrs.Add(disc.SubAttrs.Select(it => it.Net));
    }
}