namespace DungeonGame
{
    partial class F_Dungeon
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Dungeon));
            this.P_Viewport = new System.Windows.Forms.Panel();
            this.B_ToggleLogin = new System.Windows.Forms.Button();
            this.TB_Nickname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_CharacterStatus = new System.Windows.Forms.TextBox();
            this.TB_Message = new System.Windows.Forms.TextBox();
            this.B_SendMessage = new System.Windows.Forms.Button();
            this.TB_ItemInfo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.T_SyncTicker = new System.Windows.Forms.Timer(this.components);
            this.LB_Message = new System.Windows.Forms.ListBox();
            this.LB_Log = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TB_EnemyStatus = new System.Windows.Forms.TextBox();
            this.S_Slot = new DungeonGame.Slot();
            this.HB_Player = new DungeonGame.HealthBar();
            this.HB_Enemy = new DungeonGame.HealthBar();
            this.SuspendLayout();
            // 
            // P_Viewport
            // 
            this.P_Viewport.Cursor = System.Windows.Forms.Cursors.Cross;
            this.P_Viewport.Location = new System.Drawing.Point(13, 14);
            this.P_Viewport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.P_Viewport.Name = "P_Viewport";
            this.P_Viewport.Size = new System.Drawing.Size(800, 440);
            this.P_Viewport.TabIndex = 0;
            // 
            // B_ToggleLogin
            // 
            this.B_ToggleLogin.Location = new System.Drawing.Point(841, 44);
            this.B_ToggleLogin.Name = "B_ToggleLogin";
            this.B_ToggleLogin.Size = new System.Drawing.Size(250, 42);
            this.B_ToggleLogin.TabIndex = 3;
            this.B_ToggleLogin.Text = "Login";
            this.B_ToggleLogin.UseVisualStyleBackColor = true;
            // 
            // TB_Nickname
            // 
            this.TB_Nickname.Location = new System.Drawing.Point(925, 12);
            this.TB_Nickname.Name = "TB_Nickname";
            this.TB_Nickname.Size = new System.Drawing.Size(166, 26);
            this.TB_Nickname.TabIndex = 4;
            this.TB_Nickname.Text = "Reina";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(837, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nickname";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(837, 312);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "Log";
            // 
            // TB_CharacterStatus
            // 
            this.TB_CharacterStatus.Enabled = false;
            this.TB_CharacterStatus.Location = new System.Drawing.Point(516, 481);
            this.TB_CharacterStatus.Multiline = true;
            this.TB_CharacterStatus.Name = "TB_CharacterStatus";
            this.TB_CharacterStatus.Size = new System.Drawing.Size(250, 88);
            this.TB_CharacterStatus.TabIndex = 14;
            // 
            // TB_Message
            // 
            this.TB_Message.Location = new System.Drawing.Point(841, 283);
            this.TB_Message.Name = "TB_Message";
            this.TB_Message.Size = new System.Drawing.Size(168, 26);
            this.TB_Message.TabIndex = 16;
            // 
            // B_SendMessage
            // 
            this.B_SendMessage.Enabled = false;
            this.B_SendMessage.Location = new System.Drawing.Point(1015, 282);
            this.B_SendMessage.Name = "B_SendMessage";
            this.B_SendMessage.Size = new System.Drawing.Size(76, 27);
            this.B_SendMessage.TabIndex = 17;
            this.B_SendMessage.Text = "Send";
            this.B_SendMessage.UseVisualStyleBackColor = true;
            // 
            // TB_ItemInfo
            // 
            this.TB_ItemInfo.Enabled = false;
            this.TB_ItemInfo.Location = new System.Drawing.Point(231, 481);
            this.TB_ItemInfo.Multiline = true;
            this.TB_ItemInfo.Name = "TB_ItemInfo";
            this.TB_ItemInfo.Size = new System.Drawing.Size(250, 168);
            this.TB_ItemInfo.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(227, 459);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 19);
            this.label6.TabIndex = 24;
            this.label6.Text = "Item Info";
            // 
            // T_SyncTicker
            // 
            this.T_SyncTicker.Interval = 50;
            // 
            // LB_Message
            // 
            this.LB_Message.FormattingEnabled = true;
            this.LB_Message.ItemHeight = 19;
            this.LB_Message.Location = new System.Drawing.Point(841, 92);
            this.LB_Message.Name = "LB_Message";
            this.LB_Message.Size = new System.Drawing.Size(250, 175);
            this.LB_Message.TabIndex = 26;
            // 
            // LB_Log
            // 
            this.LB_Log.FormattingEnabled = true;
            this.LB_Log.ItemHeight = 19;
            this.LB_Log.Location = new System.Drawing.Point(841, 334);
            this.LB_Log.Name = "LB_Log";
            this.LB_Log.Size = new System.Drawing.Size(250, 118);
            this.LB_Log.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(512, 459);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 19);
            this.label5.TabIndex = 30;
            this.label5.Text = "My status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(801, 459);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Enemy status";
            // 
            // TB_EnemyStatus
            // 
            this.TB_EnemyStatus.Enabled = false;
            this.TB_EnemyStatus.Location = new System.Drawing.Point(805, 481);
            this.TB_EnemyStatus.Multiline = true;
            this.TB_EnemyStatus.Name = "TB_EnemyStatus";
            this.TB_EnemyStatus.Size = new System.Drawing.Size(250, 88);
            this.TB_EnemyStatus.TabIndex = 29;
            // 
            // S_Slot
            // 
            this.S_Slot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.S_Slot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.S_Slot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.S_Slot.Item = null;
            this.S_Slot.Location = new System.Drawing.Point(35, 475);
            this.S_Slot.Margin = new System.Windows.Forms.Padding(0);
            this.S_Slot.Name = "S_Slot";
            this.S_Slot.Size = new System.Drawing.Size(170, 170);
            this.S_Slot.TabIndex = 31;
            // 
            // HB_Player
            // 
            this.HB_Player.Location = new System.Drawing.Point(516, 597);
            this.HB_Player.Name = "HB_Player";
            this.HB_Player.Size = new System.Drawing.Size(250, 33);
            this.HB_Player.TabIndex = 34;
            // 
            // HB_Enemy
            // 
            this.HB_Enemy.Location = new System.Drawing.Point(805, 597);
            this.HB_Enemy.Name = "HB_Enemy";
            this.HB_Enemy.Size = new System.Drawing.Size(250, 33);
            this.HB_Enemy.TabIndex = 35;
            // 
            // F_Dungeon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 661);
            this.Controls.Add(this.HB_Enemy);
            this.Controls.Add(this.HB_Player);
            this.Controls.Add(this.S_Slot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TB_EnemyStatus);
            this.Controls.Add(this.LB_Log);
            this.Controls.Add(this.LB_Message);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TB_ItemInfo);
            this.Controls.Add(this.B_SendMessage);
            this.Controls.Add(this.TB_Message);
            this.Controls.Add(this.TB_CharacterStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_Nickname);
            this.Controls.Add(this.B_ToggleLogin);
            this.Controls.Add(this.P_Viewport);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "F_Dungeon";
            this.Text = " Dungeon Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel P_Viewport;
        private System.Windows.Forms.Button B_ToggleLogin;
        private System.Windows.Forms.TextBox TB_Nickname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_CharacterStatus;
        private System.Windows.Forms.TextBox TB_Message;
        private System.Windows.Forms.Button B_SendMessage;
        private System.Windows.Forms.TextBox TB_ItemInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer T_SyncTicker;
        private System.Windows.Forms.ListBox LB_Message;
        private System.Windows.Forms.ListBox LB_Log;
        private System.Windows.Forms.Label label5;
        private Slot S_Slot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_EnemyStatus;
        private HealthBar HB_Player;
        private HealthBar HB_Enemy;
    }
}

