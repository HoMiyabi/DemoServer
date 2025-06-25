using ZZZServer.Model;
using ZZZServer.Utils;

namespace ZZZServer.Service;

public static class DiscService
{
    public static DiscItem GachaDisc()
    {
        var disc = new DiscItem
        {
            Cid = Random.Shared.Next(1, ConfigMgr.tb.TbDiscConfig.DataList.Count + 1),
            Level = 0,
            Exp = 0,
            RoleId = null,
            Locked = Random.Shared.Next(0, 2) == 0,
            Pos = Random.Shared.Next(1, 7),
            MainAttr = null,
            SubAttrs = null
        };

        // 主词条
        disc.MainAttr = new DiscAttr();

        disc.MainAttr.AttrEntryId = ConfigMgr.tb.TbDiscAttrEntryAppearConfig[disc.Pos]
            .AttrEntryIds.RandomItem();
        var mainEntryConfig = ConfigMgr.tb.TbDiscAttrEntryConfig[disc.MainAttr.AttrEntryId];

        disc.MainAttr.AttrTypeId = (int)mainEntryConfig.AttrType;
        disc.MainAttr.Value = mainEntryConfig.MainValue;

        // 副词条
        int subAttrCount = Random.Shared.Next(3, 5);

        disc.SubAttrs = new List<DiscAttr>(subAttrCount);

        for (int i = 0; i < subAttrCount; i++)
        {
            DiscAddSubAttr(disc);
        }

        return disc;
    }

    private static void DiscAddSubAttr(DiscItem disc)
    {
        var subAttr = new DiscAttr();

        var attrEntryAppearConfig = ConfigMgr.tb.TbDiscAttrEntryAppearConfig[7];
        var attrEntryIds = attrEntryAppearConfig.AttrEntryIds;

        var availableEntryIds = attrEntryIds
            .Where(entryId => entryId != disc.MainAttr.AttrEntryId && disc.SubAttrs
                .Find(it => it.AttrEntryId == entryId) == null)
            .ToList();

        subAttr.AttrEntryId = availableEntryIds.RandomItem();

        var subEntryConfig = ConfigMgr.tb.TbDiscAttrEntryConfig[subAttr.AttrEntryId];
        subAttr.AttrTypeId = (int)subEntryConfig.AttrType;
        subAttr.Value = subEntryConfig.SubValue;

        disc.SubAttrs.Add(subAttr);
    }

    public static void UpgradeDiscSubEntry(DiscItem disc)
    {
        if (disc.SubAttrs.Count == 3)
        {
            DiscAddSubAttr(disc);
        }
        else
        {
            int subAttrIdx = Random.Shared.Next(0, 4);
            var subAttr = disc.SubAttrs[subAttrIdx];

            var attrEntryConfig = ConfigMgr.tb.TbDiscAttrEntryConfig[subAttr.AttrEntryId];
            subAttr.Value += attrEntryConfig.SubValue;
            subAttr.UpgradeTimes++;
        }
    }

    private const int maxLevel = 15;
    private static readonly List<int> upgradeLevel = [3, 6, 9, 12, 15];

    public static void UpgradeDisc(DiscItem disc, int exp)
    {
        disc.Exp += exp;

        while (disc.Level < maxLevel)
        {
            var config = ConfigMgr.tb.TbDiscUpgradeExpConfig[disc.Level + 1];
            if (disc.Exp >= config.Exp)
            {
                disc.Exp -= config.Exp;
                disc.Level++;
                if (upgradeLevel.Contains(disc.Level))
                {
                    UpgradeDiscSubEntry(disc);
                }
            }
            else
            {
                break;
            }
        }
        if (disc.Level >= maxLevel)
        {
            disc.Exp = 0;
        }
    }
}