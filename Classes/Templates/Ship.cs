using FinalP.Classes.Services;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FinalP.Classes.Templates
{
    public class Ship : GameObject
    {
        public List<(int row, int col)> Cells { get; set; }
        public string Orientation { get; set; }

        public Ship(List<(int, int)> cells, string orientation, TeamColor owner)
           : base(cells[0].Item1, cells[0].Item2, owner)
        {
            Cells = cells;
            Orientation = orientation;
        }

        // New method to color Grid cells
        public void DrawOnGrid(Grid grid)
        {
            foreach (var cell in Cells)
            {
                var border = grid.Children.Cast<Border>().First(b =>
                    Grid.GetRow(b) == cell.row && Grid.GetColumn(b) == cell.col);

                border.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                border.BorderThickness = new Thickness(0);
            }
        }

        public override void Draw(Canvas gameCanvas, double cellWidth, double cellHeight)
        {
            // Unused now, you can remove or keep for backward compatibility
        }
    }
}
