// using Kirara.Network;
//
// namespace ZZZServer.Handler;
//
// public class ReqGetDiscItems_Handler : RpcHandler<ReqGetDiscItems, RspGetDiscItems>
// {
//     protected override void Run(Session session, ReqGetDiscItems req, RspGetDiscItems rsp, Action reply)
//     {
//         if (req.Pos is < 0 or > 6)
//         {
//             rsp.Result.Code = 1;
//             rsp.Result.Msg = "驱动盘位置参数错误";
//             return;
//         }
//         var player = (Player)session.Data;
//
//         DbDisc[] arr;
//
//         if (req.Pos == 0)
//         {
//             arr = DbHelper.Db.CopyNew()
//                 .Queryable<DbDisc>()
//                 .Where(it => it.UId == player.UId)
//                 .ToArray();
//         }
//         else
//         {
//             arr = DbHelper.Db.CopyNew()
//                 .Queryable<DbEntity.DbDisc>()
//                 .Where(it => it.UId == player.UId && it.Pos == req.Pos)
//                 .ToArray();
//         }
//         rsp.Items.Add(arr.Select(it => it.Net()));
//     }
// }