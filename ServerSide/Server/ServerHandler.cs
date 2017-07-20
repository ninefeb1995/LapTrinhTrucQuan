using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Server
{
    // Server Configuration Interface.
    public partial class ServerHandler : Form
    {

        public ServerHandler()
        {
            InitializeComponent();
        }

        private IPAddress[] GetLocalIPAddress()
        {
            List<IPAddress> localIPAddresses = new List<IPAddress>();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (ipAddress != null)
                        localIPAddresses.Add(ipAddress);
                }
            }
            
            return localIPAddresses.ToArray();
        }

        private void ServerHandler_Load(object sender, EventArgs e)
        {
            var localhost = "127.0.0.1";
            cb_IP.Items.Add(localhost);
            var localIPAddresses = GetLocalIPAddress();
            foreach (var localIPAddress in localIPAddresses)
                cb_IP.Items.Add(localIPAddress);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string ip = this.cb_IP.SelectedItem.ToString();
            string port = this.tb_Port.Text;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            xmlDoc.SelectSingleNode("//SeverConfiguration/add[@key='IPAddress']").Attributes["value"].Value = ip;
            xmlDoc.SelectSingleNode("//SeverConfiguration//add[@key='Port']").Attributes["value"].Value = "1997";
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("ServerConfiguration");
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
