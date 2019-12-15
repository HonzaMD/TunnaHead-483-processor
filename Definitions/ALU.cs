using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition ALU() => new Definition("ALU")
                {@"

^ I1 8
^ I2 8
^ O 8
^ Code 11
^ Burrow
^ Carry
^ BurrowEnable

And burrow2
+ Burrow burrow2.I1
+ BurrowEnable burrow2.I2

Or caOr 3
+ addCarry.O caOr[0].I1
+ subCarry.O caOr[0].I2
+ shlCarry.O caOr[1].I1
+ shrCarry.O caOr[1].I2
+ caOr[0].O caOr[2].I1
+ caOr[1].O caOr[2].I2
+ caOr[2].O Carry


Add add
And addGate 8
+ I1[m] add.I1[m]
+ I2[m] add.I2[m]
+ add.O[m] addGate[m].I1
+ Code[0] addGate[m].I2
+ addGate[m].O O[m]

And addCarry
+ add.carry addCarry.I1
+ Code[0] addCarry.I2
+ burrow2.O add.borrow

And and 8
And andGate 8
+ I1[m] and[m].I1
+ I2[m] and[m].I2
+ and[m].O andGate[m].I1
+ Code[1] andGate[m].I2
+ andGate[m].O O[m]

Or or 8
And orGate 8
+ I1[m] or[m].I1
+ I2[m] or[m].I2
+ or[m].O orGate[m].I1
+ Code[2] orGate[m].I2
+ orGate[m].O O[m]

Add addSb
And subGate 8
Not notSb 8
Not one
+ I1[m] notSb[m].I
+ notSb[m].O addSb.I1[m]
+ I2[m] addSb.I2[m]
+ subBurrow.O1 addSb.borrow
+ addSb.O[m] subGate[m].I1
+ Code[3] subGate[m].I2
+ subGate[m].O O[m]

And subCarry
Cross subBurrow
+ addSb.carry subCarry.I1
+ Code[3] subCarry.I2
+ BurrowEnable subBurrow.C
+ one.O subBurrow.I1
+ Burrow subBurrow.I2


Shl shl
And shlGate 8
+ I1[m] shl.I2[m]
+ I2[m] shl.I1[m]
+ shl.O[m] shlGate[m].I1
+ Code[4] shlGate[m].I2
+ shlGate[m].O O[m]

And shlCarry
+ shl.carry shlCarry.I1
+ Code[4] shlCarry.I2
+ burrow2.O shl.borrow


Shl shr
And shrGate 8
+ I1[m] shr.I2[m]
+ I2[m] shr.I1[m*-1+7]
+ shr.O[m*-1+7] shrGate[m].I1
+ Code[5] shrGate[m].I2
+ shrGate[m].O O[m]

And shrCarry
+ shr.carry shrCarry.I1
+ Code[5] shrCarry.I2
+ burrow2.O shr.borrow


Not not 8
And notGate 8
+ I1[m] not[m].I
+ not[m].O notGate[m].I1
+ Code[6] notGate[m].I2
+ notGate[m].O O[m]

Not notNg 8
IncByte incNg
And negGate 8
+ I1[m] notNg[m].I
+ notNg[m].O incNg.I[m]
+ incNg.O[m] negGate[m].I1
+ Code[7] negGate[m].I2
+ negGate[m].O O[m]


                " };
    }
}
