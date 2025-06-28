using System;
using System.Collections.Concurrent;
using System.Threading;
using Google.Protobuf;

namespace Kirara.Network
{
    public class NetMsgProcessor
    {
        private readonly ConcurrentQueue<(Session session, uint cmdId, uint rpcSeq, IMessage msg)> queue = new();
        private bool isWorking;

        public void Start()
        {
            if (isWorking) return;

            isWorking = true;
            var thread = new Thread(Work)
            {
                Name = "NetMsgProcessor"
            };
            thread.Start();
        }

        public void Stop()
        {
            isWorking = false;
        }

        private void Work()
        {
            while (isWorking)
            {
                Update();
            }
        }

        public void Enqueue(Session session, uint cmdId, uint rpcSeq, IMessage msg)
        {
            queue.Enqueue((session, cmdId, rpcSeq, msg));
        }

        private void Update()
        {
            if (queue.TryDequeue(out var item))
            {
                try
                {
                    ProcessMsg(item.session, item.cmdId, item.rpcSeq, item.msg);
                }
                catch (Exception e)
                {
                    MyLog.Error("处理消息异常 " + e);
                }
            }
        }

        private void ProcessMsg(Session session, uint cmdId, uint rpcSeq, IMessage msg)
        {
            if (KiraraNetwork.MsgMeta.IsRsp(cmdId))
            {
                if (KiraraNetwork.RpcCallbacks.TryRemove(rpcSeq, out var callback))
                {
                    callback?.Invoke(msg);
                }
                else
                {
                    MyLog.Debug($"RPC回调未找到. CmdId: {cmdId}, RpcSeq: {rpcSeq}");
                }
            }
            else
            {
                if (KiraraNetwork.Handlers.TryGetValue(cmdId, out var handler))
                {
                    handler.Handle(session, msg, rpcSeq);
                }
                else
                {
                    MyLog.Debug($"没有处理方法，CmdId: {cmdId}");
                }
            }
        }
    }
}