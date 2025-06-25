using Kirara.Network;
using ZZZServer.Model;


namespace ZZZServer.Handler;

public class MsgUpdateEntityFromAutonomous_Handler : MsgHandler<MsgUpdateFromAutonomous>
{
    protected override void Run(Session session, MsgUpdateFromAutonomous msg)
    {
        var player = (Player)session.Data;
        player.Room?.UpdateFromAutonomous(player, msg.Player);
    }
}