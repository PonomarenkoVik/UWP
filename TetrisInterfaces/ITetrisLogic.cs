using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces.Enum;

namespace TetrisInterfaces
{
    public delegate void SoundT(object obj, SoundEventArg arg);
    public delegate void ShowT(object obj, ShowEventArg arg);
    public delegate void VelocChange(object obj, VelocChangedEventArg arg);
    public interface ITetrisLogic
    {
        event SoundT SoundEvent;
        event ShowT UpdateEvent;
        event Action GameOverEvent;
        event VelocChange VelocityChangeEvent;
        void Start();    
        void Move(Direction dir);      
        void Turn();
        void Step();
        bool Save();
        void Open(int idSaveP, int lvl, int burnL, int score, int idField);
        DataTable GetSavePoints();

    }
}
