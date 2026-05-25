using Microsoft.EntityFrameworkCore;
using QuanLyCongTacVien.Models;
using System;
using System.IO;

namespace QuanLyCongTacVien.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CongTacVien> CongTacViens { get; set; }
        public DbSet<QuanLyHopDongCongTacVien> QuanLyHopDongCongTacViens { get; set; }
        public DbSet<ChiTietHopDong> ChiTietHopDongs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Tên file db
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quanly_congtacvien.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
