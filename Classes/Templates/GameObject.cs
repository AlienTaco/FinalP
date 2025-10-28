using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FinalP.Classes.Templates
{
    public abstract class GameObject
    { 
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; } = 50;
        public Color Color { get; set; } = Colors.LightBlue;

        private Rectangle visual;

        public GameObject(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Creates and draws the rectangle on the Canvas
        public virtual void CreateVisual(Canvas canvas)
        {
            visual = new Rectangle
            {
                Width = Size,
                Height = Size,
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Color)
            };

            Canvas.SetLeft(visual, X);
            Canvas.SetTop(visual, Y);

            canvas.Children.Add(visual);
        }
    }
}
