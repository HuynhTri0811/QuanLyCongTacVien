namespace QuanLyCongTacVien
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView listViewCTV;
        private System.Windows.Forms.ColumnHeader columnOid;
        private System.Windows.Forms.ColumnHeader columnMaQuanLy;
        private System.Windows.Forms.ColumnHeader columnHoTen;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.listViewCTV = new System.Windows.Forms.ListView();
            this.columnOid = new System.Windows.Forms.ColumnHeader();
            this.columnMaQuanLy = new System.Windows.Forms.ColumnHeader();
            this.columnHoTen = new System.Windows.Forms.ColumnHeader();
            SuspendLayout();
            // 
            // listViewCTV
            // 
            this.listViewCTV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCTV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnOid,
            this.columnMaQuanLy,
            this.columnHoTen});
            this.listViewCTV.FullRowSelect = true;
            this.listViewCTV.HideSelection = false;
            this.listViewCTV.Location = new System.Drawing.Point(12, 12);
            this.listViewCTV.Name = "listViewCTV";
            this.listViewCTV.Size = new System.Drawing.Size(776, 426);
            this.listViewCTV.TabIndex = 0;
            this.listViewCTV.UseCompatibleStateImageBehavior = false;
            this.listViewCTV.View = System.Windows.Forms.View.Details;
            // 
            // columnOid
            // 
            this.columnOid.Text = "Oid";
            this.columnOid.Width = 60;
            // 
            // columnMaQuanLy
            // 
            this.columnMaQuanLy.Text = "MaQuanLy";
            this.columnMaQuanLy.Width = 120;
            // 
            // columnHoTen
            // 
            this.columnHoTen.Text = "HoTen";
            this.columnHoTen.Width = 200;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(this.listViewCTV);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion
    }
}
