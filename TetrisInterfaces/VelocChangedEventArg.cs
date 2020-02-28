using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisInterfaces
{
    public class VelocChangedEventArg : EventArgs
    {
        public VelocChangedEventArg(float vel)
        {
            Vel = vel;
        }

        public float Vel { get; }
    }
}
