using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QLBanMayTinh.Class;

namespace QLBanMayTinh
{
    public partial class Login : Form
    {
        public static SqlConnection Con;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Class.Function.Connect();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Con = new SqlConnection();
            Con.ConnectionString = Properties.Settings.Default.QLmaytinhConectionString;
           
                if (txtUsername.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập username", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }
                if (txtPassword.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập password", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }
                Con.Open();
                string tk = txtUsername.Text;
                string mk = txtPassword.Text;
                string sql = "select * from Account where Username = '" + tk + "' and Password = '" + mk + "'";
                SqlCommand cmd = new SqlCommand(sql,Con);
                SqlDataReader dta = cmd.ExecuteReader();
                if (dta.Read() == true)
                {
                    // MessageBox.Show("Đăng nhập thành công");
                    Trangchu frm = new Trangchu();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại");
                }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Class.Function.Disconnect();
            Application.Exit();
        }

        private void btnLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }
    }
}
