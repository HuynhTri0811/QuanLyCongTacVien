namespace QuanLyCongTacVien
{
    partial class CongTacVien_ListView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaQuanLy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgaySinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDienThoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCCCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuocTich;
        private System.Windows.Forms.DataGridViewButtonColumn colAction;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsActive;
        private System.Windows.Forms.Label lblCaption;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            colMaQuanLy = new DataGridViewTextBoxColumn();
            colHoTen = new DataGridViewTextBoxColumn();
            colNgaySinh = new DataGridViewTextBoxColumn();
            colSoDienThoai = new DataGridViewTextBoxColumn();
            colDiaChi = new DataGridViewTextBoxColumn();
            colCCCD = new DataGridViewTextBoxColumn();
            colQuocTich = new DataGridViewTextBoxColumn();
            colIsActive = new DataGridViewCheckBoxColumn();
            btnSaveList = new Button();
            colOid = new DataGridViewTextBoxColumn();
            colAction = new DataGridViewButtonColumn();
            lblCaption = new Label();
            btnReload = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colMaQuanLy, colHoTen, colNgaySinh, colSoDienThoai, colDiaChi, colCCCD, colQuocTich, colIsActive , colAction });
            dataGridView1.Location = new Point(12, 40);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1274, 572);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // colMaQuanLy
            // 
            colMaQuanLy.DataPropertyName = "MaQuanLy";
            colMaQuanLy.HeaderText = "Mã quản lý";
            colMaQuanLy.Name = "colMaQuanLy";
            // 
            // colHoTen
            // 
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.HeaderText = "Họ và tên";
            colHoTen.Name = "colHoTen";
            // 
            // colNgaySinh
            // 
            colNgaySinh.DataPropertyName = "NgaySinh";
            colNgaySinh.HeaderText = "Ngày sinh";
            colNgaySinh.Name = "colNgaySinh";
            // 
            // colSoDienThoai
            // 
            colSoDienThoai.DataPropertyName = "SoDienThoai";
            colSoDienThoai.HeaderText = "Số điện thoại";
            colSoDienThoai.Name = "colSoDienThoai";
            // 
            // colDiaChi
            // 
            colDiaChi.DataPropertyName = "DiaChi";
            colDiaChi.HeaderText = "Địa chỉ";
            colDiaChi.Name = "colDiaChi";
            // 
            // colCCCD
            // 
            colCCCD.DataPropertyName = "CCCD";
            colCCCD.HeaderText = "CCCD";
            colCCCD.Name = "colCCCD";
            // 
            // colQuocTich
            // 
            colQuocTich.DataPropertyName = "QuocTich";
            colQuocTich.HeaderText = "Quốc tịch";
            colQuocTich.Name = "colQuocTich";
            // 
            // colIsActive
            // 
            colIsActive.DataPropertyName = "IsActive";
            colIsActive.HeaderText = "Kích hoạt";
            colIsActive.Name = "colIsActive";

            // button column
            colAction.HeaderText = "Hành động";
            colAction.Name = "colAction";
            colAction.Text = "Chi tiết";
            //colAction.UseColumnTextForButtonValue = true;
            colAction.ToolTipText = "Xem chi tiết";
            colAction.Width = 100;

            // 
            // btnSaveList
            // 
            btnSaveList.Location = new Point(1147, 618);
            btnSaveList.Name = "btnSaveList";
            btnSaveList.Size = new Size(140, 31);
            btnSaveList.TabIndex = 1;
            btnSaveList.Text = "Lưu dữ liệu";
            btnSaveList.UseVisualStyleBackColor = true;
            btnSaveList.Click += button1_Click;
            // 
            // colOid
            // 
            //colOid.DataPropertyName = "Oid";
            //colOid.HeaderText = "Oid";
            //colOid.Name = "colOid";
            //colOid.ReadOnly = true;
            //colOid.Visible = false;
            //colOid.Width = 60;
            // 
            // lblCaption
            // 
            lblCaption.AutoSize = true;
            lblCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCaption.Location = new Point(12, 12);
            lblCaption.Name = "lblCaption";
            lblCaption.Size = new Size(140, 15);
            lblCaption.TabIndex = 2;
            lblCaption.Text = "Danh sách cộng tác viên";
            // 
            // btnReload
            // 
            btnReload.Location = new Point(1008, 618);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(133, 31);
            btnReload.TabIndex = 3;
            btnReload.Text = "Làm mới dữ liệu";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // CongTacVien_ListView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1298, 661);
            Controls.Add(btnReload);
            Controls.Add(btnSaveList);
            Controls.Add(dataGridView1);
            Controls.Add(lblCaption);
            Name = "CongTacVien_ListView";
            Text = "CongTacVien_ListView";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSaveList;
        private Button btnReload;
    }
}