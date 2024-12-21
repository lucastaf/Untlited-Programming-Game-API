using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Exceptions
{
    [Serializable]
    internal abstract class CompilationError : CodeException
    {
        public CompilationError(string message, string code, int line) : base(message, code, line)
        {
        }
    }

    internal class InvalidInstructionException : CompilationError
    {
        public InvalidInstructionException(int line) : base("Invalid Instruction", "CE 00", line) { }
    }

    internal class InvalidSizeException : CompilationError
    {
        public InvalidSizeException(int line) : base("Number of arguments is invalid", "CE10", line) { }
    }

    internal class InvalidInputException : CompilationError
    {
        public InvalidInputException(int line, string message = "Invalid Input") : base(message, "CE20", line) { }
    }

    internal class InvalidSimbolException : CompilationError
    {
        public InvalidSimbolException(int line, string message = "Invalid Symbol") : base(message, "CE21", line) { }
    }

}
