using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LabPong
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static MouseBinder _mouseBind = new MouseBinder();
        public static MouseBinder MouseBind
        {
            get { return _mouseBind;}
        }

        public static void NewMousePosition(Point mousePosition)
        {
            MouseBind.MousePos = mousePosition;
        }

    }
    public class MouseBinder : INotifyPropertyChanged
    {
        private System.Windows.Point _mousePos;
        public System.Windows.Point MousePos
        {
            get { return _mousePos; }
            set
            {               
                if (_mousePos == value) return;
                _mousePos = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("_mousePos"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
