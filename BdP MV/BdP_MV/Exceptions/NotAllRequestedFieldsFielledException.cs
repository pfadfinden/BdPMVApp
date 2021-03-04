using System;
using System.Runtime.Serialization;

namespace BdP_MV.Exceptions
{
    public class NotAllRequestedFieldsFilledException : Exception
    {
        public NotAllRequestedFieldsFilledException()
            : base() { }

        public NotAllRequestedFieldsFilledException(string message)
            : base(message) { }

        public NotAllRequestedFieldsFilledException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NotAllRequestedFieldsFilledException(string message, Exception innerException)
            : base(message, innerException) { }

        public NotAllRequestedFieldsFilledException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected NotAllRequestedFieldsFilledException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
