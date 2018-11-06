namespace SampleTcpServer
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.ListView = new System.Windows.Forms.ListView();
            this.ClientID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SendCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SendByte = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReciveCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReciveByte = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = new System.Windows.Forms.StatusStrip();
            this.ConnectCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.PreSecSendBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.PreSecReceBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this.ServerState = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.ServerPort = new System.Windows.Forms.TextBox();
            this.FlowTimer = new System.Windows.Forms.Timer(this.components);
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(314, 7);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 0;
            this.btnStartServer.Text = "启动";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Location = new System.Drawing.Point(410, 7);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(75, 23);
            this.btnStopServer.TabIndex = 1;
            this.btnStopServer.Text = "停止";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // ListView
            // 
            this.ListView.BackColor = System.Drawing.Color.LightCyan;
            this.ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ClientID,
            this.State,
            this.SendCount,
            this.SendByte,
            this.ReciveCount,
            this.ReciveByte});
            this.ListView.GridLines = true;
            this.ListView.Location = new System.Drawing.Point(12, 41);
            this.ListView.Name = "ListView";
            this.ListView.Size = new System.Drawing.Size(482, 319);
            this.ListView.TabIndex = 3;
            this.ListView.UseCompatibleStateImageBehavior = false;
            this.ListView.View = System.Windows.Forms.View.Details;
            // 
            // ClientID
            // 
            this.ClientID.Text = "ClientID";
            this.ClientID.Width = 79;
            // 
            // State
            // 
            this.State.Text = "State";
            this.State.Width = 54;
            // 
            // SendCount
            // 
            this.SendCount.Text = "SendCount";
            this.SendCount.Width = 80;
            // 
            // SendByte
            // 
            this.SendByte.Text = "SendByte";
            this.SendByte.Width = 90;
            // 
            // ReciveCount
            // 
            this.ReciveCount.Text = "ReciveCount";
            this.ReciveCount.Width = 81;
            // 
            // ReciveByte
            // 
            this.ReciveByte.Text = "ReciveByte";
            this.ReciveByte.Width = 86;
            // 
            // Status
            // 
            this.Status.BackColor = System.Drawing.Color.Bisque;
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectCount,
            this.toolStripStatusLabel1,
            this.PreSecSendBytes,
            this.toolStripStatusLabel2,
            this.PreSecReceBytes,
            this.ServerState});
            this.Status.Location = new System.Drawing.Point(0, 369);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(506, 25);
            this.Status.TabIndex = 4;
            // 
            // ConnectCount
            // 
            this.ConnectCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.ConnectCount.Name = "ConnectCount";
            this.ConnectCount.Size = new System.Drawing.Size(4, 20);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(352, 20);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // PreSecSendBytes
            // 
            this.PreSecSendBytes.Name = "PreSecSendBytes";
            this.PreSecSendBytes.Size = new System.Drawing.Size(34, 20);
            this.PreSecSendBytes.Text = "0B/s";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Image = global::SampleTcpServer.Properties.Resources.communication;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 20);
            // 
            // PreSecReceBytes
            // 
            this.PreSecReceBytes.Name = "PreSecReceBytes";
            this.PreSecReceBytes.Size = new System.Drawing.Size(34, 20);
            this.PreSecReceBytes.Text = "0B/s";
            // 
            // ServerState
            // 
            this.ServerState.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ServerState.Image = global::SampleTcpServer.Properties.Resources.Close;
            this.ServerState.Name = "ServerState";
            this.ServerState.Size = new System.Drawing.Size(20, 20);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "端口:";
            // 
            // ServerPort
            // 
            this.ServerPort.Location = new System.Drawing.Point(54, 8);
            this.ServerPort.Name = "ServerPort";
            this.ServerPort.Size = new System.Drawing.Size(45, 21);
            this.ServerPort.TabIndex = 6;
            this.ServerPort.Text = "9988";
            // 
            // FlowTimer
            // 
            this.FlowTimer.Interval = 1000;
            this.FlowTimer.Tick += new System.EventHandler(this.FlowTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(506, 394);
            this.Controls.Add(this.ServerPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.ListView);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.btnStartServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "TcpServer";
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnStopServer;
        public System.Windows.Forms.ListView ListView;
        public System.Windows.Forms.ColumnHeader ClientID;
        public System.Windows.Forms.ColumnHeader SendCount;
        public System.Windows.Forms.ColumnHeader SendByte;
        public System.Windows.Forms.ColumnHeader ReciveCount;
        public System.Windows.Forms.ColumnHeader ReciveByte;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel ConnectCount;
        private System.Windows.Forms.ToolStripStatusLabel ServerState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ColumnHeader State;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ServerPort;
        private System.Windows.Forms.Timer FlowTimer;
        private System.Windows.Forms.ToolStripStatusLabel PreSecSendBytes;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel PreSecReceBytes;
    }
}

