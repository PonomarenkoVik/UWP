//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TetrisLogic.Classes
//{
//    abstract class Connection : IDisposable
//    {
//        protected Connection(SqlConnectionStringBuilder conn)
//        {
//            _conn = new SqlConnection(conn.ConnectionString);
//            _conn.Open();
//        }

//        protected virtual SqlCommand GetCommand(string query)
//        {
//            return new SqlCommand
//            {
//                Connection = _conn,
//                CommandType = CommandType.StoredProcedure,
//                CommandText = query
//            };
//        }


//        public virtual void Dispose()
//        {
//            _conn.Close();
//        }


//        protected readonly SqlConnection _conn;
//    }
//}
