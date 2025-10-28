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

namespace FinalP.Classes.Services
{
    public class GameManager
    {
        private readonly Canvas Warboard;
        private readonly List<GameObject> objects =new List <GameObject>();

        public int Rows { get; } = 5;
        public int Columns { get; } = 7;
        public double CellSize { get; } = 50;

        public GameManager(Canvas canvas)
        {
            Warboard = canvas;
        }

        public void InitializeBoard()
        {
            Warboard.Children.Clear();
            objects.Clear();

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    double x = col * CellSize;
                    double y = row * CellSize;

                    var cell = new GameObject(x, y)
                    {
                        Size = CellSize
                    };

                    objects.Add(cell);
                    cell.CreateVisual(Warboard);
                }
            }
        }

        public void ClearBoard()
        {
            Warboard.Children.Clear();
            objects.Clear();
        }
    }
}
     
