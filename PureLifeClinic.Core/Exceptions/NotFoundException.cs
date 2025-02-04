using System.Runtime.Serialization;

namespace PureLifeClinic.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ErrorCode { get; }
        public NotFoundException()
        {
        }

        public NotFoundException(string? message, string? errorCode = null) : base(message)
        {
            if (!string.IsNullOrEmpty(errorCode))
                ErrorCode = errorCode;
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
