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
    /// Interaction logic for connectPage.xaml
    /// </summary>
    public partial class ConnectPage : Window
    {
        delegate void ChangeLabel(String message);
        delegate void Update(Point point);

        string[] serverIP = new string[4];
        string[] userIP = new string[4];
        public ConnectPage()
        {
            InitializeComponent();
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
            PointerAnimation.Sb.Completed += Animation_Completed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "hostClicked": HostClicked(); break;
                case "joinClicked": JoinClicked(); break;
            }

        }

        //user enters own ip and hosts game
        private void HostClicked()
        {
            userIP[0] = userIPText1.Text;
            userIP[1] = userIPText2.Text;
            userIP[2] = userIPText3.Text;
            userIP[3] = userIPText4.Text;
            OnNotification(NotificationTyp.wait);
            CheckIp(userIP);
        }

        //user enters ip of other player
        private void JoinClicked()
        {
            serverIP[0] = serverIPText1.Text;
            serverIP[1] = serverIPText2.Text;
            serverIP[2] = serverIPText3.Text;
            serverIP[3] = serverIPText4.Text;
            OnNotification(NotificationTyp.wait);
            CheckIp(serverIP);

            //if error -> OnNotification()
        }
        /// <summary>
        /// Raises errors or notifications of type <paramref name="NotificationTyp"/>
        /// </summary>
        /// <param name="NotificationTyp"></param>
        private void OnNotification(NotificationTyp notificationTyp)
        {
            switch (notificationTyp)
            {
                case NotificationTyp.conFailed: notificationText.Visibility = System.Windows.Visibility.Visible; notificationText.Text = "Connection has Failed"; break;
                case NotificationTyp.wrongInput: notificationText.Visibility = System.Windows.Visibility.Visible; notificationText.Text = "Wrong input was entered"; break;
                case NotificationTyp.wait: notificationText.Visibility = System.Windows.Visibility.Visible; notificationText.Text = "Connection is established...please wait"; break;  
            }
        }
        /// <summary>
        /// Checks the given ip address on not null, right format and adds dots for the right format
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string CheckIp(string[] ip)
        {
            //format lenght
            //not null smaller than 255
            //add dots

            string ipRet="0.0.0.0";

            try
            {

                if (ip[0] != null && ip[1] != null && ip[2] != null && ip[3] != null)
                {
                    if (Convert.ToInt16(ip[0]) < 255 && Convert.ToInt16(ip[1]) < 255 && Convert.ToInt16(ip[2]) < 255 && Convert.ToInt16(ip[3]) < 255)
                    {
                        if (Convert.ToInt16(ip[0]) >= 0 && Convert.ToInt16(ip[1]) >= 0 && Convert.ToInt16(ip[2]) >= 0 && Convert.ToInt16(ip[3]) >= 0)
                        {
                            ipRet = ip[0] + "." + ip[1] + "." + ip[2] + "." + ip[3];
                            return ipRet;
                        }
                        else OnNotification(NotificationTyp.wrongInput);
                    }
                    else OnNotification(NotificationTyp.wrongInput);
                }
                else OnNotification(NotificationTyp.wrongInput); 
            }
            catch(FormatException)
            {
                OnNotification(NotificationTyp.wrongInput);
            }
            return ipRet;
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
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (joinClicked.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = joinClicked;
                joinClicked.AnimateSelection();
            }
            if (hostClicked.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = hostClicked;
                hostClicked.AnimateSelection();
            }
        }
    }
}
