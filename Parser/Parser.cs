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
using System.Collections.ObjectModel;

namespace Untlited_Programming_Game.Parser
{
    public partial class Parser
    {
        public Parser()
        {
            InstructionsCode = new Dictionary<string, Func<string, int, Instruction>>()
            {
                {"IF", parseBranchInstruction },
                {"GOTO", parseGotoInstruction},
                //Desativados funções de print e READ pois não serão usadas no game final
                {"PRINT", parsePrintInstruction },
                {"READ", parseReadInstruction},
                {"RETURN" , parseReturnInstruction},
                {"BREAK", parseBrakeInstruction }
            };
        }

        private readonly Dictionary<string, Func<string, int, Instruction>> InstructionsCode;

        public List<Instruction> Instructions = new List<Instruction>();

        public List<CodeException> Exceptions = new List<CodeException>();

        private Dictionary<string, int> macros = new Dictionary<string, int>();
        private Dictionary<string, int> labels = new Dictionary<string, int>();

        public Instruction[] parseProgram(string instructionText)
        {
            this.Instructions.Clear();
            this.Exceptions.Clear();
            this.macros.Clear();
            this.labels.Clear();
            instructionText = instructionText.ToUpper();

            this.parseCompilationVariables(instructionText);
            int line = 0;
            foreach (string instruction in Regex.Split(instructionText, "\r\n|\r|\n"))
            {
                instruction.Trim();
                try
                {
                    if (instruction == "" || instruction[0] == '#')
                    {
                        line++;
                        continue;
                    }
                    Instruction? newInstruction = parseInstruction(instruction, line);
                    if (!(newInstruction is null))
                    {
                        this.Instructions.Add(newInstruction);
                    }
                }
                catch (CodeException e)
                {
                    Exceptions.Add(e);
                }
                line++;
            }
            return this.Instructions.ToArray();
        }

        public Instruction? parseInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts[0] == "LABEL" || instructionParts[0] == "MACRO")
            {
                return null;
            }
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

        private List<string> parseCompilationVariables(string instructionText)
        {
            List<string> instructions = new List<string>(Regex.Split(instructionText, "\r\n|\r|\n"));
            int instructionCount = 0;

            for (int i = 0; i < instructions.Count; i++) {
                string instruction = instructions[i];
                instruction = instruction.Trim();
                if(instruction == "" || instruction[0] == '#')
                {
                    continue;
                }
                else
                {
                    try
                    {

                    string[] instructionParts = instruction.Split(" ");
                    if (instructionParts[0] == "LABEL")
                    {
                        parseLabelInstruction(instruction, instructionCount, i);
                        continue;
                    }
                    else if(instructionParts[0] == "MACRO")
                    {
                        parseMacroInstruction(instruction, i);
                        continue;
                    }
                    }catch(CompilationError e)
                    {
                        Exceptions.Add(e);
                    }
                }
                instructionCount++;
            }

            return instructions;

        }

    }


}
