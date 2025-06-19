using System;
using Google.Protobuf;

namespace Kirara.Network
{
    public abstract class MsgHandler<TMsg> : IMsgHandler where TMsg : IMsg
    {
        public Type MsgType => typeof(TMsg);

        public void Handle(Session session, IMsg msg, uint rpcSeq)
        {
            try
            {
                Run(session, (TMsg)msg);
            }
            catch (Exception e)
            {
                MyLog.Error(e.ToString());
            }
        }

        protected abstract void Run(Session session, TMsg msg);
    }
}