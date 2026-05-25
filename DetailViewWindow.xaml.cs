using System.Windows;
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
