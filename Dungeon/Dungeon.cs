using System.Windows.Forms;

namespace DungeonGame
{
    public partial class F_Dungeon : Form
    {
        public F_Dungeon()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            BindUI();
        }

        /// <summary>
        /// 控件綁定至全局可見類
        /// </summary>
        private void BindUI()
        {
            Game.f_Dungeon = this;
            Game.p_Viewport = P_Viewport;
            Game.s_Slot = S_Slot;
            Game.t_SyncTicker = T_SyncTicker;
            Game.tb_Nickname = TB_Nickname;
            Game.tb_CharacterStatus = TB_CharacterStatus;
            Game.tb_EnemyStatus = TB_EnemyStatus;
            Game.tb_Message = TB_Message;
            Game.tb_ItemInfo = TB_ItemInfo;
            Game.lb_Message = LB_Message;
            Game.lb_Log = LB_Log;
            Game.b_ToggleLogin = B_ToggleLogin;
            Game.b_SendMessage = B_SendMessage;
            Game.InitControls();
        }
    }
}
