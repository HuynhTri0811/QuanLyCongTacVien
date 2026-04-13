using Microsoft.Data.Sqlite;
using System.Data;
using System.Collections.Generic;

namespace QuanLyCongTacVien
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //KhoiTaoDatabase();
            Contructor();
        }
        public void Contructor()
        {
            PopulateListViewWithSamples();
        }



        private void PopulateListViewWithSamples()
        {
            var items = new List<CongTacVien>
            {
                new CongTacVien {  MaQuanLy = "CTV001", HoTen = "Nguyen Van A", SoDienThoai = "0123456789"  },
                new CongTacVien {  MaQuanLy = "CTV002", HoTen = "Tran Thi B", SoDienThoai = "0987654321" },
            };

            listViewCTV.BeginUpdate();
            listViewCTV.Items.Clear();
            foreach (var c in items)
            {
                AddCongTacVienToListView(c);
            }
            listViewCTV.EndUpdate();
        }

        private void AddCongTacVienToListView(CongTacVien ctv)
        {
            var lvi = new ListViewItem();
            lvi.SubItems.Add(ctv.MaQuanLy ?? string.Empty);
            lvi.SubItems.Add(ctv.HoTen ?? string.Empty);
            listViewCTV.Items.Add(lvi);
        }
    }
}
