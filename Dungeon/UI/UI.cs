using System;
using System.Windows.Forms;

namespace DungeonGame
{
    public static class UI
    {
        /// <summary>
        /// 對特定控件的額外初始化
        /// </summary>
        public static void InitControls()
        {
            tb_ItemInfo.Font = new System.Drawing.Font(tb_ItemInfo.Font.Name, 10);

            f_Dungeon.Activated += delegate (object sender, EventArgs e)
            {
                if (ClientManager.isOnline)
                    kbHook.Hook();
            };

            f_Dungeon.Deactivate += delegate (object sender, EventArgs e)
            {
                if (ClientManager.isOnline)
                    kbHook.Unhook();
            };

            // tile outline
            p_Viewport.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                // Console.WriteLine(e.Location);
            };

            // select tile
            p_Viewport.Click += delegate (object sender, EventArgs e)
            {
                // Console.WriteLine(p_Viewport.PointToClientManager(Cursor.Position));
            };
        }

        private static void F_Dungeon_Activated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增至聊天訊息欄
        /// </summary>
        /// <param name="s">欲新增之訊息字串</param>
        public static void Message(string s)
        {
            lb_Message.Items.Add(s);
            AutoScrollListBox(lb_Message);
        }

        /// <summary>
        /// 新增至日誌欄
        /// </summary>
        /// <param name="s">欲新增之日誌字串</param>
        public static void Log(string s)
        {
            lb_Log.Items.Add(s);
            AutoScrollListBox(lb_Log);
        }

        /// <summary>
        /// 自動捲動ListBox
        /// </summary>
        /// <param name="lb">欲控制之ListBox</param>
        public static void AutoScrollListBox(ListBox lb)
        {
            lb.SelectedIndex = lb.Items.Count - 1;
            lb.ClearSelected();
        }

        /// <summary>
        /// 判斷姓名合法性，避免創建存檔文件或傳遞資料封包時時出現錯誤
        /// </summary>
        /// <param name="name">欲判斷之玩家名稱</param>
        /// <returns>是否為合法姓名</returns>
        private static bool IsVaildName(string name)
        {
            foreach (var s in new string[] { "\\", "\"", "/", ":", "*", "?", "<", ">", "|", ",", " ", "aux", "com1", "com2", "prn", "con", "nul" })
                if (name.Contains(s))
                    return false;

            return true;
        }

        /// <summary>
        /// 按下登入按鍵時所呼叫
        /// <para>1. 嘗試登入伺服器</para>
        /// <para>2. 判斷玩家名稱是否可用</para>
        /// <para>3. 更新介面</para>
        /// </summary>
        public static void BeginPlay()
        {
            if (IsVaildName(tb_Nickname.Text))
                ClientManager.Login(tb_Nickname.Text);
            else
                Log("Invalid name.");

            if (ClientManager.isOnline)
            {
                tb_Nickname.Enabled = false;

                while (ClientManager.isWaitingPlayerData) ;
                player = ClientManager.GetPlayerInfo();

                Log("Welcome, " + player.name + "!");

                map = new MapManager();

                p_Viewport.Controls.Add(player);

                t_SyncTicker.Enabled = true;
                b_Send.Enabled = true;
                b_Use.Enabled = true;
                b_Transfer.Enabled = true;
                b_Buy.Enabled = true;
                b_Sell.Enabled = true;
                b_Drop.Enabled = true;

                kbHook.Hook();

                b_ToggleLogin.Text = "Logout";
            }
            else
                Log("Login failed.");
        }

        /// <summary>
        /// 按下登出按鍵或關閉視窗時所呼叫
        /// <para>1. 更新UI可控性</para>
        /// <para>2. 登出伺服器</para>
        /// <para>3. 清空UI資料</para>
        /// </summary>
        public static void Destroy()
        {
            p_Viewport.Controls.Remove(player);
            player = default;

            t_SyncTicker.Enabled = false;
            b_Send.Enabled = false;
            b_Use.Enabled = false;
            b_Transfer.Enabled = false;
            b_Buy.Enabled = false;
            b_Sell.Enabled = false;
            b_Drop.Enabled = false;

            kbHook.Unhook();

            ClientManager.Logout();

            tb_CharacterStatus.Text = "";
            tb_ItemInfo.Text = "";
            tb_Nickname.Enabled = true;

            Log("Logout.");

            b_ToggleLogin.Text = "Login";
        }

        private static KeyboardHook kbHook = new KeyboardHook();

        public static Character player;
        public static MapManager map;

        public static Form f_Dungeon;
        public static Panel p_Viewport;
        public static Timer t_SyncTicker;
        public static TextBox tb_Nickname;
        public static TextBox tb_CharacterStatus;
        public static TextBox tb_Message;
        public static TextBox tb_ItemInfo;
        public static ListBox lb_Message;
        public static ListBox lb_Log;
        public static Button b_ToggleLogin;
        public static Button b_Send;
        public static Button b_Use;
        public static Button b_Transfer;
        public static Button b_Buy;
        public static Button b_Sell;
        public static Button b_Drop;
        public static Inventory inv_Player;
        public static Inventory inv_Their;
    }
}
