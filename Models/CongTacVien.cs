using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace QuanLyCongTacVien.Models
{
    [Table("CongTacVien")]
    public class CongTacVien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false)]
        public int Id { get; set; }

        // --- Sơ yếu lý lịch ---
        [MaxLength(50)]
        [DisplayName("Mã nhân sự")]
        public string? MaNhanSu { get; set; }

        [MaxLength(50)]
        [DisplayName("Mã nhân viên")]
        public string? MaNhanVien { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Họ")]
        public string Ho { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [DisplayName("Tên")]
        public string Ten { get; set; } = string.Empty;

        [MaxLength(100)]
        [DisplayName("Họ và tên")]
        public string? HoVaTen { get; set; }

        [MaxLength(100)]
        [DisplayName("Tên gọi khác")]
        public string? TenGoiKhac { get; set; }

        [DisplayName("Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        [MaxLength(100)]
        [DisplayName("Nơi sinh")]
        public string? NoiSinh { get; set; }

        [MaxLength(10)]
        [DisplayName("Giới tính")]
        public string? GioiTinh { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Số CMND")]
        public string SoCMND { get; set; } = string.Empty;

        [Required]
        [DisplayName("Ngày cấp")]
        public DateTime NgayCap { get; set; }

        [DisplayName("Ngày hết hạn")]
        public DateTime? NgayHetHan { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Nơi cấp")]
        public string NoiCap { get; set; } = string.Empty;

        [MaxLength(200)]
        [DisplayName("Quê quán")]
        public string? QueQuan { get; set; }

        [MaxLength(200)]
        [DisplayName("Địa chỉ thường trú")]
        public string? DiaChiThuongTru { get; set; }

        [MaxLength(200)]
        [DisplayName("Nơi ở hiện nay")]
        public string? NoiOHienNay { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Quốc tịch")]
        public string QuocTich { get; set; } = string.Empty;

        [MaxLength(50)]
        [DisplayName("Dân tộc")]
        public string? DanToc { get; set; }

        [MaxLength(100)]    
        [DisplayName("Email cá nhân")]
        public string? EmailCaNhan { get; set; }

        [MaxLength(50)]
        [DisplayName("Tôn giáo")]
        public string? TonGiao { get; set; }

        [MaxLength(20)]
        [DisplayName("Điện thoại di động")]
        public string? DienThoaiDiDong { get; set; }

        [MaxLength(20)]
        [DisplayName("Điện thoại nhà riêng")]
        public string? DienThoaiNhaRieng { get; set; }

        [Required]
        [DisplayName("Ngày vào trường")]
        public DateTime NgayVaoTruong { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Đơn vị")]
        public string DonVi { get; set; } = string.Empty;

        [MaxLength(100)]
        [DisplayName("Khoa bộ môn")]
        public string? TaiKhoaBoMon { get; set; }

        [MaxLength(100)]
        [DisplayName("Đơn vị công tác")]
        public string? DonViCongTac { get; set; }

        [MaxLength(200)]
        [DisplayName("Danh sách lưu trữ giấy tờ")]
        public string? DanhSachLuuTruGiayTo { get; set; }

        [MaxLength(100)]
        [DisplayName("Loại hợp đồng nhóm dịch vụ")]
        public string? LoaiHopDongNhomDichVu { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Tình trạng")]
        public string TinhTrang { get; set; } = string.Empty;

        [DisplayName("Ghi chú")]
        public string? GhiChu { get; set; }

        // --- Trình độ chuyên môn ---
        [MaxLength(100)]
        [DisplayName("Học hàm")]
        public string? HocHam { get; set; }

        [MaxLength(100)]
        [DisplayName("Học vị")]
        public string? HocVi { get; set; }

        [MaxLength(100)]
        [DisplayName("Chuyên ngành đào tạo")]
        public string? ChuyenNganhDaoTao { get; set; }

        [MaxLength(100)]
        [DisplayName("Trường đào tạo")]
        public string? TruongDaoTao { get; set; }

        [DisplayName("Ngày cấp bằng")]
        public DateTime? NgayCapBang { get; set; }

        [MaxLength(100)]
        [DisplayName("Hình thức đào tạo")]
        public string? HinhThucDaoTao { get; set; }

        [DisplayName("Năm tốt nghiệp")]
        public int? NamTotNghiep { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Trình độ văn hóa")]
        public string TrinhDoVanHoa { get; set; } = string.Empty;

        [MaxLength(100)]
        [DisplayName("Trình độ tin học")]
        public string? TrinhDoTinHoc { get; set; }

        [MaxLength(100)]
        [DisplayName("Ngoại ngữ chính")]
        public string? NgoaiNguChinh { get; set; }

        [MaxLength(100)]
        [DisplayName("Trình độ ngoại ngữ chính")]
        public string? TrinhDoNgoaiNguChinh { get; set; }

        // --- Thông tin lương ---
        [MaxLength(50)]
        [DisplayName("Mã số thuế")]
        public string? MaSoThue { get; set; }

        [DisplayName("Phần trăm tính thuế")]
        public double? PhanTramTinhThue { get; set; }

        [MaxLength(50)]
        [DisplayName("Cơ quan thuế")]
        public string? CoQuanThue { get; set; }

        [MaxLength(50)]
        [DisplayName("Tên ngân hàng")]
        public string? TenNganHang { get; set; }

        [MaxLength(50)]
        [DisplayName("Số tài khoản")]
        public string? SoTaiKhoan { get; set; }


        [Browsable(false)]
        public bool IsDelete { get; set; } = false;
    }
}
