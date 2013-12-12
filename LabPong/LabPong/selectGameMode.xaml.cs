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
    /// Interaction logic for selectGameMode.xaml
    /// </summary>
    public partial class selectGameMode : Window
    {
        public selectGameMode()
        {
            InitializeComponent();
        }

        private void OnPongClicked(object sender, RoutedEventArgs e)
        { }

        private void OnRandomClicked(object sender, RoutedEventArgs e)
        { }

        private void OnLabyrinthClicked(object sender, RoutedEventArgs e)
        { }
    }
}
