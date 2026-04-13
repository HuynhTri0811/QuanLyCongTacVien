using System;

namespace QuanLyCongTacVien
{
    public class CongTacVien
    {
        public string MaQuanLy { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? QuocTich { get; set; }
        public string? CCCD { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public bool IsActive { get; set; }

        public CongTacVien()
        {
            IsActive = true;
        }

        public CongTacVien(string maQuanLy, string hoTen, DateTime? ngaySinh = null, string? quocTich = null, string? cccd = null, string? diaChi = null, string? soDienThoai = null)
        {
            MaQuanLy = maQuanLy;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            QuocTich = quocTich;
            CCCD = cccd;
            DiaChi = diaChi;
            SoDienThoai = soDienThoai;
            IsActive = true;
        }

        public override string ToString()
        {
            return $"{HoTen}";
        }
    }
}
