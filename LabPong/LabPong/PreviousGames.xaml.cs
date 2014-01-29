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
    /// Interaction logic for PreviousGames.xaml
    /// </summary>
    public partial class PreviousGames : Window
    {
        List<String> file;
        delegate void Update(Point point);

        public PreviousGames()
        {
            InitializeComponent();
            file = System.IO.File.ReadLines("resources/highscore.txt").ToList();
            highscore.ItemsSource = file;
            title.Content = file.Count() > 1? "Last " + file.Count() + " Games":"Last Game";
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
            PointerAnimation.Sb.Completed += Animation_Completed;
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            new MainPage().Show();
            this.Close();
        }

        private void _customListener_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Position"))
                pointer.Dispatcher.Invoke(new Update(UpdateUI), ((CustomListener)sender).Position);
        }

        private void UpdateUI(Point point)
        {
            Canvas.SetLeft(pointer, (this.ActualWidth / 2 + 120) + (point.X * 3));
            Canvas.SetTop(pointer, (this.ActualHeight / 2) + (point.Y * 3));
            App.SetCursorPos((int)((this.ActualWidth / 2 + 100) + (point.X * 3)), (int)((this.ActualHeight / 2) + (point.Y * 3)));
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(pointer, e.GetPosition(this).X);
            Canvas.SetTop(pointer, e.GetPosition(this).Y);
        }

        private void back_MouseEnter(object sender, MouseEventArgs e)
        {
            PointerAnimation.AnimationTarget = back;
            back.AnimateSelection();
        }
    }
}
