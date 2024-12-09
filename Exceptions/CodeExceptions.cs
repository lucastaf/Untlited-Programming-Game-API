using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Exceptions
{
    [Serializable]
    internal class CodeException : Exception
    {
        public int line;
        public CodeException(string message, int line) : base(message) {
            this.line = line;
        }

    }

    internal class InvalidInstructionException : CodeException
    {
        public InvalidInstructionException(int line) : base("Instrução Inválida", line)
        {
        }
    }
}
