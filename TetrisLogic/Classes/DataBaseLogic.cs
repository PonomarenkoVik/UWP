using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces;
using TetrisLogic.Figures;

namespace TetrisLogic.Classes
{
    public static class DataBaseLogic
    {

        public static Figure CreateFigure(int[,] bodyCurrF, TColor colorCurrFig, FiguresTypes idCurrFigType, TetrisGameBoard board)
        {
            Figure fig = null;
            switch (idCurrFigType)
            {
                case FiguresTypes.LeftG:
                    fig = new LeftG(colorCurrFig, bodyCurrF, board);
                    break;
                case FiguresTypes.LeftZigzag:
                    fig = new LeftZigzag(colorCurrFig, bodyCurrF, board);
                    break;
                case FiguresTypes.LetterT:
                    fig = new LetterT(colorCurrFig, bodyCurrF, board);
                    break;
                case FiguresTypes.RightG:
                    fig = new RightG(colorCurrFig, bodyCurrF, board);
                    break;
                case FiguresTypes.RightZigzag:
                    fig = new RightZigzag(colorCurrFig, bodyCurrF, board);
                    break;
                case FiguresTypes.Square:
                    fig = new Square(colorCurrFig, bodyCurrF, board);
                    break;
                case FiguresTypes.Stick:
                    fig = new Stick(colorCurrFig, bodyCurrF, board);
                    break;
            }
            return fig;
        }

    }
}
