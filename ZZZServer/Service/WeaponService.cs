using ZZZServer.MongoDocEntity;

namespace ZZZServer.Service;

public static class WeaponService
{
    public static WeaponItem GachaWeapon()
    {
        var weapon = new WeaponItem
        {
            Cid = Random.Shared.Next(1, 5),
            Level = Random.Shared.Next(1, 61),
            Exp = Random.Shared.Next(0, 100),
            RoleId = null,
            Locked = Random.Shared.Next(0, 2) == 0,
            RefineLevel = Random.Shared.Next(1, 6),
            BaseAttr = null,
            AdvancedAttr = null
        };

        var config = ConfigMgr.tb.TbWeaponConfig[weapon.Cid];

        int baseAttrTypeId = (int)config.MaxLevelBaseAttrType;
        weapon.BaseAttr = new WeaponAttr
        {
            AttrTypeId = baseAttrTypeId,
            Value = config.MaxLevelBaseAttrValue
        };

        int advancedAttrTypeId = (int)config.MaxLevelAdvancedAttrType;
        weapon.AdvancedAttr = new WeaponAttr
        {
            AttrTypeId = advancedAttrTypeId,
            Value = config.MaxLevelAdvancedAttrValue
        };

        return weapon;
    }
}