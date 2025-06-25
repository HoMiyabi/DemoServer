using Kirara.Network;
using MongoDB.Bson;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class MsgGachaDisc_Handler : MsgHandler<MsgGachaDisc>
{
    protected override void Run(Session session, MsgGachaDisc message)
    {
        var player = (Player)session.Data;

        var disc = DiscService.GachaDisc();

        disc.Id = ObjectId.GenerateNewId().ToString();

        var notifyObtain = new NotifyObtainItems();
        notifyObtain.DiscItems.Add(disc.Net);

        session.Send(notifyObtain);
    }
}