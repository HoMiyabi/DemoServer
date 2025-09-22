using Kirara.Network;
using Serilog;
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
            Log.Debug("[Attack] Set Parrying: {0}, Role: {1}", msg.Parrying, role.Id);
        }
    }
}