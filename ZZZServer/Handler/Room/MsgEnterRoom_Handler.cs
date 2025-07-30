using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class MsgEnterRoom_Handler : MsgHandler<MsgEnterRoom>
{
    protected override void Run(Session session, MsgEnterRoom message)
    {
        var player = (Player)session.Data;

        var room = RoomService.GetRoom(1);
        if (room == null)
        {
            room = RoomService.NewRoom();
        }

        room.AddPlayer(player);
    }
}