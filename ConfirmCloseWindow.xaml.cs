using System.Windows;

namespace QuanLyCongTacVien
{
    public partial class ConfirmCloseWindow : Window
    {
        public MessageBoxResult Result { get; set; } = MessageBoxResult.Cancel;

        public ConfirmCloseWindow(Window owner, string message, string title = "Xác nhận lưu thay đổi")
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
            Result = MessageBoxResult.Yes;
            this.DialogResult = true;
            this.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            this.DialogResult = false;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            this.DialogResult = null;
            this.Close();
        }
    }
}
