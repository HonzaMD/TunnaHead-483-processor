using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    sealed class Byte : Component
    {
        public override Pin[] Pins { get; }
        private readonly bool[] memory = new bool[8];

        public Byte()
        {
            Pins = Enumerable.Range(0, 8).Select(i => new Pin($"O[{i}]", this))
                .Concat(Enumerable.Range(0, 8).Select(i => new Pin($"I[{i}]", this)))
                .Concat(new Pin[] { new Pin("W", this) })
                .ToArray();
        }

        public override void Compute(Processor processor)
        {
            for (int f = 0; f < 8; f++)
                Pins[f].SetSignal(memory[f], processor);
        }

        public override void WriteThrough(Processor processor)
        {
            if (Pins[16].Signal)
            {
                for (int f = 0; f < 8; f++)
                {
                    memory[f] = Pins[f+8].Signal;
                    processor.AddForbiddenWire(Pins[f+8].Wire);
                }
                processor.AddForbiddenWire(Pins[16].Wire);
                Compute(processor);
            }
        }

        public int Data
        {
            get
            {
                return memory[0].ToInt() | (memory[1].ToInt() << 1) | (memory[2].ToInt() << 2) | (memory[3].ToInt() << 3) | (memory[4].ToInt() << 4) | (memory[5].ToInt() << 5) | (memory[6].ToInt() << 6) | (memory[7].ToInt() << 7);
            }
            set
            {
                memory[0] = (value & 0x01) != 0;
                memory[1] = (value & 0x02) != 0;
                memory[2] = (value & 0x04) != 0;
                memory[3] = (value & 0x08) != 0;
                memory[4] = (value & 0x10) != 0;
                memory[5] = (value & 0x20) != 0;
                memory[6] = (value & 0x40) != 0;
                memory[7] = (value & 0x80) != 0;
            }
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
