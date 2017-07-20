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

namespace ClientSide
{
    public partial class ChattingFromFriendList : Form
    {
        private ListViewItem lvi_selected;

        
        //private TcpClient connection;
        private Image myAvatar;
        private String myUsername;
        private String Friend_Name, Friend_Username;
        private Image Friend_Avatar;
        //private StreamReader sr;
        //private StreamWriter sw;
        private ImageList friend_and_me_Image;
        private Bitmap scaled_Hide, scaled_Show;
        public delegate void Update_ListBox_Delegate(String who, String content);
        public delegate void Update_Avatar_Delegate(String who, Image content);
        public delegate void Send_Message_Delegate(String content);

        public Send_Message_Delegate send_Message;
        public Update_ListBox_Delegate update_ListBox;
        public Update_Avatar_Delegate update_Avatar;

        /// <summary>
        ///  Get ListViewItem đã được chọn
        /// </summary>
        public ListViewItem Lvi_selected
        {
            get
            {
                return lvi_selected;
            }
        }

        public String FriendUsername
        {
            get
            {
                return Friend_Username;
            }
        }

        /// <summary>
        ///  Khởi gán thành phần của lớp này
        /// </summary>
        /// <param name="MyUsername"> Username của bản thân </param>
        /// <param name="MyAvatar"> Avatar của bản thân </param>
        /// <param name="Lvi_selected"> ListViewItem được chọn trên ListViewFriend, chứa thông tin của người mà mình muốn chát </param>
        /// <param name="FriendAvatar"> Avatar của Friend, nếu không có sẽ là avatar mặc định </param>
        public ChattingFromFriendList(String MyUsername, Image MyAvatar, ListViewItem Lvi_selected, Image FriendAvatar)
        {
            InitializeComponent();
            InitializeComponent2(MyUsername, MyAvatar, Lvi_selected, FriendAvatar);
        }

        /// <summary>
        ///  Hàm khởi gán thứ 2, giúp khởi tạo các thành phần
        /// </summary>
        /// <param name="myUsername"></param>
        /// <param name="MyAvatar"></param>
        /// <param name="Lvi_selected"></param>
        /// <param name="FriendAvatar"></param>
        private void InitializeComponent2(String myUsername, Image MyAvatar, ListViewItem Lvi_selected, Image FriendAvatar)
        {
            this.lvi_selected = Lvi_selected;
            //this.connection = Connection;
            this.myAvatar = MyAvatar;
            this.myUsername = myUsername;
            //sr = new StreamReader(connection.GetStream());
            //sw = new StreamWriter(connection.GetStream());

            this.Friend_Name = Lvi_selected.Text;
            this.Friend_Username = Lvi_selected.SubItems["Username"].Text;

            if (FriendAvatar != null)
            {
                this.Friend_Avatar = FriendAvatar;
            }
            this.Friend_Avatar = Properties.Resources.DefaultAvatar;

            // Lấy Friend's and My Avatar load lên khung picture box
            Bitmap myavatar_scaled = new Bitmap(this.myAvatar, pcBoxAvatarMe.Width, pcBoxAvatarMe.Height);
            pcBoxAvatarMe.Image = myavatar_scaled;
            Bitmap friendavatar_scaled = new Bitmap(this.Friend_Avatar, pcBoxAvatarFriend.Width, pcBoxAvatarFriend.Height);
            pcBoxAvatarFriend.Image = friendavatar_scaled;
            
            // Gán 2 avatar vào ImageList
            friend_and_me_Image = new ImageList();
            friend_and_me_Image.Images.AddRange(new Image[] { this.myAvatar, this.Friend_Avatar });


            // Hiển thị name của bạn lên groupbox
            this.gBox1.Text = this.Friend_Name;

            // Hiển thị name và username lên Text của form
            this.Text = this.Friend_Name + "(" + this.Friend_Username + ")";

            // Khởi gán hình ảnh vào list view phục vụ cho việc chat
            this.lstViewView.SmallImageList = friend_and_me_Image;

            // Khởi gán 2 Delegate được gọi để cập nhật avatar và listbox
            this.update_ListBox = new Update_ListBox_Delegate(this.Update_Content_to_ListBox);
            this.update_Avatar = new Update_Avatar_Delegate(this.Update_Avatar);

            scaled_Hide = new Bitmap(Properties.Resources.AnDi, new Size(25, 25));
            scaled_Show = new Bitmap(Properties.Resources.HienRa, new Size(25, 25));
            btnHideImageFriend.Image = scaled_Hide;
            btnHideImageMe.Image = scaled_Hide;
        }

        /// <summary>
        ///  Truyền nội dung mà người dùng đã soạn để gởi đi
        /// </summary>
        /// <returns></returns>
        public void Send_Content()
        {
            if(txtBoxType.Text != "")
            {
                String content = txtBoxType.Text;
                this.txtBoxType.Lines = null;
                this.txtBoxType.Text = "";
                if (this.send_Message != null)
                {
                    this.send_Message(content);
                    this.update_ListBox(this.myUsername, content);
                }
            }
        }

        /// <summary>
        ///  Xử lý sự kiện vẽ của Column của ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstViewView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
            e.DrawDefault = true;
        }

        /// <summary>
        ///  Hàm xử lý sự kiện vẽ lại trên listview với listview.ownerdraw = true, giúp cho có thể vẽ bên trái đối với dòng chat của bạn bè, vẽ bên phải đối với dòng chat của bản thân
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstViewView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
           // Căn chỉnh bình thường là bên trái
            TextFormatFlags align = TextFormatFlags.Default; 

            // Nếu Username == myUsername, tức Message đó là của bản thân thì căn qua bên phải
            if (e.Item.SubItems[0].Text.Equals(this.myUsername))
            {
                align = TextFormatFlags.RightToLeft;
            }
            e.DrawText(align);
        }

        /// <summary>
        ///  Cập nhật nội dung gởi và nhận được lên listbox
        /// </summary>
        /// <param name="who"> Nội dung này của ai, mình hay bạn</param>
        /// <param name="content"> Nội dung này là gì</param>
        public void Update_Content_to_ListBox(String who, String content)
        {
            ListViewItem lvi;
            String[] sub_content = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            if (who.Equals(this.myUsername))
            {
                foreach(String getcontent in sub_content)
                {
                    lvi = new ListViewItem(getcontent, 0);
                    lvi.SubItems.Add(this.myUsername); // Sub Items chứa Username của content này, Sub này có index là 0
                    lvi.ForeColor = Color.Black;
                    lvi.BackColor = SystemColors.ActiveCaption;
                    lstViewView.Items.Add(lvi); // Sub Items chứa Username của content này, Sub này có index là 0
                }
                
            }
            else
            {
                foreach (String getcontent in sub_content)
                {
                    lvi = new ListViewItem(getcontent, 1);
                    lvi.SubItems.Add(this.Friend_Username);
                    lvi.ForeColor = Color.Blue;
                    lvi.BackColor = SystemColors.Window;
                    lstViewView.Items.Add(lvi); // Sub Items chứa Username của content này, Sub này có index là 0
                }
            }           
        }

        /// <summary>
        ///  Cập nhật lại hình ảnh lên ô picturebox
        /// </summary>
        /// <param name="who"> Avatar này của ai, mình hay bạn </param>
        /// <param name="content"> Hình ảnh mới </param>
        public void Update_Avatar(String who, Image content)
        {
            Bitmap image_Scaled;
            if(who.Equals(myUsername))
            {
                image_Scaled = new Bitmap(content, pcBoxAvatarMe.Width, pcBoxAvatarMe.Height);
                pcBoxAvatarMe.Image = image_Scaled;
                myAvatar = content;              
            }
            else
            {
                image_Scaled = new Bitmap(content, pcBoxAvatarFriend.Width, pcBoxAvatarFriend.Height);
                pcBoxAvatarFriend.Image = image_Scaled;
                Friend_Avatar = content;
            }
            friend_and_me_Image.Images.Clear();
            friend_and_me_Image.Images.AddRange(new Image[] { this.myAvatar, this.Friend_Avatar });
        }

        /// <summary>
        ///  Xử lý khi người dùng nhấp chọn nút Send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            this.Send_Content();
        }

        /// <summary>
        ///  Xử lý sự kiện nhấn phím trên form này
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChattingFromFriendList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar == 13)
            {
                this.Send_Content();
            }
        }

        /// <summary>
        ///  Xử lý sự kiện đóng form, không cho đóng form này chỉ ẩn đi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChattingFromFriendList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnHideImageFriend_Click(object sender, EventArgs e)
        {
            if(isHideAvatarFriend == false)
            {
                btnHideImageFriend.Image = scaled_Show;
                pcBoxAvatarFriend.Hide();
                isHideAvatarFriend = true;
            }
            else
            {
                btnHideImageFriend.Image = scaled_Hide;
                pcBoxAvatarFriend.Show();
                isHideAvatarFriend = false;
            }
            this.Compute_Size_Components();
        }

        private void btnHideImageMe_Click(object sender, EventArgs e)
        {
            if (isHideAvatarMe == false)
            {
                btnHideImageMe.Image = scaled_Show;
                pcBoxAvatarMe.Hide();
                isHideAvatarMe = true;
            }
            else
            {
                btnHideImageMe.Image = scaled_Hide;
                pcBoxAvatarMe.Show();
                isHideAvatarMe = false;
            }
            this.Compute_Size_Components();
        }

        private Boolean isHideAvatarFriend, isHideAvatarMe;
        private int reverse_Width_lstViewView;
        private Point reverse_Location_btnSend;
        private int reverse_Width_txtBoxType;
        private void Compute_Size_Components()
        {
            if (isHideAvatarFriend && isHideAvatarMe)
            {
                // Xử lý giãn lstViewView trước
                reverse_Width_lstViewView = lstViewView.Width;
                lstViewView.Width += (pcBoxAvatarFriend.Width - btnHideImageFriend.Width);              
                // Xử lý giãn btnsend
                reverse_Location_btnSend = btnSend.Location;
                int newX_LocationbtnSend = pcBoxAvatarMe.Right - btnSend.Width;
                int newY_LocationbtnSend = pcBoxAvatarMe.Bottom - btnSend.Height;
                btnSend.Location = new Point(newX_LocationbtnSend, newY_LocationbtnSend);
                //Xử lý giãn txtBoxType
                reverse_Width_txtBoxType = txtBoxType.Width;
                txtBoxType.Width = btnSend.Location.X - 3;
            }
            else
            {
                // Phục hồi lại trạng thái trước đó
                lstViewView.Width = reverse_Width_lstViewView;
                btnSend.Location = reverse_Location_btnSend;
                txtBoxType.Width = reverse_Width_txtBoxType;
            }
        }

    }
}
