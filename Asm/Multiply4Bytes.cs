using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    class Multiply4Bytes : Assembler
    {
        public Multiply4Bytes()
        {
            Jmp("start");

            LData("x1", 100);
            LData("x2", 0);
            LData("x3", 0);
            LData("x4", 0);

            LData("y1", 100);
            LData("y2", 0);
            LData("y3", 0);
            LData("y4", 0);

            LData("r1", 0);
            LData("r2", 0);
            LData("r3", 0);
            LData("r4", 0);

            Label("start");
            Ld(AR, "y1");
            OrLd(AR, "y2");
            OrLd(AR, "y3");
            OrLd(AR, "y4");
            Jnz("continue");

            Mov(AR, 1);
            St(255, AR);

            Label("continue");
            Ld(AR, "y1");
            Jn0("shift");

            Ld(AR, "r1");
            Ld(BR, "r2");
            AddLd(AR, "x1");
            Brw();
            AddLd(BR, "x2");
            DSF();
            St("r1", AR);
            DSF();
            St("r2", BR);
            DSF();
            Ld(AR, "r3");
            DSF();
            Ld(BR, "r4");
            Brw();
            AddLd(AR, "x3");
            Brw();
            AddLd(BR, "x4");
            St("r3", AR);
            St("r4", BR);

            Label("shift");
            Ld(AR, "x1");
            Ld(BR, "x2");
            Shl(AR, 1);
            Brw();
            Shl(BR, 1);
            DSF();
            St("x1", AR);
            DSF();
            St("x2", BR);
            DSF();
            Ld(AR, "x3");
            DSF();
            Ld(BR, "x4");
            Brw();
            Shl(AR, 1);
            Brw();
            Shl(BR, 1);
            St("x3", AR);
            St("x4", BR);

            Ld(AR, "y4");
            Ld(BR, "y3");
            Shr(AR, 1);
            Brw();
            Shr(BR, 1);
            DSF();
            St("y4", AR);
            DSF();
            St("y3", BR);
            DSF();
            Ld(AR, "y2");
            DSF();
            Ld(BR, "y1");
            Brw();
            Shr(AR, 1);
            Brw();
            Shr(BR, 1);
            St("y2", AR);
            St("y1", BR);

            Jmp("start");
        }
    }
}
