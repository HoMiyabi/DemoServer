// using Fantasy;
// using Fantasy.Async;
// using Fantasy.Network;
// using Fantasy.Network.Interface;
//
// namespace Entity.Handler.Disc;
//
// public class ReqCharacterReplaceDisc_Handler : RpcHandler<ReqCharacterReplaceDisc, RspCharacterReplaceDisc>
// {
//     protected override void Run(Session session, ReqCharacterReplaceDisc req, RspCharacterReplaceDisc rsp,
//         Action reply)
//     {
//         var player = PlayerService.sessionToLoggedData[session].player;
//
//         if (req.DiscPos < 1 || req.DiscPos > 6)
//         {
//             rsp.Result.Code = 1;
//             rsp.Result.Msg = "驱动盘位置参数错误";
//             return;
//         }
//
//         var chA = DbHelper.Db.CopyNew()
//             .Queryable<Entity.DbEntity.CharacterInfo>()
//             .First(x => x.Id == req.CharacterId && x.UId == player.UId);
//
//         if (chA == null)
//         {
//             rsp.Result.Code = 2;
//             rsp.Result.Msg = "角色不存在";
//             return;
//         }
//
//         var discA = DbHelper.Db.CopyNew().Queryable<Entity.DbEntity.Disc>()
//             .First(x => x.Id == chA.DiscIds[req.DiscPos - 1] && x.UId == player.UId);
//         if (discA == null)
//         {
//             rsp.Result.Code = 5;
//             rsp.Result.Msg = "驱动盘不存在";
//             return;
//         }
//
//         var discB = DbHelper.Db.CopyNew().Queryable<Entity.DbEntity.Disc>()
//             .First(x => x.Id == req.NewDiscId && x.UId == player.UId);
//         if (discB == null)
//         {
//             rsp.Result.Code = 4;
//             rsp.Result.Msg = "驱动盘不存在";
//             return;
//         }
//
//         var chB = DbHelper.Db.CopyNew().Queryable<Entity.DbEntity.CharacterInfo>()
//             .First(x => x.Id == discB.WearerId && x.UId == player.UId);
//         if (chB == null)
//         {
//             rsp.Result.Code = 3;
//             rsp.Result.Msg = "角色不存在";
//             return;
//         }
//
//         (discA.WearerId, discB.WearerId) = (discB.WearerId, discA.WearerId);
//         (chA.DiscIds[req.DiscPos - 1], chB.DiscIds[req.DiscPos - 1]) =
//             (chB.DiscIds[req.DiscPos - 1], chA.DiscIds[req.DiscPos - 1]);
//         DbHelper.Db.CopyNew().Updateable(discA).ExecuteCommand();
//         DbHelper.Db.CopyNew().Updateable(discB).ExecuteCommand();
//         DbHelper.Db.CopyNew().Updateable(chA).ExecuteCommand();
//         DbHelper.Db.CopyNew().Updateable(chB).ExecuteCommand();
//     }
// }