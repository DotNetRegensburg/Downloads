
//Some namespace mappings
using D2D = SharpDX.Direct2D1;

//Some type mappings
#if WINRT
using D2dFactory = SharpDX.Direct2D1.Factory1;
#endif
#if DESKTOP
using D2dFactory = SharpDX.Direct2D1.Factory;
#endif

namespace RK.Common.GraphicsEngine.Core
{
    public class DeviceHandlerD2D
    {
        private GraphicsCore m_core;
        private DeviceHandlerDXGI m_dxgiHandler;

        //Resources form Direct2D api
        private D2dFactory m_factory;
        //private D2D.Device m_device;
        //private D2D.DeviceContext m_deviceContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerD2D"/> class.
        /// </summary>
        /// <param name="core">The core.</param>
        /// <param name="dxgiHandler">The dxgi handler.</param>
        public DeviceHandlerD2D(GraphicsCore core, DeviceHandlerDXGI dxgiHandler)
        {
           
            //Update member variables
            m_core = core;
            m_dxgiHandler = dxgiHandler;

            //Create the factory object
            m_factory = new D2dFactory(
                D2D.FactoryType.SingleThreaded,
                core.IsDebugEnabled ? D2D.DebugLevel.Information : D2D.DebugLevel.None);
            //m_device = new D2D.Device(m_factory, dxgiHandler.Device);
            //m_deviceContext = new D2D.DeviceContext(m_device, D2D.DeviceContextOptions.None);
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            //m_deviceContext = GraphicsHelper.DisposeGraphicsObject(m_deviceContext);
            //m_device = GraphicsHelper.DisposeGraphicsObject(m_device);
            m_factory = GraphicsHelper.DisposeGraphicsObject(m_factory);
        }

        /// <summary>
        /// Gets the factory object.
        /// </summary>
        public D2D.Factory Factory
        {
            get { return m_factory; }
        }
    }
}
