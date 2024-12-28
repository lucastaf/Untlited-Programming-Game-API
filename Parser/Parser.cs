using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Instructions;
using Untlited_Programming_Game.Exceptions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Untlited_Programming_Game.Parser
{
    public partial class Parser
    {
        public Parser()
        {
            InstructionsCode = new Dictionary<string, Func<string, int, Instruction>>()
            {
                {"IF", parseBranchInstruction },
                //Desativados funções de print e READ pois não serão usadas no game final
                //{"PRINT", parsePrintInstruction },
                //{"READ", parseReadInstruction},
                {"LABEL", parseLabelInstruction },
                {"MACRO", parseMacroInstruction},
                {"GOTO", parseGotoInstruction},
            };
        }

        private readonly Dictionary<string, Func<string, int, Instruction>> InstructionsCode;

        public List<Instruction> Instructions = new List<Instruction>();

        public List<CodeException> Exceptions = new List<CodeException>();

        private Dictionary<string, int> macros = new Dictionary<string, int>();

        public Instruction[] parseProgram(string instructionText)
        {
            this.Instructions.Clear();
            this.Exceptions.Clear();


            int line = 0;
            Dictionary<string, int> macros = new Dictionary<string, int>();
            foreach (string instruction in Regex.Split(instructionText.ToUpper(), "\r\n|\r|\n"))
            {
                try
                {
                    if (instruction == "" || instruction[0] == '#')
                    {
                        line++;
                        continue;
                    }
                    Instruction newInstruction = parseInstruction(instruction, line);
                    if (!(newInstruction is MacroInstruction))
                    {
                        Instructions.Add(newInstruction);
                    }
                }
                catch (CodeException e)
                {
                    Exceptions.Add(e);
                }
                line++;
            }
            return Instructions.ToArray();
        }

        public Instruction parseInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            try
            {
                Func<string, int, Instruction> parseFunction;
                if (InstructionsCode.TryGetValue(instructionParts[0], out parseFunction))
                {
                    return parseFunction(instructionText, line);
                }
                else if (instructionParts[1] == "=")
                {
                    return parseArithmeticInstruction(instructionText, line);
                }
                else
                {
                    throw new InvalidInstructionException(line);
                };

            }
            catch (Exception e)
            {
                if (e is CodeException)
                {
                    ((CodeException)e).line = line;
                    throw e;
                }
                throw new InvalidInstructionException(line);
            }
        }

        public void addInstruction(Instruction instruction)
        {
            for (int i = 0; i < this.Instructions.Count; i++)
            {
                Instruction currentInstruction = this.Instructions[i];
                if (Exceptions[i].line == instruction.line)
                {
                    Exceptions.RemoveAt(i);
                }
                if (instruction.line < currentInstruction.line)
                {
                    this.Instructions.Insert(i, currentInstruction);
                    return;
                }
            }
            this.Instructions.Add(instruction);

        }



    }


}
