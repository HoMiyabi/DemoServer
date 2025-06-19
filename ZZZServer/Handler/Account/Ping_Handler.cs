using Kirara.Network;

namespace ZZZServer.Handler.Account;

public class Ping_Handler : RpcHandler<Ping, Pong>
{
    protected override void Run(Session session, Ping req, Pong rsp, Action reply)
    {
        rsp.UnixTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}