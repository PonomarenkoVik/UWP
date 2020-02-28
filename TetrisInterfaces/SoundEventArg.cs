using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisInterfaces.Enum;

namespace TetrisInterfaces
{
    public class SoundEventArg
    {
        public SoundEventArg(TSound sound)
        {
            Sound = sound;            
        }
        public TSound Sound { get; }
        
    }
}
