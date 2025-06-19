
namespace ZZZServer.Service;

public static class RoomService
{
    public static readonly Dictionary<int, Room> rooms = new();

    public static Room AddRoom(int id)
    {
        var room = new Room(id);
        rooms.Add(id, room);
        return room;
    }
}