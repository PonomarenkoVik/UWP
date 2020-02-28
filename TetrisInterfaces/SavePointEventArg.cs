using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisInterfaces
{
    public delegate void SavePointChoose(object sender, SavePointEventArg args);

    public class SavePointEventArg
    {
        public SavePointEventArg(int id, int level, int burnedLine, int score, int idField)
        {
            Id = id;
            Level = level;
            BurnedLine = burnedLine;
            Score = score;
            IdField = idField;
        }

        public int IdField { get; }

        public int Score { get; }

        public int BurnedLine { get; }
        public int Level { get; set; }

        public int Id { get; }
    }
}