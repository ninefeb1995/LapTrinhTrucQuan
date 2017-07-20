namespace ClientSide
{
    partial class ChattingFromFriendList
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.gBox1 = new System.Windows.Forms.GroupBox();
            this.lstViewView = new System.Windows.Forms.ListView();
            this.btnHideImageMe = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtBoxType = new System.Windows.Forms.TextBox();
            this.btnHideImageFriend = new System.Windows.Forms.Button();
            this.pcBoxAvatarMe = new System.Windows.Forms.PictureBox();
            this.pcBoxAvatarFriend = new System.Windows.Forms.PictureBox();
            this.gBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcBoxAvatarMe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBoxAvatarFriend)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(631, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(631, 40);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // gBox1
            // 
            this.gBox1.Controls.Add(this.lstViewView);
            this.gBox1.Controls.Add(this.btnHideImageMe);
            this.gBox1.Controls.Add(this.btnHideImageFriend);
            this.gBox1.Controls.Add(this.pcBoxAvatarMe);
            this.gBox1.Controls.Add(this.pcBoxAvatarFriend);
            this.gBox1.Controls.Add(this.btnSend);
            this.gBox1.Controls.Add(this.txtBoxType);
            this.gBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBox1.Location = new System.Drawing.Point(0, 64);
            this.gBox1.Name = "gBox1";
            this.gBox1.Size = new System.Drawing.Size(631, 337);
            this.gBox1.TabIndex = 3;
            this.gBox1.TabStop = false;
            this.gBox1.Text = "Name";
            // 
            // lstViewView
            // 
            this.lstViewView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lstViewView.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstViewView.Location = new System.Drawing.Point(6, 19);
            this.lstViewView.Name = "lstViewView";
            this.lstViewView.OwnerDraw = true;
            this.lstViewView.ShowGroups = false;
            this.lstViewView.Size = new System.Drawing.Size(446, 225);
            this.lstViewView.TabIndex = 6;
            this.lstViewView.UseCompatibleStateImageBehavior = false;
            this.lstViewView.View = System.Windows.Forms.View.Details;
            this.lstViewView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lstViewView_DrawColumnHeader);
            this.lstViewView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lstViewView_DrawItem);
            // 
            // btnHideImageMe
            // 
            this.btnHideImageMe.Location = new System.Drawing.Point(600, 200);
            this.btnHideImageMe.Name = "btnHideImageMe";
            this.btnHideImageMe.Size = new System.Drawing.Size(25, 25);
            this.btnHideImageMe.TabIndex = 5;
            this.btnHideImageMe.UseVisualStyleBackColor = true;
            this.btnHideImageMe.Click += new System.EventHandler(this.btnHideImageMe_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSend.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSend.Location = new System.Drawing.Point(377, 250);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 75);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "&Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtBoxType
            // 
            this.txtBoxType.Location = new System.Drawing.Point(6, 250);
            this.txtBoxType.Multiline = true;
            this.txtBoxType.Name = "txtBoxType";
            this.txtBoxType.Size = new System.Drawing.Size(368, 75);
            this.txtBoxType.TabIndex = 2;
            // 
            // btnHideImageFriend
            // 
            this.btnHideImageFriend.Location = new System.Drawing.Point(600, 121);
            this.btnHideImageFriend.Name = "btnHideImageFriend";
            this.btnHideImageFriend.Size = new System.Drawing.Size(25, 25);
            this.btnHideImageFriend.TabIndex = 5;
            this.btnHideImageFriend.UseVisualStyleBackColor = true;
            this.btnHideImageFriend.Click += new System.EventHandler(this.btnHideImageFriend_Click);
            // 
            // pcBoxAvatarMe
            // 
            this.pcBoxAvatarMe.Location = new System.Drawing.Point(458, 231);
            this.pcBoxAvatarMe.Name = "pcBoxAvatarMe";
            this.pcBoxAvatarMe.Size = new System.Drawing.Size(167, 96);
            this.pcBoxAvatarMe.TabIndex = 4;
            this.pcBoxAvatarMe.TabStop = false;
            // 
            // pcBoxAvatarFriend
            // 
            this.pcBoxAvatarFriend.Location = new System.Drawing.Point(458, 19);
            this.pcBoxAvatarFriend.Name = "pcBoxAvatarFriend";
            this.pcBoxAvatarFriend.Size = new System.Drawing.Size(167, 96);
            this.pcBoxAvatarFriend.TabIndex = 4;
            this.pcBoxAvatarFriend.TabStop = false;
            // 
            // ChattingFromFriendList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 401);
            this.Controls.Add(this.gBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ChattingFromFriendList";
            this.Text = "ChattingFromFriendList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChattingFromFriendList_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChattingFromFriendList_KeyPress);
            this.gBox1.ResumeLayout(false);
            this.gBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcBoxAvatarMe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBoxAvatarFriend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.GroupBox gBox1;
        private System.Windows.Forms.Button btnHideImageMe;
        private System.Windows.Forms.Button btnHideImageFriend;
        private System.Windows.Forms.PictureBox pcBoxAvatarMe;
        private System.Windows.Forms.PictureBox pcBoxAvatarFriend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtBoxType;
        private System.Windows.Forms.ListView lstViewView;


    }
}