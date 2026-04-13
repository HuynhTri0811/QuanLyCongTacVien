using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCongTacVien
{
    public static class Database
    {
        static string dbPath = @"E:\QuanLyCongTacVien\Database\CTV_Data.db";

        public static DataTable GetAllDanhSachCongTacVien()
        {
            DataTable dt = new DataTable();
            string connectionString = $"Data Source={dbPath}";
            using (var connection = new SqliteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var selectSql = "SELECT Oid, MaQuanLy, HoTen, SoDienThoai, NgayVaoLam, IsActive FROM CongTacVien";
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

        public static void SaveDanhSachCongTacVien(IEnumerable<CongTacVien> list)
        {
            if (list == null) return;

            string connectionString = $"Data Source={dbPath}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Use UPSERT on the unique MaQuanLy
                    var sql = @"INSERT INTO CongTacVien (MaQuanLy, HoTen, SoDienThoai, IsActive)
                                VALUES (@MaQuanLy, @HoTen, @SoDienThoai, @IsActive)
                                ON CONFLICT(MaQuanLy) DO UPDATE SET
                                    HoTen=excluded.HoTen,
                                    SoDienThoai=excluded.SoDienThoai,
                                    IsActive=excluded.IsActive;";

                    using (var command = new SqliteCommand(sql, connection, transaction))
                    {
                        var pMa = command.CreateParameter();
                        pMa.ParameterName = "@MaQuanLy";
                        command.Parameters.Add(pMa);

                        var pHo = command.CreateParameter();
                        pHo.ParameterName = "@HoTen";
                        command.Parameters.Add(pHo);

                        var pSo = command.CreateParameter();
                        pSo.ParameterName = "@SoDienThoai";
                        command.Parameters.Add(pSo);

                        var pNgay = command.CreateParameter();
                        pNgay.ParameterName = "@NgayVaoLam";
                        command.Parameters.Add(pNgay);

                        var pActive = command.CreateParameter();
                        pActive.ParameterName = "@IsActive";
                        command.Parameters.Add(pActive);

                        foreach (var c in list)
                        {
                            pMa.Value = c.MaQuanLy ?? string.Empty;
                            pHo.Value = c.HoTen ?? string.Empty;
                            pSo.Value = c.SoDienThoai ?? string.Empty;
                            pActive.Value = c.IsActive ? 1 : 0;

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public static void KhoiTaoSQL()
        {
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
        }
    }
}
