using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler.Monster;

public class MsgMonsterTakeDamage_Handler : MsgHandler<MsgMonsterTakeDamage>
{
    protected override void Run(Session session, MsgMonsterTakeDamage msg)
    {
        var player = (Player)session.Data;
        var room = player.Room;
        room?.MonsterTakeDamage(player, msg.MonsterId, msg.Damage);
    }
}