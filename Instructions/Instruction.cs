using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Instructions
{
    internal abstract class Instruction
    {
        public abstract void execute(Processor processor);
    }

    enum Operation
    {
        add,
        sub,
        mul,
        div,
        mod
    }

    internal class ArithmeticInstruction<T, U> : Instruction
    {


        private T RS1;
        private U RS2;
        private string RD;
        Operation operation;

        public ArithmeticInstruction(T RS1, U RS2, string RD, Operation operation)
        {
            if ((typeof(T) != typeof(string) && typeof(T) != typeof(int)) || (typeof(U) != typeof(string) && typeof(U) != typeof(int)))
            {
                throw new InvalidOperationException("Uma instrução aritmética só pode possuir int ou string para RS1 e RS2");
            }
            this.RS1 = RS1;
            this.RS2 = RS2;
            this.RD = RD;
            this.operation = operation;
        }

        public override void execute(Processor processor)
        {
            int value1, value2;
            int result;
            if (this.RS1 is int)
            {
                value1 = Convert.ToInt32(this.RS1);
            }
            else
            {
                value1 = processor.getRegister(Convert.ToString(this.RS1));
            }
            if (this.RS2 is int)
            {
                value2 = Convert.ToInt32(this.RS2);
            }
            else
            {
                value2 = processor.getRegister(Convert.ToString(this.RS2));
            }
            switch (operation)
            {
                case Operation.add: result = value1 + value2; break;
                case Operation.sub: result = value1 - value2; break;
                case Operation.mul: result = value1 * value2; break;
                case Operation.div: result = value1 / value2; break;
                case Operation.mod: result = value1 % value2; break;
                default: result = 0; break;
            }

            processor.setRegister(RD, result);
        }

    }
}
