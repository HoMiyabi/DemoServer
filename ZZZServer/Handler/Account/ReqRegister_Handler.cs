using Kirara.Network;
using MongoDB.Driver;
using ZZZServer.Model;
using ZZZServer.Service;

namespace ZZZServer.Handler;

public class ReqRegister_Handler : RpcHandler<ReqRegister, RspRegister>
{
    protected override void Run(Session session, ReqRegister req, RspRegister rsp, Action reply)
    {
        string username = req.Username;
        string password = req.Password;

        if (username.Length < 6)
        {
            rsp.Result = new Result { Code = 1, Msg = "用户名长度不能小于6位" };
            return;
        }

        if (password.Length < 6)
        {
            rsp.Result = new Result { Code = 2, Msg = "密码长度不能小于6位" };
            return;
        }

        var db = DbMgr.Database;
        var players = db.GetCollection<Player>("player");
        bool exist = players.Find(x => x.Username == username).Any();
        if (exist)
        {
            rsp.Result = new Result { Code = 3, Msg = "用户名已存在" };
            return;
        }

        var player = PlayerService.CreatePlayer(username, password);
        players.InsertOne(player);

        rsp.Result = new Result { Code = 0, Msg = "注册成功" };
    }
}