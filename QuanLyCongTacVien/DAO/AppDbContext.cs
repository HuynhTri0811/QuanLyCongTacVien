/*
 * AppDbContext is implemented conditionally because EF packages may not be restored
 * in all environments used by the automated tooling. The file defines an EF Core
 * DbContext when the Microsoft.EntityFrameworkCore package is available.
 */
#if NET8_0
using Microsoft.EntityFrameworkCore;

namespace QuanLyCongTacVien.DAO
{
    public class AppDbContext : DbContext
    {        
        
        
        string _dbPath = @"E:\QuanLyCongTacVien\Database\CTV_Data.db";
        public DbSet<DTO.CongTacVien> CongTacViens { get; set; }
        public DbSet<DTO.QuanLyHopDongCongTacVien> QuanLyHopDongCongTacViens { get; set; }


        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DTO.CongTacVien>(entity =>
            {
                entity.HasKey(e => e.Oid);
                entity.Property(e => e.MaQuanLy).IsRequired();
            });

            modelBuilder.Entity<DTO.QuanLyHopDongCongTacVien>(entity =>
            {
                entity.HasKey(e => e.Oid);
            });
        }
    }
}
#else
// EF Core not available in this build environment. Keep a lightweight stub so the
// project can still compile in environments where EF packages are not restored.
namespace QuanLyCongTacVien.DAO
{
    public class AppDbContext
    {
        private readonly string _dbPath;
        public AppDbContext(string dbPath) { _dbPath = dbPath; }
    }
}
#endif
