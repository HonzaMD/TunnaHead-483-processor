using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition Flags() => new Definition("Flags")
                {@"

^ AOUT 8
^ carry
^ O 8
^ W
^ SetDSF
^ SetBrw

Register Flags
+ Flags.O[m] O[m]

Or wo1
+ W wo1.I1
+ setFEnable.O wo1.I2
+ wo1.O Flags.W

Or SetF 8
Or setFEnable
And setFGate 8
+ O[m] SetF[m].I1
+ SetDSF SetF[4].I2
+ SetBrw SetF[5].I2
+ SetDSF setFEnable.I1
+ SetBrw setFEnable.I2
+ setFEnable.O setFGate[m].I1
+ SetF[m].O setFGate[m].I2
+ setFGate[m].O Flags.I[m]

And clearEnable
And clearGate 4
+ W clearEnable.I1
+ O[4] clearEnable.I2
+ O[m] clearGate[m].I1
+ clearEnable.O clearGate[m].I2
+ clearGate[m].O Flags.I[m]

Not weNot
And writeEnable
+ O[4] weNot.I
+ weNot.O writeEnable.I1
+ W writeEnable.I2

And writeGate 4
+ writeEnable.O writeGate[m].I1
+ AOUT[7] writeGate[0].I2
+ AOUT[0] writeGate[1].I2
+ zerro.O writeGate[2].I2
+ carry writeGate[3].I2
+ writeGate[m].O Flags.I[m]

Or z1 4
+ AOUT[m*2] z1[m].I1
+ AOUT[m*2+1] z1[m].I2

Or z2 2
+ z1[m*2].O z2[m].I1
+ z1[m*2+1].O z2[m].I2

Or z3
+ z2[0].O z3.I1
+ z2[1].O z3.I2

Not zerro
+ z3.O zerro.I


" };
    }
}
