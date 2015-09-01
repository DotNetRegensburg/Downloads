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
    public class DeviceHandlerDXGI
    {
        private DXGI.Factory m_factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerDXGI"/> class.
        /// </summary>
        public DeviceHandlerDXGI(GraphicsCore core, DeviceHandlerD3D11 d3d11Handler)
        {
            if (core.TargetHardware == TargetHardware.SoftwareRenderer)
            {
                m_factory = d3d11Handler.GetFactory();
            }
            else
            {
                m_factory = new DXGI.Factory1();
            }
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            m_factory = GraphicsHelper.DisposeGraphicsObject(m_factory);
        }

        /// <summary>
        /// Gets current factory object.
        /// </summary>
        /// <value>The factory.</value>
        public DXGI.Factory Factory
        {
            get { return m_factory; }
        }
    }
}
