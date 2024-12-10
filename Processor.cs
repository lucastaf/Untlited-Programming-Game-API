using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game
{
    internal class Processor
    {
        public class Register
        {
            public Register(bool readable, bool writable)
            {
                this.readable = readable; this.writable = writable;
            }
            public int value = 0;
            public bool writable { get; private set; } = true;
            public bool readable { get; private set; } = true;
        }

        //Manter publico para fazer as interfaces graficas na UNITY
        public Dictionary<string, Register> Registers { get ; private set; } = new Dictionary<string, Register>();

        private List<Instruction> Instructions = new List<Instruction>();
        public void setRegister(string name, int value)
        {
            this.Registers[name].value = value;
        } 
        public int getRegister(string name)
        {
            return Registers[name].value;  
        }
        public Processor() { 
            this.Registers.Add("ZERO", new Register(true, false));
            this.Registers.Add("A", new Register(true, true));
            this.Registers.Add("B", new Register(true, true));
            this.Registers.Add("C", new Register(true, true));
            this.Registers.Add("D", new Register(true, true));
            this.Registers.Add("Counter", new Register(true, false));
            this.Registers.Add("IN", new Register(true, false));
            this.Registers.Add("OUT", new Register(false, true));
        }


        public void addInstruction(Instruction instruction) { 
            this.Instructions.Add(instruction);
        }

        public void loadProgram(Instruction[] instructions)
        {
            this.Instructions.AddRange(instructions);
        }

        public int getLabel(string label)
        {
            int index = 0;
            foreach (var instruction in this.Instructions) {
                if(instruction is LabelInstruction)
                {
                    if(((LabelInstruction)instruction).label == label){
                        return index;
                    }
                }

                index++;
            }
            return -1;
        }

        public void Execute()
        {
            this.Instructions[Registers["Counter"].value].execute(this);
            this.Registers["Counter"].value ++;
        }

        public void RunTillFinish()
        {
            while (this.Registers["Counter"].value < this.Instructions.Count)
            {
                this.Execute();
            }
        }

        public void Print()
        {
            foreach (var register in this.Registers)
            {
                Console.WriteLine( $"{register.Key} = {register.Value.value}");
            }
        }




    }
}
