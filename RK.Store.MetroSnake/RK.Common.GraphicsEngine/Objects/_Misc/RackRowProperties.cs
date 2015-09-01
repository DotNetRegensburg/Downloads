using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Objects
{
    public class RackRowProperties
    {
        private float m_height;

        /// <summary>
        /// Initializes a new instance of the <see cref="RackRowProperties"/> class.
        /// </summary>
        public RackRowProperties()
        {
            m_height = 1f;
        }

        /// <summary>
        /// Gets or sets the height of a row.
        /// </summary>
        public float Height
        {
            get { return m_height; }
            set { m_height = value; }
        }
    }
}
