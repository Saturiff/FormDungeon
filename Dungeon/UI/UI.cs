using DungeonGame.Inventory;
using System;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 所有類別可對表單控件進行訪問的唯一渠道
    /// <para>* 僅在跨類別時使用</para>
    /// </summary>
    public static class UI
    {
        #region 初始化
        /// <summary>
        /// 對特定控件的額外初始化
        /// </summary>
        public static void InitControls()
        {
            inventory = new InventorySystem();

            tb_ItemInfo.Font = new System.Drawing.Font(tb_ItemInfo.Font.Name, 10);

            BindFormEvents();
            BindViewportEvents();
            BindInventoryEvents();
        }

        private static void BindFormEvents()
        {
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
        }

        private static void BindViewportEvents()
        {
            p_Viewport.ControlAdded += delegate (object sender, ControlEventArgs e)
            {
                e.Control.Click += InteractCharacter;
            };

            // tile outline
            p_Viewport.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                if (map != null)
                    map.Interact(e.Location);
            };

            // select tile
            p_Viewport.Click += delegate (object sender, EventArgs e)
            {
                if (map != null)
                    map.Interact(p_Viewport.PointToClient(Cursor.Position));
            };
        }

        private static void BindInventoryEvents()
        {
            b_Use.Click += delegate (object sender, EventArgs e)
            {
                inventory.Use();
            };

            b_Transfer.Click += delegate (object sender, EventArgs e)
            {
                inventory.Transfer();
            };

            b_Buy.Click += delegate (object sender, EventArgs e)
            {
                inventory.Buy();
            };

            b_Sell.Click += delegate (object sender, EventArgs e)
            {
                inventory.Sell();
            };

            b_Drop.Click += delegate (object sender, EventArgs e)
            {
                inventory.Drop();
            };
        }
        #endregion

        #region ListBox
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
        #endregion

        #region Character
        public static void SpawnCharacter(Character c) => p_Viewport.BeginInvoke((Action)delegate ()
            {
                p_Viewport.Controls.Add(c);
                
            });

        public static void DestroyCharacter(Character c) => p_Viewport.BeginInvoke((Action)delegate ()
            {
                p_Viewport.Controls.Remove(c);
            });

        private static void InteractCharacter(object sender, EventArgs e)
        {
            Character ch = (Character)sender;
            
            ClientManager.RequestCharacterItem(ch.name);

            // todo: add function to interact!
            // Console.WriteLine("Interact character: " + ((Character)sender).name);
        }
        #endregion

        #region Login/Logout
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

                SpawnCharacter(player);

                t_SyncTicker.Enabled = true;
                b_SendMessage.Enabled = true;
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
        /// 判斷姓名合法性，避免創建存檔文件或傳遞資料封包時時出現錯誤
        /// </summary>
        /// <param name="name">欲判斷之玩家名稱</param>
        /// <returns>是否為合法姓名</returns>
        private static bool IsVaildName(string name)
        {
            foreach (var s in new string[] {
                "\\", "\"", "/", ":", "*", "?", "<", ">", "|", ",",
                " ", "aux", "com1", "com2", "prn", "con", "nul" })
                if (name.Contains(s))
                    return false;

            return true;
        }

        /// <summary>
        /// 按下登出按鍵或關閉視窗時所呼叫
        /// <para>1. 將玩家由視圖中移除</para>
        /// <para>2. 更新UI可控性</para>
        /// <para>3. 登出伺服器</para>
        /// <para>4. 清空UI資料</para>
        /// </summary>
        public static void Destroy()
        {
            DestroyCharacter(player);
            player = default;

            t_SyncTicker.Enabled = false;
            b_SendMessage.Enabled = false;
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
        #endregion

        private static KeyboardHook kbHook = new KeyboardHook();

        public static Character player;
        public static MapManager map;
        public static InventorySystem inventory;

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
        public static Button b_SendMessage;
        public static Button b_Use;
        public static Button b_Transfer;
        public static Button b_Buy;
        public static Button b_Sell;
        public static Button b_Drop;
        public static InventoryGrid inv_Player;
        public static InventoryGrid inv_Their;
    }
}
