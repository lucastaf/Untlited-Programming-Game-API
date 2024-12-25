using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Exceptions;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game.Parser
{
    public static partial class Parser
    {
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
