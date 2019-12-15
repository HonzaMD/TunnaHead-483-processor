using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition IncByte() => new Definition("IncByte")
                {@"                   
^ I 8
^ O 8
Not n1
+ I[0] n1.I
+ n1.O O[0]
Xor xor 7
And and 6
+ I[m+1] xor[m].I1
+ O[m+1] xor[m].O
+ and[m].O xor[m+1].I2
+ I[0] xor[0].I2
+ I[m+1] and[m].I1
+ and[m].O and[m+1].I2
+ I[0] and[0].I2

                "};
    }
}
