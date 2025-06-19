using ZZZServer.Service;

namespace ZZZServer.SVEntity;

public class Monster
{
    public Room room;
    public int monsterId;
    public float hp;

    public Monster(Room room, int monsterId, float hp)
    {
        this.room = room;
        this.monsterId = monsterId;
        this.hp = hp;
    }
}