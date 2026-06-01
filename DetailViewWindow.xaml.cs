using System.Windows;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class DetailViewWindow : Window
    {
        private CongTacVien _congTacVien;
        private List<ChiTietHopDong> _hopDongsList = new List<ChiTietHopDong>();
        private ICollectionView? _hopDongsView;
        private Dictionary<string, HashSet<string>> _columnFilters = new Dictionary<string, HashSet<string>>();
        private string? _currentFilteringProperty = null;
        private List<FilterItem> _currentFilterItems = new List<FilterItem>();
        private bool _isUpdatingCheckedStates = false;

        public DetailViewWindow(CongTacVien congTacVien)
        {
            InitializeComponent();
            _congTacVien = congTacVien;
            
            // Set DataContext to bind UI to the model
            this.DataContext = _congTacVien;

            LoadHopDongs();
            TakeSnapshot();
        }

        private void LoadHopDongs()
        {
            if (_congTacVien.Id != 0)
            {
                using (var context = new Data.AppDbContext())
                {
                    _hopDongsList = context.ChiTietHopDongs
                        .Include(d => d.CongTacVien)
                        .Where(d => d.CongTacVienId == _congTacVien.Id && !d.IsDelete && d.QuanLyHopDong != null && !d.QuanLyHopDong.IsDelete)
                        .ToList();
                }

                _hopDongsView = System.Windows.Data.CollectionViewSource.GetDefaultView(_hopDongsList);
                _hopDongsView.Filter = FilterChiTietHopDong;
                dgChiTietHopDong.ItemsSource = _hopDongsView;
                UpdateFilterIcons();
            }
        }

        private CongTacVien? _snapshot;
        private bool _isSaving = false;

        private void CopyProperties(CongTacVien source, CongTacVien target)
        {
            foreach (var prop in typeof(CongTacVien).GetProperties())
            {
                if (prop.CanWrite && prop.CanRead && prop.Name != "Id")
                {
                    prop.SetValue(target, prop.GetValue(source));
                }
            }
        }

        private void TakeSnapshot()
        {
            _snapshot = new CongTacVien();
            CopyProperties(_congTacVien, _snapshot);
        }

        private bool HasChanges()
        {
            if (_snapshot == null) return false;
            foreach (var prop in typeof(CongTacVien).GetProperties())
            {
                if (prop.CanRead && prop.Name != "Id" && prop.Name != "IsDelete")
                {
                    var val1 = prop.GetValue(_congTacVien);
                    var val2 = prop.GetValue(_snapshot);
                    if (val1 == null && val2 == null) continue;
                    if (val1 == null || val2 == null || !val1.Equals(val2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void RollbackChanges()
        {
            if (_snapshot != null)
            {
                CopyProperties(_snapshot, _congTacVien);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _isSaving = true;
            using (var context = new Data.AppDbContext())
            {
                if (_congTacVien.Id == 0)
                {
                    context.CongTacViens.Add(_congTacVien);
                }
                else
                {
                    context.CongTacViens.Update(_congTacVien);
                }
                context.SaveChanges();
            }
            CustomMessageBox.Show("Đã lưu thông tin thành công!", "Thông báo", MessageBoxImage.Information);
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
                var confirm = new ConfirmCloseWindow(this, "Dữ liệu cộng tác viên đã có sự thay đổi. Bạn có muốn lưu lại trước khi đóng không?");
                confirm.ShowDialog();
                var result = confirm.Result;

                if (result == MessageBoxResult.Yes)
                {
                    _isSaving = true;
                    using (var context = new Data.AppDbContext())
                    {
                        if (_congTacVien.Id == 0)
                        {
                            context.CongTacViens.Add(_congTacVien);
                        }
                        else
                        {
                            context.CongTacViens.Update(_congTacVien);
                        }
                        context.SaveChanges();
                    }
                    CustomMessageBox.Show("Đã lưu thông tin thành công!", "Thông báo", MessageBoxImage.Information);
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

        }

        // --- Excel-like Filtering logic ---

        private bool FilterChiTietHopDong(object obj)
        {
            if (obj is not ChiTietHopDong detail) return false;

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

            // 1. Extract distinct values of the property across all items
            var uniqueValues = _hopDongsList
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
            var headers = FindVisualChildren<DataGridColumnHeader>(dgChiTietHopDong);
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
            if (_hopDongsView != null)
            {
                _hopDongsView.Refresh();
                UpdateFilterIcons();
            }
        }
    }

}
