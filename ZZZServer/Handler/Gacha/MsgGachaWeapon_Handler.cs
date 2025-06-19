using ZZZServer.Service;
using Kirara.Network;
using MongoDB.Bson;
using ZZZServer.MongoDocEntity;

namespace ZZZServer.Handler;

public class MsgGachaWeapon_Handler : MsgHandler<MsgGachaWeapon>
{
    protected override void Run(Session session, MsgGachaWeapon message)
    {
        var player = (Player)session.Data;

        var weapon = WeaponService.GachaWeapon();
        weapon.Id = ObjectId.GenerateNewId().ToString();

        var notifyObtain = new NotifyObtainItems();
        notifyObtain.WeaponItems.Add(weapon.Net());

        session.Send(notifyObtain);
    }
}