/*using SqlSugar;

namespace ZZZServer.DbEntity;

public class CharacterInfo
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public int UId { get; set; }
    public int ConfigId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int WeaponId { get; set; }

    [SugarColumn(IsJson = true)]
    public List<int>? DiscIds { get; set; }

    public NCharacterInfo Net()
    {
        var ret = new NCharacterInfo
        {
            Id = Id,
            ConfigId = ConfigId,
            Level = Level,
            Exp = Exp,
            WeaponId = WeaponId,
        };
        ret.DiscIds.Add(DiscIds);
        return ret;
    }
}*/