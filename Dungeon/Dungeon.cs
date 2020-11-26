using System;
using System.Windows.Forms;

namespace DungeonGame
{
    public partial class Dungeon : Form
    {
        public Dungeon()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            BindUI();
            ClientManager.SetServerIP();
        }

        /// <summary>
        /// 控件綁定至全局可見類
        /// </summary>
        private void BindUI()
        {
            UI.p_Viewport = P_Viewport;
            UI.t_SyncTicker = T_SyncTicker;
            UI.tb_Nickname = TB_Nickname;
            UI.tb_CharacterStatus = TB_CharacterStatus;
            UI.tb_Message = TB_Message;
            UI.tb_ItemInfo = TB_ItemInfo;
            UI.lb_Message = LB_Message;
            UI.lb_Log = LB_Log;
            UI.b_ToggleLogin = B_ToggleLogin;
            UI.b_Send = B_Send;
            UI.b_Use = B_Send;
            UI.b_Transfer = B_Send;
            UI.b_Buy = B_Send;
            UI.b_Sell = B_Send;
            UI.b_Drop = B_Send;
            UI.inv_Player = INV_Player;
            UI.inv_Their = INV_Their;
            UI.InitControls();
        }

        // Convert.ChangeType(TB_Nickname.Text, StatusType[CharacterStatusInfo.IP])

        #region 表單控件事件
        private void B_ToggleLogin_Click(object sender, EventArgs e)
        {
            if (!ClientManager.isOnline)
            {
                UI.BeginPlay();
            }
            else
            {
                UI.Destroy();
            }
        }

        private void B_Use_Click(object sender, EventArgs e)
        {

        }

        private void B_Transfer_Click(object sender, EventArgs e)
        {

        }

        private void B_Buy_Click(object sender, EventArgs e)
        {

        }

        private void B_Sell_Click(object sender, EventArgs e)
        {

        }

        private void B_Drop_Click(object sender, EventArgs e)
        {

        }

        private void B_Send_Click(object sender, EventArgs e)
        {
            ClientManager.SendMessage(TB_Message.Text);
        }

        private void Dungeon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ClientManager.isOnline)
                ClientManager.Logout();

            Application.ExitThread();
        }

        private void T_SyncTicker_Tick(object sender, EventArgs e) => ClientManager.UpdateUI();
        #endregion
    }
}
