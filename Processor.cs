using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Exceptions;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game
{
    public class Processor
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

        public OnReadDelegate? onRead;
        public OnChangeDelegate? onChange;

        //Manter publico para fazer as interfaces graficas na UNITY
        public Dictionary<string, Register> Registers { get; private set; } = new Dictionary<string, Register>();

        public Processor((string name, bool readable, bool writable)[] registers)
        {
            foreach (var reg in registers)
            {
                this.Registers.Add(reg.name, new Register(reg.readable, reg.writable));
            }
            this.Registers.Add("Counter", new Register(true, false));

        }
        private List<Instruction> Instructions = new List<Instruction>();
        public void setRegister(string name, int value, bool forced = false)
        {
            Register? register;
            bool registerExists = Registers.TryGetValue(name, out register);
            if (!registerExists)
            {
                throw new InvalidRegisterException(0);
            }
            else
            {
                if (register.writable || forced)
                {
                    if (onRead != null) onChange(name, value);
                    register.value = value;
                    return;
                }
                else
                {
                    throw new UnauthorizedRegisterException(0, AccessType.write);
                }
            }

        }
        public int getRegister(string name, bool forced = false)
        {
            Register? register;
            bool registerExists = Registers.TryGetValue(name, out register);
            if (!registerExists)
            {
                throw new InvalidRegisterException(0);
            }
            else
            {
                if (register.readable || forced)
                {
                    if (onRead != null) onRead(name, register.value);
                    return register.value;
                }
                else
                {
                    throw new UnauthorizedRegisterException(0, AccessType.read);
                }
            }
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

        public void addInstruction(Instruction instruction)
        {
            for (int i = 0; i < this.Instructions.Count; i++)
            {
                Instruction currentInstruction = this.Instructions[i];
                if (instruction.line < currentInstruction.line)
                {
                    this.Instructions.Insert(i, currentInstruction);
                    return;
                }
            }
            this.Instructions.Add(instruction);
        }

        public void loadProgram(Instruction[] instructions)
        {
            this.Instructions.Clear();
            this.Instructions.AddRange(instructions);
            this.setRegister("Counter", 0, true);
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

        public int getCurrentLine()
        {
            Instruction currentIntrustion = this.Instructions[Registers["Counter"].value];
            return currentIntrustion.line;
        }

        public Instruction getInstruction(int index)
        {
            return this.Instructions[index];
        }

        public void RunTillFinish()
        {
            while (this.Registers["Counter"].value < this.Instructions.Count)
            {
                this.Execute();
            }
        }


    }
}
