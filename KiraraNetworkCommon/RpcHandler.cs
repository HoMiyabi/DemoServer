﻿using System;
using Google.Protobuf;

namespace Kirara.Network
{
    public abstract class RpcHandler<TReq, TRsp> : IMsgHandler
        where TReq : IMessage where TRsp : IRsp, new()
    {
        public Type MsgType => typeof(TReq);

        public void Handle(Session session, IMsg msg, uint rpcSeq)
        {
            var rsp = new TRsp();
            bool isReply = false;

            void Reply()
            {
                if (isReply) return;
                isReply = true;

                session.Send(rsp);
            }

            try
            {
                Run(session, (TReq)msg, rsp, Reply);
            }
            catch (Exception e)
            {
                MyLog.Error(e.ToString());
            }
            finally
            {
                Reply();
            }
        }

        protected abstract void Run(Session session, TReq req, TRsp rsp, Action reply);
    }
}