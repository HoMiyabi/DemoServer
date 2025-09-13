using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler.Role;

public class MsgRoleSetDodge_Handler : MsgHandler<MsgRoleSetDodge>
{
    protected override void Run(Session session, MsgRoleSetDodge msg)
    {
        var player = (Player)session.Data;
        var role = player.Roles.FirstOrDefault(it => it.Id == msg.RoleId);
        if (role != null)
        {
            role.Dodging = msg.Dodging;
        }
    }
}