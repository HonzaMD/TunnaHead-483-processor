using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hradla.Asm
{
    abstract class Assembler
    {
        private readonly Dictionary<string, int> labels = new Dictionary<string, int>();
        private readonly List<byte> mem = new List<byte>();
        private readonly List<(int pos, string label, int delta)> unresolvedBytes = new List<(int pos, string label, int delta)>();

        public int Length => mem.Count;

        public void Build(Components.Byte[] output)
        {
            foreach (var un in unresolvedBytes)
            {
                mem[un.pos] = (byte)(labels[un.label] + un.delta);
            }

            for (int f = 0; f < mem.Count; f++)
            {
                output[f].Data = mem[f];
            }
        }

        protected ArPrm AR { get; } = new ArPrm();
        protected BrPrm BR { get; } = new BrPrm();
        protected IpPrm IP { get; } = new IpPrm();
        protected IdPrm ID(int data) => new IdPrm() { Data = data };
        protected IdPrm ID(string label, int delta = 0) => new IdPrm() { Label = label, Delta = delta };

        protected void Label(string name) => labels.Add(name, Length);

        protected void Mov(AbiPrms to, AllParameters from) => WriteInstr(Code.Mov, to, from, true);
        protected void Ld(AbiPrms to, AllParameters from) => WriteInstr(Code.Mov | Code.M1, to, from, true);
        protected void St(AllParameters to, AllParameters from) => WriteInstr(Code.Mov | Code.M3, to, from, true);
        protected void Jmp(string label) => Mov(IP, ID(label));

        protected void Add(AbPrms acc, AllParameters prm) => WriteInstr(Code.Add, acc, prm);
        protected void AddLd(AbPrms acc, AllParameters prm) => WriteInstr(Code.Add | Code.M1, acc, prm);
        protected void Sub(AbPrms acc, AllParameters prm) => WriteInstr(Code.Sub, acc, prm);
        protected void SubLd(AbPrms acc, AllParameters prm) => WriteInstr(Code.Sub | Code.M1, acc, prm);
        protected void And(AbPrms acc, AllParameters prm) => WriteInstr(Code.And, acc, prm);
        protected void AndLd(AbPrms acc, AllParameters prm) => WriteInstr(Code.And | Code.M1, acc, prm);
        protected void Or(AbPrms acc, AllParameters prm) => WriteInstr(Code.Or, acc, prm);
        protected void OrLd(AbPrms acc, AllParameters prm) => WriteInstr(Code.Or | Code.M1, acc, prm);
        protected void Shl(AbPrms acc, AllParameters prm) => WriteInstr(Code.Shl, acc, prm);
        protected void ShlLd(AbPrms acc, AllParameters prm) => WriteInstr(Code.Shl | Code.M1, acc, prm);
        protected void Shr(AbPrms acc, AllParameters prm) => WriteInstr(Code.Shr, acc, prm);
        protected void ShrLd(AbPrms acc, AllParameters prm) => WriteInstr(Code.Shr | Code.M1, acc, prm);
        protected void Not(AbPrms result, AllParameters prm) => WriteInstr(Code.Not, result, prm);
        protected void NotLd(AbPrms result, AllParameters prm) => WriteInstr(Code.Not | Code.M1, result, prm);
        protected void Neg(AbPrms result, AllParameters prm) => WriteInstr(Code.Neg, result, prm);
        protected void NegLd(AbPrms result, AllParameters prm) => WriteInstr(Code.Neg | Code.M1, result, prm);

        protected void Jz(IdPrm label) => Jump(Code.Jz, label);
        protected void Jnz(IdPrm label) => Jump(Code.Jnz, label);
        protected void Jl(IdPrm label) => Jump(Code.Jl, label);
        protected void Jge(IdPrm label) => Jump(Code.Jge, label);
        protected void Jg(IdPrm label) => Jump(Code.Jg, label);
        protected void Jle(IdPrm label) => Jump(Code.Jle, label);
        protected void J0(IdPrm label) => Jump(Code.J0, label);
        protected void Jn0(IdPrm label) => Jump(Code.Jn0, label);

        protected void DSF() => mem.Add((byte)Code.DSF);
        protected void Brw() => mem.Add((byte)Code.Brw);

        private void WriteInstr(Code code, AllParameters to, AllParameters from, bool isMove = false)
        {
            if (from.Code == PrmCode.ID && to.Code == PrmCode.ID)
                throw new InvalidOperationException("Konstanta ID nesmi byt pouzita u obou parametru");

            code |= from.AsP1 | (isMove ? to.AsP3 : to.AsP2);
            mem.Add((byte)code);

            IdPrm idp = from as IdPrm ?? to as IdPrm;
            if (idp != null)
            {
                WriteIdPrm(idp);
            }
        }

        private void WriteIdPrm(IdPrm idp)
        {
            if (idp.Label != null)
            {
                Data(idp.Label, idp.Delta);
            }
            else
            {
                Data(idp.Data);
            }
        }

        private void Jump(Code code, IdPrm label)
        {
            mem.Add((byte)code);
            WriteIdPrm(label);
        }

        protected void Data(int data) => mem.Add((byte)data);
        protected void Data(string label, int delta = 0)
        {
            unresolvedBytes.Add((Length, label, delta));
            mem.Add(0);
        }

        protected void LData(string label, int data)
        {
            Label(label);
            Data(data);
        }
        protected void LData(string label, string labelToStore)
        {
            Label(label);
            Data(labelToStore);
        }
    }
}
