/*using SqlSugar;

namespace ZZZServer.DbEntity;

public class ChatMsgRecord
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
    public int SmallUId { get; set; }
    public int BigUId { get; set; }
    public int SenderUId { get; set; }
    public long EpochMs { get; set; }
    public int Type { get; set; }
    public string? Text { get; set; }
    public int StickerConfigId { get; set; }
}*/