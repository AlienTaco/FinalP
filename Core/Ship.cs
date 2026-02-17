using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Ship
    {
        public string ShipName { get; set; }
        public List<Cell> Cells { get; set; } = new List<Cell>();
    }
}
