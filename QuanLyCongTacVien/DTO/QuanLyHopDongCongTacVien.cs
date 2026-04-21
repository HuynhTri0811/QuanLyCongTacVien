using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCongTacVien.DTO
{
    public class QuanLyHopDongCongTacVien
    {
        public int Oid { get; set; }
        public string MaQuanLy { get; set; }
        public string SoHopDong { get; set; }
        public DateTime? NgayKy { get; set; }

        public int GetOid() { return Oid; }
    }
}
