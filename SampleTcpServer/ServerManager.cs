using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcpServer;

namespace SampleTcpServer
{
    public class ServerManager
    {
        public Action<SocketReceviedData> m_OnReciveMsg;

        private AsyncTCPServer m_Server;
        private readonly Queue<SocketReceviedData> m_Queue = new Queue<SocketReceviedData>(16);

        public void Init()
        {
            m_Server = new AsyncTCPServer(9988);

            m_Server.ClientConnected += OnClickConnected;
            m_Server.ClientDisconnected += OnClickDisconnected;
            m_Server.DataRecevied += OnDataRecevied;
        }

        private bool IsStoped = false;

        private void ProcessMsg()
        {
            while (true)
            {
                Thread.Sleep(1);
                lock (m_Queue)
                {
                    while (this.m_Queue.Count > 0)
                    {
                        var _Data = m_Queue.Dequeue();
                        //解析/拆包并分发消息
                        //TODO

                        m_OnReciveMsg?.Invoke(_Data);
                    }
                }

                if (IsStoped)
                    return;
            }
        }

        public void Start()
        {
            m_Server.Start();

            IsStoped = false;
            //消息派发
            Thread _T = new Thread(ProcessMsg);
            _T.Start();
        }

        public void Stop()
        {
            m_Server.Stop();
            IsStoped = true;
        }

        private void OnClickConnected(object pSender, TCPClientSession pSession)
        {
            MainForm.Ins.UpdateCount(m_Server.m_ClientCount);
            MainForm.Ins.OnConnected(pSession);
        }

        private void OnClickDisconnected(object pSender, TCPClientSession pSession)
        {
            MainForm.Ins.UpdateCount(m_Server.m_ClientCount);
            MainForm.Ins.OnDisconnected(pSession);
        }

        private void OnDataRecevied(object pSender, TCPClientSession pSession)
        {
            lock (pSession.MsgBuffer)
            {
                var MsgBuffer = pSession.MsgBuffer;
                var MsgLen = MsgBuffer.ReadableBytes();
                if (MsgLen <= 0)
                    return;
                byte[] _MsgData = new byte[MsgLen];
                MsgBuffer.ReadBytes(_MsgData, 0, MsgLen);
                MsgBuffer.Clear();
                m_Server.Send(pSession, _MsgData);

                /*while (MsgBuffer.ReadableBytes() >= 8) //包头总长度
                {
                    int MsgBodyLen = MsgBuffer.ReadInt();
                    int MsgLen = MsgBodyLen + 4; //消息长度
                    int readLen = MsgBuffer.ReadableBytes();

                    if (MsgLen > readLen)
                    {
                        //收到到消息不完整
                        MsgBuffer.ResetReaderIndex();
                        return;
                    }

                    int MsgID = MsgBuffer.ReadInt();

                    byte[] _MsgData = new byte[MsgBodyLen];
                    MsgBuffer.ReadBytes(_MsgData, 0, MsgBodyLen);

                    lock (m_Queue)
                    {
                        m_Queue.Enqueue(new SocketReceviedData
                        {
                            mMsgData = _MsgData,
                            mMsgID = MsgID
                        });
                    }

                    //处理可能的粘包
                    MsgBuffer.ReSetBuf();
                }*/
            }
        }
    }

    //接收到但是未被解析的消息
    public class SocketReceviedData
    {
        public byte[] m_MsgData;
        public int m_MsgId;

        public SocketReceviedData(byte[] pMsgData, int pMsgId)
        {
            m_MsgData = pMsgData;
            m_MsgId = pMsgId;
        }
    }
}
