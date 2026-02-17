using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Player
    {
        public string PlayerName { get; set; }
        public string Message { get; set; }
        public List<Ship> Ships { get; set; } = new List<Ship> ();
        public List<Cell> Strikes { get; set; } = new List<Cell> ();
    }
}
