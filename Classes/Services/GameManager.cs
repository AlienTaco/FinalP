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
        private readonly Canvas gameCanvas;
        private readonly List<GameObject> objects = new();

        public int Rows { get; } = 5;
        public int Columns { get; } = 7;
        public double CellSize { get; } = 50;

        public GameManager(Canvas canvas)
        {
            gameCanvas = canvas;
        }

        public void AddObject(GameObject obj)
        {
            objects.Add(obj);
            obj.CreateVisual(gameCanvas, Colors.Gray);
        }

        public void RemoveObject(GameObject obj)
        {
            obj.Destroy(gameCanvas);
            objects.Remove(obj);
        }

        public void ClearAll()
        {
            foreach (var obj in objects)
                obj.Destroy(gameCanvas);
            objects.Clear();
        }

        public (double x, double y) GetCellPosition(int row, int col)
        {
            return (col * CellSize, row * CellSize);
        }
    }
}
     
