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
using System.Windows.Forms;
namespace LabPong
{
    /// <summary>
    /// Interaction logic for Keyboard2.xaml
    /// </summary>
    public partial class Keyboard : Window
    {
        public Keyboard()
        {
            InitializeComponent();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((System.Windows.Controls.Button)sender).Name)
            {
                case "one": SendKeys.SendWait("1"); break;
                case "two": SendKeys.Send("2"); break;
                case "three": SendKeys.Send("3"); break;
                case "four": SendKeys.Send("4"); break;
                case "five": SendKeys.Send("5"); break;
                case "six": SendKeys.Send("6"); break;
                case "seven": SendKeys.Send("7"); break;
                case "eight": SendKeys.Send("8"); break;
                case "nine": SendKeys.Send("9"); break;
                case "null": SendKeys.Send("0"); break;
                case "ok": break;
            }
        }
    }
}
