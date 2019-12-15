using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    class FirstProgram : Assembler
    {
        public FirstProgram()
        {
            Label("loop");
            Ld(AR, "B1");
            Shl(AR, 1);
            DSF();
            St("B1", AR);
            DSF();
            Ld(AR, "B2");
            Brw();
            Shl(AR, 1);
            St("B2", AR);
            Jmp("loop");

            LData("B1", 200);
            LData("B2", 0);

        }
    }
}
