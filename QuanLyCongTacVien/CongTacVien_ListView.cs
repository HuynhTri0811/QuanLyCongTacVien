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
        private List<CongTacVien> _listCongTacVien = new List<CongTacVien>();
        public CongTacVien_ListView()
        {
            InitializeComponent();
            dataGridView1.DataSource = _binding;
            //ListCongTacVien();
        }

        // Some environments raise CellClick instead of CellContentClick for button columns.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;
            if (grid.Columns[e.ColumnIndex].Name == "colAction")
            {
                if (e.RowIndex >= 0 && e.RowIndex < _listCongTacVien.Count)
                {
                    ShowDetailFor(_listCongTacVien[e.RowIndex]);
                }
            }
        }



        public void ListCongTacVien()
        {
            _listCongTacVien = GetAllDanhSachCongTacVien();
            dataGridView1.DataSource = _listCongTacVien.ToList();

            //if (_listCongTacVien == null)
            //{
            //    _binding.DataSource = typeof(CongTacVien);
            //    return;
            //}

            //_binding.DataSource = _listCongTacVien.ToList();
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
    }
}
