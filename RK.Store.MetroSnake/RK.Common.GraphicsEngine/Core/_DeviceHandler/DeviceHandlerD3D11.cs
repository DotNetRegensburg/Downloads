
using System;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using D3D = SharpDX.Direct3D;

//Some type mappings
#if WINRT
using D3dDevice = SharpDX.Direct3D11.Device1;
#endif
#if DESKTOP
using D3dDevice = SharpDX.Direct3D11.Device;
#endif

namespace RK.Common.GraphicsEngine.Core
{
    public class DeviceHandlerD3D11
    {
        private GraphicsCore m_core;
        private bool m_bgraEnabled;

        //Resources from Direct3D11 api
        private D3dDevice m_device;
        private D3D11.DeviceContext m_immediateContext;
        private D3D.FeatureLevel m_featureLevel;
         
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerD3D11"/> class.
        /// </summary>
        /// <param name="dxgiHandler">The dxgi handler.</param>
        public DeviceHandlerD3D11(GraphicsCore core)
        {
            //Update member variables
            m_core = core;

            //Build device creation flags
            D3D11.DeviceCreationFlags createFlags = D3D11.DeviceCreationFlags.BgraSupport;
            if (core.IsDebugEnabled) { createFlags |= D3D11.DeviceCreationFlags.Debug; }

            //Select type of the driver
            D3D.DriverType driverType = D3D.DriverType.Hardware;
            if (core.TargetHardware == TargetHardware.SoftwareRenderer) { driverType = D3D.DriverType.Warp; }

            //Try to create the device two times (first try: using video support on winrt target)
#if WINRT
            try
            {
                if (m_device == null)
                {
                    D3D11.DeviceCreationFlags secondTryFlags = createFlags | D3D11.DeviceCreationFlags.VideoSupport;
                    using (D3D11.Device device = new D3D11.Device(driverType, secondTryFlags, D3D.FeatureLevel.Level_9_1))
                    {
                        m_device = device.QueryInterface<D3dDevice>();
                    }
                }
            }
            catch (Exception)
            {
                //Nothing to do here..
            }

            ////First try (try to use d3d11.0 feature level)
            //try
            //{
            //    D3D11.DeviceCreationFlags firstTryFlags = createFlags | D3D11.DeviceCreationFlags.VideoSupport;
            //    using (D3D11.Device device = new D3D11.Device(driverType, firstTryFlags, D3D.FeatureLevel.Level_11_0))
            //    {
            //        m_device = device.QueryInterface<D3dDevice>();
            //    }
            //}
            //catch (Exception)
            //{
            //    //Nothing to do here..
            //}

            ////Second try (try to use d3d10 feature level)
            //if (m_device == null)
            //{
            //    try
            //    {
            //        D3D11.DeviceCreationFlags firstTryFlags = createFlags | D3D11.DeviceCreationFlags.VideoSupport;
            //        using (D3D11.Device device = new D3D11.Device(driverType, firstTryFlags, D3D.FeatureLevel.Level_10_0))
            //        {
            //            m_device = device.QueryInterface<D3dDevice>();
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        //Nothing to do here..
            //    }
            //}
#endif
#if DESKTOP
            //Try to use modern direct3d 11 for 
            try
            {
                D3D11.DeviceCreationFlags direct3D11Flags = createFlags;
                using (D3D11.Device device = new D3D11.Device(driverType, direct3D11Flags, D3D.FeatureLevel.Level_11_0))
                {
                    m_device = device.QueryInterface<D3dDevice>();
                }
            }
            catch (Exception)
            {
                //Nothing to do here..
            }

            //Second try (try to use d3d10 feature level)
            if (m_device == null)
            {
                try
                {
                    D3D11.DeviceCreationFlags firstTryFlags = createFlags;
                    using (D3D11.Device device = new D3D11.Device(driverType, firstTryFlags, D3D.FeatureLevel.Level_10_0))
                    {
                        m_device = device.QueryInterface<D3dDevice>();
                    }
                }
                catch (Exception)
                {
                    //Nothing to do here..
                }
            }
#endif
            //Fallback step (minimum feature level, each hardware shuould be able to use that (including windows phone and surface))
            if (m_device == null)
            {
                D3D11.DeviceCreationFlags secondTryFlags = createFlags;
                using (D3D11.Device device = new D3D11.Device(driverType, secondTryFlags, D3D.FeatureLevel.Level_9_1))
                {
                    m_device = device.QueryInterface<D3dDevice>();
                }
            }

            if (m_device == null) { throw new GraphicsEngineException("Unable to initialize d3d11 device!"); }

            ////Create the toolkit view of the device
            //m_tkDevice = TKGfx.GraphicsDevice.New(m_device);

            //Get and check feature level
            m_featureLevel = m_device.FeatureLevel;

            //Get immediate context from the device
            m_immediateContext = m_device.ImmediateContext;
        }


        /// <summary>
        /// Gets the factory the device was created with.
        /// </summary>
        public DXGI.Factory1 GetFactory()
        {
            if (m_device != null)
            {
                DXGI.Device dxgiDevice = new DXGI.Device(m_device.NativePointer);
                DXGI.Factory1 factory = dxgiDevice.Adapter.GetParent<DXGI.Factory1>();
                GraphicsHelper.DisposeGraphicsObject(dxgiDevice);

                return factory;
            }
            return null;
        }

        /// <summary>
        /// Gets the adapter the device was creaed with.
        /// </summary>
        public DXGI.Adapter GetAdapter()
        {
            if (m_device != null)
            {
                DXGI.Device dxgiDevice = new DXGI.Device(m_device.NativePointer);
                DXGI.Adapter adapter = dxgiDevice.Adapter;
                GraphicsHelper.DisposeGraphicsObject(dxgiDevice);

                return adapter;
            }
            return null;
        }

        /// <summary>
        /// Gets the Dxgi device object.
        /// </summary>
        public DXGI.Device GetDxgiDevice()
        {
            if (m_device != null) { return new DXGI.Device(m_device.NativePointer); }
            else { return null; }
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            m_immediateContext = GraphicsHelper.DisposeGraphicsObject(m_immediateContext);
            m_device = GraphicsHelper.DisposeGraphicsObject(m_device);

            m_bgraEnabled = false;
        }

        /// <summary>
        /// Gets current feature level.
        /// </summary>
        public D3D.FeatureLevel FeatureLevel
        {
            get { return m_featureLevel; }
        }

        /// <summary>
        /// Gets the Direct3D 11 device.
        /// </summary>
        public D3dDevice Device
        {
            get { return m_device; }
        }

        /// <summary>
        /// Gets the immediate context.
        /// </summary>
        public D3D11.DeviceContext ImmediateContext
        {
            get { return m_immediateContext; }
        }

        /// <summary>
        /// Are Direct2D textures possible?
        /// </summary>
        public bool IsDirect2DTextureEnabled
        {
            get { return m_bgraEnabled; }
        }
    }
}
