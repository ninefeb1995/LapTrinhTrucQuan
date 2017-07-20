using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    // Client Management Interface.
    
    public partial class ClientHandler : Form
    {
        private Socket clientSocket; // Client's socket.
        private string clientName; // Client's username.
        private int clientStatus; // Status of client.
        private byte[] clientData = new byte[1024*10000]; // Can receive data up to 10MB.
        private int dataSize; // Size of data received.
        private string clientRequest = null; // Client's request.
        private string timeConnect = null; // Time client establish connection with server.
        private string timeDisconnect = null; // Time client disconnect with server.
        

        private Server server;
        
        #region Constructors, Get, Set Methods.

        public Socket _clientSocket
        {
            get { return clientSocket; }
        }

        public string _clientName
        {
            get { return clientName; }
        }

        public string _clientRequest
        {
            get { return clientRequest; }
        }

        public string _timeConnect
        {
            get { return timeConnect; }
            set { timeConnect = value; }
        }

        public string _timeDisconnect
        {
            get { return timeDisconnect; }
            set { timeDisconnect = value; }
        }

        public int _clientStatus
        {
            get { return clientStatus; }
            set { clientStatus = value; }
        }

        public ClientHandler()
        {
            InitializeComponent();
        }

        public ClientHandler(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
        }

        #endregion

        // For distinguish data without header command, we need declare some signals to seperate proccesses.
        private int onchatting_signal = 0;
        private int onregister_signal = 0;

        private string fusername;

        #region Receive data from client.
        public void RecieveCallback(Server _server)
        {
            try
            {
                clientSocket.BeginReceive(clientData, 0, clientData.Length, SocketFlags.None, new AsyncCallback(_server.RecieveCallback), this);
            }
            catch
            {
                // Can't receive callback from Client.
            }
        }

        // Receive data from client.
        public byte[] Recieve(IAsyncResult ar)
        {
            int bytes = 0;
            try
            {
                bytes = clientSocket.EndReceive(ar);
            }
            catch
            {
                // No data received.
            }
            byte[] data = new byte[bytes];
            Array.Copy(clientData, data, bytes); // Copy data received into var data.
            dataSize = bytes; // Save size of the data receive for further use.
            clientRequest = Encoding.UTF8.GetString(data); // Resolve client's request from data received.
            return data;
        }

        #endregion

        #region Resolve request from received data, then respond to client.

        public void SDRTC() // Server Data-Receive-Transmit Center.
        {
            string[] requestParse = clientRequest.Split('-');

            if (requestParse[0].ToUpper().Equals("HELLOSERVER"))
            {
                // Client send a request to login with his creds.
                // HelloServer-Username=hungnv-Password=password
                string tmpUsername = requestParse[1].Split('=')[1];
                string tmpPassword = requestParse[2].Split('=')[1];

                if (Login(tmpUsername, tmpPassword))
                {
                    // Assign information for authenticated client.
                    this.clientName = tmpUsername;
                    this.timeConnect = DateTime.Now.ToString();
                    this.clientStatus = 1; // Online.
                    UpdateStatus(tmpUsername, "Online");
                    //AddNewClient();
                    // Acknowledge client has logined ok.
                    SendData("Login-OK\n\r");
                }
                else
                {
                    SendData("Login-Failed\n\r"); // Acknowledge client has logined failed.
                    if (clientSocket.Connected) // IF LOGIN FAILED, DISCONNECT THIS CLIENT.
                        clientSocket.Close(); // REQUIRE A RECONNECT NEXT LOGIN ATTEMPT.
                }
            }

            else if (requestParse[0].ToUpper().Equals("GETINFORMATIONME"))
            {
                SendClientInformation(this.clientName);
            }

            else if (requestParse[0].ToUpper().Equals("GETLISTFRIENDS"))
            {
                SendFriendsList(this.clientName);
                SendInbox(this.clientName);
                SendOutbox(this.clientName);
            }

            else if(requestParse[0].ToUpper().Equals("LISTFRIENDS-OK"))
            {
                SendInbox(this.clientName);
            }

            else if(requestParse[0].ToUpper().Equals("INBOX-OK"))
            {
                SendOutbox(this.clientName);
            }

            else if(requestParse[0].ToUpper().Equals("OUTBOX-OK"))
            {

            }

            // STEPS IN REGISTERATION. [*] KEEP A ALIVE CONNECTION WITH CLIENT DURING THIS PROCCESS.
            // SIGNAL A REGISTER PROCESS.
            else if (requestParse[0].ToUpper().Equals("REGISTER"))
            {
                // Client send a request to establish a connection for registeration.
                // WAIT FOR REGISTERATION DATA FROM CLIENT.
                this.onregister_signal = 1; // Start registeration process.
            }

            // STEP 1: CHECK USERNAME.
            else if (requestParse[0].ToUpper().Equals("CHECKUSERNAME"))
            {
                // Client want to check a username is used or not to prepare for his signup.
                // CheckUsername-Username=hungnv
                string tmpUsername = requestParse[1].Split('=')[1];
                if (CheckUsername(tmpUsername))
                    SendData("NotUsed\n\r"); // Username not used.
                else SendData("IsUsed\n\r"); // Username is used.
            }

            // STEP 2: CHECK EMAIL
            else if (requestParse[0].ToUpper().Equals("CHECKEMAIL"))
            {
                // Client want to check his email is valid or not.
                // CheckEmail-Email=hungngv.ht@gmail.com
                string tmpEmail = requestParse[1].Split('=')[1];
                if (CheckEmail(tmpEmail))
                    SendData("NotUsed\n\r");
                else SendData("IsUsed\n\r");
            }

            // ALL ABOVE STEPS PASSED. STEP 3: GET INFORMATION
            else if (requestParse[0].ToUpper().Equals("REGISTERED"))
            {
                // Client send his registeration information.
                // Registered-Username=hungngv-Password=password-Fullname=...
                // GET REGISTERATION DATA AND ASSIGNED THESE FOR THIS CLIENT.
                string tmpUsername = requestParse[1].Split('=')[1];
                string tmpPassword = requestParse[2].Split('=')[1];
                string tmpFullname = requestParse[3].Split('=')[1];
                string tmpBirthday = requestParse[4].Split('=')[1];
                string tmpGender = requestParse[5].Split('=')[1];
                string tmpEmail = requestParse[6].Split('=')[1];
                string tmpCity = requestParse[7].Split('=')[1];
                string tmpLanguage = requestParse[8].Split('=')[1];

                this.clientName = tmpUsername;

                try
                {
                    //if (CheckUsername(tmpUsername) && CheckEmail(tmpEmail))
                    //{
                        Register(tmpUsername, tmpPassword, tmpFullname, tmpGender, tmpBirthday, tmpEmail, tmpCity, tmpLanguage);
                    //}
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
            }

            // SIGNAL A START OF RECEIVING IMAGE.
            else if (requestParse[0].ToUpper().StartsWith("AVATAR"))
            {
                // Still on register signal.
                // Client signal his start of sendding an image to set as his avatar.
                // WAIT FOR AVATAR IMAGE SENT FROM CLIENT.
                // THESE IMGAE DATA BYTES INCLUDE NO HEADER REQUEST, SO LET'S PROCESS IT
                // AS THE LAST CLAUSE OF THIS IF CLAUSE.
            }

            else if (requestParse[0].ToUpper().Equals("HELLOSERVERFORGETPASSWORD"))
            {
                // Client reports that he forget his password and require new one.
                string tmpUsername = requestParse[1].Split('=')[1];
                string tmpEmail = requestParse[2].Split('=')[1];

                if (ForgetPassword(tmpUsername, tmpEmail))
                {
                    string NewPassword = GenerateNewPassword();
                    UpdateUserPassword(tmpUsername, NewPassword);
                    SendNewPassword(tmpUsername, tmpEmail, NewPassword);
                    SendData("ForgetPassword-OK\n\r");
                }
                else SendData("ForgetPassword-Failed\n\r");
            }

            else if (requestParse[0].ToUpper().Equals("CHATTINGWITHFRIEND"))
            {
                // Client send request to chat with another client.
                // Turn on on chatting signal.
                this.onchatting_signal = 1;
                this.fusername = requestParse[1].Split('=')[1]; // Username of the client to chat with.
                SendData("ChattingWithFriendOK-Avatar=\n\r");
                byte[] avatar = DatabaseHandler.LoadImage(this.fusername);
                clientSocket.Send(avatar, avatar.Length, 0);
                SendChatInvitation(this.clientName, this.fusername);
            }

            else if(requestParse[0].ToUpper().Equals("FRIENDWANTTOCHATOK"))
            {
                // Turn on on chatting signal.
                this.onchatting_signal = 1;
                this.fusername = requestParse[1].Split('=')[1];
            }
            // STEP 4: GET IMAGE.
            else
            {
                if (this.onregister_signal == 1)
                {
                    // No request, that means these bytes of data are of an image file.
                    // Get and save it.
                    byte[] image = new byte[dataSize];
                    Array.Copy(clientData, image, dataSize);
                    try
                    {
                        //MemoryStream ms = new MemoryStream(image);
                        //Image _image = Image.FromStream(ms);
                        //string imgname = this.clientName + ".avatar";
                        //_image.Save(imgname, _image.RawFormat);


                        SetAvatar(this.clientName, image);

                        // Register completed. Acknowledge client this completition.
                        SendData("Registered-OK\n\r");
                        // After registeration, close connection to release all resources assigned for this connection.
                        //if (clientSocket.Connected)
                        //clientSocket.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                        SendData("Registered-Failed\n\r"); // Register failed.
                    }

                    this.onregister_signal = 0; // TURN OFF REGISTER SIGNAL. END OF REGISTERATION PROCESS.
                }

                if (this.onchatting_signal == 1)
                {
                    string message = this.clientRequest;
                    SendOnlineMessage(this.fusername, message);
                }
            }
        }

        private void SendData(string data)
        {
            try
            {
                byte[] databytes = Encoding.UTF8.GetBytes(data);
                clientSocket.Send(databytes, databytes.Length, 0);
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        #endregion

        #region Login, Logout.
        private bool Login(string tmpUsername, string tmpPassword)
        {
            String sqlquery = "Select * From Login Where Username = '" + tmpUsername + "' and Password = '" + tmpPassword + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            if (dt.Rows.Count > 0)
                return true;
            else return false;
        }

        private void Logout()
        {
            if (clientSocket.Connected)
            {
                clientSocket.Close();
                RemoveClient();
            }
            else
            {
                RemoveClient();
                this.onchatting_signal = 0; // No chat can be make.
                this.clientStatus = -1; // Offline.
                this.timeDisconnect = DateTime.Now.ToString();
            }
        }

        #endregion

        #region Send client's information.

        private void SendClientInformation(string tmpUsername)
        {
            string sqlquery = "Select Fullname, Status from Users where Username = '" + tmpUsername + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            string Fullname = dt.Rows[0]["Fullname"].ToString();
            string Status = dt.Rows[0]["Status"].ToString();
            byte[] Avatar = DatabaseHandler.LoadImage(tmpUsername);

            string msg = "Information-OK-Name=" + Fullname + "-Status=" + Status + "-Avatar=\n\r";
            SendData(msg);
            
            clientSocket.Send(Avatar, Avatar.Length, 0);
        }
        
        private void SendFriendsList(string tmpUsername)
        {
            List<string> FUsername = new List<string>();
            List<string> Fullname = new List<string>();
            List<string> Gender = new List<string>();
            List<string> Birthday = new List<string>();
            List<string> Email = new List<string>();
            List<string> City = new List<string>();
            List<string> Language = new List<string>();
            List<string> Status = new List<string>();
            //List<byte[]> Avatar = new List<byte[]>();

            string sqlquery = "Select FUsername from Friends where Username = '" + tmpUsername + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            foreach(DataRow dr in dt.Rows)
            {
                sqlquery = "Select Username, Fullname, Gender, Birthday, Email, City, Language, Status from Users " +
                "where Username = '" + (string)dr["FUsername"] + "'";
                DataTable dt0 = DatabaseHandler.ExecuteSqlQuery(sqlquery);
                FUsername.Add((string)dt0.Rows[0]["Username"]);
                Fullname.Add((string)dt0.Rows[0]["Fullname"]);
                Gender.Add((string)dt0.Rows[0]["Gender"]);
                Birthday.Add(dt0.Rows[0]["Birthday"].ToString());
                Email.Add((string)dt0.Rows[0]["Email"]);
                City.Add((string)dt0.Rows[0]["City"]);
                Language.Add((string)dt0.Rows[0]["Language"]);
                Status.Add((string)dt0.Rows[0]["Status"]);
                //Avatar.Add(dbHandler.LoadImage((string)dr["FUsername"]));
            }
            
            string msg = null;
            for(int i = 0; i<FUsername.Count;i++)
            {
                string[] dateParse = Birthday[i].Split(' ');
                string m = dateParse[0].Split('/')[0];
                string d = dateParse[0].Split('/')[1];
                string y = dateParse[0].Split('/')[2];
                string dmy = d + "-" + m + "-" + y;

                msg += "Username=" + FUsername[i] + ",Fullname=" + Fullname[i] + ",Gender=" + Gender[i] + ",Birthday=" + dmy + ",Email=" + Email[i] + ",City=" + City[i] + ",Language=" + Language[i] + ",Status=" + Status[i] + "-";
            }

            string message = "ListFriends-" + msg + "\n\r";

            SendData(message.Remove(message.Length - 1));
        }

        private void SendInbox(string tmpUsername)
        {
            List<string> Senders = new List<string>();
            List<string> Fullnames = new List<string>();
            List<string> Sents = new List<string>();
            List<int> ReadorNots = new List<int>();
            List<string> Contents = new List<string>();

            string sqlquery = "Select Sender, Sent, Contents, ReadorNot from MessageBox where Receiver = '" + tmpUsername + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            foreach(DataRow dr in dt.Rows)
            {
                if(Senders.Count == 0 || !Senders.Contains(dr["Sender"].ToString()))
                    Senders.Add(dr["Sender"].ToString());
                if (Sents.Count == 0 || !Sents.Contains(dr["Sent"].ToString()))
                    Sents.Add(dr["Sent"].ToString());
            }

            string msg = null;

            foreach (var Sender in Senders)
            {
                sqlquery = "Select Fullname from Users where Username = '" + Sender + "'";
                DataTable dt0 = DatabaseHandler.ExecuteSqlQuery(sqlquery);
                foreach (DataRow dr in dt0.Rows)
                    Fullnames.Add(dr["Fullname"].ToString());
                foreach(var Sent in Sents)
                {
                    sqlquery = "Select ReadorNot, Contents from MessageBox where Sender='" + Sender + "' and Sent='" + Sent + "'";
                    DataTable dt1 = DatabaseHandler.ExecuteSqlQuery(sqlquery);
                    foreach(DataRow dr in dt1.Rows)
                    {
                        ReadorNots.Add((int)dr["ReadorNot"]);
                        Contents.Add(dr["Contents"].ToString());
                    }
                }
            }

            
        }

        private void SendOutbox(string tmpUsername)
        {

        }

        #endregion

        #region Register.

        private bool CheckUsername(string tmpUsername)
        {
            string sqlquery = "Select Username from Users where Username ='" + tmpUsername + "'";
            //DataTable dt = dbHandler.ExecuteSqlQuery(sqlquery);
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            if (dt.Rows.Count > 0) return false; // Used.
            else return true; // Not Used.
            
        }

        private bool  CheckEmail(string tmpEmail)
        {
            string sqlquery = "Select Email from Users where Email ='" + tmpEmail + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            if (dt.Rows.Count == 0)
                return true; // Not Used.
            else return false; // Used.
        }

        
        /*private int GenerateMsgBoxID()
        {
            char[] numchars = new char[10];
            numchars =
            "1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[5];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder number = new StringBuilder(10);
            foreach (byte b in data)
            {
                number.Append(numchars[b % (numchars.Length)]);
            }

            return Int32.Parse(number.ToString());
        }*/

        private void Register(
            string tmpUsername, string tmpPassword, string tmpFullname,
            string tmpGender, string tmpBirthday, string tmpEmail,
            string tmpCity, string tmpLanguage, Image Avatar = null
            )
        {
            //int MsgBoxID;
            //while(true)
            //{
            //    // Generate a random message id not duplicated.
            //    MsgBoxID = GenerateMsgBoxID();
            //    string sqlquery = "Select MessageBoxID from Users where MessageBoxID ='" + MsgBoxID + "'";
            //    DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            //    if (dt.Rows.Count == 0) break;
            //}
            if (tmpGender.Equals("Male")) tmpGender = "Nam";
            else tmpGender = "Nữ";
            
            string sqlcmd = "Insert into Login(Username,Password) values('" + tmpUsername + "','" + tmpPassword + "')";
            DatabaseHandler.ExecuteSqlNonQuery(sqlcmd);
            sqlcmd = "Insert into Users(Username,Fullname,Gender,Birthday,Email,City,Language,Status) values('" + tmpUsername +
                "','" + tmpFullname + "','" + tmpGender + "','" + tmpBirthday + "','" + tmpEmail + "','" + tmpCity + "','" + tmpLanguage + "','Offline')";
            DatabaseHandler.ExecuteSqlNonQuery(sqlcmd);
        }

        private void SetAvatar(string tmpUsername, byte[] Avatar)
        {
            DatabaseHandler.InsertImage(tmpUsername, Avatar);
        }

        
        #endregion

        #region Forget Password.
        private bool ForgetPassword(string tmpUsername, string tmpEmail)
        {
            string sqlquery = "Select Username,Email from User where Username = '" + tmpUsername + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            if (dt.Rows.Count > 0)
                return true;
            else return false;
        }

        private string GenerateNewPassword()
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[10];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder newpassword = new StringBuilder(10);
            foreach (byte b in data)
            {
                newpassword.Append(chars[b % (chars.Length)]);
            }

            return newpassword.ToString();
        }
        
        private void UpdateUserPassword(string tmpUsername, string NewPassword)
        {
            string sqlcmd = "Update Login set Password ='" + NewPassword + "' where Usename = '" + tmpUsername + "'";
            DatabaseHandler.ExecuteSqlNonQuery(sqlcmd);
        }

        private void SendNewPassword(string tmpUsername, string tmpEmail, string NewPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ichatsever@gmail.com");
                mail.To.Add(tmpEmail);
                mail.Subject = "Forget password";
                mail.Body = "Hello! You have request a new password for your account " + tmpUsername + " at iChat. Here is your new password: \"" + NewPassword +
                    "\". Please use this password to login your account next time!";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ichatserver@gmail.com", "123@abc");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #endregion

        #region Chatting.

        private bool FriendIsOnline(string fUsername)
        {
            string sqlquery = "Select Status from Users where Username='" + fusername + "'";
            DataTable dt = DatabaseHandler.ExecuteSqlQuery(sqlquery);
            if (dt.Rows.Count == 0)
                return false;
            else
            {
                if (dt.Rows[0]["Status"].ToString().Equals("Online"))
                    return true;
                else return false;
            }
        }

        private Socket friendSocket(string fUsername)
        {
            Socket fsocket = null;

            foreach(ClientHandler client in server.CLIENTS)
            {
                if(client.clientName == fUsername)
                {
                    fsocket = client.clientSocket;
                    break;
                }
            }

            return fsocket;
        }

        private void SendChatInvitation(string tmpUsername, string fUsername)
        {
            if(FriendIsOnline(fUsername))
            {
                DataTable dt = DatabaseHandler.ExecuteSqlQuery("Select Fullname from Users where Username ='" + tmpUsername + "'");
                string fullname = dt.Rows[0]["Fullname"].ToString();
                Socket fsocket = friendSocket(fUsername);
                string msg = "FriendWantToChat-Username=" + tmpUsername + "-Name=" + fullname + "-Avatar=\n\r";
                byte[] msgb = Encoding.UTF8.GetBytes(msg);
                fsocket.Send(msgb, msgb.Length, 0);
                byte[] avatar = DatabaseHandler.LoadImage(tmpUsername);
                fsocket.Send(avatar, avatar.Length, 0);
            }

        }

        private void SendOnlineMessage(string fUsername, string Message)
        {
            if(FriendIsOnline(fUsername))
            {
                Socket fsocket = friendSocket(fUsername);
                byte[] msg = Encoding.UTF8.GetBytes(Message);
                fsocket.Send(msg, msg.Length, 0);
            }
        }

        #endregion

        #region Client's Status.

        private void UpdateStatus(string tmpUsername, string tmpStatus)
        {
            string sqlcmd = "Update Users set Status='" + tmpStatus + "' where Username ='" + tmpUsername + "'";
            DatabaseHandler.ExecuteSqlNonQuery(sqlcmd);
        }

        private void AddNewClient()
        {
            ListViewItem lvItem = new ListViewItem(this.clientName);
            lvItem.SubItems.Add(this.clientSocket.RemoteEndPoint.ToString());
            lvItem.SubItems.Add(this.timeConnect);
            lvItem.SubItems.Add(this.timeDisconnect);

            this.lv_Clients.Items.Add(lvItem);
        }

        private void RemoveClient()
        {
            lv_Clients.Items.RemoveByKey(this.clientName);
            lv_Clients.Refresh();
        }

        #endregion
        #region Control Events.

        private void ClientHandler_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        

        private void serverConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerHandler serverHandler = new ServerHandler();
            serverHandler.Show();
        }

        private void databaseConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseHandler dbHandler = new DatabaseHandler();
            dbHandler.Show();
        }

        private void ClientHandler_Load(object sender, EventArgs e)
        {
            server = new Server();
            server.StartServer(); // Start the server.
            DatabaseHandler.ConnectToDatabaseServer();
        }

        #endregion
    }
}
