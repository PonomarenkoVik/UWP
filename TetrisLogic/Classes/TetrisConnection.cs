//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TetrisInterfaces;
//using TetrisLogic.Figures;

//namespace TetrisLogic.Classes
//{
//    internal class TetrisConnection : Connection
//    {

//        public TetrisConnection(SqlConnectionStringBuilder conn) : base(conn)
//        {
//        }

//        public bool SaveGamePoint(TetrisGameBoard board)
//        {
//            try
//            {
//                AddSavePoint(board, out int idSaveP, out int idFld, out int idCurrF, out int idNextF);
//                AddPointsField(board, idFld);
//                AddPointsFigure(board.CurrentFigure, idCurrF);
//                AddPointsFigure(board.NextFigure, idNextF);                              
//                return true;
//            }
//            catch (SqlException e)
//            {
//                Console.WriteLine(e);
//                return false;
//            }
//            finally
//            {
//                Dispose();
//            }          
//        }

//        public bool OpenGamePoint(TetrisGameBoard board, int idSavePoint, int idField)
//        {
//            try
//            {
//                GetFigure(idSavePoint, board);
//                GetField(idField, board);
//                return true;
//            }
//            catch (SqlException e)
//            {
//                Console.WriteLine(e);
//                return false;
//            }
//            finally
//            {
//                Dispose();
//            }
//        }


//        public DataTable GetSavePoints()
//        {
//            SqlCommand comm = new SqlCommand();
//            comm.CommandText = "SELECT * FROM VSavePoints";
//            comm.Connection = _conn;
//            DataTable table = new DataTable();
//            table.Load(comm.ExecuteReader());
//            table.Columns[0].ColumnName = "Id";
//            table.Columns[3].ColumnName = "Burned line";
//            table.Columns[5].ColumnName = "Id Field";
//            return table;
//        }

//        private void GetField(int idField, TetrisGameBoard board)
//        {
//            BoardPoint[,] result = new BoardPoint[board.Width, board.Height];
//            SqlCommand comm = GetCommand("GetPoints");
//            comm.Parameters.Add(new SqlParameter() { ParameterName = "@idField", DbType = DbType.Int32, Value = idField });
//            DataTable table = new DataTable();
//            table.Load(comm.ExecuteReader());

//            if (table.Rows.Count != 0)
//            {                              
//                for (int i = 0; i < table.Rows.Count; i++)
//                {
//                    var row = table.Rows[i];
//                    TColor col = (TColor)row[0];
//                    byte x = (byte)row[1];
//                    byte y = (byte)row[2];
//                    result[x, y] = new BoardPoint(col);                
//                }
//                board.Field = result;
//            }           
//        }

//        private void GetFigure(int idSavePoint, TetrisGameBoard board)
//        {
//            GetIdFigWithType(idSavePoint, out int idCurrF, out int idNextF, out int idCurrFType, out int idNextFType);
//            int[,] bodyCurrF = GetFigurePoint(idCurrF, out int colorCurrFig);
//            int[,] bodyNextF = GetFigurePoint(idNextF, out int colorNextFig);
//            board.CurrentFigure = DataBaseLogic.CreateFigure(bodyCurrF, (TColor)colorCurrFig, (FiguresTypes)idCurrFType, board);
//            board.NextFigure = DataBaseLogic.CreateFigure(bodyNextF, (TColor)colorNextFig, (FiguresTypes)idNextFType, board);
//        }

//        private void GetIdFigWithType(int idSavePoint, out int idCurrF, out int idNextF, out int idCurrFType, out int idNextFType)
//        {
//            SqlCommand comm = GetCommand("GetFigures");
//            comm.Parameters.Add(new SqlParameter() {ParameterName = "@idSaveP", DbType = DbType.Int32, Value = idSavePoint});
//            var idCurrFig = new SqlParameter()
//            {
//                ParameterName = "@idCurrFig",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            var idNextFig = new SqlParameter()
//            {
//                ParameterName = "@idNextFig",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            var idCurrFigType = new SqlParameter()
//            {
//                ParameterName = "@idCurrFigType",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            var idNextFigType = new SqlParameter()
//            {
//                ParameterName = "@idNextFigType",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            comm.Parameters.Add(idCurrFig);
//            comm.Parameters.Add(idNextFig);
//            comm.Parameters.Add(idCurrFigType);
//            comm.Parameters.Add(idNextFigType);
//            comm.ExecuteNonQuery();
//            idCurrF = (int) idCurrFig.Value;
//            idNextF = (int) idNextFig.Value;
//            idCurrFType = (int) idCurrFigType.Value;
//            idNextFType = (int) idNextFigType.Value;
//        }


