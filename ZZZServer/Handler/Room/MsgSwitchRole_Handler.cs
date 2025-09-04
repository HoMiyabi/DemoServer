using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler;

public class MsgSwitchRole_Handler : MsgHandler<MsgSwitchRole>
{
    protected override void Run(Session session, MsgSwitchRole msg)
    {
        var player = (Player)session.Data;
        player.SwitchRole(msg.FrontRoleId);
    }
}