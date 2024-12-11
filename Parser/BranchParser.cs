using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game.Parser
{
    internal static partial class Parser
    {
        private static Instruction parseBranchInstruction(string instructionText, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
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
            Branch branchType = branchMap[instructionParts[2]];
            if (instructionParts[4] == "GOTO")
            {
                if (isInt2)
                    return new BranchInstruction<int, string>(branchType, instructionParts[1], value2, instructionParts[5]);
                else
                    return new BranchInstruction<string, string>(branchType, instructionParts[1], instructionParts[3], instructionParts[5]);
            }
            else if (instructionParts[4] == "JUMP")
            {
                int dest;
                bool isDestInt = Int32.TryParse(instructionParts[5], out dest);
                if (!isDestInt) dest = macros[instructionParts[5]];
                if (isInt2)
                    return new BranchInstruction<int, int>(branchType, instructionParts[1], value2, dest);
                else
                    return new BranchInstruction<string, int>(branchType, instructionParts[1], instructionParts[3], dest);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
