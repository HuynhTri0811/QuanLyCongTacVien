using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCongTacVien
{
    public partial class CongTacVien_ListView : Form
    {
        private BindingSource _binding = new BindingSource();

        public CongTacVien_ListView()
        {
            InitializeComponent();
            dataGridView1.DataSource = _binding;
            ListCongTacVien();
        }



        public void ListCongTacVien()
        {
            dataGridView1.DataSource = _binding;

            List<CongTacVien> items = GetAllDanhSachCongTacVien();
            if (items == null)
            {
                _binding.DataSource = typeof(CongTacVien);
                return;
            }

            _binding.DataSource = items.ToList();
        }

        private List<CongTacVien> GetAllDanhSachCongTacVien()
        {
            List<CongTacVien> result = new List<CongTacVien>();
            Database.GetAllDanhSachCongTacVien().AsEnumerable().ToList().ForEach(row =>
            {
                result.Add(new CongTacVien
                {
                    HoTen = row["HoTen"]?.ToString(),
                    SoDienThoai = row["SoDienThoai"]?.ToString(),
                    MaQuanLy = row["MaQuanLy"]?.ToString(),
                    IsActive = (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value) ? Convert.ToBoolean(row["IsActive"]) : true,
                    //NgayVaoLam = (row.Table.Columns.Contains("NgayVaoLam") && row["NgayVaoLam"] != DBNull.Value) ? (DateTime?)Convert.ToDateTime(row["NgayVaoLam"]) : null
                });
            });
            return result;
        }

        public List<CongTacVien> MapDataTableToDTO(DataTable dt)
        {
            List<CongTacVien> list = new List<CongTacVien>();
            foreach (DataRow row in dt.Rows)
            {
                var dto = new CongTacVien
                {
                    // Sử dụng Convert để tránh lỗi DBNull
                    HoTen = row["HoTen"]?.ToString(),
                    SoDienThoai = row["SoDienThoai"]?.ToString()
                };
                list.Add(dto);
            }
            return list;
        }

        public void SaveDataGridView()
        {
            // Collect current items from the binding source / grid view
            var list = new List<CongTacVien>();

            // If bound to a list of CongTacVien, we can try to cast.
            if (_binding.DataSource is IEnumerable<CongTacVien> bound)
            {
                list.AddRange(bound);
            }
            else
            {
                // Fallback: read from DataGridView rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    var ctv = new CongTacVien();
                    // Try to read by column name if available
                    if (dataGridView1.Columns["colMaQuanLy"] != null)
                        ctv.MaQuanLy = row.Cells["colMaQuanLy"].Value?.ToString();
                    if (dataGridView1.Columns["colHoTen"] != null)
                        ctv.HoTen = row.Cells["colHoTen"].Value?.ToString();
                    if (dataGridView1.Columns["colSoDienThoai"] != null)
                        ctv.SoDienThoai = row.Cells["colSoDienThoai"].Value?.ToString();
                    //if (dataGridView1.Columns["colNgayVaoLam"] != null)
                    //{
                    //    var v = row.Cells["colNgayVaoLam"].Value;
                    //    if (v != null && DateTime.TryParse(v.ToString(), out var dt)) ctv.NgayVaoLam = dt;
                    //}
                    if (dataGridView1.Columns["colIsActive"] != null)
                    {
                        var v = row.Cells["colIsActive"].Value;
                        if (v != null && bool.TryParse(v.ToString(), out var b)) ctv.IsActive = b;
                    }

                    list.Add(ctv);
                }
            }

            // Save to database
            Database.SaveDanhSachCongTacVien(list);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveDataGridView();
        }
    }
}
