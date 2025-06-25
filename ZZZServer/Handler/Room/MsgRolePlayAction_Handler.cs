
using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler;

public class MsgRolePlayAction_Handler : MsgHandler<MsgRolePlayAction>
{
    protected override void Run(Session session, MsgRolePlayAction msg)
    {
        var player = (Player)session.Data;
        player.Room?.PlayerRolePlayAction(player, msg.RoleId, msg.ActionName);
    }
}