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
using System.Windows.Shapes;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for ExitConfirmation.xaml
    /// </summary>
    public partial class ExitConfirmation : Window
    {
        delegate void Update(Point point);
        public ExitConfirmation()
        {
            InitializeComponent();
            Cursor = Cursors.None;
            PointerAnimation.Sb.Completed += Animation_Completed;
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
        }

        private void _customListener_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Position")
                pointer.Dispatcher.Invoke(new Update(UpdateUI), ((CustomListener)sender).Position);
        }

        private void UpdateUI(Point point)
        {
            Canvas.SetLeft(pointer, (this.ActualWidth / 2 + 120) + (point.X * 3));
            Canvas.SetTop(pointer, (this.ActualHeight / 2) + (point.Y * 3));
            App.SetCursorPos((int)((this.ActualWidth / 2 + 100) + (point.X * 3)), (int)((this.ActualHeight / 2) + (point.Y * 3)));
        }

        // raised when the mouse pointer moves. 
        // moves the Ellipse when the mouse moves. 
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(pointer, e.GetPosition(this).X);
            Canvas.SetTop(pointer, e.GetPosition(this).Y);
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            if (PointerAnimation.AnimationTarget == yes)
                Yes_Click(sender, null);
            if (PointerAnimation.AnimationTarget == no)
                No_Click(sender, null);
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (yes.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = yes;
                yes.AnimateSelection();
            }
            if (no.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = no;
                no.AnimateSelection();
            }
        }
    }
}
