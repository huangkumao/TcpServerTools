using System;
using System.Net.Sockets;
using System.Windows.Forms;
using SampleTcpServer;

namespace TcpServer
{
    public class TCPClientSession
    {
        public long ClientID { get; private set; }

        public int SendCount = 0;
        public long SendSize = 0;
        public int ReceCount = 0;
        public long ReceSize = 0;
        public ListViewItem m_ListItem;

        public TCPClientSession(TcpClient pTcpClient, long pClientID)
        {
            if (pTcpClient == null)
                throw new ArgumentNullException(nameof(pTcpClient));

            TcpClient = pTcpClient;
            ClientID = pClientID;
            Buffer = new byte[NetworkDefine.SocketReceBufSize];
            MsgBuffer = NetByteBuffer.Allocate(NetworkDefine.SocketReceBufSize + 1024);
        }

        //与客户端相关的TcpClient
        public TcpClient TcpClient { get; }

        //获取缓冲区
        public byte[] Buffer { get; private set; }

        //消息处理Buf
        public NetByteBuffer MsgBuffer{ get; private set; }

        //获取网络流
        public NetworkStream NetworkStream => TcpClient.GetStream();

        //触发事件时可以传递的Msg
        public string EventMsg { get; set; }

        //关闭
        public void Close()
        {
            //关闭数据的接受和发送
            TcpClient.Close();
            Buffer = null;
        }

        //网络数据保存到消息缓冲区
        public void SaveMsgToBuffer(int pMsgLen)
        {
            ReceCount++;
            ReceSize += pMsgLen;

            lock (MsgBuffer)
            {
                MsgBuffer.WriteBytes(Buffer, pMsgLen);
            }

            if(ReceCount % 10 == 0)
                MainForm.Ins.UpdateReceInfo(m_ListItem, ReceCount, ReceSize);
        }

        //发送数据统计
        public void UpdateSendInfo(int pMsgLen)
        {
            SendCount++;
            SendSize += pMsgLen;

            if (ReceCount % 10 == 0)
                MainForm.Ins.UpdateSendInfo(m_ListItem, SendCount, SendSize);
        }
    }
}
