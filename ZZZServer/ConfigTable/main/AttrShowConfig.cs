
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

public sealed partial class AttrShowConfig : Luban.BeanBase
{
    public AttrShowConfig(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        AttrType = (main.EAttrType)(int)_obj.GetValue("attr_type");
        ShowName = (string)_obj.GetValue("show_name");
        ShowPct = (bool)_obj.GetValue("show_pct");
    }

    public static AttrShowConfig DeserializeAttrShowConfig(JToken _buf)
    {
        return new main.AttrShowConfig(_buf);
    }

    public readonly main.EAttrType AttrType;
    public readonly string ShowName;
    public readonly bool ShowPct;


    public const int __ID__ = -175658395;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "attrType:" + AttrType + ","
        + "showName:" + ShowName + ","
        + "showPct:" + ShowPct + ","
        + "}";
    }
}
}

