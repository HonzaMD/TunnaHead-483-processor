using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    sealed class Led : Component
    {
        public override Pin[] Pins { get; }
        public bool Memory { get; private set; }

        public Led()
        {
            Pins = new Pin[] { new Pin("I", this) };
        }

        public override void Compute(Processor processor)
        {
        }

        public override void WriteThrough(Processor processor)
        {
            Memory = Pins[0].Signal;
        }
    }
}
