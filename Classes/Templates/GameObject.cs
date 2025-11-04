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

namespace FinalP.Classes.Templates
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public class GameObject
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Length { get; private set; } // Ship size from 2 to 4
        public Orientation Rotation { get; private set; }

        public double CellSize { get; set; } = 50;
        public Rectangle VisualElement { get; private set; }
        private Canvas parentCanvas;

        public GameObject(int row, int column, int length, Orientation rotation, Canvas canvas)
        {
            Row = row;
            Column = column;
            Length = length;
            Rotation = rotation;
            parentCanvas = canvas;
            CreateVisual();
        }

        private void CreateVisual()
        {
            // Rectangle width depends on orientation and length
            double width = Rotation == Orientation.Horizontal ? Length * CellSize : CellSize;
            double height = Rotation == Orientation.Vertical ? Length * CellSize : CellSize;

            VisualElement = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(Colors.Yellow),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2,
                RadiusX = 5,
                RadiusY = 5
            };

            PlaceOnCanvas();
            parentCanvas.Children.Add(VisualElement);
        }

        private void PlaceOnCanvas()
        {
            double x = Column * CellSize;
            double y = Row * CellSize;
            Canvas.SetLeft(VisualElement, x);
            Canvas.SetTop(VisualElement, y);
        }

        // For updating position or other dynamic changes
        public void UpdatePosition(int newRow, int newColumn)
        {
            Row = newRow;
            Column = newColumn;
            PlaceOnCanvas();
        }
    }

}
