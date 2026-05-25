using System.Linq;
using System.Windows;
using System.Windows.Input;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class SelectContractWindow : Window
    {
        private int _excludeId;
        public QuanLyHopDongCongTacVien? SelectedHopDong { get; private set; }

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
                var query = context.QuanLyHopDongCongTacViens
                    .Where(h => !h.IsDelete && h.Id != _excludeId)
                    .OrderByDescending(h => h.NienDoTaiChinh)
                    .ThenBy(h => h.TruongCongTy)
                    .ToList();

                dgHopDongNguon.ItemsSource = query;
            }
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
                MessageBox.Show("Vui lòng chọn một dòng hợp đồng nguồn trong danh sách.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
