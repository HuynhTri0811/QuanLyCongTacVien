using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTacVien.Migrations
{
    /// <inheritdoc />
    public partial class AddChiTietHopDongTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietHopDong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuanLyHopDongCongTacVienId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoHopDong = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    NgayKy = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BoPhan = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    NguoiLaoDong = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    MaNhanSu = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HetHieuLuc = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChucDanh = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    InLaiThoaThuan = table.Column<bool>(type: "INTEGER", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHopDong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHopDong_QuanLyHopDongCongTacVien_QuanLyHopDongCongTacVienId",
                        column: x => x.QuanLyHopDongCongTacVienId,
                        principalTable: "QuanLyHopDongCongTacVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHopDong_QuanLyHopDongCongTacVienId",
                table: "ChiTietHopDong",
                column: "QuanLyHopDongCongTacVienId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHopDong");
        }
    }
}
