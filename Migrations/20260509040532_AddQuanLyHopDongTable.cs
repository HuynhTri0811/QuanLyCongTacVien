using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTacVien.Migrations
{
    /// <inheritdoc />
    public partial class AddQuanLyHopDongTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuanLyHopDongCongTacVien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NienDoTaiChinh = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NamHoc = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TruongCongTy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLyHopDongCongTacVien", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuanLyHopDongCongTacVien");
        }
    }
}
