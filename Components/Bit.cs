using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    sealed class Bit : Component
    {
        public override Pin[] Pins { get; }
        private bool memory;

        public Bit()
        {
            Pins = new Pin[] { new Pin("O", this), new Pin("I", this), new Pin("W", this) };
        }

        public override void Compute(Processor processor)
        {
            Pins[0].SetSignal(memory, processor);
        }

        public override void WriteThrough(Processor processor)
        {
            if (Pins[2].Signal)
            {
                memory = Pins[1].Signal;
                processor.AddForbiddenWire(Pins[1].Wire);
                processor.AddForbiddenWire(Pins[2].Wire);
                Compute(processor);
            }
        }
    }
}
