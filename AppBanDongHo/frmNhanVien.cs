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
using System.Data.SqlTypes;

namespace AppBanDongHo
{
    public partial class frmNhanVien : Form
    {
        SqlConnection connect;
        SqlCommand cmd;
        string chuoiKN;
        SqlDataAdapter adp = new SqlDataAdapter();
        DataSet dtSet;
   



        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void setValue(bool param, bool isLoad)
        {
            
            txtMaNV.Text = null;
            txtTenNV.Text = null;
            txtSDT.Text = null;
            txtEmail.Text = null;
            txtDiaChi.Text = null;
            cbxChucVu.Text = null;
            txtTimKiem.Text = null;
            btnThem.Enabled = !param;
            btnLuu.Enabled = param;
            
            if (isLoad)
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
            else
            {
                btnSua.Enabled = !param;
                btnXoa.Enabled = !param;
            }
        }
        private void LoadNV(string tk = "")
        {

            connect = new SqlConnection(chuoiKN);

            string selectNV = "select MaNV,TenNV,SDT,Email,NgaySinh,DiaChi,ChucVu from NhanVien order by MaNV";
            if (tk != "")
            {
                selectNV = string.Format("select MaNV,TenNV,SDT,Email,NgaySinh,DiaChi,ChucVu  from NhanVien where TenNV Like N'%{0}%'", tk);
            }

            adp = new SqlDataAdapter(selectNV, connect);

            dtSet = new DataSet();
            adp.Fill(dtSet, "NhanVien");
            dtgvNhanVien.DataSource = dtSet.Tables[0];

        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {

            chuoiKN = global::AppBanDongHo.Properties.Settings.Default.DoAn1_BanDongHoConnectionString;
            LoadNV();
            setValue(false, true);


        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            setValue(true, false);
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTenNV.Text != "" && txtSDT.Text != "" && txtEmail.Text != "")
            {
                connect.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                string sql = string.Format("insert into NhanVien values(N'{0}',N'{1}',{2},'{3}','{4}',N'{5}',N'{6}')",txtMaNV.Text, txtTenNV.Text, txtSDT.Text, txtEmail.Text, dpkNgaySinh.Value.ToShortDateString(), txtDiaChi.Text, cbxChucVu.Text);
                cmd.CommandText = sql;
                cmd.Connection = connect;
               
                int kq = cmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Thêm thành công.");
                    frmNhanVien_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Thêm thất bại.");
                }

                connect.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            connect.Open();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            string sqlXoa = string.Format("Delete from NhanVien where  MaNV  = N'{0}'", txtMaNV.Text);
            cmd.CommandText = sqlXoa;
            cmd.Connection = connect;


            if (MessageBox.Show("Bạn có chắc muốn xóa không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int kq = cmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Xóa thành công.");
                    frmNhanVien_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
                connect.Close();
            }
        }



        private void btnSua_Click(object sender, EventArgs e)
        {
            
            if (txtTenNV.Text != "" && txtSDT.Text != "" && txtEmail.Text != "")
            {

                connect.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                string sql = string.Format("update  NhanVien set TenNV = N'{0}', SDT = '{1}', Email = '{2}', NgaySinh = '{3}', DiaChi = N'{4}', ChucVu = N'{5}' where MaNV = '{6}' ",
                    txtTenNV.Text, txtSDT.Text, txtEmail.Text, dpkNgaySinh.Value.ToShortDateString(), txtDiaChi.Text, cbxChucVu.Text, txtMaNV.Text);
                cmd.CommandText = sql;
                cmd.Connection = connect;

                int kq = cmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Sửa thành công.");
                    frmNhanVien_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Sửa thất bại.");
                }

                connect.Close();

            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại dữ liệu", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnHTall_Click(object sender, EventArgs e)
        {
            frmNhanVien_Load(sender, e);
           
        }
       
        private void btnTK_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtTimKiem.Text))
            {
                LoadNV(txtTimKiem.Text);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNV();
            }

        }



        private void btnBoqua_Click(object sender, EventArgs e)
        {
            setValue(false, true);
        }


        private void dtvgNhanVien_Click(object sender, EventArgs e)
        {
            if (dtgvNhanVien.Rows.Count > 0)
            {
                txtMaNV.Text = dtgvNhanVien.CurrentRow.Cells[0].Value.ToString();
                txtTenNV.Text = dtgvNhanVien.CurrentRow.Cells[1].Value.ToString();
                txtSDT.Text = dtgvNhanVien.CurrentRow.Cells[2].Value.ToString();
                txtEmail.Text = dtgvNhanVien.CurrentRow.Cells[3].Value.ToString();
                dpkNgaySinh.Text = dtgvNhanVien.CurrentRow.Cells[4].Value.ToString();
                txtDiaChi.Text = dtgvNhanVien.CurrentRow.Cells[5].Value.ToString();
                cbxChucVu.Text = dtgvNhanVien.CurrentRow.Cells[6].Value.ToString();
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
                btnLuu.Enabled = false;
              
            }
        }

       

    }








}

