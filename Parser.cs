using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    class Parser : List<Definition>
    {
        private Dictionary<string, Definition> definitions;
        private Dictionary<string, Type> componentTypes;
        private readonly List<Component> components = new List<Component>();

        internal void AddComponents(Component[] cList)
        {
            components.AddRange(cList);
        }

        internal Type TryGetComponent(string name)
        {
            componentTypes.TryGetValue(name, out var t);
            return t;
        }

        internal Definition TryGetDefinition(string name)
        {
            definitions.TryGetValue(name, out var d);
            if (d != null)
                d.Parse(this);
            return d;
        }

        public Processor Build(string definition = "Main")
        {
            definitions = this.ToDictionary(d => d.Name);
            componentTypes = typeof(Component).Assembly.GetTypes().Where(t => t != typeof(Component) && typeof(Component).IsAssignableFrom(t)).ToDictionary(t => t.Name);

            var main = definitions[definition];
            main.Parse(this);
            main.Instantiate(this, "");
            return new Processor(components.ToArray());
        }
    }
}
