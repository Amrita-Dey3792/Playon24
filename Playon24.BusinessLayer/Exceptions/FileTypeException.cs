using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playon24.BusinessLayer.Exceptions
{
    public class FileTypeException : Exception
    {
        public FileTypeException() { }
        public FileTypeException(string message) : base(message) { }
    }
}
