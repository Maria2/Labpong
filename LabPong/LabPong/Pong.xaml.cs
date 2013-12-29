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
using System.Windows.Threading;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for Pong.xaml
    /// </summary>
    public partial class Pong : Window
    {
        DispatcherTimer gameTimer;
        double xSpeed = 3;
        double ySpeed = 0;


        public Pong()
        {
            InitializeComponent();
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            gameTimer.Tick +=  new EventHandler(gameTimer_Elapsed);
            gameTimer.Start();
        }

        void gameTimer_Elapsed(object sender, EventArgs e)
        {
            Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + xSpeed);
            Canvas.SetTop(Ball, Canvas.GetTop(Ball) + ySpeed);
        }


        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Canvas.SetLeft(Ball, (ActualWidth / 2) - (Ball.ActualWidth/2));
            Canvas.SetTop(Ball, (ActualHeight / 2) - (Ball.ActualHeight/2));
            Canvas.SetTop(Player, (ActualHeight / 2) - (Player.ActualHeight/2));
            Canvas.SetTop(Enemy, (ActualHeight / 2) - (Player.ActualHeight / 2));
            Canvas.SetRight(Enemy, 0);
            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
