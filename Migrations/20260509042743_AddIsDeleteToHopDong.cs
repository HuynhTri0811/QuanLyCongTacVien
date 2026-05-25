using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTacVien.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeleteToHopDong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "QuanLyHopDongCongTacVien",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "QuanLyHopDongCongTacVien");
        }
    }
}
