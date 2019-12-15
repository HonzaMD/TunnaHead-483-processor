using Hradla.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    class Led8Monitor
    {
        private readonly Led[] leds;

        public Led8Monitor(Processor processor, string name)
        {
            leds = Enumerable.Range(0, 8).Select(i => processor.GetComponent<Led>($"{name}[{i}]")).ToArray();
        }

        public int Data
        {
            get
            {
                return leds[0].Memory.ToInt() | (leds[1].Memory.ToInt() << 1) | (leds[2].Memory.ToInt() << 2) | (leds[3].Memory.ToInt() << 3) | (leds[4].Memory.ToInt() << 4) | (leds[5].Memory.ToInt() << 5) | (leds[6].Memory.ToInt() << 6) | (leds[7].Memory.ToInt() << 7);
            }
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }

    class LedNMonitor
    {
        private readonly Led[] leds;
        private readonly StringBuilder sb = new StringBuilder();

        public LedNMonitor(Processor processor, string name, int count)
        {
            leds = count > 1
                ? Enumerable.Range(0, count).Select(i => processor.GetComponent<Led>($"{name}[{i}]")).ToArray()
                : new[] { processor.GetComponent<Led>(name) };
        }

        public String Data
        {
            get
            {
                sb.Clear();
                for (int f = leds.Length - 1; f >= 0; f--)
                {
                    sb.Append(leds[f].Memory ? '1' : '0');
                }
                return sb.ToString();
            }

        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
