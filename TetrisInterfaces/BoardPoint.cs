using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisInterfaces
{

    public sealed class BoardPoint
    {
        public BoardPoint(TColor color)
        {
            Col = color;
        }
        public TColor Col { get; set; } // point color (from 0  to Figure.NumberOfColors)
    }
}
