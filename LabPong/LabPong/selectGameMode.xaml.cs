using Leap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for selectGameMode.xaml
    /// </summary>
    public partial class selectGameMode : Window
    {
        delegate void ChangeLabel(String message);
        delegate void Update(Point point);

        public selectGameMode()
        {
            InitializeComponent();
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
            PointerAnimation.Sb.Completed += Animation_Completed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "playPong": new Pong().Show(); this.Close(); break;
                case "playLabyrinth": new Pong().Show(); this.Close(); break;
                case "playRandom": RandomGame(); break;
            }

        }
        //senseless code used if 2nd game would exist just calls pong all the time
        public void RandomGame()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            if (randomNumber % 2 == 0)
            {
                new Pong().Show();
                this.Close();
            }
            else
            {
                new Pong().Show();
                this.Close();
            }
            
        }
        void _customListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "NothingTracked":
                    label.Dispatcher.Invoke(new ChangeLabel(UpdateLabel), "Nothing Tracked");
                    break;
                case "Position":
                    label.Dispatcher.Invoke(new ChangeLabel(UpdateLabel), ((CustomListener)sender).Position.ToString());
                    pointer.Dispatcher.Invoke(new Update(UpdateUI), ((CustomListener)sender).Position);
                    break;
            }
        }

        private void UpdateUI(Point point)
        {
            Canvas.SetLeft(pointer, (this.ActualWidth / 2 + 120) + (point.X * 3));
            Canvas.SetTop(pointer, (this.ActualHeight / 2) + (point.Y * 3));
            SetCursorPos((int)((this.ActualWidth / 2 + 100) + (point.X * 3)), (int)((this.ActualHeight / 2) + (point.Y * 3)));
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private void UpdateLabel(String content)
        {
            label.Content = content;
        }

        // raised when the mouse pointer moves. 
        // moves the Ellipse when the mouse moves. 
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(pointer, e.GetPosition(this).X);
            Canvas.SetTop(pointer, e.GetPosition(this).Y);
        }

        void Animation_Completed(object sender, EventArgs e)
        {
            Button_Click(PointerAnimation.AnimationTarget, null);
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (playLabyrinth.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = playLabyrinth;
                playLabyrinth.AnimateSelection();
            }
            if (playPong.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = playPong;
                playPong.AnimateSelection();
            }
            if (playLabyrinth.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = playLabyrinth;
                playLabyrinth.AnimateSelection();
            }
        }
    }
}
