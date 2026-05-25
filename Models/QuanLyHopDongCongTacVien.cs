using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace QuanLyCongTacVien.Models
{
    [Table("QuanLyHopDongCongTacVien")]
    public class QuanLyHopDongCongTacVien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NienDoTaiChinh { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string NamHoc { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string TruongCongTy { get; set; } = string.Empty;

        public int STT{ get; set; } = 1;

        [Browsable(false)]
        public bool IsDelete { get; set; }

        // Navigation property
        public virtual ICollection<ChiTietHopDong> ChiTietHopDongs { get; set; } = new List<ChiTietHopDong>();
    }
}
