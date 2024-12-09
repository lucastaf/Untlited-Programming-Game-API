using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untlited_Programming_Game.Instructions;
using Untlited_Programming_Game.Exceptions;

namespace Untlited_Programming_Game.Parser
{
    internal static class Parser
    {
        public static Instruction[] parseProgram(string[] instructionText, out CodeException[] exceptions)
        {
            int line = 0;
            List<CodeException> errors = new List<CodeException>();
            List<Instruction> instructions = new List<Instruction>(); 
            foreach (var instruction in instructionText)
            {
                try
                {
                    instructions.Add(parseInstruction(instruction, line));
                }catch (CodeException e)
                {
                    errors.Add(e);
                }
                line++;
            }
            exceptions = errors.ToArray();
            return instructions.ToArray();
        }
        public static Instruction parseInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            try
            {

                var parseFunction = InstructionsCode[instructionParts[0]];

                return parseFunction(instructionText);
            }
            catch (Exception e)
            {
                throw new InvalidInstructionException(line);
            }
        }

        private static readonly Dictionary<string, Func<string, Instruction>> InstructionsCode = new Dictionary<string, Func<string, Instruction>>()
        {
            {"add", parseArithmeticInstruction },
            {"sub", parseArithmeticInstruction },
            {"mul", parseArithmeticInstruction },
            {"div", parseArithmeticInstruction },
            {"mod", parseArithmeticInstruction },
            {"beq", parseBranchInstruction },
            {"bne", parseBranchInstruction },
            {"bgt", parseBranchInstruction },
            {"blt", parseBranchInstruction },
            {"ble", parseBranchInstruction },
            {"bge", parseBranchInstruction},
            {"print", parsePrintInstruction },
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
                    return new ArithmeticInstruction<int, int>(operation, value1, value2, instructionParts[3]);
                }
                else
                {
                    return new ArithmeticInstruction<int, string>(operation, value1, instructionParts[2], instructionParts[3]);
                }
            }
            else
            {
                if (Int32.TryParse(instructionParts[2], out value2))
                {
                    return new ArithmeticInstruction<string, int>(operation, instructionParts[1], value2, instructionParts[3]);
                }
                else
                {
                    return new ArithmeticInstruction<string, string>(operation, instructionParts[1], instructionParts[2], instructionParts[3]);
                }
            }

        }


        private static Instruction parseBranchInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            Branch branchType;
            int value2;
            int dest;
            Enum.TryParse(instructionParts[0], out  branchType);
            Int32.TryParse(instructionParts[3], out dest);
            if (Int32.TryParse(instructionParts[2], out value2))
            {
                return new BranchInstruction<int>(branchType, instructionParts[1], value2, dest);
            }else
            {
                return new BranchInstruction<string>(branchType, instructionParts[1], instructionParts[2], dest);
            }
        }

        private static Instruction parsePrintInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            return new PrintInstruction(instructionParts[1]);
        }
    }


}
