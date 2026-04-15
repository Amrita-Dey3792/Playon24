using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playon24.BusinessLayer.Exceptions
{
    public class EmptyFileException : Exception
    {
        public EmptyFileException() { }
        public EmptyFileException(string message) : base(message) { }
    }
}
