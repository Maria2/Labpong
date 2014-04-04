using Leap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        delegate void ChangeLabel(String message);
        delegate void Update(Point point);
        Boolean measure = true;

        public MainPage()
        {
            InitializeComponent();                    
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
            PointerAnimation.Sb.Completed += Animation_Completed;             
        }

        void _customListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden || !measure) return;
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
            App.SetCursorPos((int)((this.ActualWidth / 2 + 100) + (point.X * 3)), (int)((this.ActualHeight / 2) + (point.Y * 3)));
        }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "start_game": new ConnectPage().Show(); this.Hide(); break;
                case "high_score": this.Hide(); new PreviousGames().Show(); break;
                case "options": measure = false; new OptionsPage().ShowDialog(); measure = true; break;
                case "about": this.Hide(); new AboutPage().ShowDialog(); this.Show(); break;
                case "exit": measure = false; new ExitConfirmation().ShowDialog(); measure = true; break;
            }
                            
        }


        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (start_game.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = start_game;
                start_game.AnimateSelection();                
            }
            if (high_score.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = high_score;
                high_score.AnimateSelection();                
            }
            if (options.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = options;
                options.AnimateSelection();                
            }
            if (about.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = about;
                about.AnimateSelection();
            }
            if (exit.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = exit;
                exit.AnimateSelection();
            }
        }
    }
}
