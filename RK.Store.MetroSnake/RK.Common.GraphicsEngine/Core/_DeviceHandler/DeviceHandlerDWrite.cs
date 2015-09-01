
//Some namespace mappings
using DWrite = SharpDX.DirectWrite;

namespace RK.Common.GraphicsEngine.Core
{
    public class DeviceHandlerDWrite
    {
        private GraphicsCore m_core;
        private DeviceHandlerDXGI m_dxgiHandler;

        //Resources for DirectWrite
        private DWrite.Factory m_factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerDWrite"/> class.
        /// </summary>
        /// <param name="core">The core.</param>
        /// <param name="dxgiHandler">The dxgi handler.</param>
        public DeviceHandlerDWrite(GraphicsCore core, DeviceHandlerDXGI dxgiHandler)
        {
            //Update member variables
            m_core = core;
            m_dxgiHandler = dxgiHandler;

            //Create DirectWrite Factory object
            m_factory = new DWrite.Factory(DWrite.FactoryType.Shared);
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            m_factory = GraphicsHelper.DisposeGraphicsObject(m_factory);
        }

        /// <summary>
        /// Gets the Factory object.
        /// </summary>
        public DWrite.Factory Factory
        {
            get { return m_factory; }
        }
    }
}
