using System;
using System.Data;
using System.Data.SqlClient;
using TetrisInterfaces;
using TetrisInterfaces.Enum;
using TetrisLogic.Figures;
using TetrisLogic.Interfaces;

namespace TetrisLogic.Classes
{
    

    public class TetrisGameBoard : GameBoard, ITetrisLogic
    {
        public TetrisGameBoard(int width, int height, SqlConnectionStringBuilder conn) : base(width, height, conn)
        {
            _field = new BoardPoint[_width, _height];
            _burnedLines = 0;
        }



        #region Properties

        internal BoardPoint[,] Field
        {
            get => (BoardPoint[,])_field.Clone();
            set => _field = (BoardPoint[,])value.Clone();
        }
       
        internal Figure CurrentFigure
        {
            get => (Figure) _currentFigure.Clone();
            set => _currentFigure = (Figure) value.Clone();
        }
        internal Figure NextFigure
        {
            get => (Figure)_nextFigure.Clone();
            set => _nextFigure = (Figure)value.Clone();
        }
       
        internal int BurnedLine
        {
            get => _burnedLines;
            set => _burnedLines = value;
        }
        
        public event SoundT SoundEvent;
        public event ShowT UpdateEvent;
        public event Action GameOverEvent;
        public event VelocChange VelocityChangeEvent;

       

        #endregion





        #region Control Methods

        public void Start()
        {          
            SetVelocity();
            if (_currentFigure == null)
            {
                _currentFigure = Initializer.GetFigure(this);
                _currentFigure.DeleteTopFreeLineAndCenter();
                _nextFigure = Initializer.GetFigure(this);
            }
            
            Update();
        }     
        
        public void Move(Direction dir)
        {          
                if (dir == Direction.Down)
                { 
                    if ( _currentFigure.Move(dir))
                    {
                        Update();
                    }
                    else
                    {
                        CopyCurrentFigureToField();
                                            
                        CheckAndBurnLine();
                        if (CheckScoreLevelUp())
                        {
                            LevelUp();
                        }
                        ExchangeFigures();
                        if (!Logic.CheckFreeArea(_currentFigure, Field))
                        {
                            _currentFigure = null;
                            Reset();
                            GameOverEvent?.Invoke();
                        }
                        else
                        {
                            Update();
                        }                      
                    }
                }
                else
                {
                    if (_currentFigure.Move(dir))
                    {
                        SoundEvent?.Invoke(this, new SoundEventArg(TSound.Stepping));
                        Update();
                    }
                }
        }   

        public void Turn()
        {
            if (_currentFigure is IRotatable fig)
            {               
                if (fig.Turn())
                {
                    SoundEvent?.Invoke(this, new SoundEventArg(TSound.Turning));
                }
                Update();
            }
        }

        public void Step()
        {         
            Move((Direction.Down)); 
        }


        #endregion


        private bool CopyCurrentFigureToField()
        {
            bool result = true;
            int[,] body = _currentFigure.Body;
            for (int i = 0; i < body.GetLength(0); i++)
            {
                int x = body[i, 0];
                int y = body[i, 1];
                if (_field[x, y] == null)
                {
                    _field[x, y] = new BoardPoint(_currentFigure.Color);
                }
                else
                {
                    result = false;
                    break;
                }              
            }
            return result;
        }

        private void ExchangeFigures()
        {
            _currentFigure = (Figure)_nextFigure.Clone();
            _currentFigure.DeleteTopFreeLineAndCenter();
            _nextFigure = Initializer.GetFigure(this);
        }

        private void Update()
        {
            if (UpdateEvent != null)
            {
                BoardPoint[,] nextFigure = new BoardPoint[Initializer.NumberOfFigurePoint, Initializer.NumberOfFigurePoint];
                for (int i = 0; i < Initializer.NumberOfFigurePoint; i++)
                {
                    nextFigure[_nextFigure.Body[i, 0], _nextFigure.Body[i, 1]] = new BoardPoint(_nextFigure.Color);
                }

                BoardPoint[,] board = (BoardPoint[,]) _field.Clone();
                if (_currentFigure != null)
                {
                    for (int i = 0; i < _currentFigure.Body.GetLength(0); i++)
                    {
                        int x = _currentFigure.Body[i, 0];
                        int y = _currentFigure.Body[i, 1];

                        board[x, y] = new BoardPoint(_currentFigure.Color);
                    }
                }
                

                UpdateEvent(this, new ShowEventArg(board, nextFigure, _level, _burnedLines, _score));
            }
        }

        private void SetVelocity()
        {
            VelocityChangeEvent?.Invoke(this, new VelocChangedEventArg(Initializer.GetVelocityByLevel(_level)));
        }

        private void Reset()
        {

            ClearGameBoard();
            _currentFigure = null;
            _nextFigure = null;
            _level = 0;
            _burnedLines = 0;
            _score = 0;
            SetVelocity();
        }

        private void ClearGameBoard()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _field[j, i] = null;
                }
            }
        }

        private bool CheckScoreLevelUp()
        {
            return (_score - _level * Initializer.LimitScore >= Initializer.LimitScore + _level * Initializer.LimitScore) && _level < 9;
        }

        private void LevelUp()
        {
            _level++;
            SetVelocity();
            ClearGameBoard();  
            Initializer.FillBoardFieldByLevel(_field, _level);           
        }

        private void CheckAndBurnLine()
        {

            for (int i = 0; i < _height; i++)
            {
                bool lineBurn = true;
                for (int j = 0; j < _width; j++)
                {
                    if (_field[j, i] == null)
                    {
                        lineBurn = false;
                        break;
                    }
                }
                if (!lineBurn) continue;
                BurnLine(i);
            }
        }

        private void BurnLine(int line)
        {
            for (int i = line; i > 0; i--)
            {
                for (int j = 0; j < _width; j++)
                {
                    _field[j, i] = _field[j, i - 1];
                }
            }
            _score += 100;
            _burnedLines += 1;
            SoundEvent?.Invoke(this, new SoundEventArg(TSound.Burning));
        }

        public bool Save()
        {
            return Logic.SaveGame(this, _connStr);
        }

        public void Open(int idSaveP, int lvl, int burnL, int score, int idField)
        {
            using (TetrisConnection conn = new TetrisConnection(_connStr))
            {
                if (conn.OpenGamePoint(this, idSaveP, idField))
                {
                    Level = lvl;
                    BurnedLine = burnL;
                    Score = score;
                    Update();
                }               
            }                      
        }

        public DataTable GetSavePoints()
        {
            using (TetrisConnection conn = new TetrisConnection(_connStr))
            {
                return conn.GetSavePoints();
            }       
        }


        private Figure _currentFigure;
        private Figure _nextFigure;
        private BoardPoint[,] _field;
        private int _burnedLines;
    }
}
