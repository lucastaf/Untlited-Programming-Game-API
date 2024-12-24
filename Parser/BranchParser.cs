using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Exceptions;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game.Parser
{
    internal static partial class Parser
    {
        private static Instruction parseBranchInstruction(string instructionText, Dictionary<string, int> macros, int line)
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
            bool isBranchValid = branchMap.TryGetValue(instructionParts[2], out branchType);
            if (!isBranchValid) throw new InvalidSimbolException(line, "Invalid comparison");
            if (instructionParts[4] == "GOTO")
            {
                if (isInt2)
                    return new BranchInstruction<int, string>(branchType, instructionParts[1], value2, instructionParts[5], line);
                else
                    return new BranchInstruction<string, string>(branchType, instructionParts[1], instructionParts[3], instructionParts[5], line);
            }
            else if (instructionParts[4] == "JUMP")
            {
                int dest;
                bool isDestInt = Int32.TryParse(instructionParts[5], out dest);
                if (!isDestInt) isDestInt = macros.TryGetValue(instructionParts[5], out dest);
                if (!isDestInt) throw new InvalidInputException(line,"Last argument of an JUMP branch must be an number");
                if (isInt2)
                    return new BranchInstruction<int, int>(branchType, instructionParts[1], value2, dest, line);
                else
                    return new BranchInstruction<string, int>(branchType, instructionParts[1], instructionParts[3], dest, line);
            }
            else
            {
                throw new InvalidSimbolException(line, "Branch instruction must be of type GOTO or JUMP");
            }
        }

    }
}
