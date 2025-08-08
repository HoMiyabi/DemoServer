using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Kirara.Network
{
    public class Server
    {
        private readonly Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly NetMsgProcessor msgProcessor = new();
        private bool _isRunning;

        private readonly List<Session> sessions = [];
        public event Action OnBeforeClose;

        public void Run(IPEndPoint endPoint)
        {
            if (_isRunning)
            {
                MyLog.Debug("服务器已启动");
                return;
            }
            _isRunning = true;

            msgProcessor.Start();

            socket.Bind(endPoint);
            socket.Listen();

            _ = AcceptAsync();

            Task.Run(LoopCheckSessionTimeout);
            MyLog.Debug($"服务器启动, 监听在{endPoint}, 按C退出");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.C)
                {
                    OnBeforeClose?.Invoke();
                    Close();
                    _isRunning = false;
                    MyLog.Debug("服务器已停止");
                    break;
                }
            }
        }

        public void AddMsgProcessorUpdate(string key, Action<float> update)
        {
            msgProcessor.AddUpdate(key, update);
        }

        private void Close()
        {
            msgProcessor.Stop();
            socket.Close();
        }

        private async Task AcceptAsync()
        {
            while (_isRunning)
            {
                var client = await socket.AcceptAsync();
                MyLog.Debug($"客户端{client.RemoteEndPoint}连接");
                var session = new Session(client, msgProcessor);
                _ = session.ReceiveAsync();
                lock (session)
                {
                    sessions.Add(session);
                }
            }
        }

        private void LoopCheckSessionTimeout()
        {
            while (_isRunning)
            {
                Thread.Sleep(1000);
                CheckSessionTimeout();
            }
        }

        private void CheckSessionTimeout()
        {
            lock (sessions)
            {
                for (int i = 0; i < sessions.Count;)
                {
                    if (sessions[i].isClosed)
                    {
                        sessions.RemoveAt(i);
                    }
                    else if (sessions[i].CheckTimeout())
                    {
                        MyLog.Debug($"会话超时: {sessions[i]._socket.RemoteEndPoint}");
                        sessions[i].Close();
                        sessions.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }
}

