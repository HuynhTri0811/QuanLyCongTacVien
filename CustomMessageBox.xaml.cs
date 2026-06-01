using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace QuanLyCongTacVien
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(Window owner, string message, string title, MessageBoxImage iconType)
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
            SetIcon(iconType);
        }

        private void SetIcon(MessageBoxImage iconType)
        {
            gridIcon.Children.Clear();

            if (iconType == MessageBoxImage.Error)
            {
                // Draw Red Cross (Error)
                var grid = new Grid { Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                grid.Margin = new Thickness(0, 5, 0, 0);

                var ellipse1 = new Ellipse { Stroke = new SolidColorBrush(Color.FromRgb(166, 28, 36)), StrokeThickness = 3 };
                var ellipse2 = new Ellipse { Stroke = new SolidColorBrush(Color.FromRgb(166, 28, 36)), StrokeThickness = 1, Margin = new Thickness(3) };

                var path = new Path
                {
                    Stroke = new SolidColorBrush(Color.FromRgb(166, 28, 36)),
                    StrokeThickness = 4.5,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    Data = Geometry.Parse("M 17 17 L 33 33 M 33 17 L 17 33")
                };

                grid.Children.Add(ellipse1);
                grid.Children.Add(ellipse2);
                grid.Children.Add(path);
                gridIcon.Children.Add(grid);
            }
            else if (iconType == MessageBoxImage.Warning)
            {
                // Draw Yellow Exclamation (Warning)
                var grid = new Grid { Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                grid.Margin = new Thickness(0, 5, 0, 0);

                var ellipse1 = new Ellipse { Stroke = new SolidColorBrush(Color.FromRgb(230, 126, 34)), StrokeThickness = 3 };
                var ellipse2 = new Ellipse { Stroke = new SolidColorBrush(Color.FromRgb(230, 126, 34)), StrokeThickness = 1, Margin = new Thickness(3) };

                var path = new Path
                {
                    Stroke = new SolidColorBrush(Color.FromRgb(230, 126, 34)),
                    StrokeThickness = 4.5,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    Data = Geometry.Parse("M 25 15 L 25 28")
                };

                var ellipseDot = new Ellipse
                {
                    Fill = new SolidColorBrush(Color.FromRgb(230, 126, 34)),
                    Width = 4.5,
                    Height = 4.5,
                    Margin = new Thickness(0, 0, 0, 10),
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                grid.Children.Add(ellipse1);
                grid.Children.Add(ellipse2);
                grid.Children.Add(path);
                grid.Children.Add(ellipseDot);
                gridIcon.Children.Add(grid);
            }
            else // Info / Success
            {
                // Draw Green Checkmark (Information)
                var grid = new Grid { Width = 50, Height = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
                grid.Margin = new Thickness(0, 5, 0, 0);

                var ellipse1 = new Ellipse { Stroke = new SolidColorBrush(Color.FromRgb(43, 138, 62)), StrokeThickness = 3 };
                var ellipse2 = new Ellipse { Stroke = new SolidColorBrush(Color.FromRgb(43, 138, 62)), StrokeThickness = 1, Margin = new Thickness(3) };

                var path = new Path
                {
                    Stroke = new SolidColorBrush(Color.FromRgb(43, 138, 62)),
                    StrokeThickness = 4.5,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                    Data = Geometry.Parse("M 16 26 L 23 33 L 34 18")
                };

                grid.Children.Add(ellipse1);
                grid.Children.Add(ellipse2);
                grid.Children.Add(path);
                gridIcon.Children.Add(grid);
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        public static void Show(string message, string title = "Thông báo", MessageBoxImage icon = MessageBoxImage.Information)
        {
            Window activeWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive) 
                                   ?? Application.Current.MainWindow;

            if (activeWindow != null)
            {
                var msgBox = new CustomMessageBox(activeWindow, message, title, icon);
                msgBox.ShowDialog();
            }
            else
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, icon);
            }
        }
    }
}
