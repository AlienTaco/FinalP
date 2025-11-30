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
        private const int Rows = 10;
        private const int Cols = 10;
        private DispatcherTimer buildTimer;
        private int buildSecondsRemaining = 60;
        

        public GamePage()
        {
            this.InitializeComponent();

            Exit_Build.IsEnabled = false;

            InitializeGrid(PlayerGrid, Rows, Cols);
            InitializeGrid(OpponentGrid, Rows, Cols);

            gameManager = new GameManager(PlayerGrid, Rows, Cols);

            // PlayerGrid cells tap handling
            foreach (Border cell in PlayerGrid.Children)
            {
                cell.Tapped += PlayerGrid_CellTapped;
            }

            Exit_Build.Click += Exit_Build_Click;

            buildSecondsRemaining = 60; // or any duration
            BuildTimerText.Text = $"Time: {buildSecondsRemaining}";
            
        }


        private void InitializeGrid(Grid grid, int rows, int cols)
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();

            for (int r = 0; r < rows; r++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int c = 0; c < cols; c++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var cell = new Border
                    {
                        Background = new SolidColorBrush(Windows.UI.Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black),
                        BorderThickness = new Thickness(1),
                        Tag = (r, c)
                    };
                    Grid.SetRow(cell, r);
                    Grid.SetColumn(cell, c);
                    grid.Children.Add(cell);
                }
            }
        }

        private void PlayerGrid_CellTapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gameManager.IsBuildingMode) return;


            if (!gameManager.TeamSelected)  // add a bool in GameManager if you want
                return;

            var border = sender as Border;
            var tuple = ((int, int))border.Tag;
            int row = tuple.Item1;
            int col = tuple.Item2;
            gameManager.HandleGridCellTapped(row, col, border);
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
            Exit_Build.IsEnabled = true;
            StartBuildTimer();
        }

        private void ChooseRed_Click(object sender, RoutedEventArgs e)
        {
            gameManager.SetTeam(TeamColor.Red);
            TeamChoiceOverlay.Visibility = Visibility.Collapsed;
            Exit_Build.IsEnabled = true;
            StartBuildTimer();
        }

        private void Exit_Build_Click(object sender, RoutedEventArgs e)
        {
            gameManager.ExitBuildingMode();

            // Optionally disable Exit_Build button or building UI here
            Exit_Build.IsEnabled = false;
            buildTimer?.Stop();
            gameManager.ExitBuildingMode();
            Exit_Build.IsEnabled = false;
        }

        private void StartBuildTimer()
        {
            buildTimer = new DispatcherTimer();
            buildTimer.Interval = TimeSpan.FromSeconds(1);
            buildTimer.Tick += BuildTimer_Tick;
            buildTimer.Start();
        }



        private void BuildTimer_Tick(object sender, object e)
        {
            if (buildSecondsRemaining > 0)
            {
                buildSecondsRemaining--;
                BuildTimerText.Text = $"Time: {buildSecondsRemaining}";
            }
            else
            {
                buildTimer.Stop();
                Exit_Build.IsEnabled = false;
                gameManager.EndBuildingMode();
            }
        }



        

    }
}
