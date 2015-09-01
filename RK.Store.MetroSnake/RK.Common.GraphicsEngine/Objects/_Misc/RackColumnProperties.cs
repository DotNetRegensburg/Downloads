using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Objects
{
    public class RackColumnProperties
    {
        private float m_depth;

        /// <summary>
        /// Initializes a new instance of the <see cref="RackColumnProperties"/> class.
        /// </summary>
        public RackColumnProperties()
        {
            m_depth = 1f;
        }

        /// <summary>
        /// Gets or sets the depth of a column.
        /// </summary>
        public float Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }
    }
}
