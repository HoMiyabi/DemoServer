// using Fantasy;
// using Fantasy.Async;
// using Fantasy.Network;
// using Fantasy.Network.Interface;
//
// namespace Entity.Handler.Character;
//
// public class ReqSetCharacterDisc_Handler : RpcHandler<ReqSetCharacterDisc, RspSetCharacterDisc>
// {
//     protected override void Run(Session session, ReqSetCharacterDisc req, RspSetCharacterDisc rsp, Action reply)
//     {
//         // 只允许装备、卸下和替换为没有穿戴者的驱动盘
//         var player = PlayerService.sessionToLoggedData[session].player;
//
//         var ch = DbHelper.Db.CopyNew()
//             .Queryable<Entity.DbEntity.CharacterInfo>()
//             .First(x => x.Id == req.CharacterId && x.UId == player.UId);
//
//         if (ch == null)
//         {
//             rsp.Result.Code = 1;
//             rsp.Result.Msg = "角色不存在";
//             return;
//         }
//
//         if (req.DiscPos < 1 || req.DiscPos > 6)
//         {
//             rsp.Result.Code = 2;
//             rsp.Result.Msg = "驱动盘位置参数错误";
//             return;
//         }
//
//         int oldDiscId = ch.DiscIds[req.DiscPos - 1];
//         if (oldDiscId == req.NewDiscId)
//         {
//             rsp.Result.Code = 0;
//             rsp.Result.Msg = "已装备或已未装备";
//             return;
//         }
//
//         // 设置新武器的穿戴者
//         if (req.NewDiscId != 0)
//         {
//             var newDisc = DbHelper.Db.CopyNew()
//                 .Queryable<Entity.DbEntity.Disc>()
//                 .First(x => x.Id == req.NewDiscId && x.UId == player.UId);
//
//             if (newDisc == null)
//             {
//                 rsp.Result.Code = 3;
//                 rsp.Result.Msg = "驱动盘不存在";
//                 return;
//             }
//             if (newDisc.WearerId != 0)
//             {
//                 rsp.Result.Code = 4;
//                 rsp.Result.Msg = "驱动盘已被装备";
//                 return;
//             }
//             if (newDisc.Pos != req.DiscPos)
//             {
//                 rsp.Result.Code = 5;
//                 rsp.Result.Msg = "驱动盘位置错误";
//                 return;
//             }
//
//             newDisc.WearerId = ch.Id;
//             DbHelper.Db.CopyNew().Updateable(newDisc).ExecuteCommand();
//         }
//
//         // 旧武器清除穿戴者
//         if (oldDiscId != 0)
//         {
//             var oldDisc = DbHelper.Db.CopyNew()
//                 .Queryable<Entity.DbEntity.Disc>()
//                 .InSingle(oldDiscId);
//             oldDisc.WearerId = 0;
//             DbHelper.Db.CopyNew().Updateable(oldDisc).ExecuteCommand();
//         }
//
//         // 设置角色的新驱动盘
//         ch.DiscIds[req.DiscPos - 1] = req.NewDiscId;
//         DbHelper.Db.CopyNew().Updateable(ch).ExecuteCommand();
//     }
// }