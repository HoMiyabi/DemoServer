using Kirara.Network;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class MsgEnterRoom_Handler : MsgHandler<MsgEnterRoom>
{
    protected override void Run(Session session, MsgEnterRoom message)
    {
        var player = (Player)session.Data;

        int roomId = 1;
        if (!RoomService.rooms.TryGetValue(roomId, out var room))
        {
            room = RoomService.AddRoom(roomId);
        }

        room.AddPlayer(player);
    }
}