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
            this.P_Viewport = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.B_ToggleLogin = new System.Windows.Forms.Button();
            this.TB_Nickname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TB_CharacterStatus = new System.Windows.Forms.TextBox();
            this.TB_Message = new System.Windows.Forms.TextBox();
            this.B_Send = new System.Windows.Forms.Button();
            this.B_Transfer = new System.Windows.Forms.Button();
            this.B_Buy = new System.Windows.Forms.Button();
            this.B_Sell = new System.Windows.Forms.Button();
            this.B_Drop = new System.Windows.Forms.Button();
            this.TB_ItemInfo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.B_Use = new System.Windows.Forms.Button();
            this.T_SyncTicker = new System.Windows.Forms.Timer(this.components);
            this.LB_Message = new System.Windows.Forms.ListBox();
            this.LB_Log = new System.Windows.Forms.ListBox();
            this.INV_Their = new DungeonGame.Inventory();
            this.INV_Player = new DungeonGame.Inventory();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 459);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "My inventory";
            // 
            // B_ToggleLogin
            // 
            this.B_ToggleLogin.Location = new System.Drawing.Point(841, 44);
            this.B_ToggleLogin.Name = "B_ToggleLogin";
            this.B_ToggleLogin.Size = new System.Drawing.Size(250, 42);
            this.B_ToggleLogin.TabIndex = 3;
            this.B_ToggleLogin.Text = "Login";
            this.B_ToggleLogin.UseVisualStyleBackColor = true;
            this.B_ToggleLogin.Click += new System.EventHandler(this.B_ToggleLogin_Click);
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
            this.label3.Location = new System.Drawing.Point(837, 459);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "Log";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(837, 89);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Character status";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(373, 455);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 19);
            this.label5.TabIndex = 11;
            this.label5.Text = "Their inventory";
            // 
            // TB_CharacterStatus
            // 
            this.TB_CharacterStatus.Enabled = false;
            this.TB_CharacterStatus.Location = new System.Drawing.Point(841, 111);
            this.TB_CharacterStatus.Multiline = true;
            this.TB_CharacterStatus.Name = "TB_CharacterStatus";
            this.TB_CharacterStatus.Size = new System.Drawing.Size(250, 119);
            this.TB_CharacterStatus.TabIndex = 14;
            // 
            // TB_Message
            // 
            this.TB_Message.Location = new System.Drawing.Point(841, 428);
            this.TB_Message.Name = "TB_Message";
            this.TB_Message.Size = new System.Drawing.Size(168, 26);
            this.TB_Message.TabIndex = 16;
            // 
            // B_Send
            // 
            this.B_Send.Enabled = false;
            this.B_Send.Location = new System.Drawing.Point(1015, 427);
            this.B_Send.Name = "B_Send";
            this.B_Send.Size = new System.Drawing.Size(76, 27);
            this.B_Send.TabIndex = 17;
            this.B_Send.Text = "Send";
            this.B_Send.UseVisualStyleBackColor = true;
            this.B_Send.Click += new System.EventHandler(this.B_Send_Click);
            // 
            // B_Transfer
            // 
            this.B_Transfer.Enabled = false;
            this.B_Transfer.Location = new System.Drawing.Point(276, 498);
            this.B_Transfer.Name = "B_Transfer";
            this.B_Transfer.Size = new System.Drawing.Size(90, 30);
            this.B_Transfer.TabIndex = 18;
            this.B_Transfer.Text = "Transfer";
            this.B_Transfer.UseVisualStyleBackColor = true;
            this.B_Transfer.Click += new System.EventHandler(this.B_Transfer_Click);
            // 
            // B_Buy
            // 
            this.B_Buy.Enabled = false;
            this.B_Buy.Location = new System.Drawing.Point(276, 534);
            this.B_Buy.Name = "B_Buy";
            this.B_Buy.Size = new System.Drawing.Size(90, 30);
            this.B_Buy.TabIndex = 19;
            this.B_Buy.Text = "Buy";
            this.B_Buy.UseVisualStyleBackColor = true;
            this.B_Buy.Click += new System.EventHandler(this.B_Buy_Click);
            // 
            // B_Sell
            // 
            this.B_Sell.Enabled = false;
            this.B_Sell.Location = new System.Drawing.Point(276, 570);
            this.B_Sell.Name = "B_Sell";
            this.B_Sell.Size = new System.Drawing.Size(90, 30);
            this.B_Sell.TabIndex = 20;
            this.B_Sell.Text = "Sell";
            this.B_Sell.UseVisualStyleBackColor = true;
            this.B_Sell.Click += new System.EventHandler(this.B_Sell_Click);
            // 
            // B_Drop
            // 
            this.B_Drop.Enabled = false;
            this.B_Drop.Location = new System.Drawing.Point(276, 606);
            this.B_Drop.Name = "B_Drop";
            this.B_Drop.Size = new System.Drawing.Size(90, 30);
            this.B_Drop.TabIndex = 21;
            this.B_Drop.Text = "Drop";
            this.B_Drop.UseVisualStyleBackColor = true;
            this.B_Drop.Click += new System.EventHandler(this.B_Drop_Click);
            // 
            // TB_ItemInfo
            // 
            this.TB_ItemInfo.Enabled = false;
            this.TB_ItemInfo.Location = new System.Drawing.Point(633, 481);
            this.TB_ItemInfo.Multiline = true;
            this.TB_ItemInfo.Name = "TB_ItemInfo";
            this.TB_ItemInfo.Size = new System.Drawing.Size(180, 156);
            this.TB_ItemInfo.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(629, 458);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 19);
            this.label6.TabIndex = 24;
            this.label6.Text = "Item Info";
            // 
            // B_Use
            // 
            this.B_Use.Enabled = false;
            this.B_Use.Location = new System.Drawing.Point(276, 462);
            this.B_Use.Name = "B_Use";
            this.B_Use.Size = new System.Drawing.Size(90, 30);
            this.B_Use.TabIndex = 25;
            this.B_Use.Text = "Use";
            this.B_Use.UseVisualStyleBackColor = true;
            this.B_Use.Click += new System.EventHandler(this.B_Use_Click);
            // 
            // T_SyncTicker
            // 
            this.T_SyncTicker.Interval = 32;
            this.T_SyncTicker.Tick += new System.EventHandler(this.T_SyncTicker_Tick);
            // 
            // LB_Message
            // 
            this.LB_Message.FormattingEnabled = true;
            this.LB_Message.ItemHeight = 19;
            this.LB_Message.Location = new System.Drawing.Point(841, 237);
            this.LB_Message.Name = "LB_Message";
            this.LB_Message.Size = new System.Drawing.Size(250, 175);
            this.LB_Message.TabIndex = 26;
            // 
            // LB_Log
            // 
            this.LB_Log.FormattingEnabled = true;
            this.LB_Log.ItemHeight = 19;
            this.LB_Log.Location = new System.Drawing.Point(841, 481);
            this.LB_Log.Name = "LB_Log";
            this.LB_Log.Size = new System.Drawing.Size(250, 156);
            this.LB_Log.TabIndex = 27;
            // 
            // INV_Their
            // 
            this.INV_Their.Font = new System.Drawing.Font("Consolas", 7F);
            this.INV_Their.Location = new System.Drawing.Point(377, 482);
            this.INV_Their.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.INV_Their.Name = "INV_Their";
            this.INV_Their.Size = new System.Drawing.Size(250, 165);
            this.INV_Their.TabIndex = 29;
            // 
            // INV_Player
            // 
            this.INV_Player.Font = new System.Drawing.Font("Consolas", 7F);
            this.INV_Player.Location = new System.Drawing.Point(17, 482);
            this.INV_Player.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.INV_Player.Name = "INV_Player";
            this.INV_Player.Size = new System.Drawing.Size(250, 165);
            this.INV_Player.TabIndex = 28;
            // 
            // F_Dungeon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 661);
            this.Controls.Add(this.INV_Their);
            this.Controls.Add(this.INV_Player);
            this.Controls.Add(this.LB_Log);
            this.Controls.Add(this.LB_Message);
            this.Controls.Add(this.B_Use);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TB_ItemInfo);
            this.Controls.Add(this.B_Drop);
            this.Controls.Add(this.B_Sell);
            this.Controls.Add(this.B_Buy);
            this.Controls.Add(this.B_Transfer);
            this.Controls.Add(this.B_Send);
            this.Controls.Add(this.TB_Message);
            this.Controls.Add(this.TB_CharacterStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_Nickname);
            this.Controls.Add(this.B_ToggleLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.P_Viewport);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "F_Dungeon";
            this.Text = "Dungeon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dungeon_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel P_Viewport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button B_ToggleLogin;
        private System.Windows.Forms.TextBox TB_Nickname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TB_CharacterStatus;
        private System.Windows.Forms.TextBox TB_Message;
        private System.Windows.Forms.Button B_Send;
        private System.Windows.Forms.Button B_Transfer;
        private System.Windows.Forms.Button B_Buy;
        private System.Windows.Forms.Button B_Sell;
        private System.Windows.Forms.Button B_Drop;
        private System.Windows.Forms.TextBox TB_ItemInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button B_Use;
        private System.Windows.Forms.Timer T_SyncTicker;
        private System.Windows.Forms.ListBox LB_Message;
        private System.Windows.Forms.ListBox LB_Log;
        private Inventory INV_Player;
        private Inventory INV_Their;
    }
}

