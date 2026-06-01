using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class ContractDetailWindow : Window
    {
        private QuanLyHopDongCongTacVien _hopDong;
        private ObservableCollection<ChiTietHopDong> _chiTietItems = new ObservableCollection<ChiTietHopDong>();
        private ICollectionView? _chiTietView;
        private Dictionary<string, HashSet<string>> _columnFilters = new Dictionary<string, HashSet<string>>();
        private string? _currentFilteringProperty = null;
        private List<FilterItem> _currentFilterItems = new List<FilterItem>();
        private bool _isUpdatingCheckedStates = false;
        private string _originalTruongCongTy = string.Empty;
        private string _originalNamHoc = string.Empty;
        private string _originalNienDoTaiChinh = string.Empty;
        private int _originalSTT = 1;
        private List<ChiTietHopDongSnapshot> _originalDetails = new List<ChiTietHopDongSnapshot>();
        private bool _isSaving = false;

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
                ConfigureFilter();
                UpdateTotalCount();
                TakeSnapshot();
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

            var itemDetailWindow = new ContractItemDetailWindow(newItem);
            itemDetailWindow.Owner = this;
            if (itemDetailWindow.ShowDialog() == true)
            {
                _chiTietItems.Add(newItem);
                UpdateTotalCount();
            }
        }

        private void BtnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (dgChiTiet.SelectedItem is ChiTietHopDong selected)
            {
                var confirmWindow = new ConfirmDeleteWindow(this, "Bạn có chắc chắn muốn xóa dòng này không?");
                if (confirmWindow.ShowDialog() == true)
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
                    UpdateTotalCount();
                }
            }
            else
            {
                CustomMessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxImage.Warning);
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

        private bool SaveData()
        {
            try
            {
                _isSaving = true;
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
                
                // Cập nhật lại snapshot gốc sau khi lưu thành công
                TakeSnapshot();
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxImage.Error);
                return false;
            }
            finally
            {
                _isSaving = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData())
            {
                CustomMessageBox.Show("Đã lưu thông tin hợp đồng thành công!", "Thông báo", MessageBoxImage.Information);
                // Nạp lại dữ liệu để đảm bảo các navigation property (CongTacVien) được tải lại cho UI
                LoadDetails();
            }
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
                CustomMessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxImage.Warning);
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
                    var confirm = new ConfirmDeleteWindow(this, 
                        $"Bạn có chắc chắn muốn sao chép toàn bộ danh sách hợp đồng từ đợt '{sourceHopDong.TruongCongTy} - {sourceHopDong.NienDoTaiChinh}' không?\n(Các dòng hợp đồng sẽ được thêm vào danh sách hiện tại)",
                        "Xác nhận sao chép");

                    if (confirm.ShowDialog() == true)
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
                                CustomMessageBox.Show("Hợp đồng nguồn được chọn không có chi tiết hợp đồng nào để sao chép!", "Thông báo", MessageBoxImage.Information);
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
                            UpdateTotalCount();
                        }

                        CustomMessageBox.Show($"Đã sao chép thành công {copiedCount} dòng hợp đồng! Hãy nhấn nút 'Lưu' để lưu vào cơ sở dữ liệu.", "Thông báo", MessageBoxImage.Information);
                    }
                }
            }
        }

        private void UpdateTotalCount()
        {
            if (txtTotalContracts != null)
            {
                txtTotalContracts.Text = $" {_chiTietItems?.Count ?? 0}";
            }
        }

        private void ConfigureFilter()
        {
            if (_chiTietItems != null)
            {
                _chiTietView = System.Windows.Data.CollectionViewSource.GetDefaultView(_chiTietItems);
                if (_chiTietView != null)
                {
                    _chiTietView.Filter = FilterChiTiet;
                }
            }
        }

        private bool FilterChiTiet(object obj)
        {
            if (obj is not ChiTietHopDong detail) return false;
            if (detail.IsDelete) return false;

            // 1. Text Search Filter
            string searchText = txtSearchContract.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                bool matches = detail.SoHopDong != null && 
                               detail.SoHopDong.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                if (!matches) return false;
            }

            // 2. Column Filters
            foreach (var filter in _columnFilters)
            {
                string propName = filter.Key;
                HashSet<string> allowedValues = filter.Value;

                string val = GetPropertyValueAsString(detail, propName);
                string checkVal = string.IsNullOrWhiteSpace(val) ? "(Trống)" : val;

                if (!allowedValues.Contains(checkVal))
                {
                    return false;
                }
            }

            return true;
        }

        private void TxtSearchContract_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyAllFilters();
        }

        private void TakeSnapshot()
        {
            _originalTruongCongTy = _hopDong.TruongCongTy ?? string.Empty;
            _originalNamHoc = _hopDong.NamHoc ?? string.Empty;
            _originalNienDoTaiChinh = _hopDong.NienDoTaiChinh ?? string.Empty;
            _originalSTT = _hopDong.STT;

            _originalDetails = _chiTietItems.Select(d => new ChiTietHopDongSnapshot
            {
                Id = d.Id,
                SoHopDong = d.SoHopDong ?? string.Empty,
                NgayKy = d.NgayKy,
                BoPhan = d.BoPhan ?? string.Empty,
                CongTacVienId = d.CongTacVienId,
                MaNhanSu = d.MaNhanSu ?? string.Empty,
                ChucDanh = d.ChucDanh ?? string.Empty,
                TuNgay = d.TuNgay,
                DenNgay = d.DenNgay,
                HetHieuLuc = d.HetHieuLuc,
                InLaiThoaThuan = d.InLaiThoaThuan,
                IsDelete = d.IsDelete
            }).ToList();
        }

        private bool HasChanges()
        {
            if ((_hopDong.TruongCongTy ?? string.Empty) != _originalTruongCongTy) return true;
            if ((_hopDong.NamHoc ?? string.Empty) != _originalNamHoc) return true;
            if ((_hopDong.NienDoTaiChinh ?? string.Empty) != _originalNienDoTaiChinh) return true;
            if (_hopDong.STT != _originalSTT) return true;

            if (_chiTietItems.Count != _originalDetails.Count) return true;

            foreach (var item in _chiTietItems)
            {
                var original = _originalDetails.FirstOrDefault(o => o.Id == item.Id && item.Id != 0);
                if (item.Id == 0) return true;
                if (original.Id == 0) return true;

                if ((item.SoHopDong ?? string.Empty) != original.SoHopDong) return true;
                if (item.NgayKy != original.NgayKy) return true;
                if ((item.BoPhan ?? string.Empty) != original.BoPhan) return true;
                if (item.CongTacVienId != original.CongTacVienId) return true;
                if ((item.MaNhanSu ?? string.Empty) != original.MaNhanSu) return true;
                if ((item.ChucDanh ?? string.Empty) != original.ChucDanh) return true;
                if (item.TuNgay != original.TuNgay) return true;
                if (item.DenNgay != original.DenNgay) return true;
                if (item.HetHieuLuc != original.HetHieuLuc) return true;
                if (item.InLaiThoaThuan != original.InLaiThoaThuan) return true;
                if (item.IsDelete != original.IsDelete) return true;
            }

            return false;
        }

        private void RollbackChanges()
        {
            if (_hopDong.Id == 0)
            {
                _hopDong.ChiTietHopDongs.Clear();
                return;
            }

            using (var context = new Data.AppDbContext())
            {
                var dbHopDong = context.QuanLyHopDongCongTacViens
                    .Include(h => h.ChiTietHopDongs)
                        .ThenInclude(d => d.CongTacVien)
                    .FirstOrDefault(h => h.Id == _hopDong.Id);

                if (dbHopDong != null)
                {
                    _hopDong.TruongCongTy = dbHopDong.TruongCongTy;
                    _hopDong.NamHoc = dbHopDong.NamHoc;
                    _hopDong.NienDoTaiChinh = dbHopDong.NienDoTaiChinh;
                    _hopDong.STT = dbHopDong.STT;
                    _hopDong.IsDelete = dbHopDong.IsDelete;

                    _hopDong.ChiTietHopDongs.Clear();
                    foreach (var detail in dbHopDong.ChiTietHopDongs)
                    {
                        _hopDong.ChiTietHopDongs.Add(detail);
                    }
                }
            }
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
                var confirm = new ConfirmCloseWindow(this, "Dữ liệu đã có sự thay đổi. Bạn có muốn lưu lại trước khi đóng không?");
                confirm.ShowDialog();
                var result = confirm.Result;

                if (result == MessageBoxResult.Yes)
                {
                    bool saveSuccess = SaveData();
                    if (!saveSuccess)
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    RollbackChanges();
                }
                else
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                base.OnClosing(e);
            }
        }

        private struct ChiTietHopDongSnapshot
        {
            public int Id;
            public string SoHopDong;
            public DateTime? NgayKy;
            public string BoPhan;
            public int? CongTacVienId;
            public string MaNhanSu;
            public string ChucDanh;
            public DateTime? TuNgay;
            public DateTime? DenNgay;
            public bool HetHieuLuc;
            public bool InLaiThoaThuan;
            public bool IsDelete;
        }

        // --- Excel-like Filtering logic ---

        private void BtnFilterColumn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var header = button?.Tag as DataGridColumnHeader;
            var column = header?.Column;
            if (column == null) return;

            string? propName = column.SortMemberPath;
            if (string.IsNullOrEmpty(propName)) return;

            _currentFilteringProperty = propName;
            txtPopupSearch.Text = string.Empty;

            // 1. Extract distinct values of the property across all active items
            var uniqueValues = _chiTietItems
                .Where(d => !d.IsDelete)
                .Select(d => GetPropertyValueAsString(d, propName))
                .Select(val => string.IsNullOrWhiteSpace(val) ? "(Trống)" : val)
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            // 2. Check if a filter is already active for this property
            bool hasActiveFilter = _columnFilters.TryGetValue(propName, out var activeSet);

            _isUpdatingCheckedStates = true;
            _currentFilterItems.Clear();

            // Add "(Chọn tất cả)" checkbox
            var selectAllItem = new FilterItem 
            { 
                ValueText = "(Chọn tất cả)", 
                IsChecked = !hasActiveFilter || (activeSet != null && activeSet.Count == uniqueValues.Count) 
            };
            _currentFilterItems.Add(selectAllItem);

            foreach (var val in uniqueValues)
            {
                bool isChecked = !hasActiveFilter || (activeSet != null && activeSet.Contains(val));
                _currentFilterItems.Add(new FilterItem 
                { 
                    ValueText = val, 
                    IsChecked = isChecked 
                });
            }

            lstFilterValues.ItemsSource = null;
            lstFilterValues.ItemsSource = _currentFilterItems;
            _isUpdatingCheckedStates = false;

            // Show Popup next to the button
            FilterPopup.PlacementTarget = button;
            FilterPopup.IsOpen = true;
        }

        private void txtPopupSearch_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void txtPopupSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentFilteringProperty == null) return;

            string searchText = txtPopupSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                lstFilterValues.ItemsSource = _currentFilterItems;
            }
            else
            {
                var filtered = _currentFilterItems
                    .Where(item => item.ValueText == "(Chọn tất cả)" || item.ValueText.ToLower().Contains(searchText))
                    .ToList();
                lstFilterValues.ItemsSource = filtered;
            }
        }

        private void FilterCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (_isUpdatingCheckedStates) return;

            var checkBox = sender as CheckBox;
            var clickedItem = checkBox?.DataContext as FilterItem;
            if (clickedItem == null) return;

            _isUpdatingStatesInternal();
        }

        private void _isUpdatingStatesInternal()
        {
            _isUpdatingCheckedStates = true;

            var selectAllItem = _currentFilterItems.FirstOrDefault(i => i.ValueText == "(Chọn tất cả)");
            if (selectAllItem != null)
            {
                var currentSource = (lstFilterValues.ItemsSource as IEnumerable<FilterItem>)?.ToList();
                if (currentSource != null)
                {
                    var clickedItem = currentSource.FirstOrDefault(i => i.ValueText == "(Chọn tất cả)" && i.IsChecked != selectAllItem.IsChecked) 
                                      ?? _currentFilterItems.FirstOrDefault(i => i.IsChecked != i.WasChecked);

                    if (clickedItem != null)
                    {
                        if (clickedItem.ValueText == "(Chọn tất cả)")
                        {
                            bool isChecked = clickedItem.IsChecked;
                            foreach (var item in currentSource)
                            {
                                if (item.ValueText != "(Chọn tất cả)")
                                {
                                    item.IsChecked = isChecked;
                                }
                            }
                        }
                        else
                        {
                            var otherItems = currentSource.Where(i => i.ValueText != "(Chọn tất cả)").ToList();
                            if (otherItems.All(i => i.IsChecked))
                            {
                                selectAllItem.IsChecked = true;
                            }
                            else
                            {
                                selectAllItem.IsChecked = false;
                            }
                        }
                    }
                }
            }

            foreach (var item in _currentFilterItems)
            {
                item.WasChecked = item.IsChecked;
            }

            _isUpdatingCheckedStates = false;
        }

        private void BtnApplyColumnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilteringProperty))
            {
                FilterPopup.IsOpen = false;
                return;
            }

            var checkedValues = _currentFilterItems
                .Where(item => item.ValueText != "(Chọn tất cả)" && item.IsChecked)
                .Select(item => item.ValueText)
                .ToList();

            var allValues = _currentFilterItems
                .Where(item => item.ValueText != "(Chọn tất cả)")
                .Select(item => item.ValueText)
                .ToList();

            if (checkedValues.Count == allValues.Count)
            {
                _columnFilters.Remove(_currentFilteringProperty);
            }
            else
            {
                _columnFilters[_currentFilteringProperty] = new HashSet<string>(checkedValues);
            }

            FilterPopup.IsOpen = false;
            ApplyAllFilters();
        }

        private void BtnClearColumnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentFilteringProperty))
            {
                _columnFilters.Remove(_currentFilteringProperty);
            }
            FilterPopup.IsOpen = false;
            ApplyAllFilters();
        }

        private string GetPropertyValueAsString(ChiTietHopDong detail, string propName)
        {
            if (detail == null || string.IsNullOrEmpty(propName)) return string.Empty;

            if (propName == "NguoiLaoDong")
            {
                return detail.NguoiLaoDong ?? string.Empty;
            }

            var prop = typeof(ChiTietHopDong).GetProperty(propName);
            if (prop == null) return string.Empty;

            object? val = prop.GetValue(detail);
            if (val == null) return string.Empty;

            if (val is DateTime dt)
            {
                return dt.ToString("dd/MM/yyyy");
            }
            if (val is bool b)
            {
                return b ? "Có" : "Không";
            }

            return val.ToString() ?? string.Empty;
        }

        private List<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject
        {
            List<T> list = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null)
                    {
                        if (child is T t)
                        {
                            list.Add(t);
                        }
                        list.AddRange(FindVisualChildren<T>(child));
                    }
                }
            }
            return list;
        }

        private void UpdateFilterIcons()
        {
            var headers = FindVisualChildren<DataGridColumnHeader>(dgChiTiet);
            foreach (var header in headers)
            {
                var column = header.Column;
                if (column == null) continue;

                string? propName = column.SortMemberPath;
                if (string.IsNullOrEmpty(propName)) continue;

                var path = header.Template.FindName("filterPath", header) as Path;
                if (path != null)
                {
                    bool hasActiveFilter = _columnFilters.ContainsKey(propName) && _columnFilters[propName].Count > 0;
                    path.Fill = hasActiveFilter 
                        ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F62AC")) 
                        : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8899A6"));
                }
            }
        }

        private void ApplyAllFilters()
        {
            if (_chiTietView != null)
            {
                _chiTietView.Refresh();
                UpdateFilterIcons();
                UpdateTotalCount();
            }
        }
    }

}
