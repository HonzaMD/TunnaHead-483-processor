using Hradla.Asm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    class Program
    {
        static void Main()
        {
            var parser = new Parser()
            {
                Definition.Add(),
                Definition.IncByte(),
                Definition.Register(),
                Definition.Multiplex(),
                Definition.DeMultiplex(),
                Definition.Memory(),
                Definition.InstrDecoder(),
                Definition.Processor(),
                Definition.IfJumps(),
                Definition.ChangeState(),
                Definition.ALU(),
                Definition.Shl(),
                Definition.Flags(),

                new Definition("Main")
                {@"                   
Register reg
IncByte inc                    
+ reg.O[m] inc.I[m]
+ inc.O[m] reg.I[m]
Not n
+ n.O reg.W
                    
Byte sum
Add add
+ reg.O[m] add.I1[m]
+ reg.O[m] add.I2[m]
+ add.O[m] sum.I[m]
+ n.O sum.W

Memory mem
+ reg.O[m] mem.Sel[m]
+ add.O[m] mem.Bus[m]
+ n.O mem.W
                "},




                new Definition("SumTest")
                {@"
Register reg
IncByte inc                    
+ reg.O[m] inc.I[m]
+ inc.O[m] reg.I[m]
Not n
+ n.O reg.W
                    
Register sum
Add add
+ mem.Bus[m] add.I1[m]
+ sum.O[m] add.I2[m]
+ add.O[m] sum.I[m]
+ n.O sum.W

Memory mem
+ reg.O[m] mem.Sel[m]
+ n.O mem.R
                "},


            };

            var processor = parser.Build("Processor");
            
            var ip = new Led8Monitor(processor, ".IP.O");
            var ar = new Led8Monitor(processor, ".AR.O");
            var br = new Led8Monitor(processor, ".BR.O");
            var state = new Led8Monitor(processor, ".State.O");
            var ic = processor.GetByte(".IC");
            var ic2 = new LedNMonitor(processor, ".decoder.IC", 8);
            var id = processor.GetByte(".ID");
            var move = new LedNMonitor(processor, ".decoder.Mov", 1);
            var noArgs = new LedNMonitor(processor, ".decoder.NoArgs", 1);
            var usesID = new LedNMonitor(processor, ".decoder.UsesID", 1);
            var code = new LedNMonitor(processor, ".decoder.Code", 11);
            var select2 = new LedNMonitor(processor, ".decoder.Select2", 3);
            var mem = Enumerable.Range(0, 256).Select(i => processor.GetByte($".mem.byte[{i}]")).ToArray();
            var p1 = new Led8Monitor(processor, ".P1");
            var p2 = new Led8Monitor(processor, ".P2");
            var ain = new Led8Monitor(processor, ".AIN");
            var aout = new Led8Monitor(processor, ".AOUT");
            var mbus = new Led8Monitor(processor, ".mem.Bus");
            var jumpEnabled = new LedNMonitor(processor, ".changeState.JumpEnabled", 1);
            var skipJump = new LedNMonitor(processor, ".ifJumps.SkipJump", 1);
            var flags = new LedNMonitor(processor, ".flags.O", 8);
            var haltByte = mem[255];
            var instrDecoder = new DebugInstrReader(mem, ip, state);


            var prg = new QuickSort();
            prg.Build(mem);

            bool stepByStep = true;
            while (haltByte.Data == 0)
            {
                processor.RunCycle();

                instrDecoder.Update();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(instrDecoder);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($" | IP:{ip} ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"State:{state}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($" IC:{ic}[{ic2}] ID:{id} mov:{move} noArgs:{noArgs} usesId:{usesID} JE:{jumpEnabled} JS:{skipJump} instr:{code}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"AR:{ar} BR:{br} ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"P1:{p1} P2:{p2} AIN:{ain} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"AOUT:{aout} MBus:{mbus} ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Flags:{flags}");
                Console.WriteLine(String.Join(", ", mem.Take(25).Select(m => m.Data.ToString())));
                Console.WriteLine(String.Join(", ", mem.Skip(25).Take(25).Select(m => m.Data.ToString())));
                Console.WriteLine(String.Join(", ", mem.Skip(50).Take(25).Select(m => m.Data.ToString())));
                Console.WriteLine(String.Join(", ", mem.Skip(75).Take(25).Select(m => m.Data.ToString())));
                Console.WriteLine(String.Join(", ", mem.Skip(100).Take(25).Select(m => m.Data.ToString())));
                Console.WriteLine(String.Join(", ", mem.Skip(125).Take(25).Select(m => m.Data.ToString())));
                Console.WriteLine(String.Join(", ", mem.Skip(150).Take(25).Select(m => m.Data.ToString())));

                if (stepByStep)
                {
                    var str = Console.ReadLine();
                    if (str == "r")
                        stepByStep = false;
                }
            }

            Console.WriteLine("DONE");
        }
    }
}
