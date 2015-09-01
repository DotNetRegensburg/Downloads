using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace Common
{
    public class CommonLibraryException : ApplicationException
    {
        /// <summary>
        /// Creates a new CommonLibraryException object
        /// </summary>
        public CommonLibraryException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new CommonLibraryException object
        /// </summary>
        public CommonLibraryException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Creates a new CommonLibraryException object
        /// </summary>
        public CommonLibraryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
