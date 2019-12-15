using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    class DebugInstrReader
    {
        private readonly Components.Byte[] mem;
        private readonly Led8Monitor ip;
        private readonly Led8Monitor state;

        private string instr;

        public DebugInstrReader(Components.Byte[] mem, Led8Monitor ip, Led8Monitor state)
        {
            this.mem = mem;
            this.ip = ip;
            this.state = state;
        }

        public void Update()
        {
            if (state.Data == 0)
            {
                int ic = mem[ip.Data].Data;
                int id = mem[(byte)(ip.Data + 1)].Data;

                if ((ic & 0xc0) == 0)
                {
                    instr = $"Mov {ParseArg(id, (ic & 0x38) >> 3)} {ParseArg(id, ic & 0x7)}";
                }
                else if ((ic & 0xf0) == 0xf0)
                {
                    if (IsJump(ic))
                    {
                        instr = $"{(InstrCode)ic} {id}";
                    }
                    else
                    {
                        instr = $"{(InstrCode)ic}";
                    }
                }
                else
                {
                    instr = $"{(InstrCode)(ic & 0xf0)} {ParseArg(id, (ic & 0x8) >> 2)} {ParseArg(id, ic & 0x7)}";
                }
            }
        }

        private bool IsJump(int ic)
        {
            return ic < (byte)InstrCode.DSF;
        }

        private string ParseArg(int id, int prm)
        {
            bool memory = (prm & 1) != 0;
            PrmCode pcode = (PrmCode)(prm >> 1);
            string pcodeS = pcode == PrmCode.ID ? id.ToString() : pcode.ToString();
            return memory ? $"[{pcodeS}]" : pcodeS;
        }

        public override string ToString()
        {
            return instr;
        }
    }
}
