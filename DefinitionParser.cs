using Hradla.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        private class DefinitionParser
        {
            private readonly string[][] tokens;
            private readonly Parser parser;
            private readonly Definition parent;
            private readonly Dictionary<string, NameX> Names = new Dictionary<string, NameX>();

            public DefinitionParser(string[][] tokens, Parser parser, Definition parent)
            {
                this.tokens = tokens;
                this.parent = parent;
                this.parser = parser;
            }

            internal void ParseComponents()
            {
                foreach (var line in tokens)
                {
                    int index = parent.components.Count();
                    bool outPin = line[0] == "^";
                    Type ct = null;
                    if (line[0] == "*" || outPin)
                    {
                        ct = typeof(Led);
                    }
                    else
                    {
                        ct = parser.TryGetComponent(line[0]);
                    }

                    if (ct != null)
                    {
                        var name = new NameX(line, index, ct);
                        Names.Add(name.Name, name);
                        for (int f = 0; f < name.Count; f++)
                        {
                            parent.components.Add((ct, name.GetName(f)));
                            if (outPin)
                            {
                                parent.publicPins.Add((index + f, 0));
                                parent.PublicPinsNames.Add(name.GetName(f));
                            }
                        }
                    }
                }
            }

            internal void ParseDefinitions()
            {
                foreach (var line in tokens)
                {
                    int index = parent.components.Count() + parent.definitions.Count();
                    Definition def = parser.TryGetDefinition(line[0]);

                    if (def != null)
                    {
                        var name = new NameX(line, index, def);
                        Names.Add(name.Name, name);
                        for (int f = 0; f < name.Count; f++)
                        {
                            parent.definitions.Add((def, name.GetName(f)));
                        }
                    }
                }
            }

            internal void ParseConnections()
            {
                foreach (var line in tokens)
                {
                    if (line[0] == "+")
                    {
                        int i1 = line[1].IndexOf('.');
                        var c1 = i1 == -1 ? new LinkDesc(line[1]) : new LinkDesc(line[1].Substring(0, i1));
                        var p1 = i1 == -1 ? new LinkDesc(Names[c1.Name].Pins.First().Key) : new LinkDesc(line[1].Substring(i1 + 1));
                        int i2 = line[2].IndexOf('.');
                        var c2 = i2 == -1 ? new LinkDesc(line[2]) : new LinkDesc(line[2].Substring(0, i2));
                        var p2 = i2 == -1 ? new LinkDesc(Names[c2.Name].Pins.First().Key) : new LinkDesc(line[2].Substring(i2 + 1));

                        c1.Namex = Names[c1.Name];
                        p1.Namex = Names[c1.Name].Pins[p1.Name];
                        c2.Namex = Names[c2.Name];
                        p2.Namex = Names[c2.Name].Pins[p2.Name];


                        int mstart = 0;
                        int mend = 0;
                        int nstart = 0;
                        int nend = 0;
                        c1.AdjustLoop(ref mstart, ref mend, Iteration.M);
                        p1.AdjustLoop(ref mstart, ref mend, Iteration.M);
                        c2.AdjustLoop(ref mstart, ref mend, Iteration.M);
                        p2.AdjustLoop(ref mstart, ref mend, Iteration.M);
                        c1.AdjustLoop(ref nstart, ref nend, Iteration.N);
                        p1.AdjustLoop(ref nstart, ref nend, Iteration.N);
                        c2.AdjustLoop(ref nstart, ref nend, Iteration.N);
                        p2.AdjustLoop(ref nstart, ref nend, Iteration.N);  

                        for (int m = mstart; m <= mend; m++)
                        {
                            for (int n = nstart; n <= nend; n++)
                            {
                                int? c1i = c1.GetIndex(m, n);
                                int? p1i = p1.GetIndex(m, n);
                                int? c2i = c2.GetIndex(m, n);
                                int? p2i = p2.GetIndex(m, n);

                                if (c1i != null && p1i != null && c2i != null && p2i != null)
                                    parent.connections.Add((c1i.Value, p1i.Value, c2i.Value, p2i.Value));
                            }
                        }
                    }
                }
            }
        }


        private class NameX
        {
            public readonly string Name;
            public readonly int Index;
            public readonly int Count;
            public readonly Dictionary<string, NameX> Pins = new Dictionary<string, NameX>();

            public NameX(string[] line, int index, Type ct)
                : this(line, index, ((Component)Activator.CreateInstance(ct)).Pins.Select(p => p.Name).ToArray())
            { }

            public NameX(string[] line, int index, Definition def)
                : this(line, index, def.PublicPinsNames.ToArray())
            { }

            public NameX(string[] line, int index, string[] pinNames)
            {
                Index = index;
                Name = line[1];
                Count = line.Length > 2 ? int.Parse(line[2]) : 1;

                if (pinNames != null && pinNames.Length > 0)
                {
                    var pinGroups = pinNames.Select(pn => SplitIndex(pn)).GroupBy(n => n);
                    int pi = 0;
                    foreach (var g in pinGroups)
                    {
                        int pgcount = g.Count();
                        var pname = new NameX(g.Key, pi, pgcount);
                        pi += pgcount;
                        Pins.Add(pname.Name, pname);
                    }
                }
            }

            public NameX(string name, int index, int count)
            {
                Name = name;
                Index = index;
                Count = count;
            }

            private static string SplitIndex(string pn)
            {
                int i = pn.IndexOf('[');
                return i == -1 ? pn : pn.Substring(0, i);
            }

            public string GetName(int index) => Count > 1 ? $"{Name}[{index}]" : Name;
        }


        private class LinkDesc
        {
            public readonly string Name;
            public readonly int Index;
            public readonly Iteration Iteration;
            public readonly int Delta;
            public readonly int Mult = 1;
            public NameX Namex;

            public LinkDesc(string link)
            {
                int ib = link.IndexOf('[');
                if (ib == -1)
                {
                    Name = link;
                }
                else
                {
                    int icb = link.IndexOf(']');
                    Name = link.Substring(0, ib);
                    if (link[ib+1] == 'm')
                    {
                        Iteration = Iteration.M;
                    }
                    else if (link[ib + 1] == 'n')
                    {
                        Iteration = Iteration.N;
                    }
                    else
                    {
                        Index = int.Parse(link.Substring(ib + 1, icb - ib - 1));
                    }

                    if (Iteration != Iteration.None && icb - ib > 2)
                    {
                        if (link[ib + 2] == '*')
                        {
                            int i = FindNotNumberIndex(link, ib + 4);
                            Mult = int.Parse(link.Substring(ib + 3, i - ib - 3));
                            if (i != icb)
                                Delta = int.Parse(link.Substring(i, icb - i));
                        }
                        else
                        {
                            Delta = int.Parse(link.Substring(ib + 2, icb - ib - 2));
                        }
                    }
                }
            }

            private int FindNotNumberIndex(string link, int i)
            {
                while (i < link.Length)
                {
                    if (!Char.IsDigit(link[i]))
                        return i;
                    i++;
                }
                return -1;
            }

            internal void AdjustLoop(ref int start, ref int end, Iteration iter)
            {
                if (Iteration == iter && Mult == 1)
                {
                    int s = -Delta;
                    int e = Namex.Count - 1 - Delta;
                    start = Math.Min(start, s);
                    end = Math.Max(end, e);
                }
            }

            internal int? GetIndex(int m, int n)
            {
                int i = Iteration == Iteration.None ? Index : Iteration == Iteration.M ? m * Mult + Delta : n * Mult + Delta;
                if (i < 0 || i >= Namex.Count)
                    return null;
                return Namex.Index + i;
            }
        }

        public enum Iteration
        {
            None,
            M,
            N,
        }
    }
}
