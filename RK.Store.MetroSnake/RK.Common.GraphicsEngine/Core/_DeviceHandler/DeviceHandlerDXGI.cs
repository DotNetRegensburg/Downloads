
//Some namespace mappings
using DXGI = SharpDX.DXGI;

//Some type mappings
#if WINRT
using DxgiFactory = SharpDX.DXGI.Factory2;
using DxgiAdapter = SharpDX.DXGI.Adapter2;
using DxgiDevice = SharpDX.DXGI.Device2;
#endif
#if DESKTOP
using DxgiFactory = SharpDX.DXGI.Factory1;
using DxgiAdapter = SharpDX.DXGI.Adapter1;
using DxgiDevice = SharpDX.DXGI.Device1;
#endif


namespace RK.Common.GraphicsEngine.Core
{
    public class DeviceHandlerDXGI
    {
        private DxgiFactory m_factory;
        private DxgiAdapter m_adapter;
        private DxgiDevice m_device;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerDXGI"/> class.
        /// </summary>
        public DeviceHandlerDXGI(GraphicsCore core, DeviceHandlerD3D11 d3d11Handler)
        {
            m_device = d3d11Handler.Device.QueryInterface<DxgiDevice>();
            m_adapter = m_device.Adapter.QueryInterface<DxgiAdapter>();
            m_factory = m_adapter.GetParent<DxgiFactory>();
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            m_factory = GraphicsHelper.DisposeGraphicsObject(m_factory);
            m_adapter = GraphicsHelper.DisposeGraphicsObject(m_adapter);
            m_device = GraphicsHelper.DisposeGraphicsObject(m_device);
        }

        /// <summary>
        /// Gets current factory object.
        /// </summary>
        /// <value>The factory.</value>
        public DxgiFactory Factory
        {
            get { return m_factory; }
        }

        /// <summary>
        /// Gets the DXGI device.
        /// </summary>
        public DxgiDevice Device
        {
            get { return m_device; }
        }

        /// <summary>
        /// Gets current adapter used for drawing.
        /// </summary>
        public DxgiAdapter Adapter
        {
            get { return m_adapter; }
        }
    }
}
