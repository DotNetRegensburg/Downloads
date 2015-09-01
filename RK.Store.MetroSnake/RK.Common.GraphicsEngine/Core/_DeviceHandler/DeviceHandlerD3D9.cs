#if DESKTOP
using System;
using System.Runtime.InteropServices;

using D3D9 = SharpDX.Direct3D9;
using DXGI = SharpDX.DXGI;

namespace RK.Common.GraphicsEngine.Core
{
    public class DeviceHandlerD3D9
    {
        private GraphicsCore m_core;
        //private DeviceHandlerDXGI m_dxgiHandler;
        private D3D9.Direct3DEx m_direct3DEx;
        private D3D9.DeviceEx m_deviceEx;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerD3D9"/> class.
        /// </summary>
        /// <param name="core">The core.</param>
        public DeviceHandlerD3D9(GraphicsCore core)
        {
            //Update member variables
            m_core = core;

            //Just needed when on direct3d 11 hardware
            if ((core.TargetHardware == TargetHardware.DirectX11) ||
                (core.TargetHardware == TargetHardware.DirectX9))
            {
                ////Get target adapter
                //DXGI.Adapter adapter = m_dxgiHandler.Factory.GetAdapter(0);

                //Prepare device creation
                D3D9.CreateFlags createFlags =
                    D3D9.CreateFlags.HardwareVertexProcessing |
                    D3D9.CreateFlags.PureDevice |
                    D3D9.CreateFlags.FpuPreserve;
                D3D9.PresentParameters presentparams = new D3D9.PresentParameters();
                presentparams.Windowed = true;
                presentparams.SwapEffect = D3D9.SwapEffect.Discard;
                presentparams.DeviceWindowHandle = GetDesktopWindow();
                presentparams.PresentationInterval = D3D9.PresentInterval.Default;
                presentparams.BackBufferCount = 1;

                //Create the device finally
                m_direct3DEx = new D3D9.Direct3DEx();
                m_deviceEx = new D3D9.DeviceEx(m_direct3DEx, 0, D3D9.DeviceType.Hardware, IntPtr.Zero, createFlags, presentparams);
            }
            else if (core.TargetHardware == TargetHardware.SoftwareRenderer)
            {
                //Not supported in software mode
            }
        }

        /// <summary>
        /// Gets current desktop window.
        /// </summary>
        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// Gets the initialized device.
        /// </summary>
        public D3D9.DeviceEx Device
        {
            get { return m_deviceEx; }
        }

        /// <summary>
        /// Gets current DirectX context.
        /// </summary>
        public D3D9.Direct3DEx Context
        {
            get { return m_direct3DEx; }
        }
    }
}
#endif