using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTacVien.Migrations
{
    /// <inheritdoc />
    public partial class LinkChiTietHopDongToCongTacVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiLaoDong",
                table: "ChiTietHopDong");

            migrationBuilder.AddColumn<int>(
                name: "CongTacVienId",
                table: "ChiTietHopDong",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHopDong_CongTacVienId",
                table: "ChiTietHopDong",
                column: "CongTacVienId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHopDong_CongTacVien_CongTacVienId",
                table: "ChiTietHopDong",
                column: "CongTacVienId",
                principalTable: "CongTacVien",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHopDong_CongTacVien_CongTacVienId",
                table: "ChiTietHopDong");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHopDong_CongTacVienId",
                table: "ChiTietHopDong");

            migrationBuilder.DropColumn(
                name: "CongTacVienId",
                table: "ChiTietHopDong");

            migrationBuilder.AddColumn<string>(
                name: "NguoiLaoDong",
                table: "ChiTietHopDong",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }
    }
}
