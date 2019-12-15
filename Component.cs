using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    abstract class Component
    {
        public string Name { get; set; }
        public string TypeName => GetType().Name;
        public abstract Pin[] Pins { get; }
        public abstract void Compute(Processor processor);
        public virtual void WriteThrough(Processor processor) { }
    }
}
