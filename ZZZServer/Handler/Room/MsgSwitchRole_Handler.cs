using Kirara.Network;
using ZZZServer.MongoDocEntity;

namespace ZZZServer.Handler;

public class MsgSwitchRole_Handler : MsgHandler<MsgSwitchRole>
{
    protected override void Run(Session session, MsgSwitchRole message)
    {
        var player = (Player)session.Data;
        player.Room?.PlayerSwitchRole(player, message.FrontRoleId, message.Next);
    }
}