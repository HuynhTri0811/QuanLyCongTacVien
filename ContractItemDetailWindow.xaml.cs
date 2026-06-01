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

            TakeSnapshot();
        }

        // Dependency Property hoặc Simple Property cho Binding
        public ChiTietHopDong Item { get; set; }

        private ChiTietHopDongSnapshot _snapshot;
        private bool _isSaving = false;

        private void TakeSnapshot()
        {
            _snapshot = new ChiTietHopDongSnapshot
            {
                CongTacVienId = _item.CongTacVienId,
                SoHopDong = _item.SoHopDong,
                NgayKy = _item.NgayKy,
                BoPhan = _item.BoPhan,
                MaNhanSu = _item.MaNhanSu,
                HetHieuLuc = _item.HetHieuLuc,
                ChucDanh = _item.ChucDanh,
                InLaiThoaThuan = _item.InLaiThoaThuan,
                TuNgay = _item.TuNgay,
                DenNgay = _item.DenNgay
            };
        }

        private bool HasChanges()
        {
            if (_item.CongTacVienId != _snapshot.CongTacVienId) return true;
            if (_item.SoHopDong != _snapshot.SoHopDong) return true;
            if (_item.NgayKy != _snapshot.NgayKy) return true;
            if (_item.BoPhan != _snapshot.BoPhan) return true;
            if (_item.MaNhanSu != _snapshot.MaNhanSu) return true;
            if (_item.HetHieuLuc != _snapshot.HetHieuLuc) return true;
            if (_item.ChucDanh != _snapshot.ChucDanh) return true;
            if (_item.InLaiThoaThuan != _snapshot.InLaiThoaThuan) return true;
            if (_item.TuNgay != _snapshot.TuNgay) return true;
            if (_item.DenNgay != _snapshot.DenNgay) return true;
            return false;
        }

        private void RollbackChanges()
        {
            _item.CongTacVienId = _snapshot.CongTacVienId;
            _item.SoHopDong = _snapshot.SoHopDong;
            _item.NgayKy = _snapshot.NgayKy;
            _item.BoPhan = _snapshot.BoPhan;
            _item.MaNhanSu = _snapshot.MaNhanSu;
            _item.HetHieuLuc = _snapshot.HetHieuLuc;
            _item.ChucDanh = _snapshot.ChucDanh;
            _item.InLaiThoaThuan = _snapshot.InLaiThoaThuan;
            _item.TuNgay = _snapshot.TuNgay;
            _item.DenNgay = _snapshot.DenNgay;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _isSaving = true;
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_isSaving)
            {
                base.OnClosing(e);
                return;
            }

            // Force update focused TextBox binding to commit any active edits before change check
            var focused = System.Windows.Input.FocusManager.GetFocusedElement(this);
            if (focused is System.Windows.Controls.TextBox tb)
            {
                System.Windows.Data.BindingOperations.GetBindingExpression(tb, System.Windows.Controls.TextBox.TextProperty)?.UpdateSource();
            }

            if (HasChanges())
            {
                var confirm = new ConfirmCloseWindow(this, "Dữ liệu chi tiết đã có sự thay đổi. Bạn có muốn lưu lại trước khi đóng không?");
                confirm.ShowDialog();
                var result = confirm.Result;

                if (result == MessageBoxResult.Yes)
                {
                    _isSaving = true;
                    if (Item.CongTacVienId != null)
                    {
                        Item.CongTacVien = DanhSachCongTacVien.FirstOrDefault(c => c.Id == Item.CongTacVienId);
                    }
                    else
                    {
                        Item.CongTacVien = null;
                    }
                    this.DialogResult = true;
                }
                else if (result == MessageBoxResult.No)
                {
                    RollbackChanges();
                    this.DialogResult = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                this.DialogResult = false;
            }

            if (!e.Cancel)
            {
                base.OnClosing(e);
            }
        }

        private struct ChiTietHopDongSnapshot
        {
            public int? CongTacVienId;
            public string? SoHopDong;
            public DateTime? NgayKy;
            public string? BoPhan;
            public string? MaNhanSu;
            public bool HetHieuLuc;
            public string? ChucDanh;
            public bool InLaiThoaThuan;
            public DateTime? TuNgay;
            public DateTime? DenNgay;
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
