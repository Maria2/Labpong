using Leap;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        Controller controller = new Controller();
        CustomController customController = new CustomController();

        public MainPage()
        {
            InitializeComponent();            
            controller.AddListener(customController);
        }        

        // raised when the mouse pointer moves. 
        // moves the Ellipse when the mouse moves. 
        private void Canvas_MouseDown(object sender, MouseEventArgs e){
            App.NewMousePosition(e.GetPosition(this));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Exit Programm");      
            controller.RemoveListener(customController);
            controller.Dispose();
        }
    }
}
