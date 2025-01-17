using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Exceptions
{
    public abstract class ExectutionError : CodeException
    {
        public ExectutionError(string message, string code, int line) : base(message, code, line) { }
    }

    public class InvalidRegisterException : ExectutionError {
        public InvalidRegisterException(int line, string message = "Invalid Register") : base(message, "EE00", line) { }
    }

    public enum AccessType
    {
        read, write
    }
    public class UnauthorizedRegisterException : ExectutionError
    {
        public AccessType acessType { get; private set; }
        public UnauthorizedRegisterException(int line, AccessType accessType, string message = "Unauthorized Access") : base (message, "EE10", line)
        {
            this.acessType = accessType;
        }
    }


}
