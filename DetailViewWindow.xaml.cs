using System.Windows;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyCongTacVien.Models;

namespace QuanLyCongTacVien
{
    public partial class DetailViewWindow : Window
    {
        private CongTacVien _congTacVien;

        public DetailViewWindow(CongTacVien congTacVien)
        {
            InitializeComponent();
            _congTacVien = congTacVien;
            
            // Set DataContext to bind UI to the model
            this.DataContext = _congTacVien;

            LoadHopDongs();
        }

        private void LoadHopDongs()
        {
            if (_congTacVien.Id != 0)
            {
                using (var context = new Data.AppDbContext())
                {
                    var details = context.ChiTietHopDongs
                        .Include(d => d.CongTacVien)
                        .Where(d => d.CongTacVienId == _congTacVien.Id && !d.IsDelete && d.QuanLyHopDong != null && !d.QuanLyHopDong.IsDelete)
                        .ToList();
                    dgChiTietHopDong.ItemsSource = details;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
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
            MessageBox.Show("Đã lưu thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
