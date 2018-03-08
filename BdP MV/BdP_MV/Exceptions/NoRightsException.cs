using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BdP_MV.Exceptions
{
    class NoRightsException:Exception
    {
        public NoRightsException()
            : base() { }

        public NoRightsException(string message)
            : base(message) { }

        public NoRightsException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NoRightsException(string message, Exception innerException)
            : base(message, innerException) { }

        public NoRightsException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected NoRightsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

