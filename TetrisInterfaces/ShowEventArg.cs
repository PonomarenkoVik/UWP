using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces.Enum;

namespace TetrisInterfaces
{
    public class ShowEventArg
    {
        public ShowEventArg( BoardPoint[,] board, BoardPoint[,] nextFigure, int level, int burnedLine, int score)
        {         
            Board = board;
            NextFigure = nextFigure;
            Level = level;
            BurnedLine = burnedLine;
            Score = score;
        }
        public BoardPoint[,] Board { get; }
        public BoardPoint[,] NextFigure { get; }
        public int Level { get; }
        public int BurnedLine { get; }
        public int Score { get; }
    }
}
