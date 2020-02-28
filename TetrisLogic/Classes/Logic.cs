using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using TetrisInterfaces;
using TetrisInterfaces.Enum;
using TetrisLogic.Figures;

namespace TetrisLogic.Classes
{
    public static class Logic
    {
        public static void DelTopFreeLinesAndCenter(int[,] body)
        {
            int num = Initializer.SizeFigureField;
            for (int i = 0; i < Initializer.NumberOfFigurePoint; i++)
            {
                if (body[i, 1] < num )
                {
                    num = body[i, 1];
                }              
            }
            for (int i = 0; i < Initializer.NumberOfFigurePoint; i++)
            {                
                body[i, 1] -= num;
                body[i, 0] += 3;
            }
        }

        public static bool CheckFreeArea(Figure currFig, BoardPoint[,] field)
        {
            bool result = true;
            int[,] body = currFig.Body;
            for (int i = 0; i < body.GetLength(0); i++)
            {
                int x = body[i, 0];
                int y = body[i, 1];
                if (field[x, y] != null)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static void GetDeltaByDirection(Direction direction, out int dx, out int dy)
        {
            dx = 0;
            dy = 0;
            switch (direction)
            {
                case Direction.Down:
                    dy = 1;
                    break;
                case Direction.Left:
                    dx = -1;
                    break;
                case Direction.Right:
                    dx = 1;
                    break;
            }
        }

        public static int[,] GetCoordTurnedFigure(int[,] body, BoardPoint[,] field, int correction)
        {
            // determining of square of the current figure
            DeterminCoordSquareFig(body, out int xMin, out int xMax, out int yMin, out int yMax);

            // determining of of the rotation center
            byte x0 = (byte)((byte)((xMin + xMax) / 2) + correction),
                y0 = (byte)((yMin + yMax) / 2);

            // _corr - correction factor for the figure does not move when turning left or right           

            int[,] rotatedFig = new int[Initializer.NumberOfFigurePoint, 2];
            for (int i = 0; i < Initializer.NumberOfFigurePoint; i++)
            {
                //The reduction of the rotation center to the 0 point of coordinates
                int x = body[i, 0] - x0;
                int y = body[i, 1] - y0;

                //point rotation  around 0 by 90 degree. x1= x*cos(90) - y*sin(90), y1= x*sin(90) + y*cos(90); x1 = -y, y1 = x 

                //shift the center of rotation backwards
                int x1 = -y + x0;
                int y1 = x + y0;
                rotatedFig[i, 0] = x1;
                rotatedFig[i, 1] = y1;
            }
            if (!CheckAllowToTurn(rotatedFig, field))
            {
                rotatedFig = null;
            }
            return rotatedFig;
        }

        private static void DeterminCoordSquareFig(int[,] fig, out int xMin, out int xMax, out int yMin, out int yMax)
        {
            // array of coordinates of the current figure
            // determining of square of the current figure
            xMin = int.MaxValue;
            yMin = int.MaxValue;
            xMax = int.MinValue;
            yMax = int.MinValue;

            for (int i = 0; i < 4; i++)
            {
                int x = fig[i, 0];
                int y = fig[i, 1];
                if (x < xMin)
                {
                    xMin = x;
                }
                if (x > xMax)
                {
                    xMax = x;
                }
                if (y < yMin)
                {
                    yMin = y;
                }
                if (y > yMax)
                {
                    yMax = y;
                }
            }
        }

        private static bool CheckAllowToTurn(int[,] rotatedFig, BoardPoint[,] field)
        {
            bool permition = true;
            for (int i = 0; i < Initializer.NumberOfFigurePoint; i++)
            {
                int x = rotatedFig[i, 0];
                int y = rotatedFig[i, 1];
                if (x > 9 || y > 19 || x < 0 || y < 0 || field[x, y] != null)
                {
                    permition = false;
                    break;
                }
            }            
            return permition;
        }

        public static List<Point> GetBoundaryFigurePoints(int[,] body, Direction dir)
        {
            List<Point> points = new List<Point>();

            Func<int, int, List<Point>, bool> condition1 = null;
            Func<int, int, List<Point>, bool> condition2 = null;


            switch (dir)
            {
                case Direction.Down:
                    condition1 = (k, index, p) => (k == p[index].X);
                    condition2 = (k, index, p) => (k > p[index].Y);
                    break;
                case Direction.Left:
                    condition1 = (k, index, p) => (k == p[index].Y);
                    condition2 = (k, index, p) => (k < p[index].X);
                    break;
                case Direction.Right:
                    condition1 = (k, index, p) => (k == p[index].Y);
                    condition2 = (k, index, p) => (k > p[index].X);
                    break;
            }

            for (int i = 0; i < body.GetLength(0); i++)
            {
                int x = body[i, 0];
                int y = body[i, 1];
                int k1;
                int k2;
                if (dir == Direction.Down)
                {
                    k1 = x;
                    k2 = y;
                }
                else
                {
                    k1 = y;
                    k2 = x;
                }
                if (points.Count == 0)
                {
                    points.Add(new Point(x, y));
                }
                else
                {
                    bool findFitColumn = false;
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (condition1 != null && condition1(k1, j, points))
                        {
                            findFitColumn = true;
                            if (condition2(k2, j, points))
                            {
                                points[j] = new Point(x, y);
                            }
                        }
                    }

                    if (!findFitColumn)
                    {
                        points.Add(new Point(x, y));
                    }
                }
            }
            return points;
        }

        public static bool SaveGame(TetrisGameBoard board, SqlConnectionStringBuilder conn)
        {
            bool result;
            using (var connection = new TetrisConnection(conn))
            {
                result = connection.SaveGamePoint(board);
            }                     
            return result;
        }
    }
}
