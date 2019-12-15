using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    class Hadani : Assembler
    {
        public Hadani()
        {
            Jmp("code");
            LData("secret", 83);
            LData("low", 0);
            LData("hi", 100);
            LData("testsPtr", "tests");
            LData("result", -1);

            Label("code");
            Ld(AR, "hi");
            SubLd(AR, "low");
            Jnz("code1");

            Ld(AR, "low");
            St("result", AR);
            Mov(AR, 1);
            St(255, AR);

            Label("code1");
            Shr(AR, 1);
            AddLd(AR, "low");
            Ld(BR, "testsPtr");
            St(BR, AR);
            Add(BR, 1);
            St("testsPtr", BR);
            Mov(BR, AR);
            SubLd(AR, "secret");
            Jl("greater");

            St("hi", BR);
            Jmp("code");

            Label("greater");
            Add(BR, 1);
            St("low", BR);
            Jmp("code");

            Label("tests");
        }
    }
}
