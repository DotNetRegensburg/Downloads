using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace Common.GraphicsEngine
{
    public class GraphicsLibraryException : CommonLibraryException
    {
        /// <summary>
        /// Creates a new CommonLibraryException object
        /// </summary>
        public GraphicsLibraryException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new CommonLibraryException object
        /// </summary>
        public GraphicsLibraryException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Creates a new CommonLibraryException object
        /// </summary>
        public GraphicsLibraryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
