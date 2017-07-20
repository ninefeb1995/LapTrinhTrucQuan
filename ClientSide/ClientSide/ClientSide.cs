using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace ClientSide
{
    public partial class ClientSide : Form
    {
        private TcpClient TCP_Connection;
        private StreamReader sr;
        private StreamWriter sw;
        //Lưu trữ status
        private String myStatus;
        //Lưu trữ Avatar;
        private Image myAvatar;
        //Lưu trữ Name
        private String myName;
        //Lưu trữ Username;
        private String myUsername;
        // Lưu trữ default avatar
        private Image DefaultAvatar;
        // Danh sách hình ảnh của bạn bè được lưu trữ vào list này
        private ImageList listImageFriend;
        // Lưu trữ Item vừa được chọn trong ListFriend
        private ListViewItem selected_Friend;
        // Để check trạng thái cho phù hợp
        private ToolStripMenuItem ChangeStatusMenuItem_Select;
        // Lưu trữ danh sách đang chat
        private List<ChattingFromFriendList> listChatting;
        // Lưu trữ danh sách các Friends đã lấy thông tin từ server tránh lấy lại
        private Hashtable listisGotInfoFriend;
        // Tạo một đối tượng Login
        private LogIn login;

        /// <summary>
        ///  Khởi tạo một đối tượng ClientSide mới
        /// </summary>
        public ClientSide()
        {
            InitializeComponent();
            InitializeComponent2();
        }

        /// <summary>
        /// Hàm khởi tạo các thành phần thứ 2
        /// </summary>
        private void InitializeComponent2()
        {
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            //DefaultAvatar = Image.FromFile("DefaultAvatar");
            DefaultAvatar = Properties.Resources.DefaultAvatar;
            listImageFriend = new ImageList();
            listChatting = new List<ChattingFromFriendList>();
            listisGotInfoFriend = new Hashtable();
            // Khởi gán hình ảnh(icon) cho các Menu Item (Chat --> ChangeStatus)
            ChatChangeStatusOnlineMenuItem.Image = lstImageIconforMenuItem.Images["OnlineStatus"];
            ChatChangeStatusBusyMenuItem.Image = lstImageIconforMenuItem.Images["BusyStatus"];
            ChatChangeStatusOfflineMenuItem.Image = lstImageIconforMenuItem.Images["OfflineStatus"];

            // Khởi gán cho picbox loading và main loading
            //pcBoxLoading.Image = new Bitmap(Properties.Resources.Loading, pcBoxLoading.Width, pcBoxLoading.Height);
            pcBoxLoading.Image = Properties.Resources.Loading;
            pcBoxMainLoading.Image = Properties.Resources.Loading;

            
        }

        /// <summary>
        ///  Hàm xử lý tính toán locaion của picture box cho phù hợp
        /// </summary>
        private void ComputePictureBoxLoadingScaledLocation()
        {
            // Cân bằng picturebox main loading vào giữa
            int X = panelMainLoading.Width / 2;
            int Y = panelMainLoading.Height / 2;

            int newX_Location = X - (pcBoxMainLoading.Width / 2);
            int newY_Location = Y - (pcBoxMainLoading.Height / 2);
            pcBoxMainLoading.Location = new Point(newX_Location, newY_Location);
        }

        private void ClientSide_Shown(object sender, EventArgs e)
        {
            panelMainLoading.Dock = DockStyle.Fill;
            ComputePictureBoxLoadingScaledLocation();
            // Khởi gán đối tượng Login
            this.login = new LogIn();
            if (DialogResult.OK == this.login.ShowDialog())
            {
                // Lấy kết nối TCP từ Login Successful
                this.TCP_Connection = login.TCP_Client;

                // Lấy myUsername từ Login
                this.myUsername = this.login.UserName;

                // Lấy lấy thông tin đầy đủ
                Thread init_Information = new Thread(this.Load_AllContent);
                init_Information.Start();
            }
            else
            {
                Application.Exit();
            }         
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //Xử lý kéo Spliter cho hợp lý
        }

        /// <summary>
        ///  Hàm này lấy dữ liệu từ server và load tất cả để hiện ra một giao diện hoàn chỉnh sau đăng nhập
        /// </summary>
        private void Load_AllContent()
        {
            String received;
            sr = new StreamReader(TCP_Connection.GetStream());
            sw = new StreamWriter(TCP_Connection.GetStream());

            // Lấy thông tin bản thân
            //Server gởi lại
            //Gởi đợt 1: “InformationOK-Name=name,Status=status,Avatar=” 
            //Gởi đợt 2: byte[]
            sw.Write("GetInformationMe");
            sw.Flush();
            received = sr.ReadLine();
            sr.DiscardBufferedData();
            Thread GetInformationMe = new Thread(new ParameterizedThreadStart(handle_received_LoadAllContent));
            GetInformationMe.Start(received);
            //myAvatar = Image.FromStream(TCP_Connection.GetStream());
            myAvatar = DefaultAvatar;

            // Lấy listfried
            //server lúc này gởi:
            //listfriends-username=username,name=name,status=status-….-
            //(trong đó status có 3 trạng thái online, offline, busy)
            sw.Write("GetListFriends");
            sw.Flush();
            received = sr.ReadLine();
            sr.DiscardBufferedData();
            Thread GetListFriends = new Thread(new ParameterizedThreadStart(handle_received_LoadAllContent));
            GetListFriends.Start(received);

            // Lấy inbox
            // Inbox-Username=username,Name=name,ReadStatus=status,Content=date1_content1:date2_content2:…-…- 
            // (Trong đó status là 1:đã đọc, 0:chưa đọc)	
            // (date1 ứng với content1, nội dung của content1 có thể có nhiều nội dung khác nhau, mỗi nội dung phân nhau bởi dấu \r\n, và lưu ý là chúng đã sắp xếp theo thời gian)
            // VD: Inbox-Username=hung,Name=Hung Viet Nguyen,ReadStatus=0,Content=01/06/2016_Hello Mình là Hùng\r\nChúng ta kết bạn nói chuyện nhé\r\n:02/06/2016_Bạn có online k?\r\n-…-
            sw.Write("ListFriends-OK");
            sw.Flush();
            received = sr.ReadLine();
            sr.DiscardBufferedData();
            Thread Inbox = new Thread(new ParameterizedThreadStart(handle_received_LoadAllContent));
            Inbox.Start(received);

            //// Lấy outbox
            ////Outbox-Username=username,Name=name,Content=date1_content1:date2_content2:…-…-
            //sw.Write("Inbox-OK");
            //sw.Flush();
            //received = sr.ReadLine();
            //sr.DiscardBufferedData();
            //Thread Outbox = new Thread(new ParameterizedThreadStart(handle_received_LoadAllContent));
            //Outbox.Start(received);

            //sw.Write("Outbox-OK");
            //sw.Flush();

            // Load ChatRoom
            // Đã tạo 1 kênh
            try
            {
                Thread.CurrentThread.Abort();
            }
            catch(ThreadAbortException ex)
            {

            }
        }

        /// <summary>
        ///  Hàm này xử lý mọi nội dung đã nhận được để Load nội dung, được gọi bởi Load_AllContent()
        /// </summary>
        /// <param name="receive"> Một chuỗi nội dung </param>
        private void handle_received_LoadAllContent(Object receive)
        {
            try
            {
                String received = (String)receive;
                if (received.StartsWith("InformationOK"))
                {
                    String[] information = received.Split('-'); // Split “InformationOK-Name=name,Status=status,Avatar=byte[]” 

                    String[] sub_Information = information[1].Replace("Name=", "").Replace("Status=", "").Replace("Avatar=", "").Split(',');
                    // Become "name,status,byte[]"

                    // Hiển thị tên
                    this.myName = sub_Information[0];
                    btnMyName.Text = this.myName;

                    //Hiển thị status
                    if (sub_Information[1].Equals("-1"))
                    {
                        lblMyStatus.Text = "Offline";
                        lblMyStatus.ForeColor = SystemColors.AppWorkspace;
                        ChangeStatusMenuItem_Select = ChatChangeStatusOfflineMenuItem;
                    }
                    else if (sub_Information[1].Equals("0"))
                    {
                        lblMyStatus.Text = "Busy";
                        lblMyStatus.ForeColor = Color.Red;
                        ChangeStatusMenuItem_Select = ChatChangeStatusBusyMenuItem;
                    }
                    else
                    {
                        lblMyStatus.Text = "Online";
                        lblMyStatus.ForeColor = Color.Green;
                        ChangeStatusMenuItem_Select = ChatChangeStatusOnlineMenuItem;
                    }
                    ChangeStatusMenuItem_Select.Enabled = false;
                    this.myStatus = sub_Information[1];
                    //Hiển thị avatar
                    /*using (var ms = new MemoryStream()) // Thử Convert, vì chưa biết lúc ReadLine() Encoding là gì ! Chỉnh lại sau
                    {
                        myAvatar = Image.FromStream(ms);
                        Bitmap avatar_Scale = new Bitmap(myAvatar, new Size(pcBoxAvatar.Width, pcBoxAvatar.Height));
                        pcBoxAvatar.Image = avatar_Scale;
                    }*/
                    //Cách khác

                    //myAvatar = (Image)(new ImageConverter()).ConvertFrom(Avatar);
                    //myAvatar = Image.FromStream(TCP_Connection.GetStream());
                    Bitmap avatar_Scale = new Bitmap(myAvatar, new Size(pcBoxAvatar.Width, pcBoxAvatar.Height));
                    pcBoxAvatar.Image = avatar_Scale;
                    //Kiểm tra
                }

                if (received.StartsWith("ListFriends"))
                {
                    String[] information = received.Split('-'); // Split ListFriends-Username=username,Name=name,Status=status-…- 
                    for (int i = 1; i < information.Length; i++) //Bỏ qua 0 là chữ ListFriends
                    {
                        String[] sub_information = information[i].Replace("Username=", "").Replace("Name=", "").Replace("Status=", "").Split(','); //Result username name status in sub_information array
                        ListViewItem lstViewItem = new ListViewItem(sub_information[1]);
                        lstViewItem.Name = sub_information[0]; // Name của SubItems chính là Username để tiện cho việc lấy Items sau này
                        String status;
                        Color status_Color;
                        if (sub_information[2].Equals("-1"))
                        {
                            status = "Offline";
                            status_Color = SystemColors.AppWorkspace;
                        }
                        else if (sub_information[2].Equals("0"))
                        {
                            status = "Busy";
                            status_Color = Color.Red;
                        }
                        else
                        {
                            status = "Online";
                            status_Color = Color.Green;
                        }
                        lstViewItem.SubItems.Add(status, status_Color, SystemColors.Control, lstViewFriends.Font).Name = "Status";
                        lstViewItem.SubItems.Add(sub_information[0]).Name = "Username";
                        //Thêm vào ListView
                        lstViewFriends.Items.Add(lstViewItem);
                    }
                }

                if (received.StartsWith("Inbox"))
                {
                    String[] information = received.Split('-'); // Split Inbox-Username=username,Name=name,ReadStatus=status,Content=date1_content1:date2_content2:…-…- 

                    for (int i = 1; i < information.Length; i++) // Bỏ qua 0 là chữ Inbox
                    {
                        String[] sub_information = information[i].Replace("Username=", "").Replace("Name=", "").Replace("ReadStatus=", "").Replace("Content=", "").Split(','); //Result username name status in sub_information array

                        String[] sub_content_information = sub_information[3].Split(new char[] { ':' }, 2)[0].Split('_'); //Split date1_content1:date2_content2:… --> date1_content1 --> date1 content1


                        ListViewItem lstViewItem = new ListViewItem(sub_information[1]);
                        lstViewItem.Name = sub_information[0];
                        lstViewItem.SubItems.Add(sub_content_information[1].Split(new String[] { "\r\n" }, 2, StringSplitOptions.RemoveEmptyEntries)[0]).Name = "LastMessageContent";
                        lstViewItem.SubItems.Add(sub_content_information[0]).Name = "Date";


                        String status;
                        Color status_Color;
                        if (sub_information[2].Equals("0"))
                        {
                            status = "NotReadedYet";
                            status_Color = SystemColors.ControlDark;
                        }
                        else
                        {
                            status = "Readed";
                            status_Color = SystemColors.Control;
                        }

                        lstViewItem.SubItems.Add(status).Name = "Status";
                        lstViewItem.SubItems.Add(sub_information[0]).Name = "Username";
                        lstViewItem.SubItems.Add(sub_information[3]).Name = "FullContent";
                        //Thêm vào ListView 
                        lstViewInbox.Items.Add(lstViewItem);
                    }
                }

                if (received.StartsWith("Outbox"))
                {
                    String[] information = received.Split('-'); //Split Outbox-Username=username,Name=name,Content=date1_content1:...-…-
                    for (int i = 1; i < information.Length; i++)
                    {
                        String[] sub_information = information[i].Replace("Username=", "").Replace("Name=", "").Replace("Content=", "").Split(',');

                        String[] sub_content_information = sub_information[2].Split(new char[] { ':' }, 2)[0].Split('_'); //Split date1_content1:date2_content2:… --> date1_content1 --> date1 content1


                        ListViewItem lstViewItem = new ListViewItem(sub_information[1]);
                        lstViewItem.Name = sub_information[0];
                        lstViewItem.SubItems.Add(sub_content_information[1].Split(new String[] { "\r\n" }, 2, StringSplitOptions.RemoveEmptyEntries)[0]).Name = "LastMessageContent";
                        lstViewItem.SubItems.Add(sub_content_information[0]).Name = "Date";

                        lstViewItem.SubItems.Add(sub_information[0]).Name = "Username";
                        lstViewItem.SubItems.Add(sub_information[2]).Name = "FullContent";
                        //Thêm vào ListView 
                        lstViewOutbox.Items.Add(lstViewItem);
                    }
                }
                if (received.StartsWith("GetInfoMyFriendOK"))
                {
                    String[] information = received.Split('-'); // Split “GetInfoMyFriendOK-Gender=gender,Birthday=birthday,Email=email,City=city,Language=language,Avatar=”
                    String[] sub_information = information[1].Replace("Gender=", "").Replace("Birthday=", "").Replace("Email=", "").Replace("City=", "").Replace("Language", "").Replace("Avatar=", "").Split('=');

                    // Thêm thông tin lưu trữ vào Item đang được Focus
                    lstViewFriends.FocusedItem.SubItems.Add(sub_information[0]).Name = "Gender";
                    lstViewFriends.FocusedItem.SubItems.Add(sub_information[1]).Name = "Birthday";
                    lstViewFriends.FocusedItem.SubItems.Add(sub_information[2]).Name = "Email";
                    lstViewFriends.FocusedItem.SubItems.Add(sub_information[3]).Name = "City";
                    lstViewFriends.FocusedItem.SubItems.Add(sub_information[4]).Name = "Language";
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            try
            {
                Thread.CurrentThread.Abort();
            }
            catch(ThreadAbortException ex)
            {
                // FeedBack some message
            }
        }//End of handle_received_LoadAllContent method

        /// <summary>
        ///  Xử lý sự kiện nhấp đôi vào 1 mục trong list friend, bật ra popupchat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstViewFriends_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Lấy Item đã được chọn, nhầm có thông tin cần thiết
            ListViewItem selected = lstViewFriends.FocusedItem;
            // Khởi tạo mới, gởi qua gồm MyAvatar, ItemSelected, Friend Avatar, My Username cho constructor
            ChattingFromFriendList chatting = new ChattingFromFriendList(this.myUsername, this.myAvatar, selected, DefaultAvatar);

            if (!listChatting.Exists((chatting_object) =>
            {
                if (chatting_object.FriendUsername == null || "".Equals(chatting_object.FriendUsername)) return false;
                if (chatting_object.FriendUsername.Equals(chatting.FriendUsername)) return false;
                return true;

            })) // Xem coi người bạn này đã được chọn và còn chat hay không
            {
                String init = "ChattingWithFriend-Username=" + selected.SubItems[1].Text;
                sw.Write(init);
                sw.Flush();

                listChatting.Add(chatting); // Thêm chatting này vào Collections
                
                // Liên kết dữ liệu gởi
                chatting.send_Message = new ChattingFromFriendList.Send_Message_Delegate(this.Send_Message);

                //Bắt đầu luồng nhận
                Thread Thread_Receive = new Thread(new ParameterizedThreadStart(Receive_Message));
                Thread_Receive.Start(chatting);
            }
            // Cho bật Form chat ra
            chatting.Show();  
        }

        /// <summary>
        ///  Xử lý sự kiện chọn vào 1 mục trong list friend, hiển thị qua màn hình phía phải
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstViewFriends_Click(object sender, EventArgs e)
        {
            if (this.selected_Friend != (ListViewItem)sender)
            {
                panelLoading.Dock = DockStyle.Fill;
                panelWhenClickFriendItem.Dock = DockStyle.None;
                this.selected_Friend = lstViewFriends.FocusedItem;
                if (this.selected_Friend.SubItems.Count == 2) // Nếu thông tin trong SubItem = 2, thì xem như chưa có thông tin
                {
                    String init = "GetInfoMyFriend-Username=" + this.selected_Friend.SubItems[1].Text;
                    sw.Write(init);
                    sw.Flush();

                    String received = sr.ReadLine();
                    sr.DiscardBufferedData();
                    Thread GetInformationFriend = new Thread(new ParameterizedThreadStart(handle_received_LoadAllContent));
                    GetInformationFriend.Start(received);
                    try
                    {
                        Image FriendAvatar = Image.FromStream(TCP_Connection.GetStream());
                        Bitmap scaled = new Bitmap(FriendAvatar, pcBoxFriendAvatar.Width, pcBoxFriendAvatar.Height);
                        pcBoxFriendAvatar.Image = scaled;

                        listImageFriend.Images.Add(this.selected_Friend.SubItems["Username"].Text, scaled); // Thêm Avatar này vào listimage để khỏi tải lại, key của image là username
                        //lstViewFriends.FocusedItem.SubItems.Add((listImageFriend.Images.Count - 1).ToString()).Name = "IndexOnListImage"; // Lưu Index trong ImageList, để phân biệt avatar là của Item nào

                    }
                    catch (ArgumentException ex)
                    {
                        Bitmap scaled = new Bitmap(this.DefaultAvatar, pcBoxFriendAvatar.Width, pcBoxFriendAvatar.Height);
                        pcBoxFriendAvatar.Image = scaled;
                    }
                    finally
                    {
                        sw.Write("GetInfoMyFriendFinish");
                        sw.Flush();
                    }
                }
                else // Nếu thông tin trong SubItem > 2, thì xem như đã lấy thông tin, thì chỉ cần lấy ra từ SubItem và load lên
                {
                    btnFriendName.Text = this.selected_Friend.Text;  // Friend Name
                    lblFriendStatus.Text = this.selected_Friend.SubItems["Status"].Text; // Friend Status
                    lblFriendGender.Text = this.selected_Friend.SubItems["Gender"].Text; // Friend Gender
                    lblFriendBirthday.Text = this.selected_Friend.SubItems["Birthday"].Text; // Friend Birthday
                    lblFriendEmail.Text = this.selected_Friend.SubItems["Email"].Text; // Friend Email
                    lblFriendCity.Text = this.selected_Friend.SubItems["City"].Text; // Friend City
                    lblFriendLanguage.Text = this.selected_Friend.SubItems["Language"].Text; // Friend Language
                    try
                    {
                        //pcBoxFriendAvatar.Image = listImageFriend.Images[Int32.Parse(selected.SubItems["IndexOnListImage"].Text)];
                        pcBoxFriendAvatar.Image = listImageFriend.Images[this.selected_Friend.SubItems["Username"].Text]; // Lấy image ra dựa vào username
                    }
                    catch
                    {
                        MessageBox.Show("Error while load avatar"); // Sẽ thay đổi
                        Bitmap scaled = new Bitmap(this.DefaultAvatar, pcBoxFriendAvatar.Width, pcBoxFriendAvatar.Height);
                        pcBoxFriendAvatar.Image = scaled;
                    }
                }
                listisGotInfoFriend.Add(this.selected_Friend.SubItems["Username"].Text, "true");
                panelLoading.Dock = DockStyle.None;
                panelWhenClickFriendItem.Dock = DockStyle.Fill;
            }
           
        }

        /// <summary>
        ///  Hàm xử lý dữ liệu nhận từ server khi chat
        /// </summary>
        /// <param name="chatting_o"></param>
        private void Receive_Message(Object chatting_o)
        {
            ChattingFromFriendList chatting = (ChattingFromFriendList)chatting_o;

            String received = sr.ReadLine(); //Cho bên server gởi line để tiện xử lý, bằng không thử ReadToEnd()
            sr.DiscardBufferedData();

            if (received.StartsWith("ChattingWithFriendOK"))
            {
                Image FriendAvatar;
                if(listisGotInfoFriend.ContainsKey(chatting.FriendUsername))
                {
                    FriendAvatar = listImageFriend.Images[chatting.FriendUsername];
                }
                else
                {
                    FriendAvatar = Image.FromStream(TCP_Connection.GetStream());                
                }
                chatting.update_Avatar(chatting.Lvi_selected.SubItems[1].Text, FriendAvatar);
            }
            while(TCP_Connection.Connected) // Thay bằng khi còn connect
            {
                received = sr.ReadToEnd();
                //sr.DiscardBufferedData();
                chatting.update_ListBox(chatting.FriendUsername, received);
            }
        }

        /// <summary>
        ///  Hàm gởi dữ liệu đến server khi chat
        /// </summary>
        /// <param name="content"> Nội dung mà Client đã soạn để chat, gởi cho server để server forward cho bạn bè, content này không kiểm tra điều kiện rỗng </param>
        private void Send_Message(String content)
        {
            sw.Write(content);
            sw.Flush();
        }

        /// <summary>
        ///  Hàm xử lý thay đổi status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatChangeStatusMenuItem_Click(object sender, EventArgs e)
        {
            if(ChangeStatusMenuItem_Select != (ToolStripMenuItem)sender) // Nếu chọn Status khác đi so với ban đầu thì mới thực hiện
            {     
                ChangeStatusMenuItem_Select.Enabled = true;
                ChangeStatusMenuItem_Select = (ToolStripMenuItem)sender;
                ChangeStatusMenuItem_Select.Enabled = false;
                
                String init = "ChangeStatusMe-Status=" + ChangeStatusMenuItem_Select.Tag;
                sw.Write(init);
                sw.Flush();

                //Chưa xử lý nhận chuỗi “ChangeStatusMeOK” của server

                //Hiển thị status
                if (ChangeStatusMenuItem_Select.Tag.Equals("-1"))
                {
                    lblMyStatus.Text = "Offline";
                    lblMyStatus.ForeColor = SystemColors.AppWorkspace;
                    ChangeStatusMenuItem_Select = ChatChangeStatusOfflineMenuItem;
                }
                else if (ChangeStatusMenuItem_Select.Tag.Equals("0"))
                {
                    lblMyStatus.Text = "Busy";
                    lblMyStatus.ForeColor = Color.Red;
                    ChangeStatusMenuItem_Select = ChatChangeStatusBusyMenuItem;
                }
                else
                {
                    lblMyStatus.Text = "Online";
                    lblMyStatus.ForeColor = Color.Green;
                    ChangeStatusMenuItem_Select = ChatChangeStatusOnlineMenuItem;
                }
            }

        }

        /// <summary>
        ///  Xử lý khi click chọn thay đổi avatar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatProfileChangeAvatarMenuItem_Click(object sender, EventArgs e)
        {
            ChangeAvatarFromComputer();
        }

        /// <summary>
        ///  Hàm cho chọn ảnh từ máy tính
        /// </summary>
        private void ChangeAvatarFromComputer()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Jpeg|*.jpg|Png|*.png|Bitmap|*.bmp";
            open.Multiselect = false;
            open.Title = "Change Avatar";
        Label:
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Image avatar = Image.FromFile(open.FileName); // Lấy hình ảnh                 

                    String init = "ChangeAvatarMe-Avatar=";
                    sw.Write(init);
                    sw.Flush();
                    using(MemoryStream ms = new MemoryStream())
                    {
                        avatar.Save(ms, avatar.RawFormat);
                        BinaryWriter bw = new BinaryWriter(TCP_Connection.GetStream());
                        bw.Write(ms.GetBuffer());
                        bw.Flush();
                    }

                    // Load hình ảnh lên picturebox
                    this.myAvatar = avatar;
                    // Chỉnh ảnh lại để load lên picture box
                    Bitmap scaled = new Bitmap(avatar, pcBoxAvatar.Width, pcBoxAvatar.Height);
                    pcBoxAvatar.Image = scaled;
                    // Thay đổi ảnh trên tất cả khung chat
                    listChatting.ForEach((chatting) =>
                    {
                        chatting.update_Avatar(this.myUsername, avatar);
                    });
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Invalid Format");
                    goto Label;
                }
            }
        }

        private void Listen_All_Message()
        {
            StreamReader reader = new StreamReader(TCP_Connection.GetStream());
            String message = reader.ReadToEnd();
            Thread handle = new Thread(new ParameterizedThreadStart(handle_ListenAllMessage));
            handle.Start(message);       
        }

        private void handle_ListenAllMessage(object message)
        {
            String content = (String)message;
            if (content.StartsWith("FriendWantToChat-"))
            {
                Image avatar = Image.FromStream(TCP_Connection.GetStream());
                String[] information = content.Split('-'); // Split “FriendWantToChat-Username=username,Name=name,Avatar=”
                String[] sub_information = information[1].Replace("Username=", "").Replace("Name=", "").Replace("Avatar=", "").Split(',');
                ListViewItem lvi = new ListViewItem(sub_information[1]);
                lvi.Name = "Name";
                lvi.SubItems.Add(sub_information[0]).Name = "Username";
                if (this.myStatus.Equals(1)) // Nếu trạng thái của bản thân là online thì bật popup
                {
                    ChattingFromFriendList chatting = new ChattingFromFriendList(this.myUsername, this.myAvatar, lvi, avatar);                
                /*
                if (!listChatting.Exists((chatting_object) =>
                {
                  if (chatting_object.FriendUsername == null || "".Equals(chatting_object.FriendUsername)) return false;
                    if (chatting_object.FriendUsername.Equals(chatting.FriendUsername)) return false;
                    return true;
                })) // Xem coi người bạn này đã được chọn và còn chat hay không
                { */
                    // Thêm chatting này vào Collections
                    listChatting.Add(chatting); 
                    // Liên kết dữ liệu gởi
                    chatting.send_Message = new ChattingFromFriendList.Send_Message_Delegate(this.Send_Message);
                    //Bắt đầu luồng nhận
                    Thread Thread_Receive = new Thread(new ParameterizedThreadStart(Receive_Message));
                    Thread_Receive.Start(chatting);
                    chatting.Show();
                    sw.Write("FriendWantToChatOK-Username=" + this.myUsername);
                    sw.Flush();
                }
                return;
            }
            if (content.StartsWith("ChangedStatus-"))
            {
                String[] information = content.Split('-'); //Split “ChangedStatus-Username=username,Status=status”
                String[] sub_information = information[1].Replace("Username=", "").Replace("Status=", "").Split(',');

                ListViewItem new_lvi = lstViewFriends.Items[sub_information[0]];
                lstViewFriends.Items.RemoveByKey(sub_information[0]);

                new_lvi.SubItems.RemoveByKey("Status");
                String status;
                Color status_Color;
                if (sub_information[1].Equals("-1"))
                {
                    status = "Offline";
                    status_Color = SystemColors.AppWorkspace;
                }
                else if (sub_information[1].Equals("0"))
                {
                    status = "Busy";
                    status_Color = Color.Red;
                }
                else
                {
                    status = "Online";
                    status_Color = Color.Green;
                }
                new_lvi.SubItems.Add(status, status_Color, SystemColors.Control, lstViewFriends.Font).Name = "Status";
                lstViewFriends.Items.Add(new_lvi);
                return;
            }
            if (content.StartsWith("ChangedAvatar-"))
            {
                String[] information = content.Replace("Username=", "").Split('-'); // Split “ChangedAvatar-Username=username”
                Image new_Avatar = Image.FromStream(TCP_Connection.GetStream());
                listChatting.Find((chatting) =>
                {
                    if (chatting.FriendUsername.Equals(information[1])) return true;
                    return false;
                }).update_Avatar(information[1], new_Avatar); // Cho update trên ô chat của username này
                if (listisGotInfoFriend.ContainsKey(information[1]))
                {
                    // Cập nhật lại trong image list của friend
                    listImageFriend.Images.RemoveByKey(information[1]);
                    listImageFriend.Images.Add(information[1], new_Avatar); 
                }
                ListViewItem lvi = lstViewFriends.FocusedItem;
                if (lvi != null && lvi.SubItems["Username"].Equals(information[1])) // Đồng thời nếu user vừa đổi ảnh này trùng với trạng thái của user đó đang Focus thì cập nhật avatar ngay
                {
                    Bitmap scaled = new Bitmap(new_Avatar, pcBoxFriendAvatar.Width, pcBoxFriendAvatar.Height);
                    pcBoxFriendAvatar.Image = scaled;
                }
            }

        }

        private void ChatCloseMenuItem_Click(object sender, EventArgs e)
        {
            if (TCP_Connection.Connected)
            {
                sr.Close();
                sw.Close();
                TCP_Connection.Close();
            }
            Application.Exit();
        }

       

    }//End of ClientSide Class

}
