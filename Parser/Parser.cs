using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Instructions;
using Untlited_Programming_Game.Exceptions;
using System.Reflection;

namespace Untlited_Programming_Game.Parser
{
    internal static partial class Parser
    {
        public static Instruction[] parseProgram(string instructionText, out CodeException[] exceptions)
        {
            int line = 0;
            List<CodeException> errors = new List<CodeException>();
            List<Instruction> instructions = new List<Instruction>();
            Dictionary<string, int> macros = new Dictionary<string, int>();
            foreach (string instruction in instructionText.ToUpper().Split("\r\n"))
            {
                try
                {
                    if (instruction == "" || instruction[0] == '#')
                    {
                        line++;
                        continue;
                    }
                    Instruction newInstruction = parseInstruction(instruction, line, macros);
                    if (newInstruction is not MacroInstruction)
                    {
                        instructions.Add(newInstruction);
                    }
                }
                catch (CodeException e)
                {
                    errors.Add(e);
                }
                line++;
            }
            exceptions = errors.ToArray();
            return instructions.ToArray();
        }
        public static Instruction parseInstruction(string instructionText, int line, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
            try
            {
                Func<string, Dictionary<string, int>,int, Instruction> parseFunction;
                if (InstructionsCode.TryGetValue(instructionParts[0], out parseFunction))
                {
                    return parseFunction(instructionText, macros, line);
                }
                else if (instructionParts[1] == "=")
                {
                    return parseArithmeticInstruction(instructionText, macros, line);
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

        private static readonly Dictionary<string, Func<string, Dictionary<string, int>, int, Instruction>> InstructionsCode = new Dictionary<string, Func<string, Dictionary<string, int>, int, Instruction>>()
        {
            {"IF", parseBranchInstruction },
            {"PRINT", parsePrintInstruction },
            {"READ", parseReadInstruction},
            {"LABEL", parseLabelInstruction },
            {"MACRO", parseMacroInstruction},
        };

        private static Instruction parsePrintInstruction(string instructionText, Dictionary<string, int> macros, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(line);
            return new PrintInstruction(instructionParts[1], line);
        }

        private static Instruction parseReadInstruction(string instructionText, Dictionary<string, int> macros, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(line);
            return new ReadInstruction(instructionParts[1], line);
        }

        private static Instruction parseLabelInstruction(string instructionText, Dictionary<string, int> macros, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(line);
            return new LabelInstruction(instructionParts[1], line);
        }

        private static Instruction parseMacroInstruction(string instructionText, Dictionary<string, int> macros, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 3) throw new InvalidSizeException(line);
            int value;
            bool isInt = Int32.TryParse(instructionParts[2], out value);
            if (!isInt) throw new InvalidInputException(line);
            macros.Add(instructionParts[1], value);
            return new MacroInstruction(line);
        }
    
    }


}
