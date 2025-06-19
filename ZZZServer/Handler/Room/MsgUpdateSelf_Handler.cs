
using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class MsgUpdateSelf_Handler : MsgHandler<MsgUpdateSelf>
{
    protected override void Run(Session session, MsgUpdateSelf message)
    {
        var player = (Player)session.Data;
        player.room?.SyncPlayerFromAutonomous(player, message.PosRot);
    }
}