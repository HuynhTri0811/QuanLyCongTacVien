using QuanLyCongTacVien.DAO;
using QuanLyCongTacVien.DTO;
using System;
using QuanLyCongTacVien.Views;
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
        private List<CongTacVien> _listCongTacVien = new List<CongTacVien>();

        public CongTacVien_ListView()
        {
            InitializeComponent();
            Database.KhoiTaoSQL();

            ListCongTacVien();
        }

        // Some environments raise CellClick instead of CellContentClick for button columns.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            var colName = grid.Columns[e.ColumnIndex].Name;
            if (colName == "colAction")
            {
                if (e.RowIndex >= 0 && e.RowIndex < _listCongTacVien.Count)
                {
                    ShowDetailFor(_listCongTacVien[e.RowIndex]);
                }
            }
            else if (colName == "colDelete")
            {
                if (e.RowIndex >= 0 && e.RowIndex < _listCongTacVien.Count)
                {
                    var ctv = _listCongTacVien[e.RowIndex];
                    var confirm = MessageBox.Show($"Bạn có chắc muốn xóa cộng tác viên '{ctv.HoTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        // remove from database by Oid if exists
                        if (ctv.GetOid() > 0)
                        {
                            Database.XoaCongTacVienByOid(ctv.GetOid());
                        }

                        // remove from local list and refresh grid
                        _listCongTacVien.RemoveAt(e.RowIndex);
                        dataGridView1.DataSource = new BindingList<CongTacVien>(_listCongTacVien);
                    }
                }
            }
        }



        public void ListCongTacVien()
        {
            _listCongTacVien = GetAllDanhSachCongTacVien();
            dataGridView1.DataSource = new BindingList<CongTacVien>(_listCongTacVien);


        }

        private List<CongTacVien> GetAllDanhSachCongTacVien()
        {
            List<CongTacVien> result = new List<CongTacVien>();
            Database.GetAllDanhSachCongTacVien().AsEnumerable().ToList().ForEach(row =>
            {
                result.Add(new CongTacVien(row));
            });
            return result;
        }



        public void SaveDataGridView()
        {
            List<CongTacVien> listThemMoi = new List<CongTacVien>();   
            List<CongTacVien> listCapNhat  = new List<CongTacVien>();
            foreach (CongTacVien item in _listCongTacVien)
            {
                if (item.GetOid() == 0)
                    listThemMoi.Add(item);
                else
                    listCapNhat.Add(item);
            }
            Database.ThemMoiDanhSachCongTacVien(listThemMoi);
            Database.CapNhatDanhSachCongTacVien(listCapNhat);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveDataGridView();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ListCongTacVien();
        }


        private void ShowDetailFor(CongTacVien ctv)
        {
            // open detail form or show message
            var detail = new CongTacVien_DetailView(ctv);
            // If you later add a constructor that accepts a DTO, pass it here.
            detail.ShowDialog(this);
        }

        private void EnsureActionDeleteColumnsAtEnd()
        {
            // Move colAction and colDelete to the end of the column collection
            if (dataGridView1.Columns.Contains("colAction"))
            {
                var col = dataGridView1.Columns["colAction"];
                dataGridView1.Columns.Remove(col);
                dataGridView1.Columns.Add(col);
            }
            if (dataGridView1.Columns.Contains("colDelete"))
            {
                var col = dataGridView1.Columns["colDelete"];
                dataGridView1.Columns.Remove(col);
                dataGridView1.Columns.Add(col);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtFilter.Text = string.Empty;
            cbFilterBy.SelectedIndex = 0;
            ListCongTacVien();
        }

        private void ApplyFilter()
        {
            if (_listCongTacVien == null) return;
            var field = cbFilterBy.SelectedItem?.ToString();
            var keyword = txtFilter.Text?.Trim();
            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(keyword))
            {
                dataGridView1.DataSource = new BindingList<CongTacVien>(_listCongTacVien);
                return;
            }

            IEnumerable<CongTacVien> filtered = _listCongTacVien;
            switch (field)
            {
                case "MaQuanLy":
                    filtered = _listCongTacVien.Where(x => (x.MaQuanLy ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                case "HoTen":
                    filtered = _listCongTacVien.Where(x => (x.HoTen ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                case "SoDienThoai":
                    filtered = _listCongTacVien.Where(x => (x.SoDienThoai ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                case "DiaChi":
                    filtered = _listCongTacVien.Where(x => (x.DiaChi ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                case "CCCD":
                    filtered = _listCongTacVien.Where(x => (x.CCCD ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                case "QuocTich":
                    filtered = _listCongTacVien.Where(x => (x.QuocTich ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                default:
                    break;
            }

            dataGridView1.DataSource = new BindingList<CongTacVien>(filtered.ToList());
        }
    }
}
