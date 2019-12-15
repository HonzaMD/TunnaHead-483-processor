using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition Shl() => new Definition("Shl")
                {@"
^ I1 8
^ I2 8
^ O 8
^ carry
^ borrow

Or bigBit 3
+ I2[4] bigBit[0].I1
+ I2[5] bigBit[0].I2
+ I2[6] bigBit[1].I1
+ I2[7] bigBit[1].I2
+ bigBit[0].O bigBit[2].I1
+ bigBit[1].O bigBit[2].I2

Cross c1 9
+ I2[0] c1[m].C
+ I1[m] c1[m+1].I1
+ borrow c1[0].I1
+ c1[m].O1 c2[m].I1
+ c1[m].O2 c2[m+1].I1

Cross c2 10
+ I2[1] c2[m].C
+ c2[m].O1 c3[m].I1
+ c2[m].O2 c3[m+2].I1

Cross c3 10
+ I2[2] c3[m].C
+ c3[m].O1 c4[m].I1
+ c3[m].O2 c4[m+4].I1

Cross c4 10
+ I2[3] c4[m].C
+ c4[m].O1 c5[m].I1
+ c4[m].O2 c5[m+8].I1

Cross c5 10
+ bigBit[2].O c5[m].C
+ c5[m].O1 O[m-1]
+ c5[9].O1 carry

                "};
    }
}
