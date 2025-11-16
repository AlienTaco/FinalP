using FinalP.Classes.Services;
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
    public abstract class GameObject
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public TeamColor Owner { get; set; }

        protected GameObject(int row, int column, TeamColor owner)
        {
            Row = row;
            Column = column;
            Owner = owner;
        }

        public abstract void Draw(Canvas gameCanvas, double cellWidth, double cellHeight);
    }
}
