using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition : List<string>
    {
        public string Name { get; }
        private readonly List<(Type, string)> components = new List<(Type, string)>();
        private readonly List<(Definition, string)> definitions = new List<(Definition, string)>();
        private readonly List<(int, int, int, int)> connections = new List<(int, int, int, int)>();
        private readonly List<(int, int)> publicPins = new List<(int, int)>();
        public List<string> PublicPinsNames { get; } = new List<string>();
        private Color color;

        public Definition(string name)
        {
            Name = name;
        }

        public Pin[] Instantiate(Parser parser, string namePrefix)
        {
            var cList = components.Select(ct => 
            {
                var c = (Component)Activator.CreateInstance(ct.Item1);
                c.Name = $"{namePrefix}.{ct.Item2}";
                return c;
            }).ToArray();
            var dList = definitions.Select(d => d.Item1.Instantiate(parser, $"{namePrefix}.{d.Item2}")).ToArray();
            var pList = cList.Select(c => c.Pins).Concat(dList).ToArray();

            foreach (var conn in connections)
            {
                Wire.Connect(pList[conn.Item1][conn.Item2], pList[conn.Item3][conn.Item4]);
            }

            parser.AddComponents(cList);

            return publicPins.Select(pp => pList[pp.Item1][pp.Item2]).ToArray();
        }


        public void Parse(Parser parser)
        {
            try
            {
                if (color == Color.Black)
                    return;
                if (color == Color.Gray)
                    throw new InvalidOperationException("cyklus v definicich");
                color = Color.Gray;

                string[][] tokens = Preprocess();
                var dp = new DefinitionParser(tokens, parser, this);
                dp.ParseComponents();
                dp.ParseDefinitions();
                dp.ParseConnections();

                color = Color.Black;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Chyba pri parsovani definice " + Name, ex);
            }
        }

        private string[][] Preprocess()
        {
            return this.SelectMany(s => s.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)).Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToArray())
                .ToArray();
        }

        private enum Color
        {
            White, Gray, Black
        }
    }
}
