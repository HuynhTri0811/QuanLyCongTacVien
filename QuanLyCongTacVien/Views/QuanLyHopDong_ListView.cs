using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyCongTacVien.DTO;
using QuanLyCongTacVien.DAO;

namespace QuanLyCongTacVien.Views
{
    public class QuanLyHopDong_ListView : Form
    {
        private DataGridView dataGridView1;
        private Button btnAdd;
        private Button btnSave;
        private Button btnReload;
        private BindingList<QuanLyHopDongCongTacVien> _bindingList;

        public QuanLyHopDong_ListView()
        {
            InitializeComponent();
            LoadHopDongs();
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new DataGridView();
            this.btnAdd = new Button();
            this.btnSave = new Button();
            this.btnReload = new Button();

            // 
            // dataGridView1
            // 
            this.dataGridView1.Location = new Point(12, 12);
            this.dataGridView1.Size = new Size(760, 380);
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;

            // 
            // btnAdd
            // 
            this.btnAdd.Location = new Point(12, 400);
            this.btnAdd.Size = new Size(100, 30);
            this.btnAdd.Text = "Thêm hợp đồng";
            this.btnAdd.Click += BtnAdd_Click;

            // 
            // btnSave
            // 
            this.btnSave.Location = new Point(118, 400);
            this.btnSave.Size = new Size(100, 30);
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += BtnSave_Click;

            // 
            // btnReload
            // 
            this.btnReload.Location = new Point(224, 400);
            this.btnReload.Size = new Size(100, 30);
            this.btnReload.Text = "Làm mới";
            this.btnReload.Click += BtnReload_Click;

            // 
            // Form
            // 
            this.ClientSize = new Size(784, 441);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReload);
            this.Text = "Quản lý hợp đồng CTV";
        }

        private void LoadHopDongs()
        {
            try
            {
                using (var ctx = new AppDbContext(Database.DbPath))
                {
                    var list = ctx.HopDongs?.ToList() ?? new List<QuanLyHopDongCongTacVien>();
                    _bindingList = new BindingList<QuanLyHopDongCongTacVien>(list);
                    dataGridView1.DataSource = _bindingList;
                    EnsureColumns();
                }
            }
            catch (Exception ex)
            {
                // If EF isn't available or DB access failed, show empty list and log
                _bindingList = new BindingList<QuanLyHopDongCongTacVien>();
                dataGridView1.DataSource = _bindingList;
                EnsureColumns();
                Console.WriteLine("Không tải được hợp đồng: " + ex.Message);
            }
        }

        private void EnsureColumns()
        {
            // If auto columns are generated, ensure display order and column types
            dataGridView1.AutoGenerateColumns = true;
            // format NgayKy column if present
            if (dataGridView1.Columns.Contains("NgayKy"))
            {
                dataGridView1.Columns["NgayKy"].DefaultCellStyle.Format = "d";
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var item = new QuanLyHopDongCongTacVien { MaQuanLy = string.Empty, Ngay = DateTime.Now, NgayKy = DateTime.Now };
            _bindingList.Add(item);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var ctx = new AppDbContext(Database.DbPath))
                {
                    foreach (var item in _bindingList)
                    {
                        if (item.GetOid() <= 0)
                        {
                            ctx.HopDongs.Add(item);
                        }
                        else
                        {
                            ctx.HopDongs.Update(item);
                        }
                    }
                    ctx.SaveChanges();
                }
                MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadHopDongs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReload_Click(object? sender, EventArgs e)
        {
            LoadHopDongs();
        }
    }
}
