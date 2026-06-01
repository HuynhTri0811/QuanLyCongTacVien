using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class SelectContractWindow : Window
    {
        private int _excludeId;
        public QuanLyHopDongCongTacVien? SelectedHopDong { get; private set; }

        private List<QuanLyHopDongCongTacVien> _contractsList = new List<QuanLyHopDongCongTacVien>();
        private ICollectionView? _contractsView;
        private Dictionary<string, HashSet<string>> _columnFilters = new Dictionary<string, HashSet<string>>();
        private string? _currentFilteringProperty = null;
        private List<FilterItem> _currentFilterItems = new List<FilterItem>();
        private bool _isUpdatingCheckedStates = false;

        public SelectContractWindow(int excludeId)
        {
            InitializeComponent();
            _excludeId = excludeId;
            LoadContracts();
        }

        private void LoadContracts()
        {
            using (var context = new Data.AppDbContext())
            {
                // Query contracts that are not marked deleted and not the current one
                _contractsList = context.QuanLyHopDongCongTacViens
                    .Where(h => !h.IsDelete && h.Id != _excludeId)
                    .OrderByDescending(h => h.NienDoTaiChinh)
                    .ThenBy(h => h.TruongCongTy)
                    .ToList();
            }

            _contractsView = System.Windows.Data.CollectionViewSource.GetDefaultView(_contractsList);
            _contractsView.Filter = FilterSelectContract;
            dgHopDongNguon.ItemsSource = _contractsView;
            UpdateFilterIcons();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmSelection();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void dgHopDongNguon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ConfirmSelection();
        }

        private void ConfirmSelection()
        {
            if (dgHopDongNguon.SelectedItem is QuanLyHopDongCongTacVien selected)
            {
                SelectedHopDong = selected;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                CustomMessageBox.Show("Vui lòng chọn một dòng hợp đồng nguồn trong danh sách.", "Thông báo", MessageBoxImage.Warning);
            }
        }

        // --- Excel-like Filtering logic ---

        private bool FilterSelectContract(object obj)
        {
            if (obj is not QuanLyHopDongCongTacVien contract) return false;

            foreach (var filter in _columnFilters)
            {
                string propName = filter.Key;
                HashSet<string> allowedValues = filter.Value;

                string val = GetPropertyValueAsString(contract, propName);
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
            var uniqueValues = _contractsList
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

        private string GetPropertyValueAsString(QuanLyHopDongCongTacVien contract, string propName)
        {
            if (contract == null || string.IsNullOrEmpty(propName)) return string.Empty;

            var prop = typeof(QuanLyHopDongCongTacVien).GetProperty(propName);
            if (prop == null) return string.Empty;

            object? val = prop.GetValue(contract);
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
            var headers = FindVisualChildren<DataGridColumnHeader>(dgHopDongNguon);
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
            if (_contractsView != null)
            {
                _contractsView.Refresh();
                UpdateFilterIcons();
            }
        }
    }
}
