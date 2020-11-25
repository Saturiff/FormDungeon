using System;
using System.Windows.Forms;

namespace DungeonServer
{
    public partial class DungeonServer : Form
    {
        public DungeonServer()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            TB_ServerIP.Text = "127.0.0.1";

            BindUI();
        }

        // 控件綁定至全局可見類
        private void BindUI()
        {
            UI.b_ToggleServer = B_ToggleServer;
            UI.lb_Log = LB_Log;
            UI.lb_PlayerList = LB_PlayerList;
            UI.tb_ServerIP = TB_ServerIP;
            UI.tb_ServerPort = TB_ServerPort;
        }

        private void B_ToggleServer_Click(object sender, EventArgs e)
        {
            if (ServerListener.status != ServerStatus.online)
            {
                UI.Start();
            }
            else
            {
                UI.Stop();
            }
        }

        private void DungeonServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServerListener.status == ServerStatus.online)
                ServerListener.StopServer();

            Application.ExitThread();
        }
    }
}
