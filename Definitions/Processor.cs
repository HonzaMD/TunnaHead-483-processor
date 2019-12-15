using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla
{
    partial class Definition
    {
        public static Definition Processor() => new Definition("Processor")
                {@"

Register AR
Register BR
Register IP
Byte IC
Byte ID
Register State

InstrDecoder decoder
Memory mem

// vstupy pro instr dekoder
Cross decoderIn 8
+ mem.Bus[m] decoderIn[m].I1
+ IC.O[m] decoderIn[m].I2
+ State.O[0] decoderIn[m].C
+ decoderIn[m].O1 decoder.IC[m]
+ State.O[2] decoder.State2
+ notState0.O decoder.notState0

Not notState0
+ State.O[0] notState0.I
Or State0or1or3
Or State0or1
+ notState0.O State0or1.I1
+ State.O[1] State0or1.I2
+ State0or1.O State0or1or3.I1
+ State.O[3] State0or1or3.I2

// ve stavu 0, 1 a 3 ctu [IP]
And ipAddrGate 8
Or memR1
+ State0or1or3.O ipAddrGate[m].I1
+ IP.O[m] ipAddrGate[m].I2
+ ipAddrGate[m].O mem.Sel[m]
+ State0or1or3.O memR1.I1
+ memR1.O mem.R

// stav 0: [IP] -> IC
And icLoadGate 8
+ notState0.O icLoadGate[m].I1
+ mem.Bus[m] icLoadGate[m].I2
+ icLoadGate[m].O IC.I[m]
+ notState0.O IC.W

// stav 1: [IP] -> ID
And idLoadGate 8
+ State.O[1] idLoadGate[m].I1
+ mem.Bus[m] idLoadGate[m].I2
+ idLoadGate[m].O ID.I[m]
+ State.O[1] ID.W

// If Jumps zapojeni
IfJumps ifJumps
+ notState0.O ifJumps.NotState0
+ decoder.JmpCat ifJumps.JmpCat
+ decoder.JmpCode[m] ifJumps.JmpCode[m]
+ flags.O[m] ifJumps.Flags[m]

// nastaveni IP
Or IPIncEnable
Or ipw1
+ ifJumps.notSkipJump IPIncEnable.I1
+ State.O[1] IPIncEnable.I2
+ State0or1or3.O ipw1.I1
+ ipw1.O IP.W

// IP z ifJumpu
// stav 3: [IP] -> IP
And ipLoadGate 8
+ State.O[3] ipLoadGate[m].I1
+ mem.Bus[m] ipLoadGate[m].I2
+ ipLoadGate[m].O IP.I[m]

// Inc IP
IncByte incIp
And incIpGate 8
+ IP.O[m] incIp.I[m]
+ incIp.O[m] incIpGate[m].I1
+ IPIncEnable.O incIpGate[m].I2
+ incIpGate[m].O IP.I[m]

// Inc IP 2x
IncByte incIp2
And incIp2xGate 8
+ incIp.O[m] incIp2.I[m]
+ incIp2.O[m] incIp2xGate[m].I1
+ ifJumps.SkipJump incIp2xGate[m].I2
+ incIp2xGate[m].O IP.I[m]

// Change state
ChangeState changeState
Not one
+ State.O[m] changeState.I[m]
+ changeState.O[m] State.I[m]
+ one.O State.W
+ decoder.NoArgs changeState.NoArgs
+ decoder.UsesID changeState.UsesID
+ ifJumps.Enabled changeState.JumpEnabled



* P1 8
* P2 8
* AIN 8
* AOUT 8

Cross pc11 16
+ IC.O[1] pc11[m].C
+ AR.O[m] pc11[m*2].I1
+ BR.O[m] pc11[m*2].I2
+ IP.O[m] pc11[m*2+1].I1
+ ID.O[m] pc11[m*2+1].I2

Cross pc12 8
+ IC.O[2] pc12[m].C
+ pc11[m*2].O1 pc12[m].I1
+ pc11[m*2+1].O1 pc12[m].I2
+ pc12[m].O1 P1[m]

Cross pc21 16
+ decoder.Select2[1] pc21[m].C
+ AR.O[m] pc21[m*2].I1
+ BR.O[m] pc21[m*2].I2
+ IP.O[m] pc21[m*2+1].I1
+ ID.O[m] pc21[m*2+1].I2

Cross pc22 8
+ decoder.Select2[2] pc22[m].C
+ pc21[m*2].O1 pc22[m].I1
+ pc21[m*2+1].O1 pc22[m].I2
+ pc22[m].O1 P2[m]

And memReadEnable
Or memR2
+ State.O[2] memReadEnable.I1
+ IC.O[0] memReadEnable.I2
+ memReadEnable.O memR2.I1
+ memR2.O mem.R

And memReadSelGate 8
+ P1[m] memReadSelGate[m].I1
+ memReadEnable.O memReadSelGate[m].I2
+ memReadSelGate[m].O mem.Sel[m]

Cross memReadCross 8
+ memReadEnable.O memReadCross[m].C
+ P1[m] memReadCross[m].I1
+ mem.Bus[m] memReadCross[m].I2
+ memReadCross[m].O1 AIN[m]

And movGate 8
+ decoder.ActiveMov movGate[m].I1
+ AIN[m] movGate[m].I2
+ movGate[m].O AOUT[m]

And memWriteEnable
+ State.O[2] memWriteEnable.I1
+ decoder.Select2[0] memWriteEnable.I2
+ memWriteEnable.O mem.W

And memWriteSelGate 8
+ P2[m] memWriteSelGate[m].I1
+ memWriteEnable.O memWriteSelGate[m].I2
+ memWriteSelGate[m].O mem.Sel[m]

Cross memWriteCross 8
+ memWriteEnable.O memWriteCross[m].C
+ AOUT[m] memWriteCross[m].I1
+ memWriteCross[m].O2 mem.Bus[m]
+ memWriteCross[m].O1 wc1[m].I1

Cross wc1 8
+ decoder.Select2[2] wc1[m].C
+ wc1[m].O1 wc2[m*2].I1
+ wc1[m].O2 wc2[m*2+1].I1

Cross wc2 16
+ decoder.Select2[1] wc2[m].C
+ wc2[m*2].O1 AR.I[m]
+ wc2[m*2].O2 BR.I[m]
+ wc2[m*2+1].O1 IP.I[m]

Cross wcEnable0
+ memWriteEnable.O wcEnable0.C
+ State.O[2] wcEnable0.I1
+ wcEnable0.O1 wcEnable1.I1

Cross wcEnable1
+ decoder.Select2[2] wcEnable1.C
+ wcEnable1.O1 wcEnable2[0].I1
+ wcEnable1.O2 wcEnable2[1].I1

Cross wcEnable2 2
+ decoder.Select2[1] wcEnable2[m].C
+ wcEnable2[0].O1 AR.W
+ wcEnable2[0].O2 BR.W
+ wcEnable2[1].O1 IP.W


ALU alu
+ AIN[m] alu.I1[m]
+ P2[m] alu.I2[m]
+ alu.O[m] AOUT[m]
+ decoder.Code[m] alu.Code[m]


Flags flags
+ AOUT[m] flags.AOUT[m]
+ alu.Carry flags.carry
+ State.O[2] flags.W
+ decoder.SpCode[0] flags.SetDSF
+ decoder.SpCode[1] flags.SetBrw

                " };
    }
}
