using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Components
{
    class Cross : Component
    {
        public override Pin[] Pins { get; }

        public Cross()
        {
            Pins = new Pin[] { new Pin("O1", this), new Pin("O2", this), new Pin("I1", this), new Pin("I2", this), new Pin("C", this) };
        }

        public override void Compute(Processor processor)
        {
            var c = Pins[4].Signal;
            if (!c)
            {
                Pins[0].SetSignal(Pins[2].Signal, processor);
                Pins[1].SetSignal(Pins[3].Signal, processor);
            }
            else
            {
                Pins[0].SetSignal(Pins[3].Signal, processor);
                Pins[1].SetSignal(Pins[2].Signal, processor);
            }
        }
    }
}
