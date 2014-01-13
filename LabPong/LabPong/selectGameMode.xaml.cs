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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "playPong": new Pong().Show(); this.Close(); break;
                case "playLabyrinth": new Pong().Show(); this.Close(); break;
                case "playRandom": RandomGame(); break;
            }

        }
        //senseless code used if 2nd game would exist just calls pong all the time
        public void RandomGame()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            if (randomNumber % 2 == 0)
            {
                new Pong().Show();
                this.Close();
            }
            else
            {
                new Pong().Show();
                this.Close();
            }
            
        }
    }
}
