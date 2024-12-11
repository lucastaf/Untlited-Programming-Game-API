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
        private static Instruction parseArithmeticInstruction(string instructionText, Dictionary<string, int> macros)
        {
            string[] instructionParts = instructionText.Split(" ");
            Dictionary<string, Operation> operationMap = new Dictionary<string, Operation>()
            {
                {"+", Operation.add },
                {"-", Operation.sub },
                {"/", Operation.div },
                {"*", Operation.mul },
                {"%", Operation.mod },
            };
            Operation operation = operationMap[instructionParts[3]];
            int value1, value2;
            bool isInt1 = Int32.TryParse(instructionParts[2], out value1), isInt2 = Int32.TryParse(instructionParts[4], out value2);
            if (!isInt1) isInt1 = macros.TryGetValue(instructionParts[2], out value1);
            if (!isInt2) isInt2 = macros.TryGetValue(instructionParts[4], out value2);

            if (isInt1)
            {
                if (isInt2) return new ArithmeticInstruction<int, int>(operation, value1, value2, instructionParts[0]);
                else return new ArithmeticInstruction<int, string>(operation, value1, instructionParts[4], instructionParts[0]);
            }
            else
            {
                if (isInt2) return new ArithmeticInstruction<string, int>(operation, instructionParts[2], value2, instructionParts[0]);
                else return new ArithmeticInstruction<string, string>(operation, instructionParts[2], instructionParts[4], instructionParts[0]);
            }

        }
    }
}
