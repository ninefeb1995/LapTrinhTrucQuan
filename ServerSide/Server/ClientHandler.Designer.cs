namespace Server
{
    partial class ClientHandler
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
            this.mns_Menu = new System.Windows.Forms.MenuStrip();
            this.taskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hibernateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.securityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lv_Clients = new System.Windows.Forms.ListView();
            this.col_Clientname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_IPAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_timeConnect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_timeDisconnect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pb_Status = new System.Windows.Forms.ProgressBar();
            this.mns_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mns_Menu
            // 
            this.mns_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.taskToolStripMenuItem,
            this.securityToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.mns_Menu.Location = new System.Drawing.Point(0, 0);
            this.mns_Menu.Name = "mns_Menu";
            this.mns_Menu.Size = new System.Drawing.Size(484, 24);
            this.mns_Menu.TabIndex = 0;
            this.mns_Menu.Text = "menuStrip1";
            // 
            // taskToolStripMenuItem
            // 
            this.taskToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hibernateToolStripMenuItem,
            this.shutdownToolStripMenuItem});
            this.taskToolStripMenuItem.Name = "taskToolStripMenuItem";
            this.taskToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.taskToolStripMenuItem.Text = "Task";
            // 
            // hibernateToolStripMenuItem
            // 
            this.hibernateToolStripMenuItem.Name = "hibernateToolStripMenuItem";
            this.hibernateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F3)));
            this.hibernateToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.hibernateToolStripMenuItem.Text = "Hibernate";
            // 
            // shutdownToolStripMenuItem
            // 
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.shutdownToolStripMenuItem.Text = "Shutdown";
            // 
            // securityToolStripMenuItem
            // 
            this.securityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adminToolStripMenuItem1,
            this.iPFilterToolStripMenuItem});
            this.securityToolStripMenuItem.Name = "securityToolStripMenuItem";
            this.securityToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.securityToolStripMenuItem.Text = "Security";
            // 
            // adminToolStripMenuItem1
            // 
            this.adminToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPasswordToolStripMenuItem});
            this.adminToolStripMenuItem1.Name = "adminToolStripMenuItem1";
            this.adminToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.adminToolStripMenuItem1.Text = "Admin";
            // 
            // newPasswordToolStripMenuItem
            // 
            this.newPasswordToolStripMenuItem.Name = "newPasswordToolStripMenuItem";
            this.newPasswordToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.newPasswordToolStripMenuItem.Text = "Change Password";
            // 
            // iPFilterToolStripMenuItem
            // 
            this.iPFilterToolStripMenuItem.Name = "iPFilterToolStripMenuItem";
            this.iPFilterToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.iPFilterToolStripMenuItem.Text = "IP Filter";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverConfigurationToolStripMenuItem,
            this.databaseConfigurationToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // serverConfigurationToolStripMenuItem
            // 
            this.serverConfigurationToolStripMenuItem.Name = "serverConfigurationToolStripMenuItem";
            this.serverConfigurationToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.serverConfigurationToolStripMenuItem.Text = "Server Connection Editor";
            this.serverConfigurationToolStripMenuItem.Click += new System.EventHandler(this.serverConfigurationToolStripMenuItem_Click);
            // 
            // databaseConfigurationToolStripMenuItem
            // 
            this.databaseConfigurationToolStripMenuItem.Name = "databaseConfigurationToolStripMenuItem";
            this.databaseConfigurationToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.databaseConfigurationToolStripMenuItem.Text = "SQL Server Connector";
            this.databaseConfigurationToolStripMenuItem.Click += new System.EventHandler(this.databaseConfigurationToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F2)));
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.logoutToolStripMenuItem.Text = "Logout";
            // 
            // lv_Clients
            // 
            this.lv_Clients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Clientname,
            this.col_IPAddress,
            this.col_timeConnect,
            this.col_timeDisconnect});
            this.lv_Clients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_Clients.Font = new System.Drawing.Font("Arial Unicode MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lv_Clients.FullRowSelect = true;
            this.lv_Clients.GridLines = true;
            this.lv_Clients.Location = new System.Drawing.Point(0, 24);
            this.lv_Clients.Name = "lv_Clients";
            this.lv_Clients.Size = new System.Drawing.Size(484, 237);
            this.lv_Clients.TabIndex = 1;
            this.lv_Clients.UseCompatibleStateImageBehavior = false;
            this.lv_Clients.View = System.Windows.Forms.View.Details;
            // 
            // col_Clientname
            // 
            this.col_Clientname.Text = "Client Name";
            this.col_Clientname.Width = 121;
            // 
            // col_IPAddress
            // 
            this.col_IPAddress.Text = "IP Address";
            this.col_IPAddress.Width = 121;
            // 
            // col_timeConnect
            // 
            this.col_timeConnect.Text = "Start";
            this.col_timeConnect.Width = 121;
            // 
            // col_timeDisconnect
            // 
            this.col_timeDisconnect.Text = "End";
            this.col_timeDisconnect.Width = 121;
            // 
            // pb_Status
            // 
            this.pb_Status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pb_Status.Location = new System.Drawing.Point(0, 239);
            this.pb_Status.Name = "pb_Status";
            this.pb_Status.Size = new System.Drawing.Size(484, 22);
            this.pb_Status.TabIndex = 2;
            // 
            // ClientHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.pb_Status);
            this.Controls.Add(this.lv_Clients);
            this.Controls.Add(this.mns_Menu);
            this.MainMenuStrip = this.mns_Menu;
            this.Name = "ClientHandler";
            this.Text = "Server Manager Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientHandler_FormClosing);
            this.Load += new System.EventHandler(this.ClientHandler_Load);
            this.mns_Menu.ResumeLayout(false);
            this.mns_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mns_Menu;
        private System.Windows.Forms.ListView lv_Clients;
        private System.Windows.Forms.ColumnHeader col_Clientname;
        private System.Windows.Forms.ColumnHeader col_IPAddress;
        private System.Windows.Forms.ColumnHeader col_timeConnect;
        private System.Windows.Forms.ColumnHeader col_timeDisconnect;
        private System.Windows.Forms.ProgressBar pb_Status;
        private System.Windows.Forms.ToolStripMenuItem taskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem securityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hibernateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPFilterToolStripMenuItem;
    }
}