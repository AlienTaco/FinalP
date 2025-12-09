using FinalP.Classes.Services;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FinalP.Classes.Templates
{
    public class Ship
    {
        public List<(int row, int col)> Cells { get; set; }
        public string Orientation { get; set; }
        public TeamColor Owner { get; set; }

        public Ship(List<(int row, int col)> cells, string orientation, TeamColor owner)
        {
            Cells = cells;
            Orientation = orientation;
            Owner = owner;
        }

        public void DrawOnGrid(Grid grid)
        {
            foreach (var cell in Cells)
            {
                var border = grid.Children.Cast<Border>().First(b =>
                    Grid.GetRow(b) == cell.row && Grid.GetColumn(b) == cell.col);

                border.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                border.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                border.BorderThickness = new Thickness(0);
            }
        }
    }
}
