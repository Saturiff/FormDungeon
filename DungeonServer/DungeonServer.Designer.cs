namespace DungeonServer
{
    partial class DungeonServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.B_ToggleServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_ServerIP = new System.Windows.Forms.TextBox();
            this.TB_ServerPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LB_PlayerList = new System.Windows.Forms.ListBox();
            this.LB_Log = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // B_ToggleServer
            // 
            this.B_ToggleServer.Location = new System.Drawing.Point(408, 212);
            this.B_ToggleServer.Name = "B_ToggleServer";
            this.B_ToggleServer.Size = new System.Drawing.Size(220, 66);
            this.B_ToggleServer.TabIndex = 1;
            this.B_ToggleServer.Text = "Start server";
            this.B_ToggleServer.UseVisualStyleBackColor = true;
            this.B_ToggleServer.Click += new System.EventHandler(this.B_ToggleServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Player List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Log";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(460, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Server IP";
            // 
            // TB_ServerIP
            // 
            this.TB_ServerIP.Location = new System.Drawing.Point(452, 114);
            this.TB_ServerIP.Name = "TB_ServerIP";
            this.TB_ServerIP.Size = new System.Drawing.Size(128, 26);
            this.TB_ServerIP.TabIndex = 6;
            // 
            // TB_ServerPort
            // 
            this.TB_ServerPort.Enabled = false;
            this.TB_ServerPort.Location = new System.Drawing.Point(452, 180);
            this.TB_ServerPort.Name = "TB_ServerPort";
            this.TB_ServerPort.Size = new System.Drawing.Size(128, 26);
            this.TB_ServerPort.TabIndex = 8;
            this.TB_ServerPort.Text = "8800";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(460, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Server Port";
            // 
            // LB_PlayerList
            // 
            this.LB_PlayerList.FormattingEnabled = true;
            this.LB_PlayerList.ItemHeight = 19;
            this.LB_PlayerList.Location = new System.Drawing.Point(13, 36);
            this.LB_PlayerList.Name = "LB_PlayerList";
            this.LB_PlayerList.Size = new System.Drawing.Size(183, 270);
            this.LB_PlayerList.TabIndex = 12;
            // 
            // LB_Log
            // 
            this.LB_Log.FormattingEnabled = true;
            this.LB_Log.ItemHeight = 19;
            this.LB_Log.Location = new System.Drawing.Point(202, 36);
            this.LB_Log.Name = "LB_Log";
            this.LB_Log.Size = new System.Drawing.Size(183, 270);
            this.LB_Log.TabIndex = 13;
            // 
            // DungeonServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 322);
            this.Controls.Add(this.LB_Log);
            this.Controls.Add(this.LB_PlayerList);
            this.Controls.Add(this.TB_ServerPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TB_ServerIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.B_ToggleServer);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DungeonServer";
            this.Text = "Dungeon Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DungeonServer_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button B_ToggleServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_ServerIP;
        private System.Windows.Forms.TextBox TB_ServerPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox LB_PlayerList;
        private System.Windows.Forms.ListBox LB_Log;
    }
}

