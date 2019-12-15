using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition Memory() => new Definition("Memory")
        {@"

^ Bus 8
^ Sel 8
^ R
^ W

Byte byte 256

DeMultiplex read 8
And gate 8
+ byte[m].O[n] read[n].I[m]
+ Sel[m] read[n].S[m]
+ read[m].O gate[m].I1
+ R gate[m].I2
+ gate[m].O Bus[m]

Multiplex write 8
Multiplex wf
+ Bus[m] write[m].I
+ Sel[m] write[n].S[m]
+ write[m].O[n] byte[n].I[m]
+ W wf.I
+ Sel[m] wf.S[m]
+ wf.O[n] byte[n].W

        " };
    }
}
