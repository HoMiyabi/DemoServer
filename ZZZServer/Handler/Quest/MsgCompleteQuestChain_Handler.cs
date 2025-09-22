using Kirara.Network;
using Serilog;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler.Quest;

public class MsgCompleteQuestChain_Handler : MsgHandler<MsgCompleteQuestChain>
{
    protected override void Run(Session session, MsgCompleteQuestChain msg)
    {
        var player = (Player)session.Data;
        var rewords = ConfigMgr.tb.TbQuestChainConfig[msg.QuestChainCid].Rewords;
        foreach (var reword in rewords)
        {
            InventoryService.AddCurrencyCount(player, reword.CurrencyCid, reword.Count);
        }

        var notifyObtain = new NotifyObtainItems();
        notifyObtain.CurItems.Add(rewords.Select(it => new NCurrencyItem
        {
            Cid = it.CurrencyCid,
            Count = it.Count
        }));
        session.Send(notifyObtain);
    }
}