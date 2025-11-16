using FinalP.Classes.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FinalP.Classes.Templates
{
    public class Ship : GameObject
    {
        public List<(int row, int col)> Cells { get; set; }
        public string Orientation { get; set; }

        public Ship(List<(int row, int col)> cells, string orientation, TeamColor owner)
            : base(cells[0].row, cells[0].col, owner)
        {
            Cells = cells;
            Orientation = orientation;
        }

        public override void Draw(Canvas gameCanvas, double cellWidth, double cellHeight)
        {
            foreach (var cell in Cells)
            {
                var rect = new Windows.UI.Xaml.Shapes.Rectangle
                {
                    Width = cellWidth - 1,
                    Height = cellHeight - 1,
                    Fill = new SolidColorBrush(Windows.UI.Colors.Yellow),
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Black),
                    StrokeThickness = 2,
                    Opacity = 1.0 // full opacity
                };

                Canvas.SetLeft(rect, cell.col * cellWidth);
                Canvas.SetTop(rect, cell.row * cellHeight);
                gameCanvas.Children.Add(rect);
            }
        }
    }
}
