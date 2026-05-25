using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyCongTacVien.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCongTacVienSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CongTacVien");

            migrationBuilder.RenameColumn(
                name: "SoDienThoai",
                table: "CongTacVien",
                newName: "SoCMND");

            migrationBuilder.RenameColumn(
                name: "HoTen",
                table: "CongTacVien",
                newName: "TrinhDoVanHoa");

            migrationBuilder.AddColumn<string>(
                name: "ChuyenNganhDaoTao",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DanToc",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DanhSachLuuTruGiayTo",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiThuongTru",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DienThoaiDiDong",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DienThoaiNhaRieng",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonVi",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DonViCongTac",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailCaNhan",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "CongTacVien",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhThucDaoTao",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ho",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HoVaTen",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HocHam",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HocVi",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoaiHopDongNhomDichVu",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaNhanSu",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaNhanVien",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NamTotNghiep",
                table: "CongTacVien",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayCap",
                table: "CongTacVien",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayCapBang",
                table: "CongTacVien",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayHetHan",
                table: "CongTacVien",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "CongTacVien",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayVaoTruong",
                table: "CongTacVien",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NgoaiNguChinh",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiCap",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NoiOHienNay",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiSinh",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PhanTramTinhThue",
                table: "CongTacVien",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueQuan",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuocTich",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaiKhoaBoMon",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenGoiKhac",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TinhTrang",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TonGiao",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoNgoaiNguChinh",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrinhDoTinHoc",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongDaoTao",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChuyenNganhDaoTao",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DanToc",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DanhSachLuuTruGiayTo",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DiaChiThuongTru",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DienThoaiDiDong",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DienThoaiNhaRieng",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DonVi",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "DonViCongTac",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "EmailCaNhan",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "HinhThucDaoTao",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "Ho",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "HoVaTen",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "HocHam",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "HocVi",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "LoaiHopDongNhomDichVu",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "MaNhanSu",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NamTotNghiep",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NgayCap",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NgayCapBang",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NgayHetHan",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NgayVaoTruong",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NgoaiNguChinh",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NoiCap",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NoiOHienNay",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "NoiSinh",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "PhanTramTinhThue",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "QueQuan",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "QuocTich",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TaiKhoaBoMon",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TenGoiKhac",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TinhTrang",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TonGiao",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TrinhDoNgoaiNguChinh",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TrinhDoTinHoc",
                table: "CongTacVien");

            migrationBuilder.DropColumn(
                name: "TruongDaoTao",
                table: "CongTacVien");

            migrationBuilder.RenameColumn(
                name: "TrinhDoVanHoa",
                table: "CongTacVien",
                newName: "HoTen");

            migrationBuilder.RenameColumn(
                name: "SoCMND",
                table: "CongTacVien",
                newName: "SoDienThoai");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CongTacVien",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
