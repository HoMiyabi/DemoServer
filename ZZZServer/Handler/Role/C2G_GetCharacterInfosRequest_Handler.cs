// using Kirara.Network;
// using ZZZServer.Service;
//
// namespace ZZZServer.Handler.Character;
//
// public class ReqGetCharacterInfos_Handler : RpcHandler<ReqGetCharacterInfos, RspGetCharacterInfos>
// {
//     protected override void Run(Session session, ReqGetCharacterInfos req, RspGetCharacterInfos rsp, Action reply)
//     {
//         var player = (Player)session.Data;
//         var list = DbHelper.Db.CopyNew()
//             .Queryable<ZZZServer.DbEntity.CharacterInfo>()
//             .Where(it => it.UId == player.UId)
//             .ToList();
//         rsp.Result.Code = 0;
//         rsp.Result.Msg = "获取角色信息成功";
//         rsp.CharacterInfos.Add(list.Select(it => it.Net()));
//     }
//
//     // private NWeaponItem? GetWeaponItem(int weaponId)
//     // {
//     //     if (weaponId == 0) return new NWeaponItem();
//     //
//     //     var weaponDO = DbHelper.Db.Queryable<Weapon>().InSingle(weaponId);
//     //
//     //     return weaponDO.Net();
//     // }
//     //
//     // private NDiscItem? GetDiscItem(int discId)
//     // {
//     //     if (discId == 0) return new NDiscItem();
//     //     var discDO = DbHelper.Db.Queryable<Disc>().InSingle(discId);
//     //     return discDO.Net();
//     // }
// }