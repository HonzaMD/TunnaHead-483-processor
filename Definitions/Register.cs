using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition Register() => new Definition("Register")
                {@"
^ O 8
^ I 8
^ W
Cross wSel 8
Cross wSw 8
Byte m1
Byte m2
+ I[m] wSel[m].I2
+ O[m] wSel[m].I1
+ W wSel[m].C
+ wSel[m].O1 wSw[m].I2

Oscilator oscil
+ oscil.O wSw[m].C
+ wSw[m].O1 m1.I[m]
+ wSw[m].O2 m2.I[m]

Not wf
Cross wfc
+ wf.O wfc.I2
+ oscil.O wfc.C
+ wfc.O1 m1.W
+ wfc.O2 m2.W

Cross rSw 8
+ m1.O[m] rSw[m].I1
+ m2.O[m] rSw[m].I2
+ rSw[m].O1 O[m]
+ oscil.O rSw[m].C
                
" };
    }
}
