
namespace TcpServer
{
    class NetworkDefine
    {
        //允许最大的客户端链接
        public static int MaxConnect = 1024;

        //套接字发送缓冲区大小
        public static int SocketSendBufSize = 1024 * 8; // 8k

        //套接字接受缓冲区大小
        public static int SocketReceBufSize = 1024 * 8; // 8k

        //是否使用NoDelay模式
        public static bool UseNoDelay = true;
    }
}
