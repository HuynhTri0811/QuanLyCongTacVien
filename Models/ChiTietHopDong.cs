using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyCongTacVien.Models
{
    [Table("ChiTietHopDong")]
    public class ChiTietHopDong : INotifyPropertyChanged
    {
        private int _id;
        private int _quanLyHopDongCongTacVienId;
        private int? _congTacVienId;
        private string? _soHopDong;
        private DateTime? _ngayKy;
        private string? _boPhan;
        private string? _maNhanSu;
        private bool _hetHieuLuc;
        private string? _chucDanh;
        private bool _inLaiThoaThuan;
        private DateTime? _tuNgay;
        private DateTime? _denNgay;
        private bool _isDelete;
        private CongTacVien? _congTacVien;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        public int QuanLyHopDongCongTacVienId { get => _quanLyHopDongCongTacVienId; set { _quanLyHopDongCongTacVienId = value; OnPropertyChanged(); } }

        public int? CongTacVienId 
        { 
            get => _congTacVienId; 
            set 
            { 
                _congTacVienId = value; 
                OnPropertyChanged();
                // Khi Id thay đổi, ta cần reset navigation property để helper property NguoiLaoDong cập nhật
                // Tuy nhiên tốt nhất là để UI xử lý hoặc gán lại object
                OnPropertyChanged(nameof(NguoiLaoDong));
            } 
        }

        [MaxLength(100)]
        public string? SoHopDong { get => _soHopDong; set { _soHopDong = value; OnPropertyChanged(); } }

        public DateTime? NgayKy { get => _ngayKy; set { _ngayKy = value; OnPropertyChanged(); } }

        [MaxLength(200)]
        public string? BoPhan { get => _boPhan; set { _boPhan = value; OnPropertyChanged(); } }

        [MaxLength(50)]
        public string? MaNhanSu { get => _maNhanSu; set { _maNhanSu = value; OnPropertyChanged(); } }

        public bool HetHieuLuc { get => _hetHieuLuc; set { _hetHieuLuc = value; OnPropertyChanged(); } }

        [MaxLength(100)]
        public string? ChucDanh { get => _chucDanh; set { _chucDanh = value; OnPropertyChanged(); } }

        public bool InLaiThoaThuan { get => _inLaiThoaThuan; set { _inLaiThoaThuan = value; OnPropertyChanged(); } }

        public DateTime? TuNgay { get => _tuNgay; set { _tuNgay = value; OnPropertyChanged(); } }

        public DateTime? DenNgay { get => _denNgay; set { _denNgay = value; OnPropertyChanged(); } }

        public bool IsDelete { get => _isDelete; set { _isDelete = value; OnPropertyChanged(); } }

        // Navigation properties
        [ForeignKey("QuanLyHopDongCongTacVienId")]
        public virtual QuanLyHopDongCongTacVien? QuanLyHopDong { get; set; }

        [ForeignKey("CongTacVienId")]
        public virtual CongTacVien? CongTacVien 
        { 
            get => _congTacVien; 
            set 
            { 
                _congTacVien = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(NguoiLaoDong)); 
            } 
        }

        // Helper property for display
        [NotMapped]
        public string? NguoiLaoDong => CongTacVien?.HoVaTen;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
