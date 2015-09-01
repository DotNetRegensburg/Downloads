using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D3D11  = SlimDX.Direct3D11;
using D3D10  = SlimDX.Direct3D10;
using D3D101 = SlimDX.Direct3D10_1;
using DXGI   = SlimDX.DXGI;

namespace Common.GraphicsEngine.Core
{
    public class DeviceHandlerD3D101
    {
        private GraphicsCore m_core;
        private DeviceHandlerDXGI m_dxgiHandler;

        //Resources from Direct3D 10.1 api
        private D3D101.Device1 m_device;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerD3D101"/> class.
        /// </summary>
        /// <param name="dxgiHandler">The dxgi handler.</param>
        public DeviceHandlerD3D101(GraphicsCore core, DeviceHandlerDXGI dxgiHandler, DeviceHandlerD3D11 d3d11Handler)
        {
            //Update member variables
            m_core = core;
            m_dxgiHandler = dxgiHandler;

            //Just needed when on direct3d 11 hardware
            if (core.TargetHardware == TargetHardware.DirectX11)
            {
                //Get target adapter
                DXGI.Adapter adapter = m_dxgiHandler.Factory.GetAdapter(0);

                //Prepare creation flags (use BgraSupport to enable Direct2D interop)
                D3D10.DeviceCreationFlags createFlags = D3D10.DeviceCreationFlags.SingleThreaded;
                if (core.IsDebugEnabled) { createFlags |= D3D10.DeviceCreationFlags.Debug; }
                if (D3D11.Device.GetSupportedFeatureLevel(adapter) == D3D11.FeatureLevel.Level_11_0) { createFlags |= D3D10.DeviceCreationFlags.BgraSupport; }

                //Create the device
                m_device = new D3D101.Device1(adapter, D3D10.DriverType.Hardware, createFlags, D3D101.FeatureLevel.Level_10_1);
                m_device.InputAssembler.SetPrimitiveTopology(D3D10.PrimitiveTopology.TriangleList);
            }
            else if (core.TargetHardware == TargetHardware.SoftwareRenderer)
            {
                //Get target adapter
                DXGI.Adapter adapter = d3d11Handler.GetAdapter();

                //Prepare creation flags (use BgraSupport to enable Direct2D interop)
                D3D10.DeviceCreationFlags createFlags = D3D10.DeviceCreationFlags.SingleThreaded;
                if (core.IsDebugEnabled) { createFlags |= D3D10.DeviceCreationFlags.Debug; }
                createFlags |= D3D10.DeviceCreationFlags.BgraSupport;

                //Create the device
                m_device = new D3D101.Device1(adapter, D3D10.DriverType.Warp, createFlags, D3D101.FeatureLevel.Level_10_1);
                m_device.InputAssembler.SetPrimitiveTopology(D3D10.PrimitiveTopology.TriangleList);
            }
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            m_device = GraphicsHelper.DisposeGraphicsObject(m_device);
        }

        /// <summary>
        /// Gets the device object.
        /// </summary>
        public D3D101.Device1 Device
        {
            get { return m_device; }
        }
    }
}
