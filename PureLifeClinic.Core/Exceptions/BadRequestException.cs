using System.Runtime.Serialization;

namespace PureLifeClinic.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public string ErrorCode { get; }

        public BadRequestException()
        {
        }

        public BadRequestException(string? message, string? errorCode = null) : base(message)
        {
            if (!string.IsNullOrEmpty(errorCode))
                ErrorCode = errorCode;
        }
      
        public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
