using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    [Flags]
    enum Code : byte
    {
        Mov = 0x00,
        Add = 0x40,
        And = 0x50,
        Or = 0x60,
        Sub = 0x70,
        Shl = 0x80,
        Shr = 0x90,
        Not = 0xA0,
        Neg = 0xB0,

        Jz = 0xf0,
        Jnz = 0xf1,
        Jl = 0xf2,
        Jge = 0xf3,
        Jg = 0xf4,
        Jle = 0xf5,
        J0 = 0xf6,
        Jn0 = 0xf7,

        DSF = 0xf8,
        Brw = 0xf9,

        M1 = 0x01,
        AR1 = 0x00,
        BR1 = 0x02,
        IP1 = 0x04,
        ID1 = 0x06,

        AR2 = 0x00,
        BR2 = 0x08,

        M3 = 0x08,
        AR3 = 0x00,
        BR3 = 0x10,
        IP3 = 0x20,
        ID3 = 0x30,
    }


    enum InstrCode : byte
    {
        Mov = 0x00,
        Add = 0x40,
        And = 0x50,
        Or = 0x60,
        Sub = 0x70,
        Shl = 0x80,
        Shr = 0x90,
        Not = 0xA0,
        Neg = 0xB0,

        Jz = 0xf0,
        Jnz = 0xf1,
        Jl = 0xf2,
        Jge = 0xf3,
        Jg = 0xf4,
        Jle = 0xf5,
        J0 = 0xf6,
        Jn0 = 0xf7,

        DSF = 0xf8,
        Brw = 0xf9,
    }
}
