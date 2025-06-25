using ZZZServer.Model;

namespace ZZZServer.Service;

public static class InventoryService
{
    public static void AddMaterialCount(Player player, int cid, int count)
    {
        var material = player.Materials.Find(x => x.Cid == cid);
        if (material != null)
        {
            material.Count += count;
        }
        else
        {
            player.Materials.Add(new MaterialItem()
            {
                Cid = cid,
                Count = count
            });
        }
    }

    public static int GetCurrencyCount(Player player, int cid)
    {
        var item = player.Currencies.Find(it => it.Cid == cid);
        return item != null ? item.Count : 0;
    }

    public static void AddCurrencyCount(Player player, int cid, int count)
    {
        var item = player.Currencies.Find(x => x.Cid == cid);
        if (item != null)
        {
            item.Count += count;
        }
        else
        {
            player.Currencies.Add(new CurrencyItem
            {
                Cid = cid,
                Count = count
            });
        }
    }
}