using Leap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CustomListener CustomListener;
        protected override void OnExit(ExitEventArgs e)
        {
            CustomListener.EndListening();
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CustomListener = new CustomListener();
        }
    }    
    #region CustomListener
    /// <summary>
    /// The Listener class for the Controller
    /// </summary>
    public class CustomListener : Listener, INotifyPropertyChanged
    {
        Controller controller;
        long _old;
        /// <summary>
        /// The position of the Finger or hand being tracked
        /// </summary>
        Point position = new Point(-1, -1);
        int _id = -1;        

        public Point Position
        {
            get { return position; }
            set
            {
                if (value == position) return;
                position = value;
                if(PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Position"));
            }
        }

        public CustomListener()
        {
            controller = new Controller();
            controller.AddListener(this);
        }

        public override void OnFrame(Controller arg0)
        {
            Leap.Frame frame = arg0.Frame();

            if ((arg0.Frame().Timestamp - _old) > 1000) 
                _old = arg0.Frame().Timestamp;

            if (frame.Hands.IsEmpty && frame.Fingers.IsEmpty)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NothingTracked"));
                return;
            }
            if (_id == -1 || (!frame.Hand(_id).IsValid && !frame.Finger(_id).IsValid))
            {
                if (!frame.Hands.IsEmpty)
                {
                    _id = frame.Hands.Frontmost.Id;
                    Position = new Point(frame.Hand(_id).PalmPosition.x, frame.Hand(_id).PalmPosition.z);
                }
                else
                {
                    _id = frame.Fingers.Frontmost.Id;
                    Position = new Point(frame.Finger(_id).TipPosition.x, frame.Finger(_id).TipPosition.z);
                }
            }
            else
                if (frame.Hand(_id).IsValid)
                    Position = new Point(frame.Hand(_id).PalmPosition.x, frame.Hand(_id).PalmPosition.z);
                else
                    Position = new Point(frame.Finger(_id).TipPosition.x, frame.Finger(_id).TipPosition.z);
        }

        public void EndListening()
        {
            controller.RemoveListener(this);
            controller.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
    #endregion
    #region EllipseAnimation
    public static class PointerAnimation
    {
        private static Storyboard sb = new Storyboard();
        private static Button _animationTarget;

        public static Storyboard Sb
        {
            get { return sb; }
        }

        public static Button AnimationTarget
        {
            get { return PointerAnimation._animationTarget; }
            set { PointerAnimation._animationTarget = value; }
        }
    
        public static void AnimateSelection(this UIElement targetControl)
        {
            sb.Children.Clear();
            DoubleAnimation fadeInAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(3)));
            Storyboard.SetTarget(fadeInAnimation, targetControl);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            sb.Children.Add(fadeInAnimation);
            targetControl.MouseLeave += targetControl_MouseLeave;
            sb.Begin();
        }

        static void targetControl_MouseLeave(object sender, MouseEventArgs e)
        {        
            sb.Stop();
        }
    }
    #endregion