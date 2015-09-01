using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Drawing3D
{
    public class UpdateState
    {
        private TimeSpan m_updateTime;
        private Matrix4Stack m_world;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateState"/> class.
        /// </summary>
        /// <param name="updateTime">The update time.</param>
        public UpdateState(TimeSpan updateTime)
        {
            m_updateTime = updateTime;
            m_world = new Matrix4Stack(Matrix4.Identity);
        }

        /// <summary>
        /// Gets current update time.
        /// </summary>
        public TimeSpan UpdateTime
        {
            get { return m_updateTime; }
        }

        /// <summary>
        /// Gets current world transform.
        /// </summary>
        public Matrix4Stack World
        {
            get { return m_world; }
        }
    }
}
