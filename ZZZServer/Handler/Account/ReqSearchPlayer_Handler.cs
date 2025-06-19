using Kirara.Network;
using ZZZServer.MongoDocEntity;

namespace ZZZServer.Handler.Account;

public class ReqSearchPlayer_Handler : RpcHandler<ReqSearchPlayer, RspSearchPlayer>
{
    protected override void Run(Session session, ReqSearchPlayer req, RspSearchPlayer rsp, Action reply)
    {
        var player = (Player)session.Data;
        if (req.Username == player.Username)
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "不能添加自己";
            return;
        }

        var target = DbHelper.Db.CopyNew().Queryable<DbEntity.DbPlayer>()
            .First(x => x.Username == req.Username);
        if (target == null)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "找不到";
            return;
        }

        rsp.OtherPlayerInfos = new NOtherPlayer
        {
            UId = target.UId,
            Username = target.Username,
            Signature = target.Signature,
            AvatarConfigId = target.AvatarConfigId,
            IsOnline = false,
        };
        rsp.Result.Msg = "查询成功";
    }
}