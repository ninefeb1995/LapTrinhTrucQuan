using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
namespace ClientSide
{
    public partial class LogIn : Form
    {
        private String Username="";
        private String Password = "";
        private TcpClient Client;
        private IPEndPoint ip_port_server;
        private StreamReader received;
        private StreamWriter sent;
        private bool CreateAccountUsername_Error,
                        CreateAccountName_Error,
                        CreateAccountEmail_Error, 
                        CreateAccountPassword_Error,
                        CreateAccountRetypePassword_Error,
                        ForgetPasswordUsername_Error,
                        ForgetPasswordEmail_Error;
        private Image Avatar;
        private ToolStripMenuItem menu_check;
        private DialogResult result;
        public TcpClient TCP_Client
        {
            get
            {
                return Client;
            }
        }

        public String UserName
        {
            get
            {
                return Username;
            }
        }

        /// <summary>
        ///  Khởi tạo một Login
        /// </summary>
        public LogIn()
        {
            InitializeComponent();
            InitializeComponent2();
        }

        private void InitializeComponent2()
        {
            Client = new TcpClient();
            ip_port_server = new IPEndPoint(IPAddress.Parse("192.168.137.1"), 1995);

            //this.txtCreateAccountUsername.Paint += new PaintEventHandler(txtCreateAccount_Paint);
            //this.txtCreateAccountName.Paint += new PaintEventHandler(txtCreateAccount_Paint);
            //this.txtCreateAccountEmail.Paint += new PaintEventHandler(txtCreateAccount_Paint);
            //this.txtCreateAccountPassword.Paint += new PaintEventHandler(txtCreateAccount_Paint);
            //this.txtCreateAccountRetypePassword.Paint += new PaintEventHandler(txtCreateAccount_Paint);
            //this.txtForgetPasswordUsername.Paint += new PaintEventHandler(txtCreateAccount_Paint);
            //this.txtForgetPasswordEmail.Paint += new PaintEventHandler(txtCreateAccount_Paint);

            CreateAccountUsername_Error = true;
            CreateAccountName_Error = true;
            CreateAccountEmail_Error = true;
            CreateAccountPassword_Error = true;
            CreateAccountRetypePassword_Error = true;
            ForgetPasswordUsername_Error = true;
            ForgetPasswordEmail_Error = true;

            menu_check = toolconnectionautomaticMenuItem;
            menu_check.Checked = true;


            pcBoxLoading.Image = Properties.Resources.Loading;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(Username == "" || Password == "")
            {
                this.lblResult.Visible = true;
                this.lblResult.Text = "* Username and password can't be null";
                return;
            }

            try
            {
                if(!Client.Connected)
                {
                    //Kết nối đến server với IP và Port xác định
                    Client.Connect(ip_port_server);

                }
                
                if(Client.Connected)
                {
                    //Lấy Stream để nhận, gởi dữ liệu
                    received = new StreamReader(Client.GetStream());
                    sent = new StreamWriter(Client.GetStream());

                    String hashed_password = MD5_Hash(Password);
                    String initialize_to_sendtoServer = "HelloServer-Username=" + Username + "-Password=" + hashed_password;
                
                    //Gởi lời chào, kèm username, password cho server
                    sent.Write(initialize_to_sendtoServer);
                    sent.Flush();

                    //Nhận kết quả từ server, xem có login thành công hay không
                    String Result = received.ReadLine();
                    received.DiscardBufferedData();
                    if(Result.Contains("Login-OK"))
                    {
                        result = System.Windows.Forms.DialogResult.OK;
                        this.Hide();//Đóng form                        
                    }
                    else if(Result.Contains("Login-Failed"))
                    {
                        this.lblResult.Visible = true;              
                        this.lblResult.Text = "Login failed. Username or password is incorrect";
                        //received.Close();
                        //sent.Close();
                        //Client.Close();
                    }
                }
            }
            catch
            {
                
            }
                              
        }

        public new DialogResult ShowDialog()
        {
            base.ShowDialog();
            return result;
        }

