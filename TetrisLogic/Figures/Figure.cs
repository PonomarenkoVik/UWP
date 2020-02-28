using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces;
using TetrisInterfaces.Enum;
using TetrisLogic.Classes;

namespace TetrisLogic.Figures
{
    public abstract class Figure : ICloneable
    {
        protected Figure(TColor color, int[,] body, TetrisGameBoard board)
        {
            Color = color;
            _body = (int[,])body.Clone();
            Board = board;
            _correctionTurn = 0;
        }

        public TColor Color { get; }

        public int[,] Body => (int[,])_body.Clone();

        protected int CorrectionTurn
        {
            get
            {
                if (_correctionTurn == 0)
                {
                    _correctionTurn = 1;
                }
                else
                {
                    _correctionTurn = 0;
                }

                return _correctionTurn;
            }
        }

        public bool Move(Direction direction)
        {
            bool result = false;

            // correcting delta
            Logic.GetDeltaByDirection(direction, out int dx, out int dy);

            if (CheckPermissionToMove(direction))
            {
                for (int i = 0; i < _body.GetLength(0); i++)
                {
                    _body[i, 0] += dx;
                    _body[i, 1] += dy;
                    
                }
                result = true;
            }           
            return result;
        }

        protected bool TurnFigure()
        {
            bool result = true;
            int[,] turnedFigure = Logic.GetCoordTurnedFigure(_body, Board.Field, CorrectionTurn);
            if (turnedFigure != null)
            {
                _body = (int[,])turnedFigure.Clone();
            }
            else
            {
                result = false;
            }
            return result;
        }

        public void DeleteTopFreeLineAndCenter()
        {
            Logic.DelTopFreeLinesAndCenter(_body);
        }
       
        protected virtual bool CheckPermissionToMove(Direction dir)
        {
            bool result = true;


            List<Point> points = Logic.GetBoundaryFigurePoints(_body, dir);
            Logic.GetDeltaByDirection(dir, out int dx, out int dy);
            foreach (var point in points)
            {
                
                int x = point.X + dx;
                int y = point.Y + dy;                
                if (!CheckAllowedRegion(dir, x, y) || Board.Field[x, y] != null)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private  bool CheckAllowedRegion(Direction dir, int x, int y)
        {
            return x >= 0 && y >= 0 && x < Board.Width && y < Board.Height;
        }



        public virtual object Clone()
        {
            Figure fig = (Figure)this.MemberwiseClone();
            fig._body = Body;
            return fig;
        }

        public abstract FiguresTypes GetFigureType();

        protected TetrisGameBoard Board;
        private int[,] _body;
        private int _correctionTurn;
    }
}
