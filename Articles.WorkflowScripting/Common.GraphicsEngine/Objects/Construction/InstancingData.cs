using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;

namespace Common.GraphicsEngine.Objects.Construction
{
    [StructLayout(LayoutKind.Sequential)]
    public struct InstancingData
    {
        private Vector3 m_instancePosition;

        /// <summary>
        /// Retrieves or sets the instance's position
        /// </summary>
        public Vector3 InstancePosition
        {
            get { return m_instancePosition; }
            set { m_instancePosition = value; }
        }
    }
}
