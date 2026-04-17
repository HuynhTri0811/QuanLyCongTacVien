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
            Oid = (row.Table.Columns.Contains("Oid") && row["Oid"] != DBNull.Value)
                ? Convert.ToInt32(row["Oid"]) : -1;

            MaQuanLy = row.Table.Columns.Contains("MaQuanLy") ? row["MaQuanLy"]?.ToString() : null;
            HoTen = row.Table.Columns.Contains("HoTen") ? row["HoTen"]?.ToString() : null;

            // Convert NgaySinh to nullable DateTime in a single expression
            NgaySinh = (row.Table.Columns.Contains("NgaySinh") && row["NgaySinh"] != DBNull.Value)
                ? (DateTime?)Convert.ToDateTime(row["NgaySinh"]) : null;

            QuocTich = row.Table.Columns.Contains("QuocTich") ? row["QuocTich"]?.ToString() : null;
            CCCD = row.Table.Columns.Contains("CCCD") ? row["CCCD"]?.ToString() : null;
            DiaChi = row.Table.Columns.Contains("DiaChi") ? row["DiaChi"]?.ToString() : null;
            SoDienThoai = row.Table.Columns.Contains("SoDienThoai") ? row["SoDienThoai"]?.ToString() : null;

            IsActive = (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value)
                ? Convert.ToBoolean(row["IsActive"]) : true;
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
