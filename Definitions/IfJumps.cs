using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition IfJumps() => new Definition("IfJumps")
                {@"

^ Enabled
^ SkipJump
^ notSkipJump

^ NotState0
^ JmpCat
^ JmpCode 8
^ Flags 8

* JmpEnable 8

Or jen 4
+ JmpEnable[m*2] jen[m].I1
+ JmpEnable[m*2+1] jen[m].I2
+ jen[m].O Enabled

Not nEnabled
+ Enabled nEnabled.I

And and1
+ JmpCat and1.I1
+ nEnabled.O and1.I2
+ and1.O SkipJump

Not nsj
And and2
+ SkipJump nsj.I
+ nsj.O and2.I1
+ NotState0 and2.I2
+ and2.O notSkipJump


//Jz  = 0xf0,
And jz
+ Flags[2] jz.I1
+ JmpCode[0] jz.I2
+ jz.O JmpEnable[0]

//Jnz = 0xf1,
And jnz
Not jnzn
+ Flags[2] jnzn.I
+ jnzn.O jnz.I1
+ JmpCode[1] jnz.I2
+ jnz.O JmpEnable[1]

//Jl  = 0xf2,
And jl
+ Flags[0] jl.I1
+ JmpCode[2] jl.I2
+ jl.O JmpEnable[2]

//Jge = 0xf3,
And jge
Not jgen
+ Flags[0] jgen.I
+ jgen.O jge.I1
+ JmpCode[3] jge.I2
+ jge.O JmpEnable[3]

//Jg  = 0xf4,
And jg
Not jgn
Or jgo
+ Flags[0] jgo.I1
+ Flags[2] jgo.I2
+ jgo.O jgn.I
+ jgn.O jg.I1
+ JmpCode[4] jg.I2
+ jg.O JmpEnable[4]

//Jle = 0xf5,
And jle
Or jleo
+ Flags[0] jleo.I1
+ Flags[2] jleo.I2
+ jleo.O jle.I1
+ JmpCode[5] jle.I2
+ jle.O JmpEnable[5]

//J0  = 0xf6,
And j0
+ Flags[1] j0.I1
+ JmpCode[6] j0.I2
+ j0.O JmpEnable[6]

//Jn0 = 0xf7,
And jn0
Not jn0n
+ Flags[1] jn0n.I
+ jn0n.O jn0.I1
+ JmpCode[7] jn0.I2
+ jn0.O JmpEnable[7]


                " };
    }
}
