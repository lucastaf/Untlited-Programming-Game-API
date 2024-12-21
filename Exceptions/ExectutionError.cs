using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Exceptions
{
    internal abstract class ExectutionError : CodeException
    {
        public ExectutionError(string message, string code, int line) : base(message, code, line) { }
    }

    internal class InvalidRegisterException : ExectutionError {
        public InvalidRegisterException(int line, string message = "Invalid Register") : base(message, "EE00", line) { }
    }

    enum AccessType
    {
        read, write
    }
    internal class UnauthorizedAccessException : ExectutionError
    {
        public AccessType acessType { get; private set; }
        public UnauthorizedAccessException(int line, AccessType accessType, string message = "Unauthorized Access") : base (message, "EE10", line)
        {
            this.acessType = accessType;
        }
    }

    internal class InvalidLabelException : ExectutionError {
        public InvalidLabelException(int line) : base("Invalid label", "EE20", line) { }
    }


}
