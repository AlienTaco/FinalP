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
        public double Width { get; set; } = 50;
        public double Height { get; set; } = 50;
        public bool IsActive { get; set; } = true;

        protected Rectangle Visual;

        public virtual void CreateVisual(Canvas canvas, Color color)
        {
            Visual = new Rectangle
            {
                Width = Width,
                Height = Height,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(Visual, X);
            Canvas.SetTop(Visual, Y);
            canvas.Children.Add(Visual);
        }

        public virtual void UpdatePosition()
        {
            if (Visual == null) return;
            Canvas.SetLeft(Visual, X);
            Canvas.SetTop(Visual, Y);
        }

        public virtual void Destroy(Canvas canvas)
        {
            if (Visual != null)
            {
                canvas.Children.Remove(Visual);
                Visual = null;
            }
        }
    }
}
