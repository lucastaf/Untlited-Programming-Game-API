using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Instructions
{
    internal interface Instruction
    {
        public void execute(Processor processor);
    }

    enum Operation
    {
        add,
        sub,
        mul,
        div,
        mod
    }

    enum Branch
    {
        beq,
        bne,
        bgt,
        blt,
        ble,
        bge,
    }

    internal class PrintInstruction : Instruction
    {
        private string reg;
        public PrintInstruction(string reg)
        {
            this.reg = reg;
        }

        public void execute(Processor processor)
        {
            Console.WriteLine(reg + " = " + processor.getRegister(reg));
        }
    }

    internal class ReadInstruction : Instruction
    {
        private string reg;
        public ReadInstruction(string reg)
        {
            this.reg = reg;
        }

        public void execute(Processor processor)
        {
            string resultString = Console.ReadLine();
            int result = Int32.Parse(resultString);
            processor.setRegister(this.reg, result);
        }
    }

    internal class MacroInstruction : Instruction
    {
        public void execute(Processor processor)
        {
            throw new NotImplementedException();
        }
    }
}
