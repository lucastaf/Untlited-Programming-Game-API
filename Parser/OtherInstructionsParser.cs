using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Exceptions;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game.Parser
{
    public partial class Parser
    {
        private Instruction parsePrintInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(line);
            return new PrintInstruction(instructionParts[1], line);
        }

        private Instruction parseReadInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(line);
            return new ReadInstruction(instructionParts[1], line);
        }

        private Instruction parseMacroInstruction(string instructionText, int line)
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
