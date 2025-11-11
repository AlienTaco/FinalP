using FinalP.Classes.Services;
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

namespace FinalP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private GameManager gameManager;

        public GamePage()
        {
            this.InitializeComponent();

            gameManager = new GameManager(Warboard, 7, 5);

            Warboard.PointerPressed += Warboard_PointerPressed;
        }

        private void Warboard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pos = e.GetCurrentPoint(Warboard).Position;
            gameManager.HandlePointerPressed(pos);
        }

        private void SelectBlueTeam()
        {
            gameManager.SetTeam(TeamColor.Blue);
        }

        private void SelectRedTeam()
        {
            gameManager.SetTeam(TeamColor.Red);
        }


        private void RETURN_Click(object sender, RoutedEventArgs e)
        {
           Frame.Navigate(typeof(MenuPage));
        }

        private void ChooseBlue_Click(object sender, RoutedEventArgs e)
        {
            gameManager.SetTeam(TeamColor.Blue);
            TeamChoiceOverlay.Visibility = Visibility.Collapsed;
        }

        private void ChooseRed_Click(object sender, RoutedEventArgs e)
        {
            gameManager.SetTeam(TeamColor.Red);
            TeamChoiceOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
