using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Exceptions;
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


        public delegate void OnReadDelegate(string register, int value);
        public delegate void OnChangeDelegate(string register, int value);

        public OnReadDelegate onRead;
        public OnChangeDelegate onChange;

        //Manter publico para fazer as interfaces graficas na UNITY
        public Dictionary<string, Register> Registers { get; private set; } = new Dictionary<string, Register>();

        private List<Instruction> Instructions = new List<Instruction>();
        public void setRegister(string name, int value)
        {
            Register? register;
            bool registerExists = Registers.TryGetValue(name, out register);
            if (!registerExists)
            {
                throw new InvalidRegisterException(0);
            }
            else
            {
                if (register.writable)
                {
                    onChange(name, value);
                    register.value = value;
                    return;
                }
                else
                {
                    throw new UnauthorizedRegisterException(0, AccessType.write);
                }
            }

        }
        public int getRegister(string name)
        {
            Register? register;
            bool registerExists = Registers.TryGetValue(name, out register);
            if (!registerExists)
            {
                throw new InvalidRegisterException(0);
            }
            else
            {
                if (register.readable)
                {
                    onRead(name,  register.value);
                    return register.value;
                }
                else
                {
                    throw new UnauthorizedRegisterException(0, AccessType.read);
                }
            }
        }
        public Processor()
        {
            this.Registers.Add("ZERO", new Register(true, false));
            this.Registers.Add("A", new Register(true, true));
            this.Registers.Add("B", new Register(true, true));
            this.Registers.Add("C", new Register(true, true));
            this.Registers.Add("D", new Register(true, true));
            this.Registers.Add("Counter", new Register(true, false));
            this.Registers.Add("IN", new Register(true, false));
            this.Registers.Add("OUT", new Register(false, true));
        }


        public void addInstruction(Instruction instruction)
        {
            this.Instructions.Add(instruction);
        }

        public void loadProgram(Instruction[] instructions)
        {
            this.Instructions.AddRange(instructions);
        }

        public int getLabel(string label)
        {
            int index = 0;
            foreach (var instruction in this.Instructions)
            {
                if (instruction is LabelInstruction)
                {
                    if (((LabelInstruction)instruction).label == label)
                    {
                        return index;
                    }
                }

                index++;
            }
            throw new InvalidLabelException(0);
        }

        public void Execute()
        {
            Instruction instruction = this.Instructions[Registers["Counter"].value];
            try
            {
                instruction.execute(this);
                this.Registers["Counter"].value++;
            }
            catch (CodeException e)
            {
                e.line = instruction.line;
                throw e;
            }
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
                Console.WriteLine($"{register.Key} = {register.Value.value}");
            }
        }




    }
}
