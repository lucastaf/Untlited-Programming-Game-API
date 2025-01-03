﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Instructions
{
    public abstract class Instruction
    {
        public Instruction(int line)
        {
            this.line = line;
        }

        public int line { get; private set; }
        public abstract void execute(Processor processor);
    }

    public enum Operation
    {
        add,
        sub,
        mul,
        div,
        mod
    }

    public enum Branch
    {
        beq,
        bne,
        bgt,
        blt,
        ble,
        bge,
    }

    public class PrintInstruction : Instruction
    {
        private string reg;
        public PrintInstruction(string reg, int line) : base(line)
        {
            this.reg = reg;
        }

        public override void execute(Processor processor)
        {
            Console.WriteLine(reg + " = " + processor.getRegister(reg));
        }
    }

    public class ReadInstruction : Instruction
    {
        private string reg;
        public ReadInstruction(string reg, int line) : base(line)
        {
            this.reg = reg;
        }

        public override void execute(Processor processor)
        {
            string resultString = Console.ReadLine() ?? "";
            int result = Int32.Parse(resultString);
            processor.setRegister(this.reg, result);
        }
    }

    public class MacroInstruction : Instruction
    {
        public MacroInstruction(int line) : base(line) { }
        public override void execute(Processor processor)
        {
            throw new NotImplementedException();
        }
    }
}
