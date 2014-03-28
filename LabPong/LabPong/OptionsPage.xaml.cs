﻿using System;
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
        delegate void Update(Point point);

        public OptionsPage()
        {
            InitializeComponent();
            username.Text = Properties.Settings.Default.Username;
            List<String> options = new List<string>(2);
            options.Add("Default");
            options.Add("Custom");            
            audio.ItemsSource = options;
            audio.SelectedItem = Properties.Settings.Default.Track;
            App.CustomListener.PropertyChanged += _customListener_PropertyChanged;

        }

        private void _customListener_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Hidden) return;
            if (e.PropertyName == "Position")
                pointer.Dispatcher.Invoke(new Update(UpdateUI), ((CustomListener)sender).Position);
        }

        private void UpdateUI(Point point)
        {
            Canvas.SetLeft(pointer, (this.ActualWidth / 2 + 120) + (point.X * 3));
            Canvas.SetTop(pointer, (this.ActualHeight / 2) + (point.Y * 3));
            App.SetCursorPos((int)((this.ActualWidth / 2 + 100) + (point.X * 3)), (int)((this.ActualHeight / 2) + (point.Y * 3)));
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
                this.Hide();
                this.Close();
                Application.Current.MainWindow.Visibility = Visibility.Visible ;
            }            
        }  
    }
}
