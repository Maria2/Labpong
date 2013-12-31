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
        delegate void ChangePos(Rectangle player, double pos);
        delegate void ChangeScore(Label label, int score);
        delegate void ChangePosition(Point pos);

        public Pong()
        {
            InitializeComponent();
            new PongLogic();
            PongLogic.listener.PropertyChanged += pl_PropertyChanged;
        }

        void pl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("playerX"))
                Player.Dispatcher.BeginInvoke(new ChangePos(ChangePlayerPos), new object[] { Player, ((PongLogic)sender).PlayerX });
            if (e.PropertyName.Equals("playerY"))
                Enemy.Dispatcher.BeginInvoke(new ChangePos(ChangePlayerPos), new object[] { Enemy, ((PongLogic)sender).PlayerY });
            if (e.PropertyName.Equals("playerXScore"))
                ScoreX.Dispatcher.BeginInvoke(new ChangeScore(UpdateScore), new object[] { ScoreX, ((PongLogic)sender).PlayerXScore });
            if (e.PropertyName.Equals("playerYScore"))
                ScoreY.Dispatcher.BeginInvoke(new ChangeScore(UpdateScore), new object[] { ScoreY, ((PongLogic)sender).PlayerYScore });
            if (e.PropertyName.Equals("ballPos"))
                this.Dispatcher. BeginInvoke(new ChangePosition(UpdateBallPos), new object[] { ((PongLogic)sender).BallPos });
        }

        void UpdateScore(Label label, int currentScore)
        {
            label.Content = currentScore;
        }

        void UpdateBallPos(Point position)
        {
            Canvas.SetLeft(Ball, position.X);
            Canvas.SetTop(Ball, position.Y);
        }

        void ChangePlayerPos(Rectangle player, double position)
        {            
            Canvas.SetTop(player, position);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {            
            Canvas.SetTop(Player, (ActualHeight / 2) - (Player.ActualHeight/2));
            Canvas.SetTop(Enemy, (ActualHeight / 2) - (Enemy.ActualHeight / 2));
            Canvas.SetRight(Enemy, 0);
            Canvas.SetLeft(ScoreX, (ActualWidth / 2) - (ScoreX.ActualWidth / 2) - 60);
            Canvas.SetLeft(ScoreY, (ActualWidth / 2) - (ScoreY.ActualWidth / 2) + 60);
            if (ActualHeight > 0 && ActualWidth > 0)
            {
                PongLogic.WINDOW_HEIGHT = ActualHeight;
                PongLogic.WINDOW_WIDTH = ActualWidth;
                PongLogic.GameStarted = true;
            }
            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
