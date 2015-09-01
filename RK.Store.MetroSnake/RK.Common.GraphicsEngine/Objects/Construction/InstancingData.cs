using System.Runtime.InteropServices;

namespace RK.Common.GraphicsEngine.Objects.Construction
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
