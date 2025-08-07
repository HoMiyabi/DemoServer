
namespace ZZZServer.Service;

public static class RoomService
{
    private static int id = 0;
    public static readonly Dictionary<int, Room> rooms = new();

    public static Room NewRoom()
    {
        var room = new Room(++id);
        rooms.Add(id, room);
        return room;
    }

    public static Room GetRoom(int id)
    {
        return rooms.GetValueOrDefault(id);
    }

    public static void Update(float dt)
    {
        foreach (var room in rooms.Values)
        {
            room.Update(dt);
        }
    }
}