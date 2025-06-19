/*using SqlSugar;

namespace ZZZServer.DbEntity;

[SugarTable("disc")]
public class DbDisc
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public int UId { get; set; }
    public int ConfigId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int WearerId { get; set; }
    public bool Locked { get; set; }
    public int Pos { get; set; }

    [SugarColumn(IsJson = true, Length = 1000)]
    public DiscAttrDO MainAttr { get; set; }

    [SugarColumn(IsJson = true, Length = 4000)]
    public List<DiscAttrDO> SubAttrs { get; set; }

    public NDiscItem Net()
    {
        var ret = new NDiscItem
        {
            Id = Id,
            Cid = ConfigId,
            Level = Level,
            Exp = Exp,
            WearerId = WearerId,
            Locked = Locked,
            Pos = Pos,
            MainAttr = MainAttr.Net(),
        };
        ret.SubAttrs.AddRange(SubAttrs.Select(it => it.Net()));
        return ret;
    }
}*/