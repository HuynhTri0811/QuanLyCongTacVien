using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCongTacVien
{
    internal class CongTacVien_DetailView : Form
    {
        private Label lblMa;
        private Label lblHoTen;

        public CongTacVien_DetailView()
        {
            InitializeComponent();
        }

        public CongTacVien_DetailView(CongTacVien ctv) : this()
        {
            if (ctv == null) return;
            lblMa.Text = "Mã: " + (ctv.MaQuanLy ?? string.Empty);
            lblHoTen.Text = "Họ tên: " + (ctv.HoTen ?? string.Empty);
        }

        private void InitializeComponent()
        {
            this.lblMa = new Label();
            this.lblHoTen = new Label();
            this.SuspendLayout();
            // 
            // lblMa
            // 
            this.lblMa.AutoSize = true;
            this.lblMa.Location = new Point(12, 12);
            this.lblMa.Name = "lblMa";
            this.lblMa.Size = new Size(35, 13);
            this.lblMa.TabIndex = 0;
            this.lblMa.Text = "Mã: ";
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Location = new Point(12, 36);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new Size(50, 13);
            this.lblHoTen.TabIndex = 1;
            this.lblHoTen.Text = "Họ tên: ";
            // 
            // CongTacVien_DetailView
            // 
            this.ClientSize = new Size(400, 100);
            this.Controls.Add(this.lblMa);
            this.Controls.Add(this.lblHoTen);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết CTV";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
