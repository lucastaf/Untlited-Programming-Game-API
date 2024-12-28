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
        private Instruction parseArithmeticInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            int instructionSize = instructionParts.Length;
            
            Dictionary<string, Operation> operationMap = new Dictionary<string, Operation>()
            {
                {"+", Operation.add },
                {"-", Operation.sub },
                {"/", Operation.div },
                {"*", Operation.mul },
                {"%", Operation.mod },
            };
            int value1;
            bool isInt1 = Int32.TryParse(instructionParts[2], out value1);
            if (!isInt1) isInt1 = macros.TryGetValue(instructionParts[2], out value1);

            if (instructionSize == 3)
            {
                if (isInt1)
                    return new AssignInstruction<int>(value1, instructionParts[0], line);
                else
                    return new AssignInstruction<string>(instructionParts[2], instructionParts[0], line);

            }
            else if (instructionSize == 5)
            {
                int value2;
                Operation operation;
                bool isValidOperation = operationMap.TryGetValue(instructionParts[3], out operation);
                if (!isValidOperation) throw new InvalidSimbolException(line, "invalid arithmetic operation");

                bool isInt2 = Int32.TryParse(instructionParts[4], out value2);
                if (!isInt2) isInt2 = macros.TryGetValue(instructionParts[4], out value2);
                if (isInt1)
                {
                    if (isInt2) return new ArithmeticInstruction<int, int>(operation, value1, value2, instructionParts[0], line);
                    else return new ArithmeticInstruction<int, string>(operation, value1, instructionParts[4], instructionParts[0], line);
                }
                else
                {
                    if (isInt2) return new ArithmeticInstruction<string, int>(operation, instructionParts[2], value2, instructionParts[0], line);
                    else return new ArithmeticInstruction<string, string>(operation, instructionParts[2], instructionParts[4], instructionParts[0], line);
                }

            }
            else
            {
                throw new InvalidSizeException(line);
            }
        }
    }
}
