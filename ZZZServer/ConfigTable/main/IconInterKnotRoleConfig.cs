
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

public sealed partial class IconInterKnotRoleConfig : Luban.BeanBase
{
    public IconInterKnotRoleConfig(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        Id = (int)_obj.GetValue("id");
        Location = (string)_obj.GetValue("location");
    }

    public static IconInterKnotRoleConfig DeserializeIconInterKnotRoleConfig(JToken _buf)
    {
        return new main.IconInterKnotRoleConfig(_buf);
    }

    public readonly int Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Location;


    public const int __ID__ = 414510446;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "location:" + Location + ","
        + "}";
    }
}
}

