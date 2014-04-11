using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        delegate void UpdateImage(String path);
        delegate void VoidMethod();

        public Pong()
        {
            InitializeComponent();
            PongModel.pongModel.PropertyChanged += pl_PropertyChanged;            
        }

        void pl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("playerX"))
                Player.Dispatcher.BeginInvoke(new ChangePos(ChangePlayerPos), new object[] { Player, ((PongModel)sender).PlayerX });
            if (e.PropertyName.Equals("playerY"))
                Enemy.Dispatcher.BeginInvoke(new ChangePos(ChangePlayerPos), new object[] { Enemy, ((PongModel)sender).PlayerY });
            if (e.PropertyName.Equals("playerXScore"))
                ScoreX.Dispatcher.BeginInvoke(new ChangeScore(UpdateScore), new object[] { ScoreX, ((PongModel)sender).PlayerXScore });
            if (e.PropertyName.Equals("playerYScore"))
                ScoreY.Dispatcher.BeginInvoke(new ChangeScore(UpdateScore), new object[] { ScoreY, ((PongModel)sender).PlayerYScore });
            if (e.PropertyName.Equals("ballPos"))
                Ball.Dispatcher.BeginInvoke(new ChangePosition(UpdateBallPos), DispatcherPriority.DataBind, new object[] { ((PongModel)sender).BallPos });
            if (e.PropertyName.Split(new char[] { ':' })[0].Equals("add"))
                playerX_item.Dispatcher.BeginInvoke(new UpdateImage(AddItem), new object[] {e.PropertyName.Split(new char[]{':'})[1]});
            if (e.PropertyName.Equals("del"))
                playerX_item.Dispatcher.BeginInvoke(new VoidMethod(DeleteItem), null);
            if (e.PropertyName.Equals("white"))
                Dispatcher.BeginInvoke(new VoidMethod(MakeWhite), null);
            if (e.PropertyName.Equals("sizeX"))
                Dispatcher.BeginInvoke(new Action(() => Player.Height = PongModel.PlayerSizes.Y + ((PongModel)sender).ResizeX));
            if (e.PropertyName.Equals("sizeY"))
                Dispatcher.BeginInvoke(new Action(() => Enemy.Height = PongModel.PlayerSizes.Y + ((PongModel)sender).ResizeY));
            if(e.PropertyName.Equals("width"))
                Dispatcher.BeginInvoke(new VoidMethod(changeWidth), null);
        }

        void changeWidth()
        {
            if(PongModel.WINDOW_WIDTH > PongModel.WINDOW_WIDTH_Y) 
                Width = PongModel.WINDOW_WIDTH_Y;                 
            else 
                Width = PongModel.WINDOW_WIDTH;
            InvalidateVisual();
        }

        void MakeWhite()
        {
            Background = Brushes.White;
            Thread.Sleep(1000);
            Background = Brushes.Black;
        }

        void DeleteItem()
        {
            playerX_item.Children.RemoveAt(0);
        }

        void AddItem(String image)
        {
            Image im = new Image();            
            im.Source = new BitmapImage(new Uri("pack://application:,,,/items/"+image+".png"));
            playerX_item.Children.Add(im);
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
            if (ActualHeight > 0 && ActualWidth > 0 && !PongLogic.GameStarted)
            {
                PongLogic.GameStarted = true;
                PongModel.WINDOW_HEIGHT = ActualHeight;
                PongModel.WINDOW_WIDTH = ActualWidth;
                PongModel.PlayerSizes = new Point((ActualWidth / 1360) * 15, ActualHeight / 5);
                PongModel.BallSize = new Point((ActualWidth / 1360) * 50, (ActualHeight / 768) * 50);
                Player.Width = PongModel.PlayerSizes.X;
                Enemy.Width = PongModel.PlayerSizes.X;
                Player.Height = PongModel.PlayerSizes.Y;
                Enemy.Height = PongModel.PlayerSizes.Y;
                Ball.Width = PongModel.BallSize.X;
                Ball.Height = PongModel.BallSize.Y;
                PongModel.pongModel.SendPos();
            }
            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
