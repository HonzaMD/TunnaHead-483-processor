using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    class Wire
    {
        private readonly HashSet<Component> components = new HashSet<Component>();

        public bool Signal { get; private set; }
        private int refCount;
        public bool Forbidden { get; set; }

        public static void Connect(Pin p1, Pin p2)
        {
            if (p1.Wire == null && p2.Wire == null)
            {
                var w = new Wire();
                w.components.Add(p1.Parent);
                w.components.Add(p2.Parent);
                p2.Wire = p1.Wire = w;
            }
            else if (p1.Wire != null && p2.Wire != null)
            {
                if (p1.Wire != p2.Wire)
                {
                    ReplaceWire(p1.Wire, p2.Wire);
                }
            }
            else if (p1.Wire == null)
            {
                p1.Wire = p2.Wire;
                p1.Wire.components.Add(p1.Parent);
            }
            else
            {
                p2.Wire = p1.Wire;
                p2.Wire.components.Add(p2.Parent);
            }
        }

        private static void ReplaceWire(Wire old, Wire nw)
        {
            foreach (var c in old.components)
            {
                foreach (var p in c.Pins)
                {
                    if (p.Wire == old)
                        p.Wire = nw;
                }
                nw.components.Add(c);
            }
        }

        public void SetSignal(bool signal, Component fromComponent, Processor processor)
        {
            if (!signal)
            {
                refCount--;
                if (refCount > 0)
                    return;
                Signal = false;
            }
            else
            {
                refCount++;
                if (refCount > 1)
                    return;
                Signal = true;
            }

            if (Forbidden)
                throw new InvalidOperationException("Zmena vyvolava cyklus v zapisu");

            foreach (var c in components)
            {
                if (c != fromComponent)
                    processor.AddToCompute(c);
            }
        }
    }
}
