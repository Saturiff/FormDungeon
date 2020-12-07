using System;
using System.Windows.Forms;

namespace DungeonServer
{
    /// <summary>
    /// 綁定表單控件，提供啟用予關閉伺服器之功能
    /// </summary>
    public static class UI
    {
        public static void InitControls()
        {
            server = new ServerManager();

            BindEvents();
        }

        public static void BindEvents()
        {
            b_ToggleServer.Click += delegate (object sender, EventArgs e)
            {
                if (!server.isOnline)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            };

            f_DungeonServer.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (server.isOnline)
                    server.StopServer();

                Application.ExitThread();
            };
        }

        public static void AddToPlayerList(string s) => lb_PlayerList.Items.Add(s);

        public static void RemoveFromPlayerList(string s) => lb_PlayerList.Items.Remove(s);

        public static void AddLog(string s)
        {
            lb_Log.Items.Add(s);
            lb_Log.SelectedIndex = lb_Log.Items.Count - 1;
            lb_Log.ClearSelected();
        }

        public static void Start()
        {
            server.StartServer();

            lb_Log.Items.Add("-----");

            b_ToggleServer.Text = "Stop server";
        }

        public static void Stop()
        {
            server.StopServer();

            lb_PlayerList.Items.Clear();

            b_ToggleServer.Text = "Start server";
        }

        public static ServerManager server;

        public static Form f_DungeonServer;
        public static TextBox tb_ServerIP;
        public static TextBox tb_ServerPort;
        public static ListBox lb_PlayerList;
        public static ListBox lb_Log;
        public static Button b_ToggleServer;
    }
}
