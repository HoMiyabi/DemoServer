using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Google.Protobuf;

namespace Kirara.Network
{
    public class NetMessageWorker
    {
        private static Dictionary<uint, IMsgHandler> handlers;
        public readonly Dictionary<uint, Action<IMessage>> rspCallbacks = new();

        private readonly ConcurrentQueue<(Session session, uint cmdId, uint rpcSeq, IMessage msg)> queue = new();
        private bool isWorking;

        public static void Scan(Assembly assembly)
        {
            handlers = new Dictionary<uint, IMsgHandler>();

            var iMsgHandlerType = typeof(IMsgHandler);

            foreach (var type in assembly.GetTypes())
            {
                if (iMsgHandlerType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var handler = (IMsgHandler)Activator.CreateInstance(type);
                    if (handler == null)
                    {
                        throw new Exception($"{type.FullName}不能实例化");
                    }
                    var msgType = type.BaseType.GenericTypeArguments[0];
                    if (KiraraNetwork.MsgMeta.TryGetCmdId(msgType, out uint cmdId))
                    {
                        handlers.Add(cmdId, handler);
                    }
                    else
                    {
                        throw new Exception($"{type.FullName}找不到CmdId");
                    }
                }
            }
        }

        public void Start()
        {
            if (isWorking) return;

            isWorking = true;
            var thread = new Thread(Work)
            {
                Name = "NetMessageWorker"
            };
            thread.Start();
        }

        public void Stop()
        {
            isWorking = false;
        }

        public void Enqueue(Session session, uint cmdId, uint rpcSeq, IMessage msg)
        {
            queue.Enqueue((session, cmdId, rpcSeq, msg));
        }

        private void Work()
        {
            while (isWorking)
            {
                if (queue.TryDequeue(out var item))
                {
                    (var session, uint cmdId, uint rpcSeq, var msg) = item;
                    if (KiraraNetwork.MsgMeta.IsRsp(cmdId))
                    {
                        if (rspCallbacks.Remove(rpcSeq, out var callback))
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
                        if (handlers.TryGetValue(cmdId, out var handler))
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
    }
}