using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Server
{
    public partial class Server : Form
    {
        private ArrayList Clients = new ArrayList();
        private IPAddress ipServer;
        private int portServer;
        
        public ArrayList CLIENTS
        {
            get { return Clients; }
        }

        public Server()
        {
            InitializeComponent();
        }

        // Start Server.
        public void StartServer()
        {
            LoadConfig();
            StartListening();
        }

        #region Load Server configuration before start listening.

        public void LoadConfig()
        {
            try
            {
                var section = ConfigurationManager.GetSection("ServerConfiguration") as NameValueCollection;
                var ip = section["IPAddress"];
                var port = section["Port"];
                ipServer = IPAddress.Parse(ip);
                portServer = Int32.Parse(port);
            }
            catch(Exception e)
            {

                DialogResult r = MessageBox.Show("Failed in loading application settings!\nMore details:\n" + e.ToString(),"Loading Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

                if (r == DialogResult.OK)
                    Application.Exit();
            }
        }

        #endregion

        #region Server is listenning and wait for connection from Clients.
        private void StartListening()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                listener.Bind(new IPEndPoint(ipServer, portServer));
                listener.Listen(100);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            }
            catch
            {
                listener.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #endregion

        #region Server accepts a new connection from a client.
        public void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            NewClientConnection(handler);
            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
        }
        
        private void NewClientConnection(Socket clientSocket)
        {
            ClientHandler newClient = new ClientHandler(clientSocket);
            Clients.Add(newClient);
            newClient._timeConnect = DateTime.Now.ToString();
            newClient.RecieveCallback(this);
        }

        #endregion

        #region Server starts receiving data from a client and then sends data back.
        public void RecieveCallback(IAsyncResult ar)
        {
            ClientHandler client = (ClientHandler)ar.AsyncState;
            byte[] receivedData = client.Recieve(ar);
            if (receivedData.Length < 1)
            {
                // Client has disconnected.
                client._timeDisconnect = DateTime.Now.ToString();
                client._clientSocket.Close();
                Clients.Remove(client);
                return;
            }
            
            try
            {
                client.SDRTC();
            }
            catch(Exception e)
            {
                MessageBox.Show("Can't associate with remote host " + client._clientSocket.RemoteEndPoint.ToString() + "!\nMore details:\n"+e.ToString());
                // Send failure. Maybe client has disconnected, or something else.
                client._timeDisconnect = DateTime.Now.ToString();
                client._clientSocket.Close();
                Clients.Remove(client);
            }

            client.RecieveCallback(this);
        }

        #endregion

        #region Authitencation to gain access of the Server.

        private bool Authenticate(string Username, string Password)
        {
            try
            {
                var section = ConfigurationManager.GetSection("ManagerAccount/Administrator") as NameValueCollection;
                var user = section["Username"];
                var pass = section["Password"];
                if (Username.Equals(user) && Password.Equals(pass))
                    return true;
                else return false;
            }
            catch(Exception e)
            {
                MessageBox.Show("Failed in loading application settings!\nMore details:\n" + e.ToString(), "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        private void Login()
        {
            if (Authenticate(tb_User.Text, tb_Password.Text))
            {
                this.Hide();
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.Show();
            }
            else
            {
                tb_User.Focus();
                tb_User.SelectAll();
                MessageBox.Show("Username or Password not invalid!", "Authentication failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Control Events.
        private void Server_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void btn_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Login();
        }

        private void tb_User_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                tb_Password.Focus();
                tb_Password.SelectAll();
            }
            
        }

        private void tb_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Login();
        }

        #endregion

    }
}
