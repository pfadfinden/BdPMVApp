using System;
using System.Runtime.Serialization;

namespace BdP_MV.Exceptions
{
    public class NewLoginException : Exception
    {
        public NewLoginException()
            : base() { }

        public NewLoginException(string message)
            : base(message) { }

        public NewLoginException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NewLoginException(string message, Exception innerException)
            : base(message, innerException) { }

        public NewLoginException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected NewLoginException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
