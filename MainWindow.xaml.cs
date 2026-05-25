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
using Microsoft.EntityFrameworkCore;

namespace QuanLyCongTacVien;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
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
            dgCongTacVien.ItemsSource = context.CongTacViens
                .Where(c => !c.IsDelete)
                .ToList();
        }
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
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa '{selected.HoVaTen}' không?", 
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
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
            MessageBox.Show("Vui lòng chọn một người để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is TreeViewItem item)
        {
            string header = item.Header.ToString();
            
            // Tránh lỗi NullReferenceException khi UI đang khởi tạo
            if (gridCongTacVien == null || gridHopDong == null) return;

            if (header == "Cộng tác viên(New)")
            {
                gridCongTacVien.Visibility = Visibility.Visible;
                gridHopDong.Visibility = Visibility.Collapsed;
                LoadDuLieu();
            }
            else if (header == "Hợp đồng cộng tác viên")
            {
                gridCongTacVien.Visibility = Visibility.Collapsed;
                gridHopDong.Visibility = Visibility.Visible;
                LoadHopDong();
            }
            else if (header != "Quản lý cộng tác viên")
            {
                MessageBox.Show($"Chức năng '{header}' đang được cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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

                // Thêm dữ liệu chi tiết mẫu cho năm 2022 (như trong ảnh)
                var targetHopDong = hopDongs.FirstOrDefault(h => h.NienDoTaiChinh == "2022");
                if (targetHopDong != null)
                {
                    context.ChiTietHopDongs.AddRange(new List<Models.ChiTietHopDong>
                    {
                        new Models.ChiTietHopDong { QuanLyHopDongCongTacVienId = targetHopDong.Id, SoHopDong = "21102510/DHTL-...", NgayKy = new DateTime(2021, 10, 25), BoPhan = "Bộ môn Ngôn n...", MaNhanSu = "00664", ChucDanh = "Giảng viên", TuNgay = new DateTime(2021, 11, 1), DenNgay = new DateTime(2023, 10, 31) },
                        
                    });
                    context.SaveChanges();
                }
            }

            var allHopDongs = context.QuanLyHopDongCongTacViens
                .Where(h => !h.IsDelete)
                .ToList();
            
            // Cài đặt Grouping cho DataGrid
            ICollectionView view = CollectionViewSource.GetDefaultView(allHopDongs);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("TruongCongTy"));
            
            dgHopDong.ItemsSource = view;
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
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hợp đồng niên độ '{selected.NienDoTaiChinh}' không?", 
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
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
            MessageBox.Show("Vui lòng chọn một hợp đồng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
}