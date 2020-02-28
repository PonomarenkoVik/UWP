using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces;
using TetrisLogic.Figures;

namespace TetrisLogic.Classes
{
    public abstract class GameBoard
    {
        protected GameBoard(int width, int height, SqlConnectionStringBuilder conn)
        {
            Width = width;
            Height = height;
            _connStr = conn;          
            _level = 0;          
            _score = 0;
        }

        public int Width
        {
            get => _width;
            private set
            {
                if (value > 5 && value < 20)
                {
                    _width = value;
                }
                else
                {
                    throw new Exception("Not allowed range of gameboard size. Allowed size is (width = 5 - 20, height = 10 - 30)");
                }
            }
        }
        public int Height
        {
            get => _height;
            private set
            {
                if (value > 10 && value < 30)
                {
                    _height = value;
                }
                else
                {
                    throw new Exception("Not allowed range of gameboard size. Allowed size is (width = 5 - 20, height = 10 - 30)");
                }
            }
        }
        public int Level
        {
            get => _level;
            set => _level = value;
        }
        public int Score
        {
            get => _score;
            set => _score = value;
        }



      
        protected int _width;
        protected int _height;      
        protected int _level;      
        protected int _score;
        protected SqlConnectionStringBuilder _connStr;
    }
}
