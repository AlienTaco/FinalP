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
        private bool[,] occupiedCells;

        public List<GameObject> GameObjects { get; } = new List<GameObject>();
        public bool IsBuildingMode { get; private set; } = true;

        public GameManager(Canvas canvas, int cols, int rows)
        {
            this.gameCanvas = canvas;
            this.columns = cols;
            this.rows = rows;
            this.currentTeam = TeamColor.Blue;
            this.occupiedCells = new bool[rows, columns];
            this.gameCanvas.Loaded += GameCanvas_Loaded;
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.cellWidth = gameCanvas.ActualWidth / columns;
            this.cellHeight = gameCanvas.ActualHeight / rows;
        }

        public void HandlePointerPressed(Windows.Foundation.Point position)
        {
            int col = (int)(position.X / cellWidth);
            int row = (int)(position.Y / cellHeight);

            DrawSquare(row, col);
        }

        private void DrawSquare(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= columns)
                return;

            if (occupiedCells[row, col])
                return; // Skip if already occupied

            occupiedCells[row, col] = true;

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

        public void ExitBuildingMode()
        {
            IsBuildingMode = false;
            GameObjects.Clear();
            bool[,] processed = new bool[rows, columns];

            // Clear all existing visuals first
            gameCanvas.Children.Clear();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (!occupiedCells[r, c] || processed[r, c])
                        continue;

                    // Horizontal check
                    int horLen = 1;
                    List<(int, int)> horCells = new List<(int, int)> { (r, c) };
                    for (int cc = c + 1; cc < c + 4 && cc < columns; cc++)
                    {
                        if (occupiedCells[r, cc] && !processed[r, cc])
                        {
                            horCells.Add((r, cc));
                            horLen++;
                        }
                        else break;
                    }
                    if (horLen > 1)
                    {
                        var ship = new Ship(horCells, "Horizontal", currentTeam);
                        GameObjects.Add(ship);
                        foreach (var cell in horCells)
                            processed[cell.Item1, cell.Item2] = true;
                        continue;
                    }

                    // Vertical check
                    int vertLen = 1;
                    List<(int, int)> vertCells = new List<(int, int)> { (r, c) };
                    for (int rr = r + 1; rr < r + 4 && rr < rows; rr++)
                    {
                        if (occupiedCells[rr, c] && !processed[rr, c])
                        {
                            vertCells.Add((rr, c));
                            vertLen++;
                        }
                        else break;
                    }
                    if (vertLen > 1)
                    {
                        var ship = new Ship(vertCells, "Vertical", currentTeam);
                        GameObjects.Add(ship);
                        foreach (var cell in vertCells)
                            processed[cell.Item1, cell.Item2] = true;
                        continue;
                    }

                    // Single cube ship
                    var singleShip = new Ship(new List<(int, int)> { (r, c) }, "None", currentTeam);
                    GameObjects.Add(singleShip);
                    processed[r, c] = true;
                }
            }

            // Draw all ships
            foreach (var obj in GameObjects)
            {
                obj.Draw(gameCanvas, cellWidth, cellHeight);
            }
        }

    }
}


