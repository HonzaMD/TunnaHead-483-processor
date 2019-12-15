using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    class Oscilator : Component
    {
        public override Pin[] Pins { get; }
        private bool state;

        public Oscilator()
        {
            Pins = new Pin[] { new Pin("O", this) };
        }

        public override void Compute(Processor processor)
        {
            Pins[0].SetSignal(state, processor);
        }

        public void Swap() => state = !state;
    }
}
