using ZZZServer.Service;
using Kirara.Network;
using MongoDB.Bson;
using ZZZServer.Model;

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
            rsp.Result = new Result { Code = 1, Msg = "货币不足" };
            return;
        }

        InventoryService.AddCurrencyCount(player, exConfig.FromConfigId, -cost);

        for (int i = 0; i < req.ExchangeCount * exConfig.ToCount; i++)
        {
            player.Weapons.Add(WeaponService.GachaWeapon());
        }

        // todo)) 获得提示
    }
}