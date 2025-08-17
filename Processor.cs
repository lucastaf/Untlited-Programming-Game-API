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
        public delegate void OnInstructionEnd(List<(string register, int value)> registersReaded, List<(string register, int value)> registersWritten, Instruction instruction);
        public delegate void Handler();

        public OnReadDelegate? onRead;
        public OnChangeDelegate? onChange;
        public Handler? onEnd;
        public Handler? onPause;
        public OnInstructionEnd? onInstructionEnd;

        private List<(string, int)> registersReaded = new List<(string, int)>();
        private List<(string, int)> registersWritten = new List<(string, int)>();

        //Manter publico para fazer as interfaces graficas na UNITY
        public Dictionary<string, Register> Registers { get; private set; } = new Dictionary<string, Register>();

        public Processor((string name, bool readable, bool writable)[] registers)
        {
            foreach (var reg in registers)
            {
                this.Registers.Add(reg.name, new Register(reg.readable, reg.writable));
            }
            this.Registers.Add("Counter", new Register(true, false));
            this.Registers.Add("RA", new Register(true, true));

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
                    onChange?.Invoke(name, value);
                    registersWritten.Add((name, value));
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
                    onRead?.Invoke(name, register.value);
                    this.registersReaded.Add((name, register.value));
                    return register.value;
                }
                else
                {
                    throw new UnauthorizedRegisterException(0, AccessType.read);
                }
            }
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

        public bool Execute()
        {
            Instruction instruction;
            if (this.Registers["Counter"].value < this.Instructions.Count)
                instruction = this.Instructions[Registers["Counter"].value];
            else
            {
                return false;
            }

            try
            {
                instruction.execute(this);
                if (this.Registers["Counter"].value == this.Instructions.Count - 1)
                    onEnd?.Invoke();
                onInstructionEnd?.Invoke(registersReaded, registersWritten, instruction);
                registersReaded.Clear();
                registersWritten.Clear();
                this.Registers["Counter"].value++;
                return true;
            }
            catch (CodeException e)
            {
                e.line = instruction.line;
                throw e;
            }
        }

        public int getCurrentLine()
        {
            int counter = this.Registers["Counter"].value;
            if (counter >= this.Instructions.Count)
                counter = this.Instructions.Count - 1;

            Instruction currentIntrustion = this.Instructions[counter];
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

        public void Clear()
        {
            foreach (var register in this.Registers)
            {
                register.Value.value = 0;
            }
        }

    }
}
