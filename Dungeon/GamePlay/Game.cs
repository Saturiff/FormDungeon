using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace DungeonGame
{
    /// <summary>
    /// 主遊戲類，包含著遊戲初始化、客戶端、與表單控件之參考
    /// </summary>
    public static class Game
    {
        #region 初始化
        /// <summary>
        /// 對特定控件的額外初始化
        /// </summary>
        public static void InitControls()
        {
            client = new ClientManager();

            tb_ItemInfo.Font = new Font(tb_ItemInfo.Font.Name, 10);

            s_Slot.RemoveItem();

            BindEvents();

            SetDoubleBuffered(p_Viewport);
        }

        private static void BindEvents()
        {
            // form
            f_Dungeon.FormClosing += delegate (object sender, FormClosingEventArgs e)
            {
                if (client.IsOnline)
                    client.Logout();

                Application.ExitThread();
            };

            // button
            b_ToggleLogin.Click += delegate (object sender, EventArgs e)
            {
                if (!client.IsOnline)
                {
                    BeginPlay();
                }
                else
                {
                    Destroy();
                }
            };

            b_SendMessage.Click += delegate (object sender, EventArgs e)
            {
                client.SendMessage(tb_Message.Text);
                tb_Message.Clear();
            };

            // viewport
            p_Viewport.ControlAdded += delegate (object sender, ControlEventArgs e)
            {
                e.Control.Click += Interact;
            };

            p_Viewport.Click += delegate (object sender, EventArgs e)
            {
                p_Viewport.Focus();

                if (client.IsOnline)
                    Interact(sender, e);
            };

            p_Viewport.PreviewKeyDown += delegate (object sender, PreviewKeyDownEventArgs e)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                        e.IsInputKey = true;
                        break;
                }
            };

            p_Viewport.KeyDown += delegate (object sender, KeyEventArgs e)
            {
                if (client.IsOnline && IsHotkey(e.KeyCode))
                    UpdateKeyboardStatus(e, isButtonDown: true);
            };

            p_Viewport.KeyUp += delegate (object sender, KeyEventArgs e)
            {
                if (client.IsOnline && IsHotkey(e.KeyCode))
                    UpdateKeyboardStatus(e, isButtonDown: false);
            };

            p_Viewport.GotFocus += delegate (object sender, EventArgs e)
            {
                p_Viewport.Refresh();
            };

            p_Viewport.LostFocus += delegate (object sender, EventArgs e)
            {
                p_Viewport.Refresh();

                if (client.IsOnline)
                    ClearKeyboardStatus();
            };

            p_Viewport.Paint += delegate (object sender, PaintEventArgs e)
            {
                if (client.IsOnline)
                {
                    if (IsInViewport)
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

            // timer
            t_SyncTicker.Tick += delegate (object sender, EventArgs e)
            {
                client.UpdateUI();
                player.CalcMove();
            };
        }

        private static bool IsHotkey(Keys k)
            => (k == Keys.W)
            || (k == Keys.S)
            || (k == Keys.A)
            || (k == Keys.D)
            || (k == Keys.Up)
            || (k == Keys.Down)
            || (k == Keys.Left)
            || (k == Keys.Right);

        private static void UpdateKeyboardStatus(KeyEventArgs e, bool isButtonDown)
        {
            if ((e.KeyCode == Keys.W) || (e.KeyCode == Keys.Up))
                player.isMovingUp = isButtonDown;
            else if ((e.KeyCode == Keys.S) || (e.KeyCode == Keys.Down))
                player.isMovingDown = isButtonDown;
            else if ((e.KeyCode == Keys.A) || (e.KeyCode == Keys.Left))
                player.isMovingLeft = isButtonDown;
            else if ((e.KeyCode == Keys.D) || (e.KeyCode == Keys.Right))
                player.isMovingRight = isButtonDown;
        }

        private static void ClearKeyboardStatus()
            => player.isMovingUp = player.isMovingDown = player.isMovingLeft = player.isMovingRight = false;

        public static void SetDoubleBuffered(Control c)
        {
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

        #region Interactable
        public static void SpawnInViewport(Actor actor) => p_Viewport.BeginInvoke((Action)delegate ()
            {
                p_Viewport.Controls.Add(actor);
            });

        public static void DestroyFromViewport<T>(T control) => p_Viewport.BeginInvoke((Action)delegate ()
            {
                for (int i = 0; i < p_Viewport.Controls.Count; i++)
                    if (p_Viewport.Controls[i] is T c && c.Equals(control))
                    {
                        p_Viewport.Controls.Remove(p_Viewport.Controls[i]);
                        break;
                    }
            });

        /// <summary>
        /// 鼠標與Viewport中的物件互動，根據不同情況做不同行為
        /// </summary>
        /// <param name="sender">被點擊物件</param>
        /// <param name="e">EventArgs參數</param>
        private static void Interact(object sender, EventArgs e)
        {
            if (sender is IInteractable obj)
            {
                if (obj is PlayerCharacter p)
                {
                    if (p != player)
                        focusEnemyName = p.Name;

                    player.AttackTo(p.Location);
                }
                else if (obj is Pickable item)
                    if (item.DistanceOf(player) < PlayerCharacter.pickRange)
                        item.Interact();
                    else
                        player.AttackTo(item.Location);
            }
            else
                player.AttackTo(((MouseEventArgs)e).Location);
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
                client.Login(tb_Nickname.Text);
            else
                AddLog("Invalid name.");

            if (client.IsOnline)
            {
                tb_Nickname.Enabled = false;

                while (client.isWaitingPlayerData) ;
                player = client.GetPlayerCharacter();
                player.InitTick();
                AddLog("Welcome, " + player.Name + "!");

                map = new MapManager();

                SpawnInViewport(player);

                t_SyncTicker.Enabled = true;
                b_SendMessage.Enabled = true;
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
            b_ToggleLogin.Enabled = false;
            Thread.Sleep(100);

            client.Logout();
            b_ToggleLogin.Enabled = true;

            b_SendMessage.Enabled = false;
            s_Slot.RemoveItem();
            tb_CharacterStatus.Text = "";
            tb_EnemyStatus.Text = "";
            tb_ItemInfo.Text = "";
            tb_Nickname.Enabled = true;

            AddLog("Logout.");

            b_ToggleLogin.Text = "Login";
        }
        #endregion

        private static List<Control> spawnedControls = new List<Control>();

        public static string focusEnemyName;
        public static bool IsInViewport => p_Viewport.Focused;
        public static ClientManager client;
        public static PlayerCharacter player;
        public static MapManager map;
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
