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
                Func<string, Dictionary<string, int>, Instruction> parseFunction;
                if (InstructionsCode.TryGetValue(instructionParts[0], out parseFunction))
                {
                    return parseFunction(instructionText, macros);
                }
                else if (instructionParts[1] == "=")
                {
                    return parseArithmeticInstruction(instructionText, macros);
                }
                else
                {
                    throw new NotImplementedException();
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

        private static readonly Dictionary<string, Func<string, Dictionary<string, int>, Instruction>> InstructionsCode = new Dictionary<string, Func<string, Dictionary<string, int>, Instruction>>()
        {
            {"IF", parseBranchInstruction },
            {"PRINT", parsePrintInstruction },
            {"READ", parseReadInstruction},
            {"LABEL", parseLabelInstruction },
            {"MACRO", parseMacroInstruction},
        };

        private static Instruction parsePrintInstruction(string instructionText, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(0);
            return new PrintInstruction(instructionParts[1]);
        }

        private static Instruction parseReadInstruction(string instructionText, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(0);
            return new ReadInstruction(instructionParts[1]);
        }

        private static Instruction parseLabelInstruction(string instructionText, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(0);
            return new LabelInstruction(instructionParts[1]);
        }

        private static Instruction parseMacroInstruction(string instructionText, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 3) throw new InvalidSizeException(0);
            int value;
            bool isInt = Int32.TryParse(instructionParts[2], out value);
            if (!isInt) throw new InvalidInputException(0);
            macros.Add(instructionParts[1], value);
            return new MacroInstruction();
        }
    
    }


}
