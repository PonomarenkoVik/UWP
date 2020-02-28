using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces;
using TetrisLogic.Classes;
using TetrisLogic.Interfaces;

namespace TetrisLogic.Figures
{
    internal sealed class LetterT : Figure, IRotatable
    {
        public LetterT(TColor color, int[,] body, TetrisGameBoard board) : base(color, body, board)
        {
        }

        public override string ToString()
        {
            return "LetterT";
        }
        public bool Turn()
        {
            return base.TurnFigure();
        }

        public override FiguresTypes GetFigureType()
        {
            return FiguresTypes.LetterT;
        }
    }
}
