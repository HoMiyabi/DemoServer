using Kirara.Network;

namespace ZZZServer.Handler;

public class ReqGetExchangeItems_Handler : RpcHandler<ReqGetExchangeItems, RspGetExchangeItems>
{
    protected override void Run(Session session, ReqGetExchangeItems req, RspGetExchangeItems rsp, Action reply)
    {
        rsp.Items.AddRange(ConfigMgr.tb.TbExchangeConfig.DataList
            .Select(x => new NExchangeItem
            {
                ExchangeId = x.ExchangeId,
                FromConfigId = x.FromConfigId,
                FromCount = x.FromCount,
                ToConfigId = x.ToConfigId,
                ToCount = x.ToCount,
            }));
    }
}