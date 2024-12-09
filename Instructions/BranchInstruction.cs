using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Instructions
{
    internal class BranchInstruction<T> : Instruction
    {
        private Branch BranchType;
        private string RS1;
        private T RS2;
        private int Dest;

        public BranchInstruction(Branch branchType, string rS1, T rS2, int dest)
        {
            if ((typeof(T) != typeof(string) && typeof(T) != typeof(int)))
            {
                throw new InvalidOperationException("Uma instrução de Branch só pode possuir int ou string para RS2");
            }
            BranchType = branchType;
            RS1 = rS1;
            RS2 = rS2;
            Dest = dest;
        }

        public void execute(Processor processor)
        {
            int value1 = processor.getRegister(RS1);
            int value2 = 0;
            bool Valid;
            if (typeof(T) == typeof(string)){
                value2 = processor.getRegister(Convert.ToString(RS2));
            }
            else
            {
                value2 = Convert.ToInt32(RS2);
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
                processor.setRegister("Counter", Dest - 1);
            }
            
        }
    }
}
