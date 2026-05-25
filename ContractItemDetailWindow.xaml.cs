using System.Collections.Generic;
using System.Linq;
using System.Windows;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class ContractItemDetailWindow : Window
    {
        private ChiTietHopDong _item;
        public List<CongTacVien> DanhSachCongTacVien { get; set; }

        public ContractItemDetailWindow(ChiTietHopDong item)
        {
            InitializeComponent();
            _item = item;
            
            // Tải danh sách cộng tác viên để chọn
            using (var context = new Data.AppDbContext())
            {
                DanhSachCongTacVien = context.CongTacViens
                    .Where(c => !c.IsDelete)
                    .OrderBy(c => c.HoVaTen)
                    .ToList();
            }

            this.DataContext = this;
            this.Item = _item;
        }

        // Dependency Property hoặc Simple Property cho Binding
        public ChiTietHopDong Item { get; set; }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật navigation property để helper NguoiLaoDong hiển thị đúng tên ngay lập tức
            if (Item.CongTacVienId != null)
            {
                Item.CongTacVien = DanhSachCongTacVien.FirstOrDefault(c => c.Id == Item.CongTacVienId);
            }
            else
            {
                Item.CongTacVien = null;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnExportWord_Click(object sender, RoutedEventArgs e)
        {
            QuanLyHopDongCongTacVien? parentInfo = null;
            CongTacVien? selectedCtv = null;

            using (var context = new Data.AppDbContext())
            {
                if (Item.QuanLyHopDongCongTacVienId != 0)
                {
                    parentInfo = context.QuanLyHopDongCongTacViens.Find(Item.QuanLyHopDongCongTacVienId);
                }

                if (Item.CongTacVienId != null)
                {
                    selectedCtv = context.CongTacViens.Find(Item.CongTacVienId);
                }
            }

            // Sync navigation properties on Item so any downstream usage is correct
            if (selectedCtv != null)
            {
                Item.CongTacVien = selectedCtv;
            }

            Helpers.WordExportHelper.ExportContractToWord(Item, parentInfo, selectedCtv);
        }
    }
}
