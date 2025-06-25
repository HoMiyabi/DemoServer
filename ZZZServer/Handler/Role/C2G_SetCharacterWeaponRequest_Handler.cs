// using Kirara.Network;
// using ZZZServer.Model;
// using ZZZServer.Service;
//
// namespace ZZZServer.Handler.Role;
//
// public class ReqSetRoleWeapon_Handler : RpcHandler<ReqSetRoleWeapon, RspSetRoleWeapon>
// {
//     protected override void Run(Session session, ReqSetRoleWeapon req, RspSetRoleWeapon rsp,
//         Action reply)
//     {
//         // 只允许装备、卸下和替换为没有穿戴者的武器
//         var player = (Player)session.Data;
//
//         var ch = DbHelper.Db.CopyNew()
//             .Queryable<ZZZServer.DbEntity.CharacterInfo>()
//             .First(x => x.Id == req.CharacterId && x.UId == player.UId);
//
//         if (ch == null)
//         {
//             rsp.Result.Code = 1;
//             rsp.Result.Msg = "角色不存在";
//             return;
//         }
//
//         if (ch.WeaponId == req.NewWeaponId)
//         {
//             rsp.Result.Code = 0;
//             rsp.Result.Msg = "角色已装备该武器";
//             return;
//         }
//
//         // 设置新武器的穿戴者
//         if (req.NewWeaponId != 0)
//         {
//             var newWeapon = DbHelper.Db.CopyNew()
//                 .Queryable<ZZZServer.DbEntity.DbWeapon>()
//                 .First(x => x.Id == req.NewWeaponId && x.UId == player.UId);
//
//             if (newWeapon == null)
//             {
//                 rsp.Result.Code = 2;
//                 rsp.Result.Msg = "武器不存在";
//                 return;
//             }
//             if (newWeapon.WearerId != 0)
//             {
//                 rsp.Result.Code = 3;
//                 rsp.Result.Msg = "武器已被装备";
//                 return;
//             }
//
//             newWeapon.WearerId = ch.Id;
//             DbHelper.Db.CopyNew().Updateable(newWeapon).ExecuteCommand();
//         }
//
//         // 旧武器清除穿戴者
//         if (ch.WeaponId != 0)
//         {
//             var oldWeapon = DbHelper.Db.CopyNew()
//                 .Queryable<ZZZServer.DbEntity.DbWeapon>()
//                 .InSingle(ch.WeaponId);
//             oldWeapon.WearerId = 0;
//             DbHelper.Db.CopyNew().Updateable(oldWeapon).ExecuteCommand();
//         }
//
//         // 设置角色的武器
//         ch.WeaponId = req.NewWeaponId;
//         DbHelper.Db.CopyNew().Updateable(ch).ExecuteCommand();
//     }
// }