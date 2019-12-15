using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    class Pin
    {
        public string Name { get; }
        public Wire Wire { get; set; }
        public Component Parent { get; }
        private bool signaled;

        public Pin(string name, Component parent)
        {
            Name = name;
            Parent = parent;
        }

        internal void SetSignal(bool signal, Processor processor)
        {
            if (signal != signaled)
            {
                Wire?.SetSignal(signal, Parent, processor);
                signaled = signal;
            }
        }

        public bool Signal => Wire?.Signal ?? false;
    }
}
