using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition DeMultiplex() => new Definition("DeMultiplex")
                {@"

^ I 256
^ O
^ S 8

Cross c1 1
+ O c1.O1
+ S[7] c1[m].C
+ c1[m].I1 c2[m*2].O1
+ c1[m].I2 c2[m*2+1].O1

Cross c2 2
+ S[6] c2[m].C
+ c2[m].I1 c3[m*2].O1
+ c2[m].I2 c3[m*2+1].O1

Cross c3 4
+ S[5] c3[m].C
+ c3[m].I1 c4[m*2].O1
+ c3[m].I2 c4[m*2+1].O1

Cross c4 8
+ S[4] c4[m].C
+ c4[m].I1 c5[m*2].O1
+ c4[m].I2 c5[m*2+1].O1

Cross c5 16
+ S[3] c5[m].C
+ c5[m].I1 c6[m*2].O1
+ c5[m].I2 c6[m*2+1].O1

Cross c6 32
+ S[2] c6[m].C
+ c6[m].I1 c7[m*2].O1
+ c6[m].I2 c7[m*2+1].O1

Cross c7 64
+ S[1] c7[m].C
+ c7[m].I1 c8[m*2].O1
+ c7[m].I2 c8[m*2+1].O1

Cross c8 128
+ S[0] c8[m].C
+ c8[m].I1 I[m*2]
+ c8[m].I2 I[m*2+1]

                "};
    }
}
