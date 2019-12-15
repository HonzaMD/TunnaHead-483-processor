using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    class Fibonacci : Assembler
    {
        public Fibonacci()
        {
            Mov(BR, "fibStart");
            Label("loop");
            Ld(AR, BR);
            Add(BR, 1);
            AddLd(AR, BR);
            Add(BR, 1);
            St(BR, AR);
            Sub(BR, 1);
            Jmp("loop");
            Label("fibStart");
            Data(1);
            Data(1);
        }
    }
}
