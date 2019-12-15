using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    enum PrmCode
    {
        AR,
        BR,
        IP,
        ID,
    }

    abstract class AllParameters
    {
        public abstract PrmCode Code { get; }
        public Code AsP1 => (Code)((byte)Code << 1);
        public Code AsP3 => (Code)((byte)Code << 4);
        public Code AsP2 => Code == PrmCode.AR ? Asm.Code.AR2 : Code == PrmCode.BR ? Asm.Code.BR2 : throw new InvalidOperationException("nepoveleny prevod");

        public static implicit operator AllParameters(byte data) => new IdPrm() { Data = data };
        public static implicit operator AllParameters(string label) => new IdPrm() { Label = label };
    }

    class IdPrm : AllParameters
    {
        public override PrmCode Code => PrmCode.ID;
        public int Data;
        public string Label;
        public int Delta;

        public static implicit operator IdPrm(byte data) => new IdPrm() { Data = data };
        public static implicit operator IdPrm(string label) => new IdPrm() { Label = label };
    }

    abstract class AbiPrms : AllParameters
    { }

    class IpPrm : AbiPrms
    {
        public override PrmCode Code => PrmCode.IP;
    }

    abstract class AbPrms : AbiPrms
    { }

    class ArPrm : AbPrms
    {
        public override PrmCode Code => PrmCode.AR;
    }

    class BrPrm : AbPrms
    {
        public override PrmCode Code => PrmCode.BR;
    }
}
