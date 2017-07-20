namespace Server
{
    partial class DatabaseHandler
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
            this.tb_Username = new System.Windows.Forms.TextBox();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_Dataname = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_Servername = new System.Windows.Forms.ComboBox();
            this.cb_Auth = new System.Windows.Forms.ComboBox();
            this.cb_Dataname = new System.Windows.Forms.ComboBox();
            this.btn_Next = new System.Windows.Forms.Button();
            this.btn_Back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_Username
            // 
            this.tb_Username.Location = new System.Drawing.Point(163, 167);
            this.tb_Username.Name = "tb_Username";
            this.tb_Username.Size = new System.Drawing.Size(196, 20);
            this.tb_Username.TabIndex = 2;
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(163, 193);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '•';
            this.tb_Password.Size = new System.Drawing.Size(196, 20);
            this.tb_Password.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server Name:";
            // 
            // lb_Dataname
            // 
            this.lb_Dataname.AutoSize = true;
            this.lb_Dataname.Enabled = false;
            this.lb_Dataname.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Dataname.Location = new System.Drawing.Point(64, 109);
            this.lb_Dataname.Name = "lb_Dataname";
            this.lb_Dataname.Size = new System.Drawing.Size(93, 14);
            this.lb_Dataname.TabIndex = 6;
            this.lb_Dataname.Text = "Database Name:";
            this.lb_Dataname.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(64, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "Authentication:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(90, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "Username:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(93, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 14);
            this.label6.TabIndex = 9;
            this.label6.Text = "Password:";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Enabled = false;
            this.btn_Connect.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Connect.Location = new System.Drawing.Point(221, 232);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(75, 23);
            this.btn_Connect.TabIndex = 8;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(302, 232);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Matura MT Script Capitals", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(94, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "SQL Server Connector";
            // 
            // cb_Servername
            // 
            this.cb_Servername.FormattingEnabled = true;
            this.cb_Servername.Location = new System.Drawing.Point(163, 71);
            this.cb_Servername.Name = "cb_Servername";
            this.cb_Servername.Size = new System.Drawing.Size(196, 21);
            this.cb_Servername.TabIndex = 0;
            // 
            // cb_Auth
            // 
            this.cb_Auth.FormattingEnabled = true;
            this.cb_Auth.Location = new System.Drawing.Point(163, 138);
            this.cb_Auth.Name = "cb_Auth";
            this.cb_Auth.Size = new System.Drawing.Size(196, 21);
            this.cb_Auth.TabIndex = 1;
            // 
            // cb_Dataname
            // 
            this.cb_Dataname.Enabled = false;
            this.cb_Dataname.FormattingEnabled = true;
            this.cb_Dataname.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cb_Dataname.Location = new System.Drawing.Point(163, 106);
            this.cb_Dataname.Name = "cb_Dataname";
            this.cb_Dataname.Size = new System.Drawing.Size(196, 21);
            this.cb_Dataname.TabIndex = 5;
            this.cb_Dataname.Visible = false;
            // 
            // btn_Next
            // 
            this.btn_Next.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Next.Location = new System.Drawing.Point(140, 232);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(75, 23);
            this.btn_Next.TabIndex = 4;
            this.btn_Next.Text = "Next";
            this.btn_Next.UseVisualStyleBackColor = true;
            // 
            // btn_Back
            // 
            this.btn_Back.Enabled = false;
            this.btn_Back.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Back.Location = new System.Drawing.Point(59, 232);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(75, 23);
            this.btn_Back.TabIndex = 7;
            this.btn_Back.Text = "Back";
            this.btn_Back.UseVisualStyleBackColor = true;
            this.btn_Back.Visible = false;
            // 
            // DatabaseHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 276);
            this.Controls.Add(this.btn_Back);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.cb_Dataname);
            this.Controls.Add(this.cb_Auth);
            this.Controls.Add(this.cb_Servername);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_Dataname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Password);
            this.Controls.Add(this.tb_Username);
            this.Name = "DatabaseHandler";
            this.Text = "Connect to SQL Server";
            this.Load += new System.EventHandler(this.DatabaseHandler_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_Username;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_Dataname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_Servername;
        private System.Windows.Forms.ComboBox cb_Auth;
        private System.Windows.Forms.ComboBox cb_Dataname;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.Button btn_Back;
    }
}