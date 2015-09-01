using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D2D = SlimDX.Direct2D;

namespace Common.GraphicsEngine.Core
{
    public class DeviceHandlerD2D
    {
        private GraphicsCore m_core;
        private DeviceHandlerDXGI m_dxgiHandler;

        //Resources form Direct2D api
        private D2D.Factory m_factory;

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
            m_factory = new D2D.Factory(
                D2D.FactoryType.SingleThreaded,
                core.IsDebugEnabled ? D2D.DebugLevel.Information : D2D.DebugLevel.None);
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
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
