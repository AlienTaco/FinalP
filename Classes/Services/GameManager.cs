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
        private readonly Grid gameGrid;
        private readonly int rows;
        private readonly int columns;
        private bool[,] occupiedCells;
        

        public List<GameObject> GameObjects { get; } = new List<GameObject>();
        public bool IsBuildingMode { get; private set; } = true;
        public bool TeamSelected { get; private set; }
        public TeamColor CurrentTeam { get; private set; } = TeamColor.Blue;


        public GameManager(Grid grid, int rows, int cols)
        {
            this.gameGrid = grid;
            this.rows = rows;
            this.columns = cols;
            this.occupiedCells = new bool[rows, cols];
        }

        public void HandleGridCellTapped(int row, int col, Border cellBorder)
        {
            if (occupiedCells[row, col]) return;

            occupiedCells[row, col] = true;

            cellBorder.Background = CurrentTeam == TeamColor.Blue
                ? new SolidColorBrush(Windows.UI.Colors.Blue)
                : new SolidColorBrush(Windows.UI.Colors.Red);
        }

        public void SetTeam(TeamColor team)
        {
            CurrentTeam = team;
            TeamSelected = true;

        }

        public void ExitBuildingMode()
        {
            IsBuildingMode = false;
            GameObjects.Clear();
            bool[,] processed = new bool[rows, columns];

            // Define ship limits and tracking
            Dictionary<int, int> ShipLimits = new Dictionary<int, int>
    {
        {4, 1}, // One 4-block ship
        {3, 2}, // Two 3-block ships
        {2, 3}, // Three 2-block ships
        {1, 4}  // Four 1-block ships
    };
            Dictionary<int, int> placedShips = new Dictionary<int, int>
    {
        {4, 0},
        {3, 0},
        {2, 0},
        {1, 0}
    };

            bool CanPlaceShip(int size) => placedShips[size] < ShipLimits[size];

            // Clear cell backgrounds
            foreach (Border cell in gameGrid.Children)
                cell.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (!occupiedCells[r, c] || processed[r, c])
                        continue;

                    // Horizontal ships
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
                    if (horLen > 1 && CanPlaceShip(horLen))
                    {
                        var ship = new Ship(horCells, "Horizontal", CurrentTeam);
                        GameObjects.Add(ship);
                        placedShips[horLen]++;
                        foreach (var cell in horCells)
                            processed[cell.Item1, cell.Item2] = true;
                        continue;
                    }

                    // Vertical ships
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
                    if (vertLen > 1 && CanPlaceShip(vertLen))
                    {
                        var ship = new Ship(vertCells, "Vertical", CurrentTeam);
                        GameObjects.Add(ship);
                        placedShips[vertLen]++;
                        foreach (var cell in vertCells)
                            processed[cell.Item1, cell.Item2] = true;
                        continue;
                    }

                    // Single cube ship
                    if (CanPlaceShip(1))
                    {
                        var singleShip = new Ship(new List<(int, int)> { (r, c) }, "None", CurrentTeam);
                        GameObjects.Add(singleShip);
                        placedShips[1]++;
                    }
                    processed[r, c] = true;
                }
            }

            // Draw Ships (mark cells yellow with black border)
            foreach (var ship in GameObjects.OfType<Ship>())
            {
                ship.DrawOnGrid(gameGrid);
            }
        }
        private static readonly Dictionary<int, int> ShipLimits = new Dictionary<int, int>
{
    {4, 1}, // Only one 4-block ship allowed
    {3, 2}, // Two 3-block ships allowed
    {2, 3}, // Three 2-block ships allowed
    {1, 4}  // Four 1-block ships allowed
};

        private Dictionary<int, int> placedShips = new Dictionary<int, int> // size -> count
{
    {4, 0},
    {3, 0},
    {2, 0},
    {1, 0}
};


        public void EndBuildingMode()
        {
            if (IsBuildingMode == false)
                return;
            ExitBuildingMode();
        }





    }
}



