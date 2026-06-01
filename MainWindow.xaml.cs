using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using Microsoft.EntityFrameworkCore;

namespace QuanLyCongTacVien;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Models.CongTacVien> _allCongTacViens = new List<Models.CongTacVien>();
    private ICollectionView? _congTacVienView;
    private Dictionary<string, HashSet<string>> _columnFilters = new Dictionary<string, HashSet<string>>();
    private string? _currentFilteringProperty = null;
    private List<FilterItem> _currentFilterItems = new List<FilterItem>();
    private bool _isUpdatingCheckedStates = false;
    
    private List<Models.QuanLyHopDongCongTacVien> _allHopDongs = new List<Models.QuanLyHopDongCongTacVien>();
    private ICollectionView? _hopDongView;
    private Dictionary<string, HashSet<string>> _contractFilters = new Dictionary<string, HashSet<string>>();
    private string? _currentFilteringGridName = null;

    public MainWindow()
    {
        InitializeComponent();
        
        // Ensure database is created if we don't use migrations or as a fallback
        using (var context = new Data.AppDbContext())
        {
            context.Database.EnsureCreated();
        }

        LoadDuLieu();
    }

    private void BtnLoadData_Click(object sender, RoutedEventArgs e)
    {
        LoadDuLieu();
    }

    private void dgCongTacVien_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        // Ép kiểu đối tượng PropertyDescriptor để đọc metadata của thuộc tính đang được quét
        if (e.PropertyDescriptor is PropertyDescriptor descriptor)
        {
            // Nếu thuộc tính được gắn [Browsable(false)], descriptor.IsBrowsable sẽ trả về false
            if (!descriptor.IsBrowsable)
            {
                e.Cancel = true; // Hủy bỏ việc tự động tạo cột cho thuộc tính này
                return;
            }
                    // 2. Gán tiêu đề cột bằng DisplayName 
        // (Nếu property không gắn [DisplayName], nó sẽ tự lấy tên gốc)
                    e.Column.Header = descriptor.DisplayName; 
        }
    }

    void LoadDuLieu()
    {
        using (var context = new Data.AppDbContext())
        {
            _allCongTacViens = context.CongTacViens
                .Where(c => !c.IsDelete)
                .ToList();
        }
        
        _congTacVienView = CollectionViewSource.GetDefaultView(_allCongTacViens);
        _congTacVienView.Filter = FilterCongTacVien;
        dgCongTacVien.ItemsSource = _congTacVienView;
        
        UpdateFilterIcons();
        UpdateStatistics();
    }

    private void BtnAddNew_Click(object sender, RoutedEventArgs e)
    {
        var newCtv = new Models.CongTacVien
        {
            NgayVaoTruong = DateTime.Now,
            NgayCap = DateTime.Now,
            QuocTich = "Việt Nam",
            TinhTrang = "Đang làm việc",
            TrinhDoVanHoa = "Đại học"
        };
        
        var detailWindow = new DetailViewWindow(newCtv);
        if (detailWindow.ShowDialog() == true)
        {
            // Tải lại dữ liệu sau khi thêm mới thành công
            LoadDuLieu();
        }
    }

    private void dgCongTacVien_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (dgCongTacVien.SelectedItem is Models.CongTacVien selected)
        {
            var detailWindow = new DetailViewWindow(selected);
            if (detailWindow.ShowDialog() == true)
            {
                // Tải lại dữ liệu nếu có thay đổi và nhấn Lưu
                LoadDuLieu();
            }
        }
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (dgCongTacVien.SelectedItem is Models.CongTacVien selected)
        {
            using (var context = new Data.AppDbContext())
            {
                bool existsInContract = context.ChiTietHopDongs
                    .Any(d => d.CongTacVienId == selected.Id && !d.IsDelete && d.QuanLyHopDong != null && !d.QuanLyHopDong.IsDelete);

                if (existsInContract)
                {
                    CustomMessageBox.Show("Cộng tác viên này đã tồn tại trong quản lý hợp đồng cộng tác viên, không thể xóa!", 
                        "Lỗi", MessageBoxImage.Error);
                    return;
                }
            }

            var confirmWindow = new ConfirmDeleteWindow(this, $"Bạn có chắc chắn muốn xóa '{selected.HoVaTen}' không?");
            if (confirmWindow.ShowDialog() == true)
            {
                using (var context = new Data.AppDbContext())
                {
                    selected.IsDelete = true;
                    context.CongTacViens.Update(selected);
                    context.SaveChanges();
                }
                
                // Tải lại danh sách
                LoadDuLieu();
            }
        }
        else
        {
            CustomMessageBox.Show("Vui lòng chọn một người để xóa.", "Thông báo", MessageBoxImage.Warning);
        }
    }

    private void lstNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstNavigation.SelectedItem is ListBoxItem item)
        {
            // Tránh lỗi NullReferenceException khi UI đang khởi tạo
            if (gridCongTacVien == null || gridHopDong == null) return;

            if (item == lbiCongTacVien)
            {
                gridCongTacVien.Visibility = Visibility.Visible;
                gridHopDong.Visibility = Visibility.Collapsed;
                LoadDuLieu();
            }
            else if (item == lbiHopDong)
            {
                gridCongTacVien.Visibility = Visibility.Collapsed;
                gridHopDong.Visibility = Visibility.Visible;
                LoadHopDong();
            }
            else if (item == lbiTinhLuong)
            {
                CustomMessageBox.Show("Chức năng 'Hồ sơ tính lương (Cộng tác viên)' đang được cập nhật!", "Thông báo", MessageBoxImage.Information);
            }
        }
    }

    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyAllFilters();
    }

    private void UpdateStatistics()
    {
        // Tránh lỗi NullReferenceException khi UI đang khởi tạo
        if (txtStatTotal == null || txtStatActive == null || txtStatNew == null) return;

        using (var context = new Data.AppDbContext())
        {
            int total = context.CongTacViens.Count(c => !c.IsDelete);
            int active = context.CongTacViens.Count(c => !c.IsDelete && c.TinhTrang == "Đang làm việc");
            
            var now = DateTime.Now;
            int newThisMonth = context.CongTacViens.Count(c => !c.IsDelete && c.NgayVaoTruong.Month == now.Month && c.NgayVaoTruong.Year == now.Year);

            txtStatTotal.Text = $" {total}";
            txtStatActive.Text = $" {active}";
            txtStatNew.Text = $" {newThisMonth}";
        }
    }

    private void LoadHopDong()
    {
        using (var context = new Data.AppDbContext())
        {
            if (!context.QuanLyHopDongCongTacViens.Any())
            {
                var hopDongs = new List<Models.QuanLyHopDongCongTacVien>
                {
                    new Models.QuanLyHopDongCongTacVien { NienDoTaiChinh = "2021", NamHoc = "2021", TruongCongTy = "Trường Đại học Luật" },
                };
                context.QuanLyHopDongCongTacViens.AddRange(hopDongs);
                context.SaveChanges();
            }

            _allHopDongs = context.QuanLyHopDongCongTacViens
                .Where(h => !h.IsDelete)
                .ToList();
            
            _hopDongView = CollectionViewSource.GetDefaultView(_allHopDongs);
            _hopDongView.Filter = FilterHopDong;
            
            // Cài đặt Grouping cho DataGrid
            _hopDongView.GroupDescriptions.Clear();
            _hopDongView.GroupDescriptions.Add(new PropertyGroupDescription("TruongCongTy"));
            
            dgHopDong.ItemsSource = _hopDongView;
            UpdateFilterIcons();
        }
    }

    private void dgHopDong_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (dgHopDong.SelectedItem is Models.QuanLyHopDongCongTacVien selected)
        {
            var detailWindow = new ContractDetailWindow(selected);
            detailWindow.ShowDialog();
            // Tải lại sau khi đóng nếu cần
            LoadHopDong();
        }
    }

    private void BtnLoadHopDong_Click(object sender, RoutedEventArgs e)
    {
        LoadHopDong();
    }

    private void BtnAddHopDong_Click(object sender, RoutedEventArgs e)
    {
        var newHopDong = new Models.QuanLyHopDongCongTacVien
        {
            TruongCongTy = "Trường Đại học Luật",
            NamHoc = DateTime.Now.Year.ToString(),
            NienDoTaiChinh = DateTime.Now.Year.ToString()
        };

        var detailWindow = new ContractDetailWindow(newHopDong);
        detailWindow.ShowDialog();
        LoadHopDong();
    }

    private void BtnDeleteHopDong_Click(object sender, RoutedEventArgs e)
    {
        if (dgHopDong.SelectedItem is Models.QuanLyHopDongCongTacVien selected)
        {
            using (var context = new Data.AppDbContext())
            {
                bool hasDetails = context.ChiTietHopDongs
                    .Any(d => d.QuanLyHopDongCongTacVienId == selected.Id && !d.IsDelete);

                if (hasDetails)
                {
                    CustomMessageBox.Show("Đợt quản lý hợp đồng này vẫn còn chi tiết hợp đồng bên trong, không thể xóa!", 
                        "Lỗi", MessageBoxImage.Error);
                    return;
                }
            }

            var confirmWindow = new ConfirmDeleteWindow(this, $"Bạn có chắc chắn muốn xóa hợp đồng niên độ '{selected.NienDoTaiChinh}' không?");
            if (confirmWindow.ShowDialog() == true)
            {
                using (var context = new Data.AppDbContext())
                {
                    selected.IsDelete = true;
                    context.QuanLyHopDongCongTacViens.Update(selected);
                    context.SaveChanges();
                }
                
                LoadHopDong();
            }
        }
        else
        {
            CustomMessageBox.Show("Vui lòng chọn một hợp đồng để xóa.", "Thông báo", MessageBoxImage.Warning);
        }
    }

    private void BtnExportCTV_Click(object sender, RoutedEventArgs e)
    {
        using (var context = new Data.AppDbContext())
        {
            var data = context.CongTacViens.Where(c => !c.IsDelete).ToList();
            Helpers.ExcelExportHelper.ExportToExcel(data, "DanhSachCongTacVien.xlsx", "Cộng tác viên");
        }
    }

    private void BtnExportHopDong_Click(object sender, RoutedEventArgs e)
    {
        using (var context = new Data.AppDbContext())
        {
            var data = context.QuanLyHopDongCongTacViens.Where(h => !h.IsDelete).ToList();
            Helpers.ExcelExportHelper.ExportToExcel(data, "DanhSachHopDong.xlsx", "Hợp đồng");
        }
    }

    // --- Excel-like Filtering logic ---

    private bool FilterCongTacVien(object obj)
    {
        if (obj is not Models.CongTacVien ctv) return false;

        // 1. Text Search Filter
        string searchText = txtSearch.Text.ToLower().Trim();
        if (!string.IsNullOrEmpty(searchText))
        {
            bool matchesSearch = (ctv.HoVaTen != null && ctv.HoVaTen.ToLower().Contains(searchText)) ||
                                 (ctv.MaNhanSu != null && ctv.MaNhanSu.ToLower().Contains(searchText)) ||
                                 (ctv.MaNhanVien != null && ctv.MaNhanVien.ToLower().Contains(searchText)) ||
                                 (ctv.TenGoiKhac != null && ctv.TenGoiKhac.ToLower().Contains(searchText)) ||
                                 (ctv.NoiSinh != null && ctv.NoiSinh.ToLower().Contains(searchText));
            if (!matchesSearch) return false;
        }

        // 2. Column filters
        foreach (var filter in _columnFilters)
        {
            string propName = filter.Key;
            HashSet<string> allowedValues = filter.Value;

            string val = GetPropertyValueAsString(ctv, propName);
            string checkVal = string.IsNullOrWhiteSpace(val) ? "(Trống)" : val;

            if (!allowedValues.Contains(checkVal))
            {
                return false;
            }
        }

        return true;
    }

    private bool FilterHopDong(object obj)
    {
        if (obj is not Models.QuanLyHopDongCongTacVien hd) return false;

        foreach (var filter in _contractFilters)
        {
            string propName = filter.Key;
            HashSet<string> allowedValues = filter.Value;

            string val = GetContractPropertyValueAsString(hd, propName);
            string checkVal = string.IsNullOrWhiteSpace(val) ? "(Trống)" : val;

            if (!allowedValues.Contains(checkVal))
            {
                return false;
            }
        }

        return true;
    }

    private void BtnFilterColumn_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var header = button?.Tag as DataGridColumnHeader;
        var column = header?.Column;
        if (column == null) return;

        var dataGrid = FindVisualParent<DataGrid>(header);
        if (dataGrid == null) return;

        string? propName = column.SortMemberPath;
        if (string.IsNullOrEmpty(propName)) return;

        _currentFilteringProperty = propName;
        _currentFilteringGridName = dataGrid.Name;
        txtPopupSearch.Text = string.Empty;

        List<string> uniqueValues;
        bool hasActiveFilter;
        HashSet<string>? activeSet = null;

        if (_currentFilteringGridName == "dgCongTacVien")
        {
            uniqueValues = _allCongTacViens
                .Select(ctv => GetPropertyValueAsString(ctv, propName))
                .Select(val => string.IsNullOrWhiteSpace(val) ? "(Trống)" : val)
                .Distinct()
                .OrderBy(v => v)
                .ToList();
            hasActiveFilter = _columnFilters.TryGetValue(propName, out activeSet);
        }
        else // "dgHopDong"
        {
            uniqueValues = _allHopDongs
                .Select(hd => GetContractPropertyValueAsString(hd, propName))
                .Select(val => string.IsNullOrWhiteSpace(val) ? "(Trống)" : val)
                .Distinct()
                .OrderBy(v => v)
                .ToList();
            hasActiveFilter = _contractFilters.TryGetValue(propName, out activeSet);
        }

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
        if (string.IsNullOrEmpty(_currentFilteringProperty) || string.IsNullOrEmpty(_currentFilteringGridName))
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

        if (_currentFilteringGridName == "dgCongTacVien")
        {
            if (checkedValues.Count == allValues.Count)
            {
                _columnFilters.Remove(_currentFilteringProperty);
            }
            else
            {
                _columnFilters[_currentFilteringProperty] = new HashSet<string>(checkedValues);
            }
        }
        else // dgHopDong
        {
            if (checkedValues.Count == allValues.Count)
            {
                _contractFilters.Remove(_currentFilteringProperty);
            }
            else
            {
                _contractFilters[_currentFilteringProperty] = new HashSet<string>(checkedValues);
            }
        }

        FilterPopup.IsOpen = false;
        ApplyAllFilters();
    }

    private void BtnClearColumnFilter_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_currentFilteringProperty) && !string.IsNullOrEmpty(_currentFilteringGridName))
        {
            if (_currentFilteringGridName == "dgCongTacVien")
            {
                _columnFilters.Remove(_currentFilteringProperty);
            }
            else
            {
                _contractFilters.Remove(_currentFilteringProperty);
            }
        }
        FilterPopup.IsOpen = false;
        ApplyAllFilters();
    }

    private string GetPropertyValueAsString(Models.CongTacVien ctv, string propName)
    {
        if (ctv == null || string.IsNullOrEmpty(propName)) return string.Empty;

        var prop = typeof(Models.CongTacVien).GetProperty(propName);
        if (prop == null) return string.Empty;

        object? val = prop.GetValue(ctv);
        if (val == null) return string.Empty;

        if (val is DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        return val.ToString() ?? string.Empty;
    }

    private string GetContractPropertyValueAsString(Models.QuanLyHopDongCongTacVien hd, string propName)
    {
        if (hd == null || string.IsNullOrEmpty(propName)) return string.Empty;

        var prop = typeof(Models.QuanLyHopDongCongTacVien).GetProperty(propName);
        if (prop == null) return string.Empty;

        object? val = prop.GetValue(hd);
        if (val == null) return string.Empty;

        return val.ToString() ?? string.Empty;
    }

    private T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
    {
        if (child == null) return null;
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);
        if (parentObject == null) return null;
        if (parentObject is T parent) return parent;
        return FindVisualParent<T>(parentObject);
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
        // 1. dgCongTacVien
        var headersCtv = FindVisualChildren<DataGridColumnHeader>(dgCongTacVien);
        foreach (var header in headersCtv)
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

        // 2. dgHopDong
        var headersHd = FindVisualChildren<DataGridColumnHeader>(dgHopDong);
        foreach (var header in headersHd)
        {
            var column = header.Column;
            if (column == null) continue;

            string? propName = column.SortMemberPath;
            if (string.IsNullOrEmpty(propName)) continue;

            var path = header.Template.FindName("filterPath", header) as Path;
            if (path != null)
            {
                bool hasActiveFilter = _contractFilters.ContainsKey(propName) && _contractFilters[propName].Count > 0;
                path.Fill = hasActiveFilter 
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F62AC")) 
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8899A6"));
            }
        }
    }

    private void ApplyAllFilters()
    {
        if (_congTacVienView != null)
        {
            _congTacVienView.Refresh();
        }
        if (_hopDongView != null)
        {
            _hopDongView.Refresh();
        }
        UpdateFilterIcons();
        UpdateStatistics();
    }
}
