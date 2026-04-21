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
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
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
            colAction = new DataGridViewButtonColumn();
            colDelete = new DataGridViewButtonColumn();
            btnSaveList = new Button();
            colOid = new DataGridViewTextBoxColumn();
            lblCaption = new Label();
            btnReload = new Button();
            tabControlMain = new TabControl();
            tabDanhSach = new TabPage();
            cbFilterBy = new ComboBox();
            txtFilter = new TextBox();
            btnFilter = new Button();
            btnClearFilter = new Button();
            tabQuanLyHopDong = new TabPage();
            dataGridHopDong = new DataGridView();
            colHopDongSo = new DataGridViewTextBoxColumn();
            btnAddHopDong = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabControlMain.SuspendLayout();
            tabDanhSach.SuspendLayout();
            tabQuanLyHopDong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridHopDong).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.DataError += dataGridView1_DataError; 
            // initialize columns
            colMaQuanLy = new DataGridViewTextBoxColumn();
            colHoTen = new DataGridViewTextBoxColumn();
            colNgaySinh = new DataGridViewTextBoxColumn();
            colSoDienThoai = new DataGridViewTextBoxColumn();
            colDiaChi = new DataGridViewTextBoxColumn();
            colCCCD = new DataGridViewTextBoxColumn();
            colQuocTich = new DataGridViewTextBoxColumn();
            colIsActive = new DataGridViewCheckBoxColumn();

            colMaQuanLy.DataPropertyName = "MaQuanLy";
            colMaQuanLy.HeaderText = "Mã quản lý";
            colMaQuanLy.Name = "colMaQuanLy";

            colHoTen.DataPropertyName = "HoTen";
            colHoTen.HeaderText = "Họ và tên";
            colHoTen.Name = "colHoTen";

            // NgaySinh as date-time picker column
            colNgaySinh.DataPropertyName = "NgaySinh";
            colNgaySinh.HeaderText = "Ngày sinh";
            colNgaySinh.Name = "colNgaySinh";

            colSoDienThoai.DataPropertyName = "SoDienThoai";
            colSoDienThoai.HeaderText = "Số điện thoại";
            colSoDienThoai.Name = "colSoDienThoai";

            colDiaChi.DataPropertyName = "DiaChi";
            colDiaChi.HeaderText = "Địa chỉ";
            colDiaChi.Name = "colDiaChi";

            colCCCD.DataPropertyName = "CCCD";
            colCCCD.HeaderText = "CCCD";
            colCCCD.Name = "colCCCD";

            colQuocTich.DataPropertyName = "QuocTich";
            colQuocTich.HeaderText = "Quốc tịch";
            colQuocTich.Name = "colQuocTich";

            colIsActive.DataPropertyName = "IsActive";
            colIsActive.HeaderText = "Kích hoạt";
            colIsActive.Name = "colIsActive";

            // add columns in order, action/delete at end
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colMaQuanLy, colHoTen, colNgaySinh, colSoDienThoai, colDiaChi, colCCCD, colQuocTich, colIsActive, colAction, colDelete });

            dataGridView1.Location = new Point(12, 30);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1374, 599);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.AutoGenerateColumns = false;
            // 
            // colAction
            // 
            colAction.HeaderText = "Hành động";
            colAction.Name = "colAction";
            colAction.Text = "Chi tiết";
            colAction.ToolTipText = "Xem chi tiết";
            colAction.UseColumnTextForButtonValue = true;
            colAction.Width = 50;
            // colDelete
            // 
            colDelete.HeaderText = "Xóa";
            colDelete.Name = "colDelete";
            colDelete.Text = "Xóa";
            colDelete.ToolTipText = "Xóa dòng";
            colDelete.UseColumnTextForButtonValue = true;
            colDelete.Width = 60;

            // 
            // btnSaveList
            // 
            btnSaveList.Location = new Point(1246, 635);
            btnSaveList.Name = "btnSaveList";
            btnSaveList.Size = new Size(140, 31);
            btnSaveList.TabIndex = 1;
            btnSaveList.Text = "Lưu dữ liệu";
            btnSaveList.UseVisualStyleBackColor = true;
            btnSaveList.Click += button1_Click;
            // 
            // colOid
            // 
            colOid.Name = "colOid";
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
            btnReload.Location = new Point(1107, 635);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(133, 31);
            btnReload.TabIndex = 3;
            btnReload.Text = "Làm mới dữ liệu";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabDanhSach);
            tabControlMain.Controls.Add(tabQuanLyHopDong);
            tabControlMain.Location = new Point(12, 40);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1400, 700);
            tabControlMain.TabIndex = 0;
            // 
            // tabDanhSach
            // 
            tabDanhSach.Controls.Add(lblCaption);
            tabDanhSach.Controls.Add(cbFilterBy);
            tabDanhSach.Controls.Add(txtFilter);
            tabDanhSach.Controls.Add(btnFilter);
            tabDanhSach.Controls.Add(btnClearFilter);
            tabDanhSach.Controls.Add(dataGridView1);
            tabDanhSach.Controls.Add(btnReload);
            tabDanhSach.Controls.Add(btnSaveList);


            tabDanhSach.Location = new Point(4, 24);
            tabDanhSach.Name = "tabDanhSach";
            tabDanhSach.Padding = new Padding(3);
            tabDanhSach.Size = new Size(1392, 672);
            tabDanhSach.TabIndex = 0;
            tabDanhSach.Text = "Danh sách CTV";
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Items.AddRange(new object[] { "MaQuanLy", "HoTen", "SoDienThoai", "DiaChi", "CCCD", "QuocTich" });
            cbFilterBy.Location = new Point(800, 6);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(150, 23);
            cbFilterBy.TabIndex = 3;
            // 
            // txtFilter
            // 
            txtFilter.Location = new Point(960, 6);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new Size(200, 23);
            txtFilter.TabIndex = 4;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(1166, 6);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(60, 23);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Lọc";
            btnFilter.Click += btnFilter_Click;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Location = new Point(1232, 6);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(80, 23);
            btnClearFilter.TabIndex = 6;
            btnClearFilter.Text = "Xóa lọc";
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // tabQuanLyHopDong
            // 
            tabQuanLyHopDong.Controls.Add(dataGridHopDong);
            tabQuanLyHopDong.Controls.Add(btnAddHopDong);
            tabQuanLyHopDong.Location = new Point(4, 24);
            tabQuanLyHopDong.Name = "tabQuanLyHopDong";
            tabQuanLyHopDong.Size = new Size(1392, 672);
            tabQuanLyHopDong.TabIndex = 1;
            tabQuanLyHopDong.Text = "Quản lý hợp đồng";
            // 
            // dataGridHopDong
            // 
            dataGridHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridHopDong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridHopDong.Columns.AddRange(new DataGridViewColumn[] { colHopDongSo });
            dataGridHopDong.Location = new Point(6, 30);
            dataGridHopDong.Name = "dataGridHopDong";
            dataGridHopDong.Size = new Size(1258, 490);
            dataGridHopDong.TabIndex = 0;
            // 
            // colHopDongSo
            // 
            colHopDongSo.HeaderText = "Số hợp đồng";
            colHopDongSo.Name = "colHopDongSo";
            // 
            // btnAddHopDong
            // 
            btnAddHopDong.Location = new Point(1147, 526);
            btnAddHopDong.Name = "btnAddHopDong";
            btnAddHopDong.Size = new Size(120, 31);
            btnAddHopDong.TabIndex = 1;
            btnAddHopDong.Text = "Thêm hợp đồng";
            // 
            // CongTacVien_ListView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1429, 753);
            Controls.Add(tabControlMain);
            Name = "CongTacVien_ListView";
            Text = "CongTacVien_ListView";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabControlMain.ResumeLayout(false);
            tabDanhSach.ResumeLayout(false);
            tabDanhSach.PerformLayout();
            tabQuanLyHopDong.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridHopDong).EndInit();
            ResumeLayout(false);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            object valueNgayTaiO = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;

            // Hiển thị thông báo chi tiết
            MessageBox.Show(
                $"Giá trị '{valueNgayTaiO}' không hợp lệ tại dòng {e.RowIndex + 1}, cột {e.ColumnIndex + 1}.\n" +
                $"Chi tiết lỗi: {e.Exception.Message}",
                "Lỗi nhập liệu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            e.Cancel = true;
        }

        #endregion
        private Button btnSaveList;
        private Button btnReload;
        private TextBox txtFilter;
        private ComboBox cbFilterBy;
        private Button btnFilter;
        private Button btnClearFilter;
        private TabControl tabControlMain;
        private TabPage tabDanhSach;
        private TabPage tabQuanLyHopDong;
        private DataGridView dataGridHopDong;
        private Button btnAddHopDong;
        private DataGridViewTextBoxColumn colHopDongSo;
    }
}