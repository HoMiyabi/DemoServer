using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler.Monster;

public class MsgMonsterTakeDamage_Handler : MsgHandler<MsgMonsterTakeDamage>
{
    protected override void Run(Session session, MsgMonsterTakeDamage msg)
    {
        var player = (Player)session.Data;
        var room = player.Room;
        var monster = room.monsters.FirstOrDefault(x => x.monsterId == msg.MonsterId);
        if (monster != null)
        {
            monster.TakeDamage(player, msg);
        }
    }
}