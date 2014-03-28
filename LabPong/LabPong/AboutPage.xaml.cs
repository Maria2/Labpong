using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Window
    {
        delegate void Update(Point point);

        public AboutPage()
        {
            InitializeComponent();
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
            PointerAnimation.Sb.Completed += Animation_Completed;
        }

        private void _customListener_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Hidden) return;
            if (e.PropertyName.Equals("Position"))
                pointer.Dispatcher.Invoke(new Update(UpdateUI), ((CustomListener)sender).Position);
        }

        private void UpdateUI(Point point)
        {
            Canvas.SetLeft(pointer, (this.ActualWidth / 2 + 90) + (point.X * 5));
            Canvas.SetTop(pointer, (this.ActualHeight / 2) + (point.Y * 4));
            App.SetCursorPos((int)((this.ActualWidth / 2 + 90) + (point.X * 5)), (int)((this.ActualHeight / 2) + (point.Y * 4)));
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();            
            Application.Current.MainWindow.Visibility = Visibility.Visible;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(pointer, e.GetPosition(this).X);
            Canvas.SetTop(pointer, e.GetPosition(this).Y);
        }

        private void back_MouseEnter(object sender, MouseEventArgs e)
        {
            if (back.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = back;
                back.AnimateSelection();
            }
        }
    }
}
