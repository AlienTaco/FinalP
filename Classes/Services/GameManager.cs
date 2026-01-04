using FinalP.Classes.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;
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

        public Client _client;
        public string PlayerName { get; set; }
        public List<Ship> PlayerShips { get; } = new List<Ship>();
        public List<Ship> EnemyShips { get; } = new List<Ship>();
        public bool IsBuildingMode { get; private set; } = true;
        public bool TeamSelected { get; private set; }
        public TeamColor CurrentTeam { get; private set; } = TeamColor.Blue;
        public bool IsBattleMode { get; private set; } = false;
        private readonly Random rng = new Random();
        public Action<string> OnShowMessage;


        public GameManager(Grid grid, int rows, int cols)
        {
            this.gameGrid = grid;
            this.rows = rows;
            this.columns = cols;
            this.occupiedCells = new bool[rows, cols];
        }


        public async Task Init(string serverIp, string playerName)
        {
            PlayerName = playerName;
            _client = new Client(serverIp, 1111);
            _client.Events.MessageReceived += MessageReceivedAsync;
            _client.Connect();
            await _client.SendAsync($"Register|{playerName}");

        }

        private async void MessageReceivedAsync(object sender, MessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Data);
            var parts = message.Split('|'); 
            switch (parts[0])
            {
                case "Register": OnShowMessage?.Invoke(parts[1]); break;
            }
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
            PlayerShips.Clear();

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
                        PlayerShips.Add(ship);
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
                        PlayerShips.Add(ship);
                        placedShips[vertLen]++;
                        foreach (var cell in vertCells)
                            processed[cell.Item1, cell.Item2] = true;
                        continue;
                    }

                    // Single cube ship
                    if (CanPlaceShip(1))
                    {
                        var singleShip = new Ship(new List<(int, int)> { (r, c) }, "None", CurrentTeam);
                        PlayerShips.Add(singleShip);
                        placedShips[1]++;
                    }
                    processed[r, c] = true;
                }
            }

            // Draw Ships (mark cells yellow with black border)
            foreach (var ship in PlayerShips)
                ship.DrawOnGrid(gameGrid);
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


        public void GenerateEnemyFleet(Grid enemyGrid)
        {
            EnemyShips.Clear();

            // Ship sizes: 1x4‑block, 2x3‑block, 3x2‑block, 4x1‑block
            int[] sizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

            foreach (int size in sizes)
                PlaceRandomEnemyShip(size);

            // Draw them (for testing)
           // foreach (var ship in EnemyShips)
            //    ship.DrawOnGrid(enemyGrid);
        }

        private void PlaceRandomEnemyShip(int size)
        {
            while (true)
            {
                bool vertical = rng.Next(2) == 0;

                int maxRow = vertical ? rows - size + 1 : rows;
                int maxCol = vertical ? columns : columns - size + 1;

                int startRow = rng.Next(0, maxRow);
                int startCol = rng.Next(0, maxCol);

                var cells = new List<(int row, int col)>();
                bool overlap = false;

                for (int i = 0; i < size; i++)
                {
                    int r = vertical ? startRow + i : startRow;
                    int c = vertical ? startCol : startCol + i;

                    // check overlap with existing enemy ships
                    foreach (var s in EnemyShips)
                    {
                        if (s.Cells.Any(cell => cell.row == r && cell.col == c))
                        {
                            overlap = true;
                            break;
                        }
                    }

                    if (overlap)
                        break;      // abort this candidate position

                    cells.Add((r, c)); // only add when we know there is no overlap
                }

                if (!overlap && cells.Count == size)
                {
                    var ship = new Ship(cells, vertical ? "Vertical" : "Horizontal", TeamColor.Red);
                    EnemyShips.Add(ship);
                    return;             // ship placed successfully
                }

                // otherwise: loop again and try a new random position
            }
        }
        public void StartBattleMode()
        {
            IsBuildingMode = false;
            IsBattleMode = true;
        }

        public void HandlePlayerShot(Grid enemyGrid, Border cellBorder, int row, int col)
        {
            // Don’t allow shooting same cell twice
            if (cellBorder.Background is SolidColorBrush brush &&
                (brush.Color == Windows.UI.Colors.Red || brush.Color == Windows.UI.Colors.Gray))
                return;

            // Check if any enemy ship occupies this cell
            var hitShip = EnemyShips.FirstOrDefault(ship =>
                ship.Cells.Any(cell => cell.row == row && cell.col == col));

            if (hitShip != null)
            {
                // HIT: mark red
                cellBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);

                // Optional: check if ship is sunk
                bool allHit = hitShip.Cells.All(cell =>
                {
                    var b = enemyGrid.Children.Cast<Border>().First(x =>
                        Grid.GetRow(x) == cell.row && Grid.GetColumn(x) == cell.col);

                    return (b.Background as SolidColorBrush)?.Color == Windows.UI.Colors.Red;
                });

                if (allHit)
                {
                    // e.g. outline sunk ship in dark red or log a message
                }
            }
            else
            {
                // MISS: mark gray
                cellBorder.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
            }
        }




    }
}



