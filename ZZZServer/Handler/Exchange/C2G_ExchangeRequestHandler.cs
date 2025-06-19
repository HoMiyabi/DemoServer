using ZZZServer.Service;
using Kirara.Network;
using ZZZServer.MongoDocEntity;

namespace ZZZServer.Handler;

public class ReqExchangeHandler : RpcHandler<ReqExchange, RspExchange>
{
    protected override void Run(Session session, ReqExchange req, RspExchange rsp, Action reply)
    {
        var player = (Player)session.Data;
        var exConfig = ConfigMgr.tb.TbExchangeConfig[req.ExchangeId];

        int coin = InventoryService.GetCurrencyCount(player, exConfig.FromConfigId);

        int cost = exConfig.FromCount * req.ExchangeCount;
        if (coin < cost)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "货币不足";
            return;
        }

        InventoryService.AddCurrencyCount(player, exConfig.FromConfigId, -cost);

        var list = new List<DbWeapon>(req.ExchangeCount * exConfig.ToCount);
        for (int i = 0; i < req.ExchangeCount * exConfig.ToCount; i++)
        {
            list.Add(WeaponService.GachaWeapon(player.UId));
        }

        DbHelper.Db.CopyNew().Insertable(list).ExecuteCommand();

        rsp.Result.Code = 0;
        rsp.Result.Msg = "兑换成功";
    }
}