using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    class Xor : Component
    {
        public override Pin[] Pins { get; }

        public Xor()
        {
            Pins = new Pin[] { new Pin("O", this), new Pin("I1", this), new Pin("I2", this) };
        }

        public override void Compute(Processor processor)
        {
            Pins[0].SetSignal(Pins[1].Signal ^ Pins[2].Signal, processor);
        }
    }
}
