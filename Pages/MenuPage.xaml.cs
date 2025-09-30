using FinalP.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FinalP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        public MenuPage()
        {
            this.InitializeComponent();
        }

        private void START_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        private void OPTIONS_Click(object sender, RoutedEventArgs e)
        {
                Grid.Visibility = Visibility.Visible;
        }

        private void CLOSE_Click(object sender, RoutedEventArgs e)
        {
            Grid.Visibility = Visibility.Collapsed;
        }

        private void CLOSE1_Click(object sender, RoutedEventArgs e)
        {
            Grid1.Visibility = Visibility.Collapsed;
        }

        private void HELPBTN_Click(object sender, RoutedEventArgs e)
        {
            Grid1.Visibility = Visibility.Visible;
        }
    }
}
