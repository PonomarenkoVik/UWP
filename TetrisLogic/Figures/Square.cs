using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces;
using TetrisLogic.Classes;

namespace TetrisLogic.Figures
{
    internal sealed class Square: Figure
    {
        public Square(TColor color, int[,] body, TetrisGameBoard board) : base(color, body, board)
        {
        }

        public override string ToString()
        {
            return "Square";
        }

        public override FiguresTypes GetFigureType()
        {
            return FiguresTypes.Square;
        }
    }
}
