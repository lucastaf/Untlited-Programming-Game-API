using System;
using System.Reflection;

namespace Untlited_Programming_Game.Instructions
{
    public class BranchInstruction<T> : Instruction where T : notnull
    {
        private Branch BranchType;
        private string RS1;
        private T RS2;
        private int dest;

        public BranchInstruction(Branch branchType, string rS1, T rS2, int dest, int line) : base(line)
        {
            if ((typeof(T) != typeof(string) && typeof(T) != typeof(int)))
            {
                throw new InvalidOperationException("Uma instrução de Branch só pode possuir int ou string para RS2");
            }
            BranchType = branchType;
            RS1 = rS1;
            RS2 = rS2;
            this.dest = dest;
        }

        public override void execute(Processor processor)
        {
            int value1 = processor.getRegister(RS1);
            int value2 = 0;
            bool Valid;
            if (typeof(T) == typeof(string))
            {
                value2 = processor.getRegister((string)(object)RS2);
            }
            else
            {
                value2 = (int)(object)RS2;
            }
            switch (BranchType)
            {
                case Branch.beq: Valid = value1 == value2; break;
                case Branch.bne: Valid = value1 != value2; break;
                case Branch.bge: Valid = value1 >= value2; break;
                case Branch.ble: Valid = value1 <= value2; break;
                case Branch.bgt: Valid = value1 > value2; break;
                case Branch.blt: Valid = value1 < value2; break;
                default: Valid = false; break;
            }
            if (Valid)
            {
                processor.setRegister("RA", processor.getRegister("Counter", true), true);
                processor.setRegister("Counter", dest - 1, true);
            }

        }
    }



    public class GotoInstruction : Instruction
    {
        private string label;
        private int dest;
        public GotoInstruction(string label, int line, int dest) : base(line)
        {
            this.label = label;
            this.dest = dest;
        }

        public override void execute(Processor processor)
        {
            processor.setRegister("RA", processor.getRegister("Counter", true), true);
            processor.setRegister("Counter", dest - 1, true);
        }
    }

    public class ReturnInstruction : Instruction
    {
        private string? ReturnReg;
        public ReturnInstruction(string? returnReg, int line) : base(line)
        {
            if (ReturnReg != null) this.ReturnReg = returnReg;
        }

        public override void execute(Processor processor)
        {
            int returnPoint;
            if (ReturnReg != null)
            {
                returnPoint = processor.getRegister(ReturnReg, true);
            }
            else
            {
                returnPoint = processor.getRegister("RA", true);
            }
            processor.setRegister("Counter", returnPoint, true);
        }
    }
}
