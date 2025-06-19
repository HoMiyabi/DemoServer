/*using SqlSugar;
using ZZZServer.Utils;

namespace ZZZServer.DbEntity;

[SugarTable("player")]
public class DbPlayer
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int UId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }

    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }

    public int AvatarConfigId { get; set; }

    public string Signature { get; set; }

    [SugarColumn(IsJson = true)]
    public List<int> FriendUIds { get; set; }

    [SugarColumn(IsJson = true)]
    public List<int> FriendRequestUIds { get; set; }

    [SugarColumn(IsJson = true, Length = 1000)]
    public Dictionary<int, MaterialItemDO> MaterialItems { get; set; }

    [SugarColumn(IsJson = true, Length = 1000)]
    public Dictionary<int, CurrencyItemDO> CurrencyItems { get; set; }

    [SugarColumn(IsJson = true, Length = 100)]
    public List<int> GroupChCids { get; set; }

    public int FrontChIdx { get; set; }

    public NPlayerInfo Net()
    {
        var ret = new NPlayerInfo
        {
            UId = UId,
            Username = Username,
            Signature = Signature,
            AvatarCid = AvatarConfigId,
            FrontChIdx = FrontChIdx,
        };
        ret.FriendUIds.Add(FriendUIds);
        ret.FriendRequestUIds.Add(FriendRequestUIds);
        ret.MatItems.Add(MaterialItems.Values.Select(it =>it.Net()));
        ret.CurItems.Add(CurrencyItems.Values.Select(it =>it.Net()));
        ret.GroupChCids.Add(GroupChCids);

        return ret;
    }

    public NRoomSimPlayerInfo NRoomSimPlayerInfo()
    {
        var ret = new NRoomSimPlayerInfo
        {
            UId = UId,
            PosRot = new NPosRot()
            {
                Pos = new NFloat3().CopyPosFrom(this),
                Rot = new NFloat3().CopyRotFrom(this)
            },
            FrontChIdx = FrontChIdx,
        };
        ret.GroupChCids.Add(GroupChCids);
        return ret;
    }
}*/