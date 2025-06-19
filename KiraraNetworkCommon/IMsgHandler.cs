using System;
using Google.Protobuf;

namespace Kirara.Network
{
    public interface IMsgHandler
    {
        Type MsgType { get; }
        void Handle(Session session, IMsg msg, uint rpcSeq);
    }
}