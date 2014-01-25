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

namespace LabPong
{
    /// <summary>
    /// Interaction logic for ExitConfirmation.xaml
    /// </summary>
    public partial class ExitConfirmation : Window
    {
        public ExitConfirmation()
        {
            InitializeComponent();
            Cursor = Cursors.None;
            PointerAnimation.Sb.Completed += Animation_Completed;
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            if (PointerAnimation.AnimationTarget == yes)
                Yes_Click(sender, null);
            if (PointerAnimation.AnimationTarget == no)
                No_Click(sender, null);
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (yes.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = yes;
                yes.AnimateSelection();
            }
            if (no.IsMouseOver)
            {
                PointerAnimation.AnimationTarget = no;
                no.AnimateSelection();
            }
        }
    }
}
