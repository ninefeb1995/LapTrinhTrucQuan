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
using System.Text.RegularExpressions;

namespace ClientSide
{
    public enum ConnectionManualResult
    {
        Save = 0,
        Cancel = 3,
    }
    public partial class ConnectionManual : Form
    {
        private ConnectionManualResult Result;
        private IPAddress ip;

        public IPAddress Ip
        {
            get 
            { 
                if(Invalid_Host)
                {
                    return IPAddress.Parse("127.0.0.1");
                }
                return ip; 
            }
        }
        private int port;

        public int Port
        {
            get 
            {
                if (Invalid_Port)
                {
                    return 1995;
                }
                return port; 
            }
        }
        bool Invalid_Host, Invalid_Port;

        /// <summary>
        ///  Khởi tạo một đối tượng ConnectionManual
        /// </summary>
        public ConnectionManual()
        {
            InitializeComponent();
            Result = ConnectionManualResult.Cancel;
            Invalid_Host = true;
            Invalid_Port = true;
        }

        public new ConnectionManualResult ShowDialog()
        {
            base.ShowDialog();
            return Result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Invalid_Host || Invalid_Port)
            {
                MessageBox.Show("Invalid information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Result = ConnectionManualResult.Save;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = ConnectionManualResult.Cancel;
            this.Close();
        }

        private void txtHost_Leave(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(txtHost.Text, out ip))
            {
                Invalid_Host = true;
                return;
            }
            Invalid_Host = false;
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            Regex expression = new Regex("[0-9]");

            if (!expression.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }*/
            short keycode = (short)e.KeyChar;
            if ((keycode < 48 || keycode > 57) && keycode != 8)
            {
                e.Handled = true;
                return;
            }
            if ((txtPort.Text.Length >= 5 || txtPort.Text.Length < 0) && keycode != 8)
            {
                e.Handled = true;
                return;
            }
        }

        private void txtPort_Leave(object sender, EventArgs e)
        {
            //port = Int32.Parse(txtPort.Text);
            if (!Int32.TryParse(txtPort.Text, out port))
            {
                Invalid_Port = true;
                return;
            }
            if (port < 0 || port > 65536)
            {
                Invalid_Port = true;
                return;
            }
            Invalid_Port = false;
        }

       


    }
}
