using System;
using TetrisInterfaces;
using TetrisLogic.Figures;

namespace TetrisLogic.Classes
{
    public static class Initializer
    {
        public const int SizeFigureField = 4;
        public const int NumberOfColors = 6;
        public const int LimitScore = 2000;
        public const int NumberOfFigurePoint = 4;
        public const int WidthHeighFigureField = 4;
        public const int NumberOfLevels = 10;
        public const int NumberOfFigures = 7;

        public static float GetVelocityByLevel(int lvl)
        {
            return LevelVelocity[lvl];
        }

        #region Data for game levels


        //velocity of moving of the figure on each of ten level
        private static readonly float[] LevelVelocity = { 1, 1, 1.2f, 1.2f, 1.5f, 1.71f, 2, 2.4f, 3, 6 };


        private static readonly byte[][,] FillingOfLevels ={
            null,                                 //Level 1
            null,                                 //Level 2
            null,                                 //Level 3
            new byte[,] {{1,1,0,1,1,0,0,1,1,1}},  //Level 4
 
            new byte[,] {
                {1,1,0,1,1,0,0,1,1,1},   //Level 5
                {0,1,1,1,0,1,1,1,0,1}},

            new byte[,] {
                {1,1,1,1,1,0,0,1,1,1},   //Level 6
                {0,1,0,1,0,1,1,1,1,1},
                {0,1,0,1,1,1,0,0,1,0}},

            new byte[,] {
                {1,1,0,1,1,0,0,1,1,1},   //Level 7
                {0,1,0,1,0,1,0,0,0,1},
                {0,1,0,1,0,1,1,1,1,1}},

            new byte[,] {
                {1,1,0,1,1,0,0,1,1,1},   //Level 8
                {1,1,1,0,1,0,1,1,0,1},
                {1,1,0,0,1,0,1,0,1,1},
                {1,0,1,1,0,0,0,1,1,1}},

            new byte[,] {
                {1,1,0,1,1,0,1,1,1,1},   //Level 9
                {0,1,1,1,1,0,1,0,1,0},
                {0,1,0,0,1,0,1,0,1,1},
                {1,1,1,1,1,0,1,0,0,1}},

            new byte[,] {
                {1,0,0,1,1,0,1,1,1,1},   //Level 10
                {0,0,1,1,1,0,1,0,1,0},
                {0,0,1,0,1,0,1,0,1,1},
                {1,0,1,1,1,0,1,0,0,1}}
        };


        #endregion

        public static Figure GetFigure(TetrisGameBoard board)
        {
            Figure fig;
            int choseFigure = Rnd.Next(0, NumberOfFigures);
            TColor color = (TColor)Rnd.Next(1, NumberOfColors + 1);
            int[,] body;
            switch (choseFigure)
            {
                case 0:
                    body = new int[,]{{1, 0},{2, 0},{2, 1},{2, 2}};
                    fig = new LeftG(color, (int[,])body.Clone(), board);
                    break;
                case 1:
                    body = new int[,] {{2, 0}, {2, 1}, {1, 2}, {2, 2}};
                    fig = new RightG(color, (int[,])body.Clone(), board);
                    break;
                case 2:
                    body = new int[,] {{1, 0}, {1, 1}, {2, 1}, {2, 2}};
                    fig = new LeftZigzag(color, (int[,])body.Clone(), board);
                    break;
                case 3:
                    body = new int[,] {{2, 0}, {1, 1}, {2, 1}, {1, 2}};
                    fig = new RightZigzag(color, (int[,])body.Clone(), board);
                    break;
                case 4:
                    body = new int[,] {{1, 1}, {2, 1}, {3, 1}, {2, 2}};
                    fig = new LetterT(color, (int[,])body.Clone(), board);
                    break;
                case 5:
                    body = new int[,] {{1, 1}, {2, 1}, {1, 2}, {2, 2}};
                    fig = new Square(color, (int[,])body.Clone(), board);
                    break;
                case 6:
                    body = new int[,] {{2, 0}, {2, 1}, {2, 2}, {2, 3}};
                    fig = new Stick(color, (int[,])body.Clone(), board);
                    break;
                default:
                    body = new int[,] {{1, 1}, {2, 1}, {1, 2}, {2, 2}};
                    fig = new Square(color, (int[,])body.Clone(), board);
                    break;                    
            }
            return fig;
        }

        public static void FillBoardFieldByLevel(BoardPoint[,] field, int level)
        {
            Random rnd = new Random();
            byte[,] fillLevel = FillingOfLevels[level];
            // index of array Levels number 0 fit Level 4 of the game
            if (fillLevel != null)
            {
                for (int i = fillLevel.GetLength(0) - 1; i >= 0; i--)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (fillLevel[i, j] == 1)
                        {
                            //entering the points of the figure into the Field of the structure board
                            field[j, 19 - i] = new BoardPoint((TColor)(rnd.Next(0, Initializer.NumberOfColors)));
                        }
                    }
                }
            }          
        }       


        private static readonly Random Rnd = new Random();
    }
}
