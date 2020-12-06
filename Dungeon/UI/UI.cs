using System;
using System.Drawing;
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
            tb_ItemInfo.Font = new Font(tb_ItemInfo.Font.Name, 10);
            s_Slot.RemoveItem();

            BindFormEvents();
            BindViewportEvents();

            SetDoubleBuffered(p_Viewport);
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
                e.Control.Click += Interact;
            };

            p_Viewport.Click += delegate (object sender, EventArgs e)
            {
                p_Viewport.Focus();
            };

            p_Viewport.GotFocus += delegate (object sender, EventArgs e)
            {
                p_Viewport.Refresh();
            };

            p_Viewport.LostFocus += delegate (object sender, EventArgs e)
            {
                p_Viewport.Refresh();
            };

            p_Viewport.Paint += delegate (object sender, PaintEventArgs e)
            {
                if (ClientManager.isOnline)
                {
                    if (isInViewport)
                        ControlPaint.DrawBorder(e.Graphics, p_Viewport.ClientRectangle,
                            Color.DarkOrange, 5, ButtonBorderStyle.Solid,  // left
                            Color.DarkOrange, 5, ButtonBorderStyle.Solid,  // top
                            Color.DarkOrange, 5, ButtonBorderStyle.Solid,  // right
                            Color.DarkOrange, 5, ButtonBorderStyle.Solid); // bottom
                    else
                        ControlPaint.DrawBorder(e.Graphics, p_Viewport.ClientRectangle,
                            Color.DarkGreen, 3, ButtonBorderStyle.Solid,  // left
                            Color.DarkGreen, 3, ButtonBorderStyle.Solid,  // top
                            Color.DarkGreen, 3, ButtonBorderStyle.Solid,  // right
                            Color.DarkGreen, 3, ButtonBorderStyle.Solid); // bottom
                }
            };
        }

        private static void SetDoubleBuffered(Control c)
        {
            // Taxes: Remote Desktop Connection and painting
            // http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        #endregion

        #region ListBox
        /// <summary>
        /// 新增至聊天訊息欄
        /// </summary>
        /// <param name="s">欲新增之訊息字串</param>
        public static void AddTextMessage(string s)
        {
            lb_Message.Items.Add(s);
            AutoScrollListBox(lb_Message);
        }

        /// <summary>
        /// 新增至日誌欄
        /// </summary>
        /// <param name="s">欲新增之日誌字串</param>
        public static void AddLog(string s)
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
        public static void SpawnInViewport(CharacterBase c) => p_Viewport.BeginInvoke((Action)delegate ()
            {
                p_Viewport.Controls.Add(c);

            });

        public static void DestroyFromViewport(CharacterBase c) => p_Viewport.BeginInvoke((Action)delegate ()
            {
                p_Viewport.Controls.Remove(c);
            });

        /// <summary>
        /// 鼠標與Viewport中的物件互動
        /// </summary>
        /// <param name="sender">被點擊物件</param>
        /// <param name="e">EventArgs參數</param>
        private static void Interact(object sender, EventArgs e)
        {
            if (sender is IInteractable obj)
            {
                if (obj is Player p && p != player)
                    focusEnemyName = p.name;
                else if (obj is Pickable item)
                    item.Interact();
            }
        }
        #endregion

        #region Pickable
        public static void SpawnInViewport(Pickable p) => p_Viewport.BeginInvoke((Action)delegate ()
        {
            p_Viewport.Controls.Add(p);

        });

        public static void DestroyFromViewport(Pickable p) => p_Viewport.BeginInvoke((Action)delegate ()
        {
            for (int i = 0; i < p_Viewport.Controls.Count; i++)
                if (p_Viewport.Controls[i] is Pickable _p && _p == p)
                {
                    p_Viewport.Controls.Remove(p_Viewport.Controls[i]);
                    break;
                }
        });

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
                AddLog("Invalid name.");

            if (ClientManager.isOnline)
            {
                tb_Nickname.Enabled = false;

                while (ClientManager.isWaitingPlayerData) ;
                player = ClientManager.GetPlayerCharacter();

                AddLog("Welcome, " + player.name + "!");

                map = new MapManager();

                SpawnInViewport(player);

                t_SyncTicker.Enabled = true;
                b_SendMessage.Enabled = true;

                kbHook.Hook();

                b_ToggleLogin.Text = "Logout";
            }
            else
                AddLog("Login failed.");
        }

        /// <summary>
        /// 判斷姓名合法性，避免創建存檔文件或傳遞資料封包時時出現錯誤
        /// </summary>
        /// <param name="name">欲判斷之玩家名稱</param>
        /// <returns>是否為合法姓名</returns>
        private static bool IsVaildName(string name)
        {
            if (name == string.Empty)
                return false;

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
            player.Dispose();

            t_SyncTicker.Enabled = false;
            b_SendMessage.Enabled = false;

            kbHook.Unhook();

            ClientManager.Logout();

            s_Slot.RemoveItem();
            tb_CharacterStatus.Text = "";
            tb_EnemyStatus.Text = "";
            tb_ItemInfo.Text = "";
            tb_Nickname.Enabled = true;

            AddLog("Logout.");

            b_ToggleLogin.Text = "Login";
        }
        #endregion

        private static KeyboardHook kbHook = new KeyboardHook();

        public static Player player;
        public static MapManager map;
        public static string focusEnemyName;
        public static bool isInViewport => p_Viewport.Focused;

        public static Form f_Dungeon;
        public static Panel p_Viewport;
        public static Slot s_Slot;
        public static Timer t_SyncTicker;
        public static TextBox tb_Nickname;
        public static TextBox tb_CharacterStatus;
        public static TextBox tb_EnemyStatus;
        public static TextBox tb_Message;
        public static TextBox tb_ItemInfo;
        public static ListBox lb_Message;
        public static ListBox lb_Log;
        public static Button b_ToggleLogin;
        public static Button b_SendMessage;
    }
}
