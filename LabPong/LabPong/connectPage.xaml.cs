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
    /// Interaction logic for connectPage.xaml
    /// </summary>
    public partial class ConnectPage : Window
    {
        string[] serverIP = new string[4];
        string[] userIP = new string[4];
        public ConnectPage()
        {
            InitializeComponent();
        }

        //user enters own ip and hosts game
        private void OnHostClicked(object sender, RoutedEventArgs e)
        {
            userIP[0] = userIPText1.Text;
            userIP[1] = userIPText2.Text;
            userIP[2] = userIPText3.Text;
            userIP[3] = userIPText4.Text;
            OnNotification(NotificationTyp.wait);
            CheckIp(userIP);
        }

        //user enters ip of other player
        private void OnJoinClicked(object sender, RoutedEventArgs e)
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
    }
}
