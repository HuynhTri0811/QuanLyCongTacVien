using Microsoft.Data.Sqlite;
using QuanLyCongTacVien.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCongTacVien.DAO
{
    public static class Database
    {
        static string dbPath = @"E:\QuanLyCongTacVien\Database\CTV_Data.db";
        public static string DbPath => dbPath;

        public static DataTable GetAllDanhSachCongTacVien()
        {
            DataTable dt = new DataTable();
            string connectionString = $"Data Source={dbPath}";
            using (var connection = new SqliteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var selectSql = "SELECT Oid, MaQuanLy, HoTen, NgaySinh, QuocTich, CCCD, DiaChi, SoDienThoai, IsActive FROM CongTacVien";
                    using (var command = new SqliteCommand(selectSql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

        
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi truy vấn: " + ex.Message);
                }
            }

            return dt;
        }


        public static void XoaCongTacVienByOid(int oid)
        {
            if (oid <= 0) return;
            string connectionString = $"Data Source={dbPath}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("DELETE FROM CongTacVien WHERE Oid = @Oid", connection))
                {
                    var p = command.CreateParameter();
                    p.ParameterName = "@Oid";
                    p.Value = oid;
                    command.Parameters.Add(p);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void ThemMoiDanhSachCongTacVien(IEnumerable<CongTacVien> list)
        {
            if (list == null) return;

            string connectionString = $"Data Source={dbPath}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Use UPSERT on the unique MaQuanLy
                    var sql = @"INSERT INTO CongTacVien (MaQuanLy, HoTen,NgaySinh, QuocTich,CCCD,DiaChi,SoDienThoai, IsActive)
                                VALUES (@MaQuanLy, @HoTen, @NgaySinh, @QuocTich, @CCCD, @DiaChi, @SoDienThoai, @IsActive)
                                ON CONFLICT(Oid) DO UPDATE SET
                                    MaQuanLy=excluded.MaQuanLy,
                                    HoTen=excluded.HoTen,
                                    NgaySinh=excluded.NgaySinh,
                                    QuocTich=excluded.QuocTich,
                                    CCCD=excluded.CCCD,
                                    DiaChi=excluded.DiaChi,
                                    SoDienThoai=excluded.SoDienThoai,
                                    IsActive=excluded.IsActive;";

                    using (var command = new SqliteCommand(sql, connection, transaction))
                    {
                        var pMaQuanLy = command.CreateParameter();
                        pMaQuanLy.ParameterName = "@MaQuanLy";
                        command.Parameters.Add(pMaQuanLy);

                        var pHoTen = command.CreateParameter();
                        pHoTen.ParameterName = "@HoTen";
                        command.Parameters.Add(pHoTen);

                        var pNgaySinh = command.CreateParameter();
                        pNgaySinh.ParameterName = "@NgaySinh";
                        command.Parameters.Add(pNgaySinh);
                        
                        var pQuocTich = command.CreateParameter();
                        pQuocTich.ParameterName = "@QuocTich";
                        command.Parameters.Add(pQuocTich);

                        var pCCCD = command.CreateParameter();
                        pCCCD.ParameterName = "@CCCD";
                        command.Parameters.Add(pCCCD);

                        var pDiaChi = command.CreateParameter();
                        pDiaChi.ParameterName = "@DiaChi";
                        command.Parameters.Add(pDiaChi);

                        var pSoDienThoai = command.CreateParameter();
                        pSoDienThoai.ParameterName = "@SoDienThoai";
                        command.Parameters.Add(pSoDienThoai);

                        var pNgayVaoLam = command.CreateParameter();
                        pNgayVaoLam.ParameterName = "@NgayVaoLam";
                        command.Parameters.Add(pNgayVaoLam);

                        var pActive = command.CreateParameter();
                        pActive.ParameterName = "@IsActive";
                        command.Parameters.Add(pActive);

                        foreach (var item in list)
                        {
                            pMaQuanLy.Value = item.MaQuanLy ?? string.Empty;
                            pHoTen.Value = item.HoTen ?? string.Empty;
                            pNgaySinh.Value = item.NgaySinh.HasValue ? (object)item.NgaySinh.Value : DBNull.Value;
                            pQuocTich.Value = item.QuocTich ?? string.Empty;
                            pCCCD.Value = item.CCCD ?? string.Empty;
                            pDiaChi.Value = item.DiaChi ?? string.Empty;
                            pSoDienThoai.Value = item.SoDienThoai ?? string.Empty;
                            pActive.Value = item.IsActive ? 1 : 0;

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }

            MessageBox.Show("Lưu danh sách cộng tác viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); 
        }

        public static void KhoiTaoSQL()
        {
            // Ensure directory exists for DB file
            try
            {
                var dir = System.IO.Path.GetDirectoryName(dbPath);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
            }
            catch { /* ignore directory creation errors here */ }

            string connectionString = $"Data Source={dbPath}";

            using (var connection = new SqliteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Tạo bảng nếu chưa tồn tại (gồm các cột cần thiết)
                    var createTableSql = @"CREATE TABLE IF NOT EXISTS CongTacVien (
                        Oid INTEGER PRIMARY KEY AUTOINCREMENT,
                        MaQuanLy TEXT UNIQUE,
                        HoTen TEXT,
                        NgaySinh DATETIME,
                        QuocTich TEXT,
                        CCCD TEXT,
                        DiaChi TEXT,
                        SoDienThoai TEXT,
                        NgayVaoLam DATETIME,
                        IsActive INTEGER DEFAULT 1
                    );";
                    using (var command = new SqliteCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Kết nối SQLite thành công!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi kết nối: " + ex.Message);
                }
            }

#if NET8_0
            // If EF Core is available, ensure EF migrations / model are applied as well
            try
            {
                using (var ctx = new AppDbContext())
                {
                    ctx.Database.EnsureCreated();
                }
            }
            catch
            {
                // ignore if EF isn't available at runtime
            }
#endif
        }

        internal static void CapNhatDanhSachCongTacVien(List<CongTacVien> listCapNhat)
        {
            if (listCapNhat == null) return;

            string connectionString = $"Data Source={dbPath}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Update existing rows by Oid only
                    var sql = @"UPDATE CongTacVien SET
                                    MaQuanLy = @MaQuanLy,
                                    HoTen = @HoTen,
                                    NgaySinh = @NgaySinh,
                                    QuocTich = @QuocTich,
                                    CCCD = @CCCD,
                                    DiaChi = @DiaChi,
                                    SoDienThoai = @SoDienThoai,
                                    IsActive = @IsActive
                                WHERE Oid = @Oid;";

                    using (var command = new SqliteCommand(sql, connection, transaction))
                    {
                        var pMaQuanLy = command.CreateParameter();
                        pMaQuanLy.ParameterName = "@MaQuanLy";
                        command.Parameters.Add(pMaQuanLy);

                        var pHoTen = command.CreateParameter();
                        pHoTen.ParameterName = "@HoTen";
                        command.Parameters.Add(pHoTen);

                        var pNgaySinh = command.CreateParameter();
                        pNgaySinh.ParameterName = "@NgaySinh";
                        command.Parameters.Add(pNgaySinh);

                        var pQuocTich = command.CreateParameter();
                        pQuocTich.ParameterName = "@QuocTich";
                        command.Parameters.Add(pQuocTich);

                        var pCCCD = command.CreateParameter();
                        pCCCD.ParameterName = "@CCCD";
                        command.Parameters.Add(pCCCD);

                        var pDiaChi = command.CreateParameter();
                        pDiaChi.ParameterName = "@DiaChi";
                        command.Parameters.Add(pDiaChi);

                        var pSoDienThoai = command.CreateParameter();
                        pSoDienThoai.ParameterName = "@SoDienThoai";
                        command.Parameters.Add(pSoDienThoai);

                        var pNgayVaoLam = command.CreateParameter();
                        pNgayVaoLam.ParameterName = "@NgayVaoLam";
                        command.Parameters.Add(pNgayVaoLam);

                        var pActive = command.CreateParameter();
                        pActive.ParameterName = "@IsActive";
                        command.Parameters.Add(pActive);

                        var pOid = command.CreateParameter();
                        pOid.ParameterName = "@Oid";
                        command.Parameters.Add(pOid);

                        foreach (var item in listCapNhat)
                        {
                            // Only update if Oid is present (> 0)
                            if (item.GetOid() <= 0) continue;

                            pMaQuanLy.Value = item.MaQuanLy ?? string.Empty;
                            pHoTen.Value = item.HoTen ?? string.Empty;
                            pNgaySinh.Value = item.NgaySinh.HasValue ? (object)item.NgaySinh.Value : DBNull.Value;
                            pQuocTich.Value = item.QuocTich ?? string.Empty;
                            pCCCD.Value = item.CCCD ?? string.Empty;
                            pDiaChi.Value = item.DiaChi ?? string.Empty;
                            pSoDienThoai.Value = item.SoDienThoai ?? string.Empty;
                            pActive.Value = item.IsActive ? 1 : 0;
                            pOid.Value = item.GetOid();

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
