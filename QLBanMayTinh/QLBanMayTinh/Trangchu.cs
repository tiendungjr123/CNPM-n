using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanMayTinh
{
    public partial class Trangchu : Form
    {
        public Trangchu()
        {
            InitializeComponent();
        }

        private void Trangchu_Load(object sender, EventArgs e)
        {
            Class.Function.Connect();
        }
        

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            FormNhanVien frm = new FormNhanVien();
            frm.MdiParent = this;
            frm.Show();
        }

        private void mnuKhach_Click(object sender, EventArgs e)
        {
            FromKhachHang frm = new FromKhachHang();
            frm.MdiParent = this;
            frm.Show();
        }

       
        private void mnuHoaDon_Click(object sender, EventArgs e)
        {
            FormHoaDon frm = new FormHoaDon();
            frm.MdiParent = this;
            frm.Show();
        }

        private void mnuHangHoa_Click(object sender, EventArgs e)
        {
            FormHang frm = new FormHang();
            frm.MdiParent = this;
            frm.Show();
        }

        private void mnuFile_Click(object sender, EventArgs e)
        {
            Class.Function.Disconnect();
            Application.Exit();
        }
    }
}
