using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class ContractDetailWindow : Window
    {
        private QuanLyHopDongCongTacVien _hopDong;
        private ObservableCollection<ChiTietHopDong> _chiTietItems;

        public ContractDetailWindow(QuanLyHopDongCongTacVien hopDong)
        {
            InitializeComponent();
            _hopDong = hopDong;
            this.DataContext = _hopDong;
            
            LoadDetails();
        }

        private void LoadDetails()
        {
            using (var context = new Data.AppDbContext())
            {
                // Tải hợp đồng cùng với các chi tiết liên quan (chỉ lấy những cái chưa xóa)
                var hopDongWithDetails = context.QuanLyHopDongCongTacViens
                    .Include(h => h.ChiTietHopDongs)
                        .ThenInclude(d => d.CongTacVien)
                    .FirstOrDefault(h => h.Id == _hopDong.Id);

                if (hopDongWithDetails != null)
                {
                    _hopDong = hopDongWithDetails;
                }
                
                this.DataContext = _hopDong;
                
                // Lọc bỏ những mục đã bị đánh dấu xóa
                var items = _hopDong.ChiTietHopDongs.Where(d => !d.IsDelete).ToList();
                _chiTietItems = new ObservableCollection<ChiTietHopDong>(items);
                dgChiTiet.ItemsSource = _chiTietItems;
            }
        }

        private void BtnAddRow_Click(object sender, RoutedEventArgs e)
        {
            var newItem = new ChiTietHopDong
            {
                QuanLyHopDongCongTacVienId = _hopDong.Id,
                NgayKy = DateTime.Now,
                TuNgay = DateTime.Now
            };
            _chiTietItems.Add(newItem);
        }

        private void BtnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (dgChiTiet.SelectedItem is ChiTietHopDong selected)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    selected.IsDelete = true;
                    // Nếu là item mới chưa có Id thì xóa khỏi list luôn
                    if (selected.Id == 0)
                    {
                        _chiTietItems.Remove(selected);
                    }
                    else
                    {
                        // Nếu đã có trong DB thì chỉ ẩn đi, việc cập nhật IsDelete sẽ thực hiện khi nhấn Lưu
                        _chiTietItems.Remove(selected);
                        
                        // Đảm bảo item này vẫn nằm trong collection của _hopDong để EF cập nhật
                        if (!_hopDong.ChiTietHopDongs.Contains(selected))
                        {
                            _hopDong.ChiTietHopDongs.Add(selected);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgChiTiet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgChiTiet.SelectedItem is ChiTietHopDong selected)
            {
                // Kết thúc việc chỉnh sửa trên Grid nếu có để tránh lỗi khi Refresh
                dgChiTiet.CommitEdit(DataGridEditingUnit.Row, true);

                var itemDetailWindow = new ContractItemDetailWindow(selected);
                itemDetailWindow.Owner = this;
                if (itemDetailWindow.ShowDialog() == true)
                {
                    try
                    {
                        dgChiTiet.Items.Refresh();
                    }
                    catch (System.InvalidOperationException)
                    {
                        // Nếu vẫn không cho Refresh, ta gán lại ItemsSource để ép giao diện cập nhật
                        dgChiTiet.ItemsSource = null;
                        dgChiTiet.ItemsSource = _chiTietItems;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Data.AppDbContext())
            {
                // Đồng bộ dữ liệu từ ObservableCollection vào _hopDong.ChiTietHopDongs
                foreach (var item in _chiTietItems)
                {
                    if (!_hopDong.ChiTietHopDongs.Contains(item))
                    {
                        _hopDong.ChiTietHopDongs.Add(item);
                    }
                }

                // Ngắt liên kết các đối tượng CongTacVien để tránh lỗi trùng lặp Tracking (vì chúng đến từ các context khác nhau)
                // EF sẽ dựa vào CongTacVienId để lưu đúng liên kết
                foreach (var detail in _hopDong.ChiTietHopDongs)
                {
                    detail.CongTacVien = null;
                }

                if (_hopDong.Id == 0)
                {
                    context.QuanLyHopDongCongTacViens.Add(_hopDong);
                }
                else
                {
                    context.QuanLyHopDongCongTacViens.Update(_hopDong);
                }
                
                context.SaveChanges();
            }
            MessageBox.Show("Đã lưu thông tin hợp đồng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            
            // Nạp lại dữ liệu để đảm bảo các navigation property (CongTacVien) được tải lại cho UI
            LoadDetails();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnExportDetails_Click(object sender, RoutedEventArgs e)
        {
            if (_chiTietItems != null && _chiTietItems.Count > 0)
            {
                Helpers.ExcelExportHelper.ExportToExcel(_chiTietItems, $"ChiTietHopDong_{_hopDong.NienDoTaiChinh}.xlsx", "Chi tiết hợp đồng");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCopyOldContracts_Click(object sender, RoutedEventArgs e)
        {
            // Open selection dialog, excluding current contract
            var selectWindow = new SelectContractWindow(_hopDong.Id);
            selectWindow.Owner = this;
            if (selectWindow.ShowDialog() == true)
            {
                var sourceHopDong = selectWindow.SelectedHopDong;
                if (sourceHopDong != null)
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn sao chép toàn bộ danh sách hợp đồng từ đợt '{sourceHopDong.TruongCongTy} - {sourceHopDong.NienDoTaiChinh}' không?\n(Các dòng hợp đồng sẽ được thêm vào danh sách hiện tại)",
                        "Xác nhận sao chép",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    if (result == MessageBoxResult.Yes)
                    {
                        int copiedCount = 0;
                        using (var context = new Data.AppDbContext())
                        {
                            // Fetch all active lines from the selected source
                            var sourceLines = context.ChiTietHopDongs
                                .Include(d => d.CongTacVien)
                                .Where(d => d.QuanLyHopDongCongTacVienId == sourceHopDong.Id && !d.IsDelete)
                                .ToList();

                            if (sourceLines.Count == 0)
                            {
                                MessageBox.Show("Hợp đồng nguồn được chọn không có chi tiết hợp đồng nào để sao chép!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }

                            // Clone and append to the current collection
                            foreach (var line in sourceLines)
                            {
                                var newLine = new ChiTietHopDong
                                {
                                    QuanLyHopDongCongTacVienId = _hopDong.Id,
                                    SoHopDong = line.SoHopDong,
                                    NgayKy = line.NgayKy,
                                    BoPhan = line.BoPhan,
                                    CongTacVienId = line.CongTacVienId,
                                    MaNhanSu = line.MaNhanSu,
                                    ChucDanh = line.ChucDanh,
                                    TuNgay = line.TuNgay,
                                    DenNgay = line.DenNgay,
                                    HetHieuLuc = line.HetHieuLuc,
                                    InLaiThoaThuan = line.InLaiThoaThuan,
                                    CongTacVien = line.CongTacVien
                                };
                                _chiTietItems.Add(newLine);
                            }
                            copiedCount = sourceLines.Count;
                        }

                        MessageBox.Show($"Đã sao chép thành công {copiedCount} dòng hợp đồng! Hãy nhấn nút 'Lưu' để lưu vào cơ sở dữ liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
