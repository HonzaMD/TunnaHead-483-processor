using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition Add() => new Definition("Add")
                {@"
^ I1 8
^ I2 8
^ O 8
^ carry
^ borrow

Xor xorI 8
Xor xorC 8
+ I1[m] xorI[m].I1
+ I2[m] xorI[m].I2
+ xorI[m].O xorC[m].I1
+ c[m].O xorC[m+1].I2
+ xorC[m].O O[m]

And andI 8
And andC 8
Or c 8
+ I1[m] andI[m].I1
+ I2[m] andI[m].I2
+ xorI[m].O andC[m].I1
+ c[m].O andC[m+1].I2
+ andI[m].O c[m].I1
+ andC[m].O c[m].I2

+ borrow xorC[0].I2
+ borrow andC[0].I2
+ c[7].O carry

                "};
    }
}
