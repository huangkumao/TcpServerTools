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
            CheckForIllegalCrossThreadCalls = false;

            btnStopServer.Enabled = false;
        }

        //启动服务器 
        private void btnStartServer_Click(object sender, System.EventArgs e)
        {
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
        public void UpdateCount(int pCount)
        {
            ConnectCount.Text = "当前链接数 :" + pCount;
        }

        public void OnConnected(TCPClientSession pSession)
        {
            ListViewItem item = new ListViewItem();
            item.Text = pSession.ClientID.ToString();
            item.SubItems.Add("正常");
            item.SubItems.Add("1");
            item.SubItems.Add("2");
            item.SubItems.Add("3");
            item.SubItems.Add("4");
            ListView.Items.Add(item);
        }

        public void OnDisconnected(TCPClientSession pSession)
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
}