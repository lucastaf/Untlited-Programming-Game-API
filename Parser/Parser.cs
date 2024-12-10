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
    internal static class Parser
    {
        public static Instruction[] parseProgram(string instructionText, out CodeException[] exceptions)
        {
            int line = 0;
            List<CodeException> errors = new List<CodeException>();
            List<Instruction> instructions = new List<Instruction>();
            foreach (string instruction in instructionText.ToUpper().Split("\r\n"))
            {
                try
                {
                    if (instruction == "" || instruction[0] == '#')
                    {
                        continue;
                    }
                    instructions.Add(parseInstruction(instruction, line));
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
        public static Instruction parseInstruction(string instructionText, int line)
        {
            string[] instructionParts = instructionText.Split(" ");
            try
            {
                Func<string, Instruction> parseFunction;
                if (InstructionsCode.TryGetValue(instructionParts[0], out parseFunction))
                {
                    return parseFunction(instructionText);
                }
                else
                {
                    return parseArithmeticInstruction(instructionText);
                };

            }
            catch (Exception e)
            {
                throw new InvalidInstructionException(line);
            }
        }

        private static readonly Dictionary<string, Func<string, Instruction>> InstructionsCode = new Dictionary<string, Func<string, Instruction>>()
        {
            {"IF", parseBranchInstruction },
            {"PRINT", parsePrintInstruction },
            {"LABEL", parseLabelInstruction },
        };

        private static Instruction parseArithmeticInstruction(string instructionText)
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
            if (Int32.TryParse(instructionParts[2], out value1))
            {
                if (Int32.TryParse(instructionParts[4], out value2))
                    return new ArithmeticInstruction<int, int>(operation, value1, value2, instructionParts[0]);
                else
                    return new ArithmeticInstruction<int, string>(operation, value1, instructionParts[4], instructionParts[0]);
            }
            else
            {
                if (Int32.TryParse(instructionParts[4], out value2))
                    return new ArithmeticInstruction<string, int>(operation, instructionParts[2], value2, instructionParts[0]);
                else
                    return new ArithmeticInstruction<string, string>(operation, instructionParts[2], instructionParts[4], instructionParts[0]);
            }

        }

        private static Instruction parseBranchInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            Branch branchType;
            int value2;

            Dictionary<string, Branch> branchMap = new Dictionary<string, Branch>()
            {
                {">", Branch.bgt },
                {"<", Branch.blt },
                {"=", Branch.beq },
                {"!=", Branch.bne },
                {"<=", Branch.ble },
                {">=", Branch.bge }
            };
            if (instructionParts[4] == "GOTO")
            {
                if (Int32.TryParse(instructionParts[3], out value2))
                    return new BranchInstruction<int, string>(branchMap[instructionParts[2]], instructionParts[1], value2, instructionParts[5]);
                else
                    return new BranchInstruction<string, string>(branchMap[instructionParts[2]], instructionParts[1], instructionParts[3], instructionParts[5]);
            }
            else if (instructionParts[4] == "JUMP")
            {
                int dest = Int32.Parse(instructionParts[5]);
                if (Int32.TryParse(instructionParts[3], out value2))
                    return new BranchInstruction<int, int>(branchMap[instructionParts[2]], instructionParts[1], value2, dest);
                else
                    return new BranchInstruction<string, int>(branchMap[instructionParts[2]], instructionParts[1], instructionParts[3], dest);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static Instruction parsePrintInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            return new PrintInstruction(instructionParts[1]);
        }

        private static Instruction parseLabelInstruction(string instructionText)
        {
            string[] instructionParts = instructionText.Split(" ");
            return new LabelInstruction(instructionParts[1]);
        }
    }


}
