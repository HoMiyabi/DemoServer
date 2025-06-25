using Kirara.Network;
using MongoDB.Driver;
using Serilog;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class ReqLogin_Handler : RpcHandler<ReqLogin, RspLogin>
{
    protected override void Run(Session session, ReqLogin req, RspLogin rsp, Action reply)
    {
        var db = DbHelper.Database;
        var players = db.GetCollection<Player>("player");
        var player = players.Find(x => x.Username == req.Username).FirstOrDefault();

        if (player == null || player.Password != req.Password)
        {
            rsp.Result.Code = 1;
            rsp.Result.Msg = "用户名或密码错误";
            return;
        }

        if (PlayerService.UidToPlayer.ContainsKey(player.Uid))
        {
            rsp.Result.Code = 2;
            rsp.Result.Msg = "用户已登录";
            return;
        }

        string uid = player.Uid;
        session.Data = player;
        PlayerService.UidToPlayer.TryAdd(uid, player);
        session.OnDisconnected += () =>
        {
            session.Data = null;
            if (PlayerService.UidToPlayer.Remove(uid, out var p))
            {
                Log.Debug($"玩家离开 UId={uid}");
                PlayerService.SavePlayer(p);
            }
        };
    }
}