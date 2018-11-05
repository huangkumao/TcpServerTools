using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TcpServer
{
    //TcpListener实现异步TCP服务器
    public class AsyncTCPServer : IDisposable
    {
        //客户端会话列表
        private readonly Dictionary<long, TCPClientSession> m_Clients;

        private bool disposed;

        //客户端ID(自增)
        private long m_ClientID = 1000;

        //服务器使用的异步TcpListener
        private TcpListener m_Listener;

        //构造器
        public AsyncTCPServer(int listenPort)
            : this(IPAddress.Any, listenPort)
        {
        }

        public AsyncTCPServer(IPEndPoint localEP)
            : this(localEP.Address, localEP.Port)
        {
        }

        public AsyncTCPServer(IPAddress localIPAddress, int listenPort)
        {
            Address = localIPAddress;
            Port = listenPort;

            m_Clients = new Dictionary<long, TCPClientSession>(32);

            m_Listener = new TcpListener(Address, Port);
            m_Listener.Server.SendBufferSize = NetworkDefine.SocketSendBufSize;
            m_Listener.Server.ReceiveBufferSize = NetworkDefine.SocketReceBufSize;
            m_Listener.Server.NoDelay = NetworkDefine.UseNoDelay;
            m_Listener.AllowNatTraversal(true);
        }

        //当前的连接的客户端数
        public int m_ClientCount { get; private set; }

        //服务器是否正在运行
        public bool IsRunning { get; private set; }

        //监听的IP地址
        public IPAddress Address { get; }

        //监听的端口
        public int Port { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //启动服务器
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                m_Listener.Start();
                m_Listener.BeginAcceptTcpClient(HandleTcpClientAccepted, m_Listener);
            }
        }

        //启动服务器
        public void Start(int backlog)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                m_Listener.Start(backlog);
                m_Listener.BeginAcceptTcpClient(HandleTcpClientAccepted, m_Listener);
            }
        }

        //停止服务器
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                m_Listener.Stop();
                lock (m_Clients)
                {
                    //关闭所有客户端连接
                    CloseAllClient();
                }
            }
        }

        //处理客户端连接的函数
        private void HandleTcpClientAccepted(IAsyncResult ar)
        {
            if (IsRunning)
            {
                var _Client = m_Listener.EndAcceptTcpClient(ar);
                if (m_ClientCount > NetworkDefine.MaxConnect)
                {
                    //TODO
                    _Client.Close();
                    return;
                }

                var _Session = new TCPClientSession(_Client, m_ClientID++);
                lock (m_Clients)
                {
                    m_ClientCount++;
                    m_Clients.Add(_Session.ClientID, _Session);
                    OnClientConnected(_Session);
                }

                var _Stream = _Session.NetworkStream;
                //开始异步读取数据
                _Stream.BeginRead(_Session.Buffer, 0, _Session.Buffer.Length, HandleDataReceived, _Session);

                m_Listener.BeginAcceptTcpClient(HandleTcpClientAccepted, ar.AsyncState);
            }
        }

        //数据接受回调函数
        private void HandleDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                var _Session = (TCPClientSession) ar.AsyncState;
                var _Stream = _Session.NetworkStream;

                var _RecvSize = 0;
                try
                {
                    _RecvSize = _Stream.EndRead(ar);
                    if (_RecvSize == 0) //已经断开
                        lock (m_Clients)
                        {
                            m_Clients.Remove(_Session.ClientID);
                            //触发客户端连接断开事件
                            OnClientDisconnected(_Session);
                            return;
                        }

                    //保存消息并继续接受
                    _Session.SaveMsgToBuffer(_RecvSize);
                    _Stream.BeginRead(_Session.Buffer, 0, _Session.Buffer.Length, HandleDataReceived, _Session);
                }
                catch
                {
                    lock (m_Clients)
                    {
                        m_Clients.Remove(_Session.ClientID);
                        //触发客户端连接断开事件
                        OnClientDisconnected(_Session);
                        return;
                    }
                }

                //触发数据收到事件
                OnDataRecevied(_Session);
            }
        }

        //发送数据
        public void Send(TCPClientSession session, byte[] data)
        {
            Send(session.TcpClient, data, data.Length);
        }

        public void Send(TCPClientSession session, byte[] data, int pLen)
        {
            Send(session.TcpClient, data, pLen);
        }

        //异步发送数据至指定的客户端
        public void Send(TcpClient client, byte[] data, int pLen)
        {
            if (!IsRunning)
                throw new InvalidProgramException("This TCP Scoket server has not been started.");

            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            client.GetStream().BeginWrite(data, 0, pLen, SendDataEnd, client);
        }

        //发送数据完成处理函数
        private void SendDataEnd(IAsyncResult ar)
        {
            ((TcpClient) ar.AsyncState).GetStream().EndWrite(ar);
            OnCompletedSend(null);
        }

        //连接建立事件
        public event EventHandler<TCPClientSession> ClientConnected;

        private void OnClientConnected(TCPClientSession session)
        {
            ClientConnected?.Invoke(this, session);
        }

        //连接断开事件
        public event EventHandler<TCPClientSession> ClientDisconnected;

        private void OnClientDisconnected(TCPClientSession session)
        {
            m_ClientCount--;
            session.EventMsg = "连接断开";
            ClientDisconnected?.Invoke(this, session);
        }

        //接收到数据事件
        public event EventHandler<TCPClientSession> DataRecevied;

        private void OnDataRecevied(TCPClientSession session)
        {
            DataRecevied?.Invoke(this, session);
        }

        //数据发送完毕事件
        public event EventHandler<TCPClientSession> CompletedSend;

        private void OnCompletedSend(TCPClientSession session)
        {
            CompletedSend?.Invoke(this, session);
        }

        //关闭一个与客户端之间的会话
        public void Close(TCPClientSession pSession, bool pRemove = true)
        {
            if (pSession != null)
            {
                pSession.Close();
                if (pRemove)
                    m_Clients.Remove(pSession.ClientID);
                m_ClientCount--;
            }
        }

        //关闭所有的客户端会话,与所有的客户端连接会断开
        public void CloseAllClient()
        {
            foreach (var _Session in m_Clients)
                Close(_Session.Value, false);
            m_ClientCount = 0;
            m_Clients.Clear();
        }

        protected virtual void Dispose(bool pDisposing)
        {
            if (!disposed)
            {
                if (pDisposing)
                    try
                    {
                        Stop();
                        m_Listener = null;
                    }
                    catch (SocketException)
                    {
                    }

                disposed = true;
            }
        }
    }
}