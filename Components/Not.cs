using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    class Not : Component
    {
        public override Pin[] Pins { get; }

        public Not()
        {
            Pins = new Pin[] { new Pin("O", this), new Pin("I", this) };
        }

        public override void Compute(Processor processor)
        {
            Pins[0].SetSignal(!Pins[1].Signal, processor);
        }
    }
}
