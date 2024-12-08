using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Instructions;

namespace Untlited_Programming_Game.Parser
{
    internal static class Parser
    {
        public static Instruction parseInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            var parseFunction = InstructionsCode[instructionParts[0]];
            return parseFunction(instructionText);
        }

        private static readonly Dictionary<string, Func<string, Instruction>> InstructionsCode = new Dictionary<string, Func<string, Instruction>>()
        {
            {"add", parseArithmeticInstruction },
            {"sub", parseArithmeticInstruction },
            {"mul", parseArithmeticInstruction },
            {"div", parseArithmeticInstruction },
            {"mod", parseArithmeticInstruction }  
        };

        private static Instruction parseArithmeticInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            Operation operation;
            Enum.TryParse(instructionParts[0], out operation);
            int value1, value2;
            if (Int32.TryParse(instructionParts[1], out value1))
            {
                if (Int32.TryParse(instructionParts[2], out value2))
                {
                    return new ArithmeticInstruction<int, int>(value1, value2, instructionParts[3], operation);
                }
                else
                {
                    return new ArithmeticInstruction<int, string>(value1, instructionParts[2], instructionParts[3], operation);
                }
            }
            else
            {
                if (Int32.TryParse(instructionParts[2], out value2))
                {
                    return new ArithmeticInstruction<string, int>(instructionParts[1], value2, instructionParts[3], operation);
                }
                else
                {
                    return new ArithmeticInstruction<string, string>(instructionParts[1], instructionParts[2], instructionParts[3], operation);
                }
            }

        }

    }


}
