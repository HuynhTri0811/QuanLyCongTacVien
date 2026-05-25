using System;
using System.Windows.Forms;
using QuanLyCongTacVien.DTO;

namespace QuanLyCongTacVien.Views
{
    public partial class QuanLyHopDong_DetailView : Form
    {
        private QuanLyHopDongCongTacVien _hopDong;

        public QuanLyHopDong_DetailView(QuanLyHopDongCongTacVien hopDong = null)
        {
            InitializeComponent();
            _hopDong = hopDong ?? new QuanLyHopDongCongTacVien();
            LoadData();
        }

        private void LoadData()
        {
            // TODO: Bind _hopDong properties to controls
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // TODO: Save logic for _hopDong
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
