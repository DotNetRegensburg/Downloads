using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;
using DXGI  = SlimDX.DXGI;

namespace Common.GraphicsEngine.Core
{
    public class DeviceHandlerD3D11
    {
        private GraphicsCore m_core;
        private DeviceHandlerDXGI m_dxgiHandler;
        private bool m_bgraEnabled;

        //Resources from Direct3D11 api
        private D3D11.Device m_device;
        private D3D11.DeviceContext m_immediateContext;
         
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerD3D11"/> class.
        /// </summary>
        /// <param name="dxgiHandler">The dxgi handler.</param>
        public DeviceHandlerD3D11(GraphicsCore core, DeviceHandlerDXGI dxgiHandler)
        {
            //Update member variables
            m_core = core;
            m_dxgiHandler = dxgiHandler;

            //Build device creation flags
            D3D11.DeviceCreationFlags createFlags = D3D11.DeviceCreationFlags.None;
            if (core.IsDebugEnabled) { createFlags |= D3D11.DeviceCreationFlags.Debug; }

            //Initialize device and adapter
            if (core.TargetHardware == TargetHardware.SoftwareRenderer)
            {
                m_device = new D3D11.Device(D3D11.DriverType.Warp, createFlags);
                m_immediateContext = m_device.ImmediateContext;
            }
            else
            {
                //Get target adapter
                DXGI.Adapter targetAdapter = dxgiHandler.Factory.GetAdapter(0);

                //Add BGRA-Support if neccessary (needed for Direct2D interop)
                if (D3D11.Device.GetSupportedFeatureLevel(targetAdapter) == D3D11.FeatureLevel.Level_11_0)
                {
                    createFlags |= D3D11.DeviceCreationFlags.BgraSupport;
                    m_bgraEnabled = true;
                }

                //Create the device
                m_device = new D3D11.Device(
                    targetAdapter,
                    createFlags);
                m_immediateContext = m_device.ImmediateContext;
            }
        }

        /// <summary>
        /// Gets the factory the device was created with.
        /// </summary>
        public DXGI.Factory GetFactory()
        {
            if (m_device != null)
            {
                DXGI.Device dxgiDevice = new DXGI.Device(m_device);
                DXGI.Factory factory = dxgiDevice.Adapter.GetParent<DXGI.Factory>();
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
                DXGI.Device dxgiDevice = new DXGI.Device(m_device);
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
            if (m_device != null) { return new DXGI.Device(m_device); }
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
        /// Gets the Direct3D 11 device.
        /// </summary>
        public D3D11.Device Device
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
