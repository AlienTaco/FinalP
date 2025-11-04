using FinalP.Classes.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Orientation = FinalP.Classes.Templates.Orientation;

namespace FinalP.Classes.Services
{
    public class GameManager
    {
        private readonly Canvas gameCanvas;
        private readonly Random random = new Random();

        public int Rows { get; } = 25;
        public int Columns { get; } = 25;
        public double CellSize { get; } = 50;

        private readonly List<GameObject> gameShips = new List<GameObject>();

        public GameManager(Canvas canvas)
        {
            gameCanvas = canvas;
        }







        public void InitializeBoardWithRandomShips(int numberOfShips)
        {
            gameCanvas.Children.Clear();
            gameShips.Clear();

            int attempts = 0;
            while (gameShips.Count < numberOfShips && attempts < 1000)
            {
                attempts++;

                // Random size between 2 and 4
                int length = random.Next(2, 5);

                // Random orientation
                var rot = (Orientation)random.Next(0, 2); // 0=Horizontal,1=Vertical

                // Calculate max starting point based on size and orientation
                int maxStartRow = rot == Orientation.Vertical ? Rows - length : Rows - 1;
                int maxStartCol = rot == Orientation.Horizontal ? Columns - length : Columns - 1;

                int startRow = random.Next(2, maxStartRow + 1);
                int startCol = random.Next(2, maxStartCol + 1);

                // Check for overlap with existing ships
                if (!IsOverlapping(startRow, startCol, length, rot))
                {
                    var ship = new GameObject(startRow, startCol, length, rot, gameCanvas);
                    gameShips.Add(ship);
                }
            }
        }




        private bool IsOverlapping(int row, int col, int length, Orientation rot)
        {
            foreach (var ship in gameShips)
            {
                for (int i = 0; i < length; i++)
                {
                    int checkRow = rot == Orientation.Vertical ? row + i : row;
                    int checkCol = rot == Orientation.Horizontal ? col + i : col;

                    for (int j = 0; j < ship.Length; j++)
                    {
                        int shipRow = ship.Rotation == Orientation.Vertical ? ship.Row + j : ship.Row;
                        int shipCol = ship.Rotation == Orientation.Horizontal ? ship.Column + j : ship.Column;

                        if (checkRow == shipRow && checkCol == shipCol)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void DrawGridOnCanvas(Canvas canvas, int rows, int columns, double cellSize)
        {
            double width = columns * cellSize;
            double height = rows * cellSize;

            // Clear existing lines if any
            canvas.Children.Clear();

            // Draw vertical lines
            for (int col = 0; col <= columns; col++)
            {
                var line = new Windows.UI.Xaml.Shapes.Line
                {
                    X1 = col * cellSize,
                    Y1 = 0,
                    X2 = col * cellSize,
                    Y2 = height,
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Black),
                    StrokeThickness = 1
                };
                canvas.Children.Add(line);
            }

            // Draw horizontal lines
            for (int row = 0; row <= rows; row++)
            {
                var line = new Windows.UI.Xaml.Shapes.Line
                {
                    X1 = 0,
                    Y1 = row * cellSize,
                    X2 = width,
                    Y2 = row * cellSize,
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Black),
                    StrokeThickness = 1
                };
                canvas.Children.Add(line);
            }
        }
    }
}
     
