using Kirara.Network;
using ZZZServer.Model;

namespace ZZZServer.Handler.Role;

public class MsgRoleSetParry_Handler : MsgHandler<MsgRoleSetParry>
{
    protected override void Run(Session session, MsgRoleSetParry msg)
    {
        var player = (Player)session.Data;
        var role = player.Roles.FirstOrDefault(it => it.Id == msg.RoleId);
        if (role != null)
        {
            role.Parrying = msg.Parrying;
        }
    }
}