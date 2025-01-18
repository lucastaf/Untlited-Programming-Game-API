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
        private Instruction parseBranchInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 6) throw new InvalidSizeException(line);
            int value2;
            bool isInt2 = Int32.TryParse(instructionParts[3], out value2);
            if (!isInt2)
                isInt2 = macros.TryGetValue(instructionParts[3], out value2);
            Dictionary<string, Branch> branchMap = new Dictionary<string, Branch>()
            {
                {">", Branch.bgt },
                {"<", Branch.blt },
                {"=", Branch.beq },
                {"!=", Branch.bne },
                {"<=", Branch.ble },
                {">=", Branch.bge }
            };
            Branch branchType;
            int destination;
            bool isBranchValid = branchMap.TryGetValue(instructionParts[2], out branchType);
            if (!isBranchValid) throw new InvalidSimbolException(line, "Invalid comparison");
            bool isDestValid = labels.TryGetValue(instructionParts[5], out destination);
            if (!isDestValid) throw new InvalidLabelException(line);
            if (instructionParts[4] == "GOTO")
            {
                if (isInt2)
                    return new BranchInstruction<int>(branchType, instructionParts[1], value2, destination, line);
                else
                    return new BranchInstruction<string>(branchType, instructionParts[1], instructionParts[3], destination, line);
            }
            else
            {
                throw new InvalidSimbolException(line, "Branch instruction must be of type GOTO or JUMP");
            }
        }

        private Instruction parseGotoInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            if (instructionParts.Length != 2) throw new InvalidSizeException(line);
            int destination;
            bool isDestValid = labels.TryGetValue(instructionParts[1], out destination);
            if (!isDestValid) throw new InvalidLabelException(line);
            return new GotoInstruction(instructionParts[1], line, destination);
        }

    }
}