        /// <summary>
        ///  Hàm băm MD5 password, giúp an toàn hơn
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public String MD5_Hash(String Data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashed = md5.ComputeHash(Encoding.ASCII.GetBytes(Data));
            return BitConverter.ToString(hashed).Replace("-", "");
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if(txtUserName.Text == "")
            {
                lblResult.Visible = true;
                lblResult.Text = "* Username can't be null";
                return;
            }
            this.Username = txtUserName.Text;
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                lblResult.Visible = true;
                lblResult.Text = "* Password can't be null";
                return;
            }
            this.Password = txtPassword.Text;
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            lblResult.Visible = false;
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            lblResult.Visible = false;
        }

        /// <summary>
        ///  Thoát chương trình khi nhấp chọn Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileexitMenuItem_Click(object sender, EventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            Application.Exit();
        }

        /// <summary>
        ///  Hàm này xử lý khi nút CreateAccount được nhấn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            // Cho bật panel Loading trước
            panelLoading.Dock = DockStyle.Fill;
            panelLoading.BackColor = Color.SkyBlue;
            //if(!Client.Connected)
            //{
            //    //Client.Close();

            //}
            try
            {
                if(!Client.Connected)
                {
                    Client.Connect(ip_port_server);
                }               
                received = new StreamReader(Client.GetStream());
                sent = new StreamWriter(Client.GetStream());

                sent.Write("Register");
                sent.Flush();
                // Tắt panel loading
                panelLoading.Dock = DockStyle.None;
                panelLoading.BackColor = Color.Transparent;
                // Cho bật ra panel xử lý CreateAccount
                panel2.Dock = DockStyle.Fill;
                //panel1.Controls.Add(this.menuLogin);//Xem lại
                this.Size = new Size(655, 600);

                accountcreateoneMenuItem.Enabled = false; // Khi đã nhấp Create Account thì menu này không còn hiệu lực
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Tắt panel loading
                panelLoading.Dock = DockStyle.None;
                panelLoading.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        ///  Hàm gởi Avatar cho server
        /// </summary>
        private void Send_Avatar_to_Server()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter send = new BinaryWriter(Client.GetStream());
            this.Avatar.Save(ms, this.Avatar.RawFormat);          
            send.Write(ms.GetBuffer());
            send.Flush();
            ms.Close();
        }

        /// <summary>
        ///  Hàm này xử lý sự kiện vẽ lại trên textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreateAccount_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            TextBox what_textbox = (TextBox)sender;
            Rectangle area_textbox = new Rectangle(what_textbox.Location.X, what_textbox.Location.Y, what_textbox.Size.Width, what_textbox.Size.Height);
            area_textbox.Inflate(1, 1);
            ControlPaint.DrawBorder(g, area_textbox, Color.Red, ButtonBorderStyle.Solid);
        }


        /// <summary>
        ///  Hàm này xử lý sự kiện Click vào nút IAgree - Send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAgree_Click(object sender, EventArgs e)
        {
            bool Occur_Error = CreateAccountEmail_Error |
                                 CreateAccountName_Error |
                                   CreateAccountPassword_Error |
                                     CreateAccountRetypePassword_Error |
                                       CreateAccountUsername_Error;
            // Nếu có lỗi xảy ra, thì báo ô đỏ từng text box có lỗi, ngược lại thì cho gởi đến server
            if (Occur_Error)
            {
                lblResult_Final.Text = "Occur error !!!";
                lblResult_Final.Visible = true;
                if (CreateAccountUsername_Error)
                {
                    txtCreateAccountUsername.Invalidate();
                }
                if (CreateAccountName_Error)
                {
                    txtCreateAccountName.Invalidate();
                }
                if (CreateAccountEmail_Error)
                {
                    txtCreateAccountEmail.Invalidate();
                }
                if (CreateAccountPassword_Error)
                {
                    txtCreateAccountPassword.Invalidate();
                }
                if (CreateAccountRetypePassword_Error)
                {
                    txtCreateAccountRetypePassword.Invalidate();
                }
                return;
            }

            String hashed_password = MD5_Hash(txtCreateAccountPassword.Text);

            String register = "Registered-Username=" + txtCreateAccountUsername.Text +
                                "-Password=" + hashed_password +
                                "-Name=" + txtCreateAccountName.Text +
                                "-Birthday=" + dataCreateAccountBirthday.Value.ToShortDateString() +
                                "-Gender=" + cmbCreateAccountGender.Text +
                                "-Email=" + txtCreateAccountEmail.Text +
                                "-City=" + txtCreateAccountProvinceCity.Text + // Có thể rỗng ""
                                "-Language=" + cmbCreateAccountLanguage.Text.Remove(0, 1).Trim(); // Có thể rỗng ""
            try
            {
                sent.Write(register);
                sent.Flush();
                sent.Write("Avatar=");
                sent.Flush();
                if (this.Avatar == null)
                {
                    this.Avatar = Properties.Resources.DefaultAvatar;
                }                             
                Send_Avatar_to_Server();
                String result_registered;
                result_registered = received.ReadLine();
                received.DiscardBufferedData();
                if (result_registered.Contains("Registered-OK"))
                {
                    //sent.Close();
                    //received.Close();
                    //Client.Close();
                    MessageBox.Show("Register successful, now you can login !", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Cho xử lý tắt panel đăng ký, đồng thời hiện sẵn username vừa đăng ký vào ô username đăn nhập
                    panel2.Dock = DockStyle.None;
                    //panel2.Controls.Remove(this.menuLogin); //Xem lại
                    this.Size = new Size(553, 313);
                    // Xử lý menu item createone
                    this.accountcreateoneMenuItem.Enabled = true;
                    this.txtUserName.Text = txtCreateAccountUsername.Text;
                  
                    this.txtPassword.Focus();
                }
                else
                {
                    MessageBox.Show("Sorry, Some problems and you can't register rightnow !", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// Nút này đồng nghĩa với nút cancel khi đang ở trong giao diện đăng ký
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIHaveSignin_Click(object sender, EventArgs e)
        {
            panel2.Dock = DockStyle.None;
            //panel2.Controls.Remove(this.menuLogin); //Xem lại
            this.Size = new Size(553, 313);
            // Xử lý menu item createone
            this.accountcreateoneMenuItem.Enabled = true;
            //if (Client.Connected)
            //{
            //    sent.Close();
            //    received.Close();
            //    Client.Close();
            //}          
        }

   

        /// <summary>
        ///  Xử lý lấy ảnh từ máy tính làm Avatar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "JPEG|*.jpg|PNG|*.png|Bitmap|*.bmp";
            open.Multiselect = false;
            open.Title = "Avatar";

            if (open.ShowDialog() == DialogResult.OK)
            {
                this.Avatar = Image.FromFile(open.FileName);
                Bitmap scaled = new Bitmap(this.Avatar, pcboxAvatar.Width, pcboxAvatar.Height);
                pcboxAvatar.Image = scaled;                
            }
        }

        /// <summary>
        ///  Hàm này xử lý khi TextBox Create Account Username mất Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreateAccountUsername_Leave(object sender, EventArgs e)
        {
            String username = txtCreateAccountUsername.Text;

            // Khi ô Text này rỗng thì sẽ thông báo, và không xử lý
            if (username == "")
            {
                ToolTip1.Show("Sorry, The username is not null", txtCreateAccountUsername);
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Username.ForeColor = Color.Red;
                lblResult_Username.Text = "!";
                CreateAccountUsername_Error = true;
                return;
            }

            String checked_result = "";
            sent.Write("CheckUsername-Username=" + username);
            sent.Flush();
            //Chứa kết quả khi server phản hồi
            checked_result = received.ReadLine();
            received.DiscardBufferedData();

            // Username đã tồn tại
            if (checked_result.Contains("IsUsed"))
            {
                ToolTip1.Show("Sorry, it looks like " + username + " belongs to an existing account !", txtCreateAccountUsername);
                CreateAccountUsername_Error = true;
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Username.ForeColor = Color.Red;
                lblResult_Username.Text = "!!!";
                return;
            }
            else if (checked_result.Contains("NotUsed"))
            {
                CreateAccountUsername_Error = false;
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Username.Text = "o";
                lblResult_Username.ForeColor = Color.Green;
                return;
            }
            CreateAccountUsername_Error = false;
        }

        /// <summary>
        ///  Hàm này xử lý khi TextBox Create Account Username được Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreateAccountUsername_Enter(object sender, EventArgs e)
        {
            lblResult_Username.Text = "*";
            lblResult_Username.ForeColor = Color.Red;
            CreateAccountUsername_Error = false;
        }
        /// <summary>
        ///  Xử lý Username không chứa khoảng trắng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCreateAccountUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void txtCreateAccountName_Enter(object sender, EventArgs e)
        {
            lblResult_Name.Text = "*";
            lblResult_Name.ForeColor = Color.Red;
            CreateAccountName_Error = false;
        }

        private void txtCreateAccountName_Leave(object sender, EventArgs e)
        {
            String name = txtCreateAccountName.Text.Trim();

            // Khi ô Text này rỗng thì sẽ thông báo, và không xử lý
            if (name == "")
            {
                ToolTip1.Show("Sorry, The name is not null", txtCreateAccountName);              
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Name.ForeColor = Color.Red;
                lblResult_Name.Text = "!";
                CreateAccountName_Error = true;
                return;
            }
            lblResult_Name.ForeColor = Color.Green;
            lblResult_Name.Text = "o";
            CreateAccountName_Error = false;
        }

        private void txtCreateAccountEmail_Leave(object sender, EventArgs e)
        {
            String email = txtCreateAccountEmail.Text;

            // Khi ô Text này rỗng thì sẽ thông báo, và không xử lý
            if (email == "")
            {
                ToolTip1.Show("Sorry, The email is not null", txtCreateAccountEmail);
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Email.ForeColor = Color.Red;
                lblResult_Email.Text = "!";
                CreateAccountEmail_Error = true;
                return;
            }

            Regex match_email = new Regex(@"^[\w\.]+@[\w]+\.[\w]+"); // ^ bắt đầu bằng chữ, [\w] word (chữ, số, _) \. dấu chấm, + có một hoặc nhiều

            if(!match_email.IsMatch(email))
            {
                ToolTip1.Show("Sorry, The email is not match format", txtCreateAccountEmail);
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Email.ForeColor = Color.Red;
                lblResult_Email.Text = "!";
                CreateAccountEmail_Error = true;
                return;
            }

            lblResult_Email.ForeColor = Color.Green;
            lblResult_Email.Text = "o";

            String checked_result = "";
            sent.Write("CheckEmail-Email=" + email);
            sent.Flush();
            //Chứa kết quả khi server phản hồi
            checked_result = received.ReadLine();
            received.DiscardBufferedData();

            // Username đã tồn tại
            if (checked_result.Contains("IsUsed"))
            {
                ToolTip1.Show("Sorry, it looks like " + email + " belongs to an existing !", txtCreateAccountEmail);
                CreateAccountEmail_Error = true;
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Email.ForeColor = Color.Red;
                lblResult_Email.Text = "!!!";
                return;
            }
            else if (checked_result.Contains("NotUsed"))
            {
                CreateAccountEmail_Error = false;
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Email.Text = "o";
                lblResult_Email.ForeColor = Color.Green;
                return;
            }

        }

        private void txtCreateAccountEmail_Enter(object sender, EventArgs e)
        {
            lblResult_Email.Text = "*";
            lblResult_Email.ForeColor = Color.Red;
            CreateAccountEmail_Error = false;
        }

        private void txtCreateAccountEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void txtCreateAccountPassword_Leave(object sender, EventArgs e)
        {
            String password = txtCreateAccountPassword.Text;

            // Khi ô Text này rỗng thì sẽ thông báo, và không xử lý
            if (password == "")
            {
                ToolTip1.Show("Sorry, The password is not null", txtCreateAccountPassword);
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Password.ForeColor = Color.Red;
                lblResult_Password.Text = "!";
                CreateAccountPassword_Error = true;
                return;
            }

            if (password.Length <= 5)
            {
                ToolTip1.Show("Sorry, The password is least 6 characters", txtCreateAccountPassword);
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_Password.ForeColor = Color.Red;
                lblResult_Password.Text = "!";
                CreateAccountPassword_Error = true;
                return;
            }
            lblResult_Password.ForeColor = Color.Green;
            lblResult_Password.Text = "o";
            CreateAccountPassword_Error = false;
        }

        private void txtCreateAccountPassword_Enter(object sender, EventArgs e)
        {
            lblResult_Password.ForeColor = Color.Red;
            lblResult_Password.Text = "*";
            CreateAccountPassword_Error = false;
        }

        private void txtCreateAccountRetypePassword_Leave(object sender, EventArgs e)
        {
            String retype_password = txtCreateAccountRetypePassword.Text;

            // Khi ô Text này rỗng thì sẽ thông báo, và không xử lý
            if (retype_password == "")
            {
                ToolTip1.Show("Sorry, The password is not null", txtCreateAccountRetypePassword);
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_RetypePassword.ForeColor = Color.Red;
                lblResult_RetypePassword.Text = "!";
                CreateAccountRetypePassword_Error = true;
                return;
            }

            if (!retype_password.Equals(txtCreateAccountPassword.Text))
            {
                //Xử lý cho thay đổi label kết quả kế bên textbox
                lblResult_RetypePassword.ForeColor = Color.Red;
                lblResult_RetypePassword.Text = "!";
                lblResult_RetypePassword_.Text = "Retype password is not matched";
                lblResult_RetypePassword_.Visible = true;
                CreateAccountRetypePassword_Error = true;
                return;
            }
            CreateAccountRetypePassword_Error = false;
            lblResult_RetypePassword.ForeColor = Color.Green;
            lblResult_RetypePassword.Text = "o";
            lblResult_RetypePassword_.ForeColor = Color.Green;
            lblResult_RetypePassword_.Text = "Retype password is matched";
            lblResult_RetypePassword_.Visible = true;
        }

        private void txtCreateAccountRetypePassword_Enter(object sender, EventArgs e)
        {
            lblResult_RetypePassword.Text = "*";
            lblResult_RetypePassword.ForeColor = Color.Red;
            CreateAccountRetypePassword_Error = false;
            lblResult_RetypePassword_.Visible = false;
        }

        private void accountcreateoneMenuItem_Click(object sender, EventArgs e)
        {
            btnCreateAccount.PerformClick();
        }

        private void accountforgetpasswordMenuItem_Click(object sender, EventArgs e)
        {
            //if(Client.Connected)
            //{
            //    Client.Close(); // Mọi trường hợp đóng kết nối trước
            //}

            // Bật panel loading
            panelLoading.Dock = DockStyle.Fill;
            panelLoading.BackColor = Color.SkyBlue;

            Thread new_Thread = new Thread(() =>
            {
            Label:
                try
                {
                    if (!Client.Connected)
                    {
                        Client.Connect(ip_port_server);
                        
                    }
                }
                catch (Exception ex)
                {
                    if (DialogResult.Retry == MessageBox.Show(ex.Message, "Information", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information))
                    {
                        goto Label;
                    }
                }
                Thread.CurrentThread.Abort();
            });
            new_Thread.Start();
            new_Thread.Join();
            Label1:
            try
            {
                if (!Client.Connected)
                {
                    Client.Connect(ip_port_server);
                }
                sent = new StreamWriter(Client.GetStream());
                received = new StreamReader(Client.GetStream());

                // Tắt panel loading
                panelLoading.Dock = DockStyle.None;
                panelLoading.BackColor = Color.Transparent;
                // Bật panel 3
                panel3.Dock = DockStyle.Fill;
                // Cho Disabled 2 menu này
                accountforgetpasswordMenuItem.Enabled = false;
                accountcreateoneMenuItem.Enabled = false;
                
            }
            catch (Exception ex)
            {
                if (DialogResult.Retry == MessageBox.Show(ex.Message, "Information", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information))
                {
                    goto Label1;
                }
                // Tắt panel loading
                panelLoading.Dock = DockStyle.None;
                panelLoading.BackColor = Color.Transparent;
                // Cho Enabled 2 menu này
                accountforgetpasswordMenuItem.Enabled = true;
                accountcreateoneMenuItem.Enabled = true;
            }
           
        }
       

        private void txtForgetPasswordUsername_Leave(object sender, EventArgs e)
        {
            if(txtForgetPasswordUsername.Text == "")
            {
                ForgetPasswordUsername_Error = true;
                return;
            }
            ForgetPasswordUsername_Error = false;
        }

        private void txtForgetPasswordEmail_Leave(object sender, EventArgs e)
        {
            if (txtForgetPasswordEmail.Text == "")
            {
                ForgetPasswordEmail_Error = true;
                return;
            }
            ForgetPasswordEmail_Error = false;
        }

        /// <summary>
        ///  Hàm này xử lý sự kiện khi mà nút Continue trong panel Forget Password, được Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForgetPasswordContinue_Click(object sender, EventArgs e)
        {
            String username = txtForgetPasswordUsername.Text,
                   email = txtForgetPasswordEmail.Text;
            if (username == "" || email == "")
            {
                lblForgetPassword_Result.Text = "Please enter Username and Email associated with this username !";
                lblForgetPassword_Result.ForeColor = Color.Red;
                lblForgetPassword_Result.BackColor = SystemColors.Control;
                lblForgetPassword_Result.Visible = true;
                if (ForgetPasswordUsername_Error)
                {
                    txtForgetPasswordUsername.Invalidate();
                }
                if(ForgetPasswordEmail_Error)
                {
                    txtForgetPasswordEmail.Invalidate();
                }
                return;
            }
            String send_to_server = "HelloServerForgetPassword-Username=" +
                                        txtForgetPasswordUsername.Text +
                                          "-Email=" +
                                             txtForgetPasswordEmail.Text;

            sent.Write(send_to_server);

            String receive_from_server = received.ReadLine();
            received.DiscardBufferedData();

            if (receive_from_server.Contains("ForgetPassword-OK")) // Server trả về rằng, username password đã khớp, và password mới đã được gởi tới email này
            {
                //Client.Close();
                //sent.Close();
                //received.Close();
                panel3.Dock = DockStyle.None;
                accountforgetpasswordMenuItem.Enabled = true;
                accountcreateoneMenuItem.Enabled = true;
                MessageBox.Show("Everything's OK, please check your email to get new password !", "Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (receive_from_server.Contains("ForgetPassword-Failed"))
            {
                lblForgetPassword_Result.Text = "We didn't recognize these sign in details, please check and try again !";
                lblForgetPassword_Result.ForeColor = Color.Black;
                lblForgetPassword_Result.BackColor = Color.Tomato;
                lblForgetPassword_Result.Visible = true;
            }
        }

        /// <summary>
        ///  Hàm này xử lý sự kiện khi mà nút Cancel trong panel Forget Password được Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForgetPasswordCancel_Click(object sender, EventArgs e)
        {
            panel3.Dock = DockStyle.None;
            accountforgetpasswordMenuItem.Enabled = true;
            accountcreateoneMenuItem.Enabled = true;
            //if (Client.Connected)
            //{
            //    Client.Close();
            //    sent.Close();
            //    received.Close();
            //}
        }

        /// <summary>
        ///  Hàm này xử lý sự kiện menu item Manual được click, menu này có nhiệm vụ thiết đặt IP, Port server thủ công
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolconnectionmanualMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionManual con_manual = new ConnectionManual();
            if(con_manual.ShowDialog() == ConnectionManualResult.Save)
            {
                ip_port_server.Address = con_manual.Ip;
                ip_port_server.Port = con_manual.Port;
                menu_check.Checked = false;
                menu_check = (ToolStripMenuItem)sender;
                menu_check.Checked = true;
                if (Client.Connected)
                {
                    Client.Close();
                }
                Client.Connect(ip_port_server);
                sent = new StreamWriter(Client.GetStream());
                received = new StreamReader(Client.GetStream());
            }
        }

        /// <summary>
        ///  Hàm này xử lý sự kiện menu item Automatic được click, menu này có nhiệm vụ thiết đặt IP, Port server tự động
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolconnectionautomaticMenuItem_Click(object sender, EventArgs e)
        {
            ip_port_server.Address = IPAddress.Loopback;
            ip_port_server.Port = 1995;
            menu_check.Checked = false;
            menu_check = (ToolStripMenuItem)sender;
            menu_check.Checked = true;
            if (Client.Connected)
            {
                Client.Close();
            }
            Client.Connect(ip_port_server);
            sent = new StreamWriter(Client.GetStream());
            received = new StreamReader(Client.GetStream());
        }

        /// <summary>
        ///  Xử lý đóng các kết nối nếu có, trước khi đóng form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    result = System.Windows.Forms.DialogResult.Cancel;
        //    if (Client.Connected)
        //    {
        //        received.Close();
        //        sent.Close();
        //        Client.Close();
        //    }
        //    Application.Exit();
        //}

        

        
    }
}
