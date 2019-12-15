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
            Mov(AR, 1);
            Jnz("test");
            Label("test");
        }
    }
}
