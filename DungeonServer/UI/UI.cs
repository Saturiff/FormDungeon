using System.Windows.Forms;

namespace DungeonServer
{
    public static class UI
    {
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
            ServerListener.StartServer();

            lb_Log.Items.Add("-----");

            b_ToggleServer.Text = "Stop server";
        }

        public static void Stop()
        {
            ServerListener.StopServer();

            lb_PlayerList.Items.Clear();

            b_ToggleServer.Text = "Start server";
        }

        public static TextBox tb_ServerIP;
        public static TextBox tb_ServerPort;
        public static ListBox lb_PlayerList;
        public static ListBox lb_Log;
        public static Button b_ToggleServer;
    }
}
