using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    class QuickSort : Assembler
    {
        public QuickSort()
        {
            Jmp("code");
            LData("low", 0);
            LData("hi", 0);
            LData("stackPtr", "stackTop");
            LData("low2", 0);
            LData("hi2", 0);
            LData("pivot", 0);
            LData("temp", 0);

            Label("code");
            Ld(BR, "stackPtr");
            Mov(AR, BR);
            Sub(AR, "stack");
            Jnz("code1");

            Mov(AR, 1);
            St(255, AR);

            Label("code1");
            Sub(BR, 1);
            Ld(AR, BR);
            St("hi", AR);
            St("hi2", AR);
            Sub(BR, 1);
            Ld(AR, BR);
            St("low", AR);
            St("low2", AR);
            St("stackPtr", BR);

            Mov(BR, AR);
            SubLd(AR, "hi");
            Jge("code");

            // najdi pivota
            Ld(AR, BR);
            St("pivot", AR);

            Label("goLeft");
            Ld(BR, "low2");
            Label("goLeft2");
            Ld(AR, BR);
            SubLd(AR, "pivot");
            Jge("goRight");
            Add(BR, 1);
            Mov(AR, BR);
            SubLd(AR, "hi2");
            Jle("goLeft2");
            St("low2", BR);
            Jmp("divide");

            Label("goRight");
            St("low2", BR);
            Ld(BR, "hi2");
            Label("goRight2");
            Ld(AR, BR);
            SubLd(AR, "pivot");
            Jle("swap");
            Sub(BR, 1);
            Mov(AR, BR);
            SubLd(AR, "low2");
            Jge("goRight2");
            St("hi2", BR);
            Jmp("divide");

            Label("swap");
            Ld(AR, BR);
            St("temp", AR);
            Ld(AR, "low2");
            Ld(AR, AR);
            St(BR, AR);
            St("hi2", BR);
            Ld(BR, "low2");
            Ld(AR, "temp");
            St(BR, AR);

            Add(BR, 1);
            Ld(AR, "hi2");
            Sub(AR, 1);
            St("hi2", AR);
            Sub(AR, BR);
            Jge("goLeft2");
            St("low2", BR);

            Label("divide");
            Ld(BR, "stackPtr");
            Ld(AR, "low");
            St(BR, AR);
            Add(BR, 1);
            Ld(AR, "hi2");
            St(BR, AR);
            Add(BR, 1);
            Ld(AR, "low2");
            St(BR, AR);
            Add(BR, 1);
            Ld(AR, "hi");
            St(BR, AR);
            Add(BR, 1);
            St("stackPtr", BR);
            Jmp("code");


            Label("toSort");
            Data(45);
            Data(45);
            Data(46);
            Data(49);
            Data(42);
            Data(43);
            Data(47);
            Data(48);
            Data(41);
            Data(44);

            Label("stack");
            Data("toSort");
            Data("stack", -1);
            Label("stackTop");
        }
    }
}
