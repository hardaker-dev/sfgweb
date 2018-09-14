using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
