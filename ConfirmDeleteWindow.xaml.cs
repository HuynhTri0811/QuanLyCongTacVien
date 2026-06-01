using System.Windows;

namespace QuanLyCongTacVien
{
    public partial class ConfirmDeleteWindow : Window
    {
        public ConfirmDeleteWindow(Window owner, string message, string title = "Xác nhận xóa")
        {
            InitializeComponent();
            this.Owner = owner;
            
            if (owner.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.Left = owner.Left;
                this.Top = owner.Top;
                this.Width = owner.Width;
                this.Height = owner.Height;
            }
            
            txtTitle.Text = title;
            txtMessage.Text = message;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
