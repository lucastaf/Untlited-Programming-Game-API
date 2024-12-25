using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untlited_Programming_Game.Exceptions
{
    [Serializable]
    public class CodeException : Exception
    {
        public int line;
        public string code;
        public CodeException(string message, string code, int line) : base(message)
        {
            this.line = line;
            this.code = code;
        }

    }



}
