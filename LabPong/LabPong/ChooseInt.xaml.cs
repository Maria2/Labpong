using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for ChooseInt.xaml
    /// </summary>
    public partial class ChooseInt : Window
    {
        delegate void Update(Point point);
        bool measure = true;

        public ChooseInt(String[] ip)
        {
            InitializeComponent();
            Cursor = Cursors.None;
            List<String> ips = new List<string>(ip);
            IP.ItemsSource = ips;
            IP.SelectedIndex = 0;
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
        }

        private void _customListener_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!measure) return;
            if (e.PropertyName == "Position")
                pointer.Dispatcher.Invoke(new Update(UpdateUI), ((CustomListener)sender).Position);
        }

        // raised when the mouse pointer moves. 
        // moves the Ellipse when the mouse moves. 
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(pointer, e.GetPosition(this).X);
            Canvas.SetTop(pointer, e.GetPosition(this).Y);
        }

        private void UpdateUI(Point point)
        {
            Canvas.SetLeft(pointer, Left + (point.X * 3));
            Canvas.SetTop(pointer, Top + (Math.Abs(point.Y) * 3));
            App.SetCursorPos((int)(Left + (point.X * 3)), (int)(Top + (Math.Abs(point.Y) * 3)));
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IP.IsMouseOver)
                IP.SelectedIndex = (IP.SelectedIndex + 1) % IP.Items.Count;
            else
            {
                Properties.Settings.Default.IP = (string)IP.SelectedItem;
                Properties.Settings.Default.Save();
                this.Dispatcher.BeginInvoke(new Action(() => measure = false), null);
                this.Dispatcher.BeginInvoke(new Action(this.Close), null); 
            }
        }
    }
}
