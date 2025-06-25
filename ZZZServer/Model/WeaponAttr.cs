namespace ZZZServer.Model;

public class WeaponAttr
{
    public int AttrTypeId { get; set; }
    public float Value { get; set; }

    public NWeaponAttr Net()
    {
        return new NWeaponAttr
        {
            AttrTypeId = AttrTypeId,
            Value = Value
        };
    }
}