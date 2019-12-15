using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition ChangeState() => new Definition("ChangeState")
                {@"

^ I 4
^ O 4
^ NoArgs
^ UsesID
^ JumpEnabled

Not notI 4
Not notNA
Or notFast0
Not notUid
Not notJE
+ I[m] notI[m].I
+ NoArgs notNA.I
+ UsesID notUid.I
+ JumpEnabled notFast0.I1
+ notNA.O notFast0.I2
+ JumpEnabled notJE.I

Or or1
And and1
And and1b
+ I[0] or1.I1
+ notFast0.O or1.I2
+ or1.O and1.I1
+ notI[2].O and1.I2
+ and1.O and1b.I1
+ notI[3].O and1b.I2
+ and1b.O O[0]

And and2
+ notI[0].O and2.I1
+ UsesID and2.I2
+ and2.O O[1]

And and3
And and4
And and5
Or or3
+ notI[0].O and3.I1
+ notFast0.O and3.I2
+ notUid.O and4.I1
+ notJE.O and4.I2
+ and3.O and5.I1
+ and4.O and5.I2
+ I[1] or3.I1
+ and5.O or3.I2
+ or3.O O[2]

And and6
+ notI[0].O and6.I1
+ JumpEnabled and6.I2
+ and6.O O[3]


                " };
    }
}
