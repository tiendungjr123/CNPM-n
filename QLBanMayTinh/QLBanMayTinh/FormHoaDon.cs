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
    public partial class FormHoaDon : Form
    {
        DataTable tblCTHD;
        public FormHoaDon()
        {
            InitializeComponent();
        }

        private void FormHoaDon_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            txtMaHD.ReadOnly = true;
            txtTenNhanVien.ReadOnly = true;
            txtTenKhachHang.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            mtbDienThoai.ReadOnly = true;
            txtTenHang.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtGiamGia.Text = "0";
            Function.FillCombo("SELECT MaKH FROM TBLKhachHang", cboMaKhachHang, "MaKH", "TenKH");
            cboMaKhachHang.SelectedIndex = -1;
            Function.FillCombo("SELECT MaNV FROM tblNhanVien", cboMaNhanVien ,"MaNV", "TenNV");
            cboMaNhanVien.SelectedIndex = -1;
            Function.FillCombo("SELECT MaHang FROM TblHang", cboMaHangHoa, "MaHang", "TenHang");
            cboMaHangHoa.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHD.Text != "")
            {
                LoadInfoHoaDon();
                btnXoa.Enabled = true;
            }
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.GiaBan, a.GiamGia,a.ThanhTien FROM ChiTietHD AS a, tblHang AS b WHERE a.MaHD = N'" + txtMaHD.Text + "' AND a.MaHang=b.MaHang";
            tblCTHD = Function.GetDataToTable(sql);
            dgvHoaDon.DataSource = tblCTHD;
            dgvHoaDon.Columns[0].HeaderText = "Mã hàng";
            dgvHoaDon.Columns[1].HeaderText = "Tên hàng";
            dgvHoaDon.Columns[2].HeaderText = "Số lượng";
            dgvHoaDon.Columns[3].HeaderText = "Đơn giá";
            dgvHoaDon.Columns[4].HeaderText = "Giảm giá %";
            dgvHoaDon.Columns[5].HeaderText = "Thành tiền"; 
            dgvHoaDon.Columns[0].Width = 80;
            dgvHoaDon.Columns[1].Width = 130;
            dgvHoaDon.Columns[2].Width = 80;
            dgvHoaDon.Columns[3].Width = 90;
            dgvHoaDon.Columns[4].Width = 90;
            dgvHoaDon.Columns[5].Width = 90;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayBan FROM tblHoaDon WHERE MaHD = N'" + txtMaHD.Text + "'";
            mskNgayBan.Value = DateTime.Parse(Function.GetFieldValues(str));
            str = "SELECT MaNV FROM tblHoaDon WHERE MaHD = N'" + txtMaHD.Text + "'";
            cboMaNhanVien.Text = Function.GetFieldValues(str);
            str = "SELECT MaKH FROM tblHoaDon WHERE MaHD = N'" + txtMaHD.Text + "'";
            cboMaKhachHang.Text = Function.GetFieldValues(str);
           
           
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHD.Text = Function.CreateKey("HDB");
            LoadDataGridView();
        }
        private void ResetValues()
        {
            txtMaHD.Text = "";
            mskNgayBan.Value = DateTime.Now;
            cboMaNhanVien.Text = "";
            cboMaKhachHang.Text = "";
            cboMaHangHoa.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon;
            sql = "SELECT MaHD FROM tblHoaDon WHERE MaHD=N'" + txtMaHD.Text + "'";
            if (!Function.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
               
                if (cboMaNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhachHang.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhachHang.Focus();
                    return;
                }
                sql = "INSERT INTO tblHoaDon(MaHD, NgayBan, MaNV, MaKH) VALUES (N'" + txtMaHD.Text.Trim() + "','" +
                        mskNgayBan.Value + "',N'" + cboMaNhanVien.SelectedValue + "',N'" +
                        cboMaKhachHang.SelectedValue + "')";
                Function.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMaHangHoa.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHangHoa.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }
            sql = "SELECT MaHang FROM ChiTietHD WHERE MaHang=N'" + cboMaHangHoa.SelectedValue + "' AND MaHD = N'" + txtMaHD.Text.Trim() + "'";
            if (Function.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaHangHoa.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM TblHang WHERE MaHang = N'" + cboMaHangHoa.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO ChiTietHD(MaHD,MaHang,SoLuong,DonGia, GiamGia,ThanhTien) VALUES (N'" + txtMaHD.Text.Trim() + "',N'" + cboMaHangHoa.SelectedValue + "','" + txtSoLuong.Text + "','" + txtDonGia.Text + "','" + txtGiamGia.Text + "','" + txtThanhTien.Text + "')";
            Function.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE TblHang SET SoLuong =" + SLcon + " WHERE MaHang= N'" + cboMaHangHoa.SelectedValue + "'";
            Function.RunSQL(sql);
            
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
        }
        private void ResetValuesHang()
        {
            cboMaHangHoa.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void cboMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaNhanVien.Text == "")
            {
                txtTenNhanVien.Text = "";
            }
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select TenNV from tblNhanVien where MaNV =N'" + cboMaNhanVien.SelectedValue + "'";
            txtTenNhanVien.Text = Function.GetFieldValues(str);
        }

        private void cboMaKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaKhachHang.Text == "")
            {
                txtTenKhachHang.Text = "";
                txtDiaChi.Text = "";
                mtbDienThoai.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TenKH from TBLKhachHang where MaKH = N'" + cboMaKhachHang.SelectedValue + "'";
            txtTenKhachHang.Text = Function.GetFieldValues(str);
            str = "Select DiaChi from TBLKhachHang where MaKH = N'" + cboMaKhachHang.SelectedValue + "'";
            txtDiaChi.Text = Function.GetFieldValues(str);
            str = "Select DienThoai from TBLKhachHang where MaKH= N'" + cboMaKhachHang.SelectedValue + "'";
            mtbDienThoai.Text = Function.GetFieldValues(str);
        }

        private void cboMaHangHoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaHangHoa.Text == "")
            {
                txtTenHang.Text = "";
                txtDonGia.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TenHang FROM TblHang WHERE MaHang =N'" + cboMaHangHoa.SelectedValue + "'";
            txtTenHang.Text = Function.GetFieldValues(str);
            str = "SELECT GiaBan FROM TblHang WHERE MaHang =N'" + cboMaHangHoa.SelectedValue + "'";
            txtDonGia.Text = Function.GetFieldValues(str);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaHang,SoLuong FROM ChiTietHD WHERE MaHD = N'" + txtMaHD.Text + "'";
                DataTable tblHang = Function.GetDataToTable(sql);
                for (int hang = 0; hang <=tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE TblHang SET SoLuong =" + slcon + " WHERE MaHang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    Function.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE ChiTietHD WHERE MaHD=N'" + txtMaHD.Text + "'";
                Function.RunSqlDel(sql);

                //Xóa hóa đơn
                sql = "DELETE tblHoaDon WHERE MaHD=N'" + txtMaHD.Text + "'";
                Function.RunSqlDel(sql);
                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
            }
        }

        private void cboMaHD_DropDown(object sender, EventArgs e)
        {
            Function.FillCombo("SELECT MaHD FROM tblHoaDon", cboMaHD, "MaHD", "MaHD");
            cboMaHD.SelectedIndex = -1;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboMaHD.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHD.Focus();
                return;
            }
            txtMaHD.Text = cboMaHD.Text;
            LoadInfoHoaDon();
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            cboMaHD.SelectedIndex = -1;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;

        }

        private void txtGiamGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;

        }

        private void dgvHoaDon_DoubleClick(object sender, EventArgs e)
        {
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (tblCTHD.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaHangxoa = dgvHoaDon.CurrentRow.Cells["MaHang"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvHoaDon.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHoaDon.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE ChiTietHD WHERE MaHD=N'" + txtMaHD.Text + "' AND MaHang = N'" + MaHangxoa + "'";
                Function.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHang WHERE MaHang = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE TblHang SET SoLuong =" + slcon + " WHERE MaHang= N'" + MaHangxoa + "'";
                Function.RunSQL(sql);
              
                LoadDataGridView();
            }
        }

        private void FormHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetValues();
        }
    }
}
