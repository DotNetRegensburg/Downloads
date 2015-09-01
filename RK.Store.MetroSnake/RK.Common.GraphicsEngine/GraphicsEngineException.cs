using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.GraphicsEngine
{
    public class GraphicsEngineException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsEngineException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GraphicsEngineException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsEngineException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GraphicsEngineException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
