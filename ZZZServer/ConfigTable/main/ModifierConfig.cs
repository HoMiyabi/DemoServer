
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

public sealed partial class ModifierConfig : Luban.BeanBase
{
    public ModifierConfig(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        AttrType = (main.EAttrType)(int)_obj.GetValue("attr_type");
        DeltaValue = (float)_obj.GetValue("delta_value");
    }

    public static ModifierConfig DeserializeModifierConfig(JToken _buf)
    {
        return new main.ModifierConfig(_buf);
    }

    public readonly main.EAttrType AttrType;
    public readonly float DeltaValue;


    public const int __ID__ = -895071762;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "attrType:" + AttrType + ","
        + "deltaValue:" + DeltaValue + ","
        + "}";
    }
}
}

