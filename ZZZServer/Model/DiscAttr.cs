namespace ZZZServer.Model;

public class DiscAttr
{
    public int AttrEntryId { get; set; }
    public int AttrTypeId { get; set; }
    public float Value { get; set; }
    public int UpgradeTimes { get; set; }

    public NDiscAttr Net => new()
    {
        AttrEntryId = AttrEntryId,
        AttrTypeId = AttrTypeId,
        Value = Value,
        UpgradeTimes = UpgradeTimes
    };
}