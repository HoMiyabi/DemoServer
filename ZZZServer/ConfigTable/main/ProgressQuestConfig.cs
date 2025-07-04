
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using Newtonsoft.Json.Linq;



namespace cfg.main
{

public abstract partial class ProgressQuestConfig : main.QuestConfig
{
    public ProgressQuestConfig(JToken _buf)  : base(_buf) 
    {
        JObject _obj = _buf as JObject;
        Count = (int)_obj.GetValue("count");
    }

    public static ProgressQuestConfig DeserializeProgressQuestConfig(JToken _buf)
    {
        var _obj=_buf as JObject;
        switch (_obj.GetValue("$type").ToString())
        {
            case "DefeatQuestConfig": return new main.DefeatQuestConfig(_buf);
            case "GatherQuestConfig": return new main.GatherQuestConfig(_buf);
            case "UpgradeQuestConfig": return new main.UpgradeQuestConfig(_buf);
            default: throw new SerializationException();
        }
    }

    public readonly int Count;



    public override void ResolveRef(Tables tables)
    {
        base.ResolveRef(tables);
    }

    public override string ToString()
    {
        return "{ "
        + "questCid:" + QuestCid + ","
        + "nextQuestCid:" + NextQuestCid + ","
        + "name:" + Name + ","
        + "desc:" + Desc + ","
        + "npcOverrides:" + Luban.StringUtil.CollectionToString(NpcOverrides) + ","
        + "completeGetQuestChainCid:" + CompleteGetQuestChainCid + ","
        + "trunkNpcCids:" + Luban.StringUtil.CollectionToString(TrunkNpcCids) + ","
        + "count:" + Count + ","
        + "}";
    }
}
}

