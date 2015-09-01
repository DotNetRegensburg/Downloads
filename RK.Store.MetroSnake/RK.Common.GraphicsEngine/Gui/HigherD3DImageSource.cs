using System;
using System.Windows;
using System.Windows.Interop;
using RK.Common.GraphicsEngine.Core;

using D3D11 = SharpDX.Direct3D11;
using D3D9 = SharpDX.Direct3D9;
using DXGI = SharpDX.DXGI;

namespace RK.Common.GraphicsEngine.Gui
{
    public class HigherD3DImageSource : D3DImage, IDisposable
    {
        private volatile static int s_activeClients;
        private static D3D9.Direct3DEx s_d3dContext;
        private static D3D9.DeviceEx m_d3dDevice;

        private D3D9.Texture m_d3dRenderTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="HigherD3DImageSource"/> class.
        /// </summary>
        public HigherD3DImageSource()
        {
            this.StartD3D();
            
            s_activeClients++;
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        public void Dispose()
        {
            SetRenderTarget(null);

            m_d3dRenderTarget = GraphicsHelper.DisposeGraphicsObject(this.m_d3dRenderTarget);
            s_activeClients--;

            this.EndD3D();
        }

        /// <summary>
        /// Invalidates the direct3D image.
        /// </summary>
        public void InvalidateD3DImage()
        {
            if (this.m_d3dRenderTarget != null)
            {
                base.Lock();
                base.AddDirtyRect(new Int32Rect(0, 0, base.PixelWidth, base.PixelHeight));
                base.Unlock();
            }
        }

        /// <summary>
        /// Sets the render target of this D3DImage object.
        /// </summary>
        /// <param name="renderTarget">The render target to set.</param>
        public void SetRenderTarget(D3D11.Texture2D renderTarget)
        {
            if (this.m_d3dRenderTarget != null)
            {
                this.m_d3dRenderTarget = null;

                base.Lock();
                base.SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
                base.Unlock();
            }

            if (renderTarget == null) { return; }

            if (!IsShareable(renderTarget))
            {
                throw new ArgumentException("Texture must be created with ResourceOptionFlags.Shared");
            }

            D3D9.Format format = HigherD3DImageSource.TranslateFormat(renderTarget);
            if (format == D3D9.Format.Unknown)
            {
                throw new ArgumentException("Texture format is not compatible with OpenSharedResource");
            }

            IntPtr handle = GetSharedHandle(renderTarget);
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException("Handle");
            }

            //Map the texture to the D3DImage base class
            this.m_d3dRenderTarget = new D3D9.Texture(
                m_d3dDevice,
                renderTarget.Description.Width, 
                renderTarget.Description.Height,
                1, D3D9.Usage.RenderTarget, format, D3D9.Pool.Default, ref handle);
            using (D3D9.Surface surface = this.m_d3dRenderTarget.GetSurfaceLevel(0))
            {
                base.Lock();
                base.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                base.Unlock();
            }
        }

        /// <summary>
        /// Starts 3D rendering.
        /// </summary>
        private void StartD3D()
        {
            if (HigherD3DImageSource.s_activeClients != 0)
            {
                return;
            }

            s_d3dContext = GraphicsCore.Current.HandlerD3D9.Context;
            m_d3dDevice = GraphicsCore.Current.HandlerD3D9.Device;
        }

        /// <summary>
        /// Ends 3D rendering
        /// </summary>
        private void EndD3D()
        {
            if (HigherD3DImageSource.s_activeClients != 0)
                return;

            GraphicsHelper.SafeDispose(ref this.m_d3dRenderTarget);
        }

        /// <summary>
        /// Gets the handle that can be used for resource sharing.
        /// </summary>
        /// <param name="Texture">The texture to be shared.</param>
        private IntPtr GetSharedHandle(D3D11.Texture2D Texture)
        {
            using (DXGI.Resource resource = Texture.QueryInterface<DXGI.Resource>())
            {
                return resource.SharedHandle;
            }
        }

        /// <summary>
        /// Gets the format for sharing.
        /// </summary>
        /// <param name="Texture">The texture to get the format for.</param>
        private static D3D9.Format TranslateFormat(D3D11.Texture2D Texture)
        {
            switch (Texture.Description.Format)
            {
                case SharpDX.DXGI.Format.R10G10B10A2_UNorm:
                    return SharpDX.Direct3D9.Format.A2B10G10R10;

                case SharpDX.DXGI.Format.R16G16B16A16_Float:
                    return SharpDX.Direct3D9.Format.A16B16G16R16F;

                case SharpDX.DXGI.Format.B8G8R8A8_UNorm:
                    return SharpDX.Direct3D9.Format.A8R8G8B8;

                default:
                    return SharpDX.Direct3D9.Format.Unknown;
            }
        }

        /// <summary>
        /// Is the given texture sharable?
        /// </summary>
        /// <param name="textureToCheck">The checker to check.</param>
        private static bool IsShareable(D3D11.Texture2D textureToCheck)
        {
            return (textureToCheck.Description.OptionFlags & D3D11.ResourceOptionFlags.Shared) != 0;
        }

        public bool HasRenderTarget
        {
            get { return m_d3dRenderTarget != null; }
        }
    }
}
