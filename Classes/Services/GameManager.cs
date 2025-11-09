using FinalP.Classes.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


namespace FinalP.Classes.Services
{
    public enum TeamColor
    {
        Blue,
        Red
    }

    public class GameManager
    {
        private Canvas gameCanvas;
        private int rows;
        private int columns;
        private double cellWidth;
        private double cellHeight;
        private TeamColor currentTeam;

        // Constructor
        public GameManager(Canvas canvas, int cols, int rows)
        {
            this.gameCanvas = canvas;
            this.columns = cols;
            this.rows = rows;
            this.currentTeam = TeamColor.Blue;
            this.gameCanvas.Loaded += GameCanvas_Loaded;
        }

        // Loaded Event Handler
        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.cellWidth = gameCanvas.ActualWidth / columns;
            this.cellHeight = gameCanvas.ActualHeight / rows;
        }


        // Call this when pointer is pressed, passing the pointer position on canvas
        public void HandlePointerPressed(Windows.Foundation.Point position)
        {
            int col = (int)(position.X / cellWidth);
            int row = (int)(position.Y / cellHeight);

            DrawSquare(row, col);
        }

        private void DrawSquare(int row, int col)
        {
            var rect = new Rectangle
            {
                Width = cellWidth - 1,
                Height = cellHeight - 1,
                Fill = currentTeam == TeamColor.Blue
                    ? new SolidColorBrush(Colors.Blue)
                    : new SolidColorBrush(Colors.Red),
                Opacity = 0.8
            };

            Canvas.SetLeft(rect, col * cellWidth);
            Canvas.SetTop(rect, row * cellHeight);
            gameCanvas.Children.Add(rect);
        }

        public void SetTeam(TeamColor team)
        {
            currentTeam = team;
        }
    }
}
     
