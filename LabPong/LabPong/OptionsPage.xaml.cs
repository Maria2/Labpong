using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public OptionsPage()
        {
            InitializeComponent();
            username.Text = Properties.Settings.Default.Username;
            List<String> options = new List<string>(2);
            options.Add("Default");
            options.Add("Custom");            
            audio.ItemsSource = options;
            audio.SelectedItem = Properties.Settings.Default.Track;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (audio.IsMouseOver)
                audio.SelectedIndex = (audio.SelectedIndex + 1) % 2;
            if (username.IsMouseOver)
                username.Focus();
            if (back.IsMouseOver)
            {
                Properties.Settings.Default.Username = username.Text;
                Properties.Settings.Default.Track = (string)audio.SelectedItem;
                Properties.Settings.Default.Save();
                this.Close();
                Application.Current.MainWindow.Visibility = Visibility.Visible ;
            }            
        }  
    }
}
