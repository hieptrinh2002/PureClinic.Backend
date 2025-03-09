using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Exceptions
{
    public class ErrorException : Exception
    {
        public string ErrorCode { get; }

        public ErrorException()
        {
        }

        public ErrorException(string? message, string? errorCode = null) : base(message)
        {
            if (!string.IsNullOrEmpty(errorCode))
                ErrorCode = errorCode;
        }

        public ErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
