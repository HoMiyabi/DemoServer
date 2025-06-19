/*using SqlSugar;

namespace ZZZServer.DbEntity;

[SugarTable("weapon")]
public class DbWeapon
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public int UId { get; set; }
    public int ConfigId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int WearerId { get; set; }
    public bool Locked { get; set; }
    public int RefineLevel { get; set; }

    [SugarColumn(IsJson = true, Length = 1000)]
    public WeaponAttrDO BaseAttr { get; set; }

    [SugarColumn(IsJson = true, Length = 1000)]
    public WeaponAttrDO AdvancedAttr { get; set; }

    public NWeaponItem Net()
    {
        return new NWeaponItem
        {
            Id = Id,
            Cid = ConfigId,
            Level = Level,
            Exp = Exp,
            WearerId = WearerId,
            Locked = Locked,
            RefineLevel = RefineLevel,
            BaseAttr = BaseAttr.Net(),
            AdvancedAttr = AdvancedAttr.Net()
        };
    }
}*/