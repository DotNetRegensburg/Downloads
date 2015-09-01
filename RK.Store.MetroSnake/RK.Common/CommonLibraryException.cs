using System;
using System.Runtime.Serialization;

namespace RK.Common
{
    public class CommonLibraryException : Exception
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
    }
}
