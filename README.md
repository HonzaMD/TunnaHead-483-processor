# TunnaHead-483-processor
Simple 8 bit processor build on logic gate simulator

## 4 ways to select operand

* AR general purpose register and accumulator
* BR general purpose register and accumulator
* IP instruction pointer
* ID constant

## 8 Bit

* 256 bytes of memory
* instructions are coded in 1 or 2 bytes. (2nd byte is data constant when needed)
* MOV, up to 11 arithmetic instructions, up to 16 jumps or special instructions.

## max 3 cycles to execute an instruction

1. loads and decodes instruction
2. loads the optional constant
3. executes

## Simple text syntax to create circuits

```
Xor xorI 8
Xor xorC 8
+ I1[m] xorI[m].I1
+ I2[m] xorI[m].I2
+ xorI[m].O xorC[m].I1
+ c[m].O xorC[m+1].I2
+ xorC[m].O O[m]
```

## Assembler support and samples

```csharp
    class Fibonacci : Assembler
    {
        public Fibonacci()
        {
            Mov(BR, "fibStart");
            Label("loop");
            Ld(AR, BR);
            Add(BR, 1);
            AddLd(AR, BR);
            Add(BR, 1);
            St(BR, AR);
            Sub(BR, 1);
            Jmp("loop");
            Label("fibStart");
            Data(1);
            Data(1);
        }
    }
```