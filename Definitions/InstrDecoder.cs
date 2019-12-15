using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition InstrDecoder() => new Definition("InstrDecoder")
                {@"
^ Mov
^ ActiveMov
^ NoArgs
^ UsesID
^ JmpCat
^ SpCat
^ Code 11
^ JmpCode 8
^ SpCode 8
^ Select2 3

^ IC 8
^ State2
^ notState0


Or omov
Not nmov
+ IC[7] omov.I1
+ IC[6] omov.I2
+ omov.O nmov.I
+ nmov.O Mov

And acmand
+ Mov acmand.I1
+ State2 acmand.I2
+ acmand.O ActiveMov

And naand 3
+ IC[7] naand[0].I1
+ IC[6] naand[0].I2
+ IC[5] naand[1].I1
+ IC[4] naand[1].I2
+ naand[0].O naand[2].I1
+ naand[1].O naand[2].I2
+ naand[2].O NoArgs


Cross c1 1
+ State2 c1.I1
+ IC[7] c1[m].C
+ c1[m].O1 c2[m*2].I1
+ c1[m].O2 c2[m*2+1].I1

Cross c2 2
+ IC[6] c2[m].C
+ c2[m].O1 c3[m*2].I1
+ c2[m].O2 c3[m*2+1].I1

Cross c3 4
+ IC[5] c3[m].C
+ c3[m].O1 c4[m*2].I1
+ c3[m].O2 c4[m*2+1].I1

Cross c4 8
+ IC[4] c4[m].C
+ c4[m].O1 Code[m*2-4]
+ c4[m].O2 Code[m*2-3]

And idand 4
Or idor
Not idnot
+ IC[5] idand[0].I1
+ IC[4] idand[0].I2
+ IC[2] idand[1].I1
+ IC[1] idand[1].I2
+ Mov idand[2].I1
+ idand[0].O idand[2].I2
+ idand[2].O idor.I1
+ NoArgs idnot.I
+ idnot.O idand[3].I1
+ idand[1].O idand[3].I2
+ idand[3].O idor.I2
+ idor.O UsesID

Cross scr 3
+ Mov scr[m].C
+ IC[m+3] scr[m].I2
+ IC[3] scr[1].I1
+ scr[m].O1 Select2[m]


And x0and
+ NoArgs x0and.I1
+ notState0 x0and.I2

Cross x1 1
+ x0and.O x1.I1
+ IC[3] x1[m].C
+ x1[m].O1 x2[m*2].I1
+ x1[m].O2 x2[m*2+1].I1
+ x1[m].O1 JmpCat
+ x1[m].O2 SpCat

Cross x2 2
+ IC[2] x2[m].C
+ x2[m].O1 x3[m*2].I1
+ x2[m].O2 x3[m*2+1].I1

Cross x3 4
+ IC[1] x3[m].C
+ x3[m].O1 x4[m*2].I1
+ x3[m].O2 x4[m*2+1].I1

Cross x4 8
+ IC[0] x4[m].C
+ x4[m].O1 JmpCode[m*2]
+ x4[m].O2 JmpCode[m*2+1]
+ x4[m].O1 SpCode[m*2-8]
+ x4[m].O2 SpCode[m*2-7]



                " };
    }
}
