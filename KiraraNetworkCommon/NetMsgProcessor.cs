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
        private int interval = 20;
        private long prevTime = 0;
        private readonly ConcurrentDictionary<string, Action<float>> updates = new();
        private readonly ConcurrentQueue<Action> taskQueue = new();
        public static NetMsgProcessor Instance { get; private set; }

        public bool AddUpdate(string key, Action<float> update)
        {
            return updates.TryAdd(key, update);
        }

        public void Start()
        {
            if (isWorking) return;
            Instance = this;

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

        public void EnqueueTask(Action task)
        {
            if (task == null) return;
            taskQueue.Enqueue(task);
        }

        private void Update()
        {
            while (queue.TryDequeue(out var item))
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

            while (taskQueue.TryDequeue(out var task))
            {
                try
                {
                    task();
                }
                catch (Exception e)
                {
                    MyLog.Error("处理任务异常 " + e);
                }
            }

            // Tick驱动
            long currTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (prevTime == 0)
            {
                prevTime = currTime;
            }
            else
            {
                while (currTime - prevTime >= interval)
                {
                    foreach (var update in updates)
                    {
                        try
                        {
                            update.Value(interval / 1000f);
                        }
                        catch (Exception e)
                        {
                            MyLog.Error("Tick更新异常: " + e);
                        }
                    }
                    prevTime += interval;
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