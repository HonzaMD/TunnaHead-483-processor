using Hradla.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    class Processor
    {
        private readonly HashSet<Component> toCompute;
        private readonly Component[] components;
        private readonly Oscilator[] oscilators;
        private readonly HashSet<Wire> forbidden = new HashSet<Wire>();
        private readonly Dictionary<string, Component> componentsByName;

        public Processor(Component[] components)
        {
            Console.WriteLine($"> Processor 483 'TunnaHead' with {components.Length} logical components READY");
            this.components = components;
            toCompute = new HashSet<Component>(components);
            this.oscilators = components.Select(c => c as Oscilator).Where(c => c != null).ToArray();
            componentsByName = components.ToDictionary(c => c.Name);
        }

        internal void AddToCompute(Component c) => toCompute.Add(c);

        internal void AddForbiddenWire(Wire w)
        {
            if (w != null)
            {
                w.Forbidden = true;
                forbidden.Add(w);
            }
        }


        public void RunCycle()
        {
            var c1count = Compute();
            foreach (var c in components)
                c.WriteThrough(this);
            var c2count = Compute();
            ClearForbidden();
            //Console.WriteLine($"Cycle recomputes: {c1count}, {c2count}");
            foreach (var oscilator in oscilators)
            {
                oscilator.Swap();
                AddToCompute(oscilator);
            }
        }

        private void ClearForbidden()
        {
            foreach (var w in forbidden)
                w.Forbidden = false;
            forbidden.Clear();
        }

        private int Compute()
        {
            int count = 0;
            while (toCompute.Count > 0)
            {
                var tocArr = toCompute.ToArray();
                toCompute.Clear();
                foreach (var c in tocArr)
                {
                    c.Compute(this);
                    count++;
                }
            }
            return count;
        }

        public T GetComponent<T>(string name) where T : Component
            => (T)componentsByName[name];

        public Components.Byte GetByte(string name) => (Components.Byte)componentsByName[name];
    }
}
