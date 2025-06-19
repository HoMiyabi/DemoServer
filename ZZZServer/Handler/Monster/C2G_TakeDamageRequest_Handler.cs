using Kirara.Network;
using ZZZServer.MongoDocEntity;

namespace ZZZServer.Handler.Monster;

public class ReqTakeDamage_Handler : MsgHandler<MsgMonsterTakeDamage>
{
    protected override void Run(Session session, MsgMonsterTakeDamage msg)
    {
        var player = (Player)session.Data;
        var room = player.Room;
        room?.MonsterTakeDamage(player, msg.MonsterId, msg.Damage);
    }
}