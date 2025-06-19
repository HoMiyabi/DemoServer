
using Kirara.Network;
using ZZZServer.MongoDocEntity;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class MsgPlayAction_Handler : MsgHandler<MsgPlayAction>
{
    protected override void Run(Session session, MsgPlayAction message)
    {
        var player = (Player)session.Data;
        player.Room?.PlayerChPlayAction(playerData, message.ActionName);
    }
}