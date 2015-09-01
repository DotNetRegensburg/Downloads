using System;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Core
{
    public class GraphicsCore
    {
        private static GraphicsCore s_current;

        private bool m_debugEnabled;
        private TargetHardware m_targetHardware;
        private DeviceHandlerDXGI m_dxgiHandler;
        private DeviceHandlerD3D101 m_d3d101Handler;
        private DeviceHandlerD3D11 m_d3d11Handler;
        private DeviceHandlerD2D m_d2dHandler;
        private DeviceHandlerDWrite m_dWriteHandler;
        private GraphicsConfiguration m_currentConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsCore"/> class.
        /// </summary>
        private GraphicsCore(TargetHardware targetHardware, bool debugEnabled)
        {
            CommonUtil.StartTimeWatching("COMMON_INITIALIZE_3D");
            try
            {
                //Upate common members
                m_debugEnabled = debugEnabled;
                m_targetHardware = targetHardware;

                //Use different orders dependent on target hardware
                if (m_targetHardware == TargetHardware.SoftwareRenderer)
                {
                    m_d3d11Handler = new DeviceHandlerD3D11(this, m_dxgiHandler);
                    m_dxgiHandler = new DeviceHandlerDXGI(this, m_d3d11Handler);
                    m_d3d101Handler = new DeviceHandlerD3D101(this, m_dxgiHandler, m_d3d11Handler);
                }
                else
                {
                    //Create all device handlers
                    m_dxgiHandler = new DeviceHandlerDXGI(this, m_d3d11Handler);
                    m_d3d11Handler = new DeviceHandlerD3D11(this, m_dxgiHandler);
                    m_d3d101Handler = new DeviceHandlerD3D101(this, m_dxgiHandler, m_d3d11Handler);
                }

                m_d2dHandler = new DeviceHandlerD2D(this, m_dxgiHandler); 
                m_dWriteHandler = new DeviceHandlerDWrite(this, m_dxgiHandler);
                m_currentConfiguration = new GraphicsConfiguration();
            }
            catch(Exception ex)
            {
                m_dxgiHandler = null;
                m_d3d101Handler = null;
                m_d3d11Handler = null;
                m_d2dHandler = null;
                m_dWriteHandler = null;
                m_currentConfiguration = null;
            }
            TimeSpan init3DTime = CommonUtil.StopTimeWatching("COMMON_INITIALIZE_3D");
        }

        /// <summary>
        /// Initializes the graphics engine.
        /// </summary>
        public static void Initialize()
        {
            if (s_current != null) { return; }

            //Get supported feature level
            D3D11.FeatureLevel featureLevel = D3D11.FeatureLevel.Level_9_1;
            try
            {
                featureLevel = D3D11.Device.GetSupportedFeatureLevel();
            }
            catch (Exception)
            {
                Initialize(TargetHardware.SoftwareRenderer, false);
                return;
            }

            //Call initialization methods
            switch (featureLevel)
            {
                case D3D11.FeatureLevel.Level_11_0:
                    Initialize(TargetHardware.DirectX11, false);
                    break;

                default:
                    Initialize(TargetHardware.DirectX9, false);
                    break;
            }
        }

        /// <summary>
        /// Initializes the GraphicsCore object.
        /// </summary>
        public static void Initialize(TargetHardware targetHardware, bool enableDebug)
        {
            if (s_current != null) { return; }

            s_current = new GraphicsCore(targetHardware, enableDebug);
        }

        /// <summary>
        /// Unloads the GraphicsCore object.
        /// </summary>
        public static void Unload()
        {
            if (s_current != null) { s_current.UnloadResources(); }
            s_current = null;
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        private void UnloadResources()
        {
            m_d2dHandler.UnloadResources();
            m_d3d11Handler.UnloadResources();
            m_d3d101Handler.UnloadResources();
            m_dxgiHandler.UnloadResources();
            m_dWriteHandler.UnloadResources();

            m_d2dHandler = null;
            m_d3d11Handler = null;
            m_d3d101Handler = null;
            m_dxgiHandler = null;
            m_dWriteHandler = null;
        }

        /// <summary>
        /// Gets current singleton instance.
        /// </summary>
        public static GraphicsCore Current
        {
            get
            {
                if (s_current == null) { throw new ApplicationException("GraphicsCore not initialized!"); }
                return s_current;
            }
        }

        /// <summary>
        /// Is GraphicsCore initialized?
        /// </summary>
        public static bool IsInitialized
        {
            get { return (s_current != null) && (s_current.m_dxgiHandler != null); }
        }

        /// <summary>
        /// Is debug enabled?
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return m_debugEnabled; }
        }

        /// <summary>
        /// Gets current DXGI handler.
        /// </summary>
        public DeviceHandlerDXGI HandlerDXGI
        {
            get { return m_dxgiHandler; }
        }

        /// <summary>
        /// Gets current Direct3D 11 handler.
        /// </summary>
        public DeviceHandlerD3D11 HandlerD3D11
        {
            get { return m_d3d11Handler; }
        }

        /// <summary>
        /// Gets current Direct3D 10.1 handler.
        /// </summary>
        public DeviceHandlerD3D101 HandlerD3D101
        {
            get { return m_d3d101Handler; }
        }

        /// <summary>
        /// Gets current Direct2D handler.
        /// </summary>
        public DeviceHandlerD2D HandlerD2D
        {
            get { return m_d2dHandler; }
        }

        /// <summary>
        /// Gets current DirectWrite handler.
        /// </summary>
        public DeviceHandlerDWrite HandlerDWrite
        {
            get { return m_dWriteHandler; }
        }

        /// <summary>
        /// Gets the target hardware.
        /// </summary>
        public TargetHardware TargetHardware
        {
            get { return m_targetHardware; }
        }

        /// <summary>
        /// Gets current graphics configuration.
        /// </summary>
        public GraphicsConfiguration Configuration
        {
            get { return m_currentConfiguration; }
        }
    }
}