//        private int[,] GetFigurePoint(int idFigure, out int color)
//        {
//            int[,] result = null;
//            color = 0;
//            SqlCommand comm = GetCommand("GetPoints");
//            comm.Parameters.Add(new SqlParameter() { ParameterName = "@idFig", DbType = DbType.Int32, Value = idFigure });
//            DataTable table = new DataTable();
//            table.Load(comm.ExecuteReader());
           
//            if (table.Rows.Count != 0)
//            {
//                result = new int[table.Rows.Count, 2];
//                color = (int)table.Rows[0][0];
//                for (int i = 0; i < table.Rows.Count; i++)
//                {
//                    var row = table.Rows[i];
//                    result[i, 0] = (byte)row[1];
//                    result[i, 1] = (byte)row[2];
//                }
//            }
//            return result;
//        }


//        private void AddPointsField(TetrisGameBoard board, int idFld)
//        {
//            SqlCommand comm = GetSqlCommandForAddPoint(out SqlParameter color, out SqlParameter x, out SqlParameter y);
//            comm.Parameters.Add(new SqlParameter() { ParameterName = "@idField", DbType = DbType.Int32, Value = idFld });


//            for (byte i = 0; i < board.Field.GetLength(1); i++)
//            {
//                for (byte j = 0; j < board.Field.GetLength(0); j++)
//                {
//                    if (board.Field[j, i] != null)
//                    {
//                        color.Value = (int)board.Field[j, i].Col;
//                        x.Value = j;
//                        y.Value = i;
//                        comm.ExecuteNonQuery();
//                    }                   
//                }
//            }

//        }

//        private void AddPointsFigure(Figure fig, int idFig)
//        {
//            SqlCommand comm = GetSqlCommandForAddPoint(out SqlParameter color, out SqlParameter x, out SqlParameter y);
//            comm.Parameters.Add(new SqlParameter() { ParameterName = "@idFig", DbType = DbType.Int32, Value = idFig });
//            color.Value = (int)fig.Color;
//            for (byte i = 0; i < fig.Body.GetLength(0); i++)
//            {                        
//                x.Value = fig.Body[i, 0];
//                y.Value = fig.Body[i, 1];
//                comm.ExecuteNonQuery();   
//            }
//        }


//        private SqlCommand GetSqlCommandForAddPoint(out SqlParameter color, out SqlParameter x, out SqlParameter y)
//        {
//            SqlCommand comm = GetCommand("AddPoint");
//            color = new SqlParameter() {ParameterName = "@idColorP", DbType = DbType.Int32};
//            x = new SqlParameter() {ParameterName = "@X", DbType = DbType.Int16};
//            y = new SqlParameter() {ParameterName = "@Y", DbType = DbType.Int16};
//            comm.Parameters.Add(color);
//            comm.Parameters.Add(x);
//            comm.Parameters.Add(y);
//            return comm;
//        }


//        private void AddSavePoint(TetrisGameBoard board, out int idSaveP, out int idFld, out int idCurrF, out int idNextF)
//        {
//            SqlCommand comm = GetCommand("AddSavePoint");
//            AddInputParameter(board, comm);
//            SqlParameter idSavePoint = new SqlParameter()
//            {
//                ParameterName = "@IdSaveP",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            SqlParameter idField = new SqlParameter()
//            {
//                ParameterName = "@IdFld",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            SqlParameter idCurrFig = new SqlParameter()
//            {
//                ParameterName = "@IdCurrFigure",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };

//            SqlParameter idNextFig = new SqlParameter()
//            {
//                ParameterName = "@IdNextFigure",
//                DbType = DbType.Int32,
//                Direction = ParameterDirection.Output
//            };
//            comm.Parameters.Add(idSavePoint);
//            comm.Parameters.Add(idField);
//            comm.Parameters.Add(idCurrFig);
//            comm.Parameters.Add(idNextFig);
//            comm.ExecuteNonQuery();
//            idSaveP = (int)idSavePoint.Value;
//            idFld = (int)idField.Value;
//            idCurrF = (int)idCurrFig.Value;
//            idNextF = (int) idNextFig.Value;
//        }

//        private static void AddInputParameter(TetrisGameBoard board, SqlCommand comm)
//        {
//            comm.Parameters.Add(new SqlParameter() {ParameterName = "@Level", DbType = DbType.Int32, Value = board.Level + 1});
//            comm.Parameters.Add(new SqlParameter(){ParameterName = "@BurLine", DbType = DbType.Int32, Value = board.BurnedLine });
//            comm.Parameters.Add(new SqlParameter() {ParameterName = "@Score", DbType = DbType.Int32, Value = board.Score});
//            comm.Parameters.Add(new SqlParameter(){ParameterName = "@idCurrFigType", DbType = DbType.Int32, Value = (int) board.CurrentFigure.GetFigureType()});
//            comm.Parameters.Add(new SqlParameter(){ParameterName = "@idNextFigType", DbType = DbType.Int32, Value = (int) board.NextFigure.GetFigureType()});
//        }
//    }
//}
