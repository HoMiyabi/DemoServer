using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class MsgGatherMaterial_Handler : MsgHandler<MsgGatherMaterial>
{
    protected override void Run(Session session, MsgGatherMaterial msg)
    {
        var player = (Player)session.Data;

        InventoryService.AddMaterialCount(player, msg.MaterialCid, msg.Count);

        var notifyObtain = new NotifyObtainItems();
        notifyObtain.MatItems.Add(new NMaterialItem
        {
            Cid = msg.MaterialCid,
            Count = msg.Count
        });

        session.Send(notifyObtain);
    }
}