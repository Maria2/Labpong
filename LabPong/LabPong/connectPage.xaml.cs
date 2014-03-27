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
        delegate void host();
        delegate void hostbutton();
        delegate void join(String ip);
        Boolean enable = true;

        // checked ips used for connection
        private string joinIP;

        public string JoinIP
        {
            get { return joinIP; }
            set { joinIP = value; }
        }



        //ip arrays needed for progress
        string[] serverIP = new string[4];
        string[] userIP = new string[4];
        string numbers = "";
        int ipServerField = 0; //1,2,3,4
        Boolean deleteNum = false;
        public ConnectPage()
        {
            InitializeComponent();
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;
            PointerAnimation.Sb.Completed += Animation_Completed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!enable) return;
            switch (((Button)sender).Name)
            {
                case "hostClicked": new host(new PongManager(this).hostGame).BeginInvoke(null, null); break;
                case "joinClicked": JoinClicked(); new join(new PongManager(this).joinGame).BeginInvoke(joinIP, null, null); break;
                case "one": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "two": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "three": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "four": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "five": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "six": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "seven": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "eight": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "nine": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "null": numbers += ((Button)sender).Content; Override_ButtonText(ipServerField); break;
                case "delete": deleteNum = true; Override_ButtonText(ipServerField); break;
            }

        }

        ////user enters own ip and hosts game
        //private void HostClicked()
        //{
        //    userIP[0] = userIPText1.Text;
        //    userIP[1] = userIPText2.Text;
        //    userIP[2] = userIPText3.Text;
        //    userIP[3] = userIPText4.Text;
        //    OnNotification(NotificationTyp.wait);
        //    CheckIp(userIP);
        //    if (userIP != null)
        //        Jo = userIP[0] + "." + userIP[1] + "." + userIP[2] + "." + userIP[3];
        //    isHost = true;
        //}

        //user enters ip of other player
        private void JoinClicked()
        {
            serverIP[0] = serverIPText1.Text;
            serverIP[1] = serverIPText2.Text;
            serverIP[2] = serverIPText3.Text;
            serverIP[3] = serverIPText4.Text;
            OnNotification(NotificationTyp.wait);
            CheckIp(serverIP);
            joinIP = serverIP[0] + "." + serverIP[1] + "." + serverIP[2] + "." + serverIP[3];

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
                case NotificationTyp.selectField: notificationText.Visibility = System.Windows.Visibility.Visible; notificationText.Text = "Please hover over the text field u want to input first"; break;
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

            string ipRet = "0.0.0.0";

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
            catch (FormatException)
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
            SetCursorPos((int)((this.ActualWidth / 2 + 100) + (point.X * 3)), (int)((this.ActualHeight / 2) + (point.Y * 3)));
        }

        public void Toggle_HostButton()
        {
            hostClicked.Dispatcher.BeginInvoke(new host(toggleHost), null);
        }

        private void toggleHost()
        {
            hostClicked.IsEnabled = !hostClicked.IsEnabled;
            joinClicked.IsEnabled = !joinClicked.IsEnabled;
            enable = !enable;
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
            if (!enable) return;
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
        //Mouse enter method for the server IP input TextFields 
        private void Text_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "serverIPText1": serverIPText1.Focus(); ipServerField = 1; break;
                case "serverIPText2": serverIPText2.Focus(); ipServerField = 2; break;
                case "serverIPText3": serverIPText3.Focus(); ipServerField = 3; break;
                case "serverIPText4": serverIPText4.Focus(); ipServerField = 4; break;
            }
        }
        /* Mouse enter for the keyboard Buttons 
        * checks witch serverIPField was selected then writes in there
        * animates the button via the AnimateShortSelection() method
        * */
        public void Number_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "one":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "two":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "three":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "four":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "five":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "six":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "seven":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "eight":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "nine":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "null":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
                case "delete":
                    if (((Button)sender).IsMouseOver)
                    {
                        PointerAnimation.AnimationTarget = ((Button)sender);
                        ((Button)sender).AnimateShortSelection();
                    }
                    break;
            }


        }
        public void Override_ButtonText(int ipServerField)
        {
            switch (ipServerField)
            {
                case 0: OnNotification(NotificationTyp.selectField); break;
                case 1: if (deleteNum) { serverIPText1.Text = ""; deleteNum = false; } else { serverIPText1.Text += numbers; }; break;
                case 2: if (deleteNum) { serverIPText2.Text = ""; deleteNum = false; } else { serverIPText2.Text += numbers; }; break;
                case 3: if (deleteNum) { serverIPText3.Text = ""; deleteNum = false; } else { serverIPText3.Text += numbers; }; break;
                case 4: if (deleteNum) { serverIPText4.Text = ""; deleteNum = false; } else { serverIPText4.Text += numbers; }; break;

            }
            numbers = "";
        }
    }
}