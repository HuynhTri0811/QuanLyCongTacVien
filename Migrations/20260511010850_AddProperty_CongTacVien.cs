using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTacVien.Migrations
{
    /// <inheritdoc />
    public partial class AddProperty_CongTacVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "STT",
                table: "QuanLyHopDongCongTacVien",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CoQuanThue",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoTaiKhoan",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNganHang",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STT",
                table: "QuanLyHopDongCongTacVien");

            migrationBuilder.DropColumn(
                name: "CoQuanThue",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "SoTaiKhoan",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TenNganHang",
                table: "CongTacVien");
        }
    }
}
