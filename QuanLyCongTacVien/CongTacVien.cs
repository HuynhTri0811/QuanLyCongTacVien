using System;
using System.Data;

namespace QuanLyCongTacVien
{
    public class CongTacVien
    {

        private int Oid { get; set; }
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

        public CongTacVien(DataRow row)
        {
            Oid = row["Oid"] != DBNull.Value ? Convert.ToInt32(row["Oid"]) : -1;
            MaQuanLy = row["MaQuanLy"]?.ToString();
            HoTen = row["HoTen"]?.ToString();
            NgaySinh = Common.ConvertStringToDateTime(row["NgaySinh"].ToString());
            QuocTich = row["QuocTich"]?.ToString();
            CCCD = row["CCCD"]?.ToString();
            DiaChi = row["DiaChi"]?.ToString();
            SoDienThoai = row["SoDienThoai"]?.ToString();
            IsActive = (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value) ? Convert.ToBoolean(row["IsActive"]) : true;
        }
        public override string ToString()
        {
            return $"{HoTen}";
        }

        public int GetOid()
        {
            return Oid;
        }   

    }
}
