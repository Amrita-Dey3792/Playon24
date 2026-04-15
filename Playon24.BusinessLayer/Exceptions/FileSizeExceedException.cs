using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playon24.BusinessLayer.Exceptions
{
    public class FileSizeExceedException : Exception
    {
        public FileSizeExceedException() { }
        public FileSizeExceedException(string message) : base(message) { }
    }
}
