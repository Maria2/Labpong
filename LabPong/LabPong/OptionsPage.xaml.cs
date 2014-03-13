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
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : Window
    {
        private static string username1="";
        public OptionsPage()
        {
            InitializeComponent();
            username.Text = Properties.Settings.Default.Username;
            username1 = username.Text;
            List<String> options = new List<string>(2);
            options.Add("Default");
            options.Add("Custom");
            audio.ItemsSource = options;
            audio.SelectedValue = Properties.Settings.Default.Username;
        }

        public static string getUsername()
        {
            return username1;
        }
    }
}
