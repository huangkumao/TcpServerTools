using System.Windows.Forms;
using TcpServer;

namespace SampleTcpServer
{
    public partial class MainForm : Form
    {
        public static MainForm Ins = null;

        ServerManager mManager = new ServerManager();

        public MainForm()
        {
            Ins = this;
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;

            btnStopServer.Enabled = false;
            FlowTimer.Start();
        }

        //启动服务器 
        private void btnStartServer_Click(object sender, System.EventArgs e)
        {
            ListView.Items.Clear();

            int port;
            if (int.TryParse(ServerPort.Text, out port))
                mManager.Init(port);
            else
                mManager.Init();

            mManager.Start();
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;
            ServerState.Image = Properties.Resources.Normal;
        }

        //停止服务器
        private void btnStopServer_Click(object sender, System.EventArgs e)
        {
            mManager.Stop();
            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;
            ServerState.Image = Properties.Resources.Close;
        }

        //事件回调
        delegate void AsynUpdateCount(int pCount);
        public void UpdateCount(int pCount)
        {
            if (InvokeRequired)
            {
                Invoke(new AsynUpdateCount(delegate (int _Count)
                {
                    ConnectCount.Text = "当前链接数 :" + _Count;
                }), pCount);
            }
            else
            {
                ConnectCount.Text = "当前链接数 :" + pCount;
            }
        }

        delegate void AsynOnConnected(TCPClientSession pSession);
        public void OnConnected(TCPClientSession pSession)
        {
            if (InvokeRequired)
            {
                Invoke(new AsynOnConnected(delegate (TCPClientSession _Session)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = _Session.ClientID.ToString();
                    item.SubItems.Add("正常");
                    item.SubItems.Add("0");
                    item.SubItems.Add("0");
                    item.SubItems.Add("0");
                    item.SubItems.Add("0");
                    ListView.Items.Add(item);
                    _Session.m_ListItem = item;
                }), pSession);
            }
            else
            {
                ListViewItem item = new ListViewItem();
                item.Text = pSession.ClientID.ToString();
                item.SubItems.Add("正常");
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                item.SubItems.Add("0");
                ListView.Items.Add(item);
                pSession.m_ListItem = item;
            }
        }

        delegate void AsynOnDisconnected(TCPClientSession pSession);
        public void OnDisconnected(TCPClientSession pSession)
        {
            if (InvokeRequired)
            {
                Invoke(new AsynOnDisconnected(delegate (TCPClientSession _Session)
                {
                    foreach (ListViewItem item in ListView.Items)
                    {
                        if (item.SubItems[0].Text == _Session.ClientID.ToString())
                        {
                            item.SubItems[1].Text = "断开";
                        }
                    }
                }), pSession);
            }
            else
            {
                foreach (ListViewItem item in ListView.Items)
                {
                    if (item.SubItems[0].Text == pSession.ClientID.ToString())
                    {
                        item.SubItems[1].Text = "断开";
                    }
                }
            }
        }

        delegate void AsynUpdateSendInfo(ListViewItem pItem, int pCount, long pSzie);
        public void UpdateSendInfo(ListViewItem pItem, int pCount, long pSzie)
        {
            if (InvokeRequired)
            {
                Invoke(new AsynUpdateSendInfo(delegate (ListViewItem _Item, int _Count, long _Szie)
                {
                    _Item.SubItems[2].Text = _Count + "";
                    _Item.SubItems[3].Text = _Szie + "";
                }), pItem, pCount, pSzie);
            }
            else
            {
                pItem.SubItems[2].Text = pCount + "";
                pItem.SubItems[3].Text = pSzie + "";
            }
        }

        delegate void AsynUpdateReceInfo(ListViewItem pItem, int pCount, long pSzie);
        public void UpdateReceInfo(ListViewItem pItem, int pCount, long pSzie)
        {
            if (InvokeRequired)
            {
                Invoke(new AsynUpdateReceInfo(delegate (ListViewItem _Item, int _Count, long _Szie)
                {
                    _Item.SubItems[4].Text = _Count + "";
                    _Item.SubItems[5].Text = _Szie + "";
                }), pItem, pCount, pSzie);
            }
            else
            {
                pItem.SubItems[4].Text = pCount + "";
                pItem.SubItems[5].Text = pSzie + "";
            }
        }

        private long lastSendBytes = 0;
        private long lastReceBytes = 0;
        private void FlowTimer_Tick(object sender, System.EventArgs e)
        {
            PreSecSendBytes.Text = AsyncTCPServer.s_AllSendBytes - lastSendBytes + @"B/s";
            PreSecReceBytes.Text = AsyncTCPServer.s_AllReceBytes - lastReceBytes + @"B/s";
            lastSendBytes = AsyncTCPServer.s_AllSendBytes;
            lastReceBytes = AsyncTCPServer.s_AllReceBytes;
        }
    }
}