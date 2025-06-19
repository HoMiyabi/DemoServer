// using Kirara.Network;
// using ZZZServer.MongoDoc;
//
// namespace ZZZServer.Handler;
//
// public class ReqGetWeaponItems_Handler : RpcHandler<ReqGetWeaponItems, RspGetWeaponItems>
// {
//     protected override void Run(Session session, ReqGetWeaponItems req, RspGetWeaponItems rsp, Action reply)
//     {
//         var player = (Player)session.Data;
//         var arr = DbHelper.Db.CopyNew().Queryable<DbWeapon>()
//             .Where(it => it.UId == player.UId).ToArray();
//         rsp.Items.Add(arr.Select(it => it.Net()));
//     }
// }