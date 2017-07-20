namespace Server
{
    partial class Server
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
            this.tb_User = new System.Windows.Forms.TextBox();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.Header = new System.Windows.Forms.Label();
            this.btn_Login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_User
            // 
            this.tb_User.Location = new System.Drawing.Point(53, 46);
            this.tb_User.Name = "tb_User";
            this.tb_User.Size = new System.Drawing.Size(224, 20);
            this.tb_User.TabIndex = 0;
            this.tb_User.Text = "Admin";
            this.tb_User.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_User_KeyDown);
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(53, 72);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '•';
            this.tb_Password.Size = new System.Drawing.Size(224, 20);
            this.tb_Password.TabIndex = 1;
            this.tb_Password.Text = "admin";
            this.tb_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_Password_KeyDown);
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.Font = new System.Drawing.Font("Matura MT Script Capitals", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Header.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Header.Location = new System.Drawing.Point(49, 9);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(228, 20);
            this.Header.TabIndex = 3;
            this.Header.Text = "iChat Server Authentication";
            // 
            // btn_Login
            // 
            this.btn_Login.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Login.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btn_Login.Location = new System.Drawing.Point(203, 105);
            this.btn_Login.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(74, 21);
            this.btn_Login.TabIndex = 2;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            this.btn_Login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_Login_KeyDown);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 138);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.tb_Password);
            this.Controls.Add(this.tb_User);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Server";
            this.Text = "Authentication";
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_User;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Label Header;
        private System.Windows.Forms.Button btn_Login;
    }
}

