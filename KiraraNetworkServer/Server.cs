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

        public List<Session> sessions = new();

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

            MyLog.Debug($"服务器启动 监听在{endPoint}");
            AppDomain.CurrentDomain.ProcessExit += OnCancelKeyPress;
            Console.CancelKeyPress += OnCancelKeyPress;
            while (_isRunning)
            {
                Thread.Sleep(1);
                CheckSessionTimeout();
            }
        }

        private void OnCancelKeyPress(object sender, EventArgs e)
        {
            MyLog.Debug("Bye~");
            Stop();
            _isRunning = false;
        }

        private void Stop()
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
                sessions.Add(session);
            }
        }

        private void CheckSessionTimeout()
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

