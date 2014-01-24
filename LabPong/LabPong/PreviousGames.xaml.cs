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
    /// Interaction logic for PreviousGames.xaml
    /// </summary>
    public partial class PreviousGames : Window
    {
        List<String> file;
        public PreviousGames()
        {
            InitializeComponent();
            file = System.IO.File.ReadLines("resources/highscore.txt").ToList();
            highscore.ItemsSource = file;
            title.Content = file.Count() > 1? "Last " + file.Count() + " Games":"Last Game";
            
        }
    }
}
