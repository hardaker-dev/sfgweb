using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SaasFeeGuides.Exceptions
{
    public class BadRequestException : Exception
    {
        public int ErrorCode { get; set; }

        public BadRequestException(string message, HttpStatusCode errorCode, Exception innerException) : this(message, (int)errorCode, innerException)
        {
        }
        public BadRequestException(string message,int errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
        public BadRequestException(string message, int errorCode) : this(message,errorCode,null)
        {
        }
        public BadRequestException(string message, HttpStatusCode errorCode) : this(message,(int)errorCode)
        {
        }
    }
}
