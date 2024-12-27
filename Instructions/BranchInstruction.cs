using System;

namespace Untlited_Programming_Game.Instructions
{
    public class BranchInstruction<T, U> : Instruction where T : notnull where U : notnull
    {
        private Branch BranchType;
        private string RS1;
        private T RS2;
        private U Dest;

        public BranchInstruction(Branch branchType, string rS1, T rS2, U dest, int line) : base(line)
        {
            if ((typeof(T) != typeof(string) && typeof(T) != typeof(int)))
            {
                throw new InvalidOperationException("Uma instrução de Branch só pode possuir int ou string para RS2");
            }
            if ((typeof(U) != typeof(string) && typeof(U) != typeof(int)))
            {
                throw new InvalidOperationException("Uma instrução de Branch só pode possuir int ou string para Destino");
            }
            BranchType = branchType;
            RS1 = rS1;
            RS2 = rS2;
            Dest = dest;
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
                if (typeof(U) == typeof(int))
                {
                    int counter = processor.getRegister("Counter", true);
                    processor.setRegister("Counter", (int)(object)Dest + counter - 1, true);
                }else if (typeof(U) == typeof(string))
                {
                    int counter = processor.getLabel((string)(object)Dest);
                    processor.setRegister("Counter", counter - 1, true);
                }
            }

        }
    }

    public class LabelInstruction : Instruction
    {
        public LabelInstruction(string label, int line) : base(line)
        {
            this.label = label;
        }

        public string label { get; private set; }

        public override void execute(Processor processor)
        {
            //Manter função vazia
        }
    }
}
