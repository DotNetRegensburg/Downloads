using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;

//Some namespace mappings
using Buffer = SlimDX.Direct3D11.Buffer;
using D2D    = SlimDX.Direct2D;
using GDI    = System.Drawing;
using DXGI   = SlimDX.DXGI;
using D3D10  = SlimDX.Direct3D10;
using D3D101 = SlimDX.Direct3D10_1;
using D3D11  = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Core
{
    public static class GraphicsHelper
    {
        /// <summary>
        /// Converts a System.Drawing.Bitmap to a DirectX 11 texture object.
        /// </summary>
        /// <param name="device">Device on wich the resource should be created.</param>
        /// <param name="bitmap">The source bitmap.</param>
        public static D3D11.Texture2D CreateTextureFromBitmap(D3D11.Device device, Bitmap bitmap)
        {
            return CreateTextureFromBitmap(device, bitmap, 1);
        }

        /// <summary>
        /// Adjusts the given texture size to meed the hardware's requirements.
        /// </summary>
        public static Size AdjustTextureSize(Size currentSize)
        {
            Size result = currentSize;
            for (int loop = 1; loop < 6500; loop *= 2)
            {
                if (result.Width <= loop) { result.Width = loop; break; }
            }
            for (int loop = 1; loop < 6500; loop *= 2)
            {
                if (result.Height <= loop) { result.Height = loop; break; }
            }
            return result;
        }

        /// <summary>
        /// Converts a System.Drawing.Bitmap to a DirectX 11 texture object.
        /// </summary>
        /// <param name="device">Device on wich the resource should be created.</param>
        /// <param name="bitmap">The source bitmap.</param>
        /// <param name="mipLevels">Total count of levels for mipmapping.</param>
        public static D3D11.Texture2D CreateTextureFromBitmap(D3D11.Device device, Bitmap bitmap, int mipLevels)
        {
            D3D11.Texture2D result = null;

            //Lock bitmap so it can be accessed for texture loading
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                //Convert pixel format
                byte* startPointer = (byte*)bitmapData.Scan0.ToPointer();
                for (int loop = 0; loop < (bitmapData.Stride / 4) * bitmapData.Height; loop++)
                {
                    byte blueValue = startPointer[loop * 4];
                    byte greenValue = startPointer[loop * 4 + 1];
                    byte redValue = startPointer[loop * 4 + 2];
                    byte alphaValue = startPointer[loop * 4 + 3];
                    startPointer[loop * 4] = redValue;
                    startPointer[loop * 4 + 1] = greenValue;
                    startPointer[loop * 4 + 2] = blueValue;
                    startPointer[loop * 4 + 3] = alphaValue;
                }
            }

            //Open a reading stream for bitmap memory
            DataStream dataStream = new DataStream(
                bitmapData.Scan0,
                bitmapData.Stride * bitmapData.Height,
                true, false);
            DataRectangle dataRectangle = new DataRectangle(bitmapData.Stride, dataStream);
            try
            {
                //Load the texture
                result = new D3D11.Texture2D(device, new D3D11.Texture2DDescription()
                {
                    BindFlags = D3D11.BindFlags.ShaderResource | D3D11.BindFlags.RenderTarget,
                    CpuAccessFlags = D3D11.CpuAccessFlags.None,
                    Format = DXGI.Format.R8G8B8A8_UNorm,
                    OptionFlags = D3D11.ResourceOptionFlags.None | D3D11.ResourceOptionFlags.GenerateMipMaps,
                    MipLevels = 0,
                    Usage = D3D11.ResourceUsage.Default,
                    Width = bitmap.Width,
                    Height = bitmap.Height,
                    ArraySize = 1,
                    SampleDescription = new DXGI.SampleDescription(1, 0)
                }, new DataRectangle[] { dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle });

                //Workaround for now... auto generate mip-levels
                D3D11.ShaderResourceView shaderResourceView = new D3D11.ShaderResourceView(device, result);
                device.ImmediateContext.GenerateMips(shaderResourceView);
                shaderResourceView.Dispose();
            }
            finally
            {
                //Free bitmap-access resources
                dataStream.Dispose();
                bitmap.UnlockBits(bitmapData);
            }

            return result;
        }

        /// <summary>
        /// Creates a default SwapChain for the given target control.
        /// </summary>
        /// <param name="targetControl">Target control of the swap chain.</param>
        /// <param name="factory">Factory for SwapChain creation.</param>
        /// <param name="renderDevice">Graphics device.</param>
        public static DXGI.SwapChain CreateDefaultSwapChain(Control targetControl, DXGI.Factory factory, D3D11.Device renderDevice)
        {
            //Create description of the swap chain
            DXGI.SwapChainDescription swapChainDesc = new DXGI.SwapChainDescription();
            swapChainDesc.OutputHandle = targetControl.Handle;
            swapChainDesc.IsWindowed = true;
            swapChainDesc.BufferCount = 1;
            swapChainDesc.Flags = DXGI.SwapChainFlags.AllowModeSwitch;
            swapChainDesc.ModeDescription = new DXGI.ModeDescription(
                targetControl.Width,
                targetControl.Height,
                new Rational(60, 1),
                DXGI.Format.R8G8B8A8_UNorm);
            swapChainDesc.SampleDescription = new DXGI.SampleDescription(1, 0);
            swapChainDesc.SwapEffect = DXGI.SwapEffect.Discard;
            swapChainDesc.Usage = DXGI.Usage.RenderTargetOutput;

            //Create and return the swap chain and the render target
            return new DXGI.SwapChain(factory, renderDevice, swapChainDesc);
        }

        /// <summary>
        /// Creates a default viewport for the given control.
        /// </summary>
        /// <param name="targetControl">Target control object.</param>
        public static D3D11.Viewport CreateDefaultViewport(Control targetControl)
        {
            return CreateDefaultViewport(
                targetControl.Width,
                targetControl.Height);
        }

        /// <summary>
        /// Creates a default viewport for the given width and height
        /// </summary>
        /// <param name="targetControl">Target control object.</param>
        public static D3D11.Viewport CreateDefaultViewport(int width, int height)
        {
            D3D11.Viewport result = new D3D11.Viewport(
                0f, 0f,
                (float)width, (float)height,
                0f, 1f);
            return result;
        }

        /// <summary>
        /// Creates a standard texture with the given width and height.
        /// </summary>
        /// <param name="device">Graphics device.</param>
        /// <param name="width">Width of generated texture.</param>
        /// <param name="height">Height of generated texture.</param>
        public static D3D11.Texture2D CreateTexture(D3D11.Device device, int width, int height)
        {
            D3D11.Texture2DDescription textureDescription = new D3D11.Texture2DDescription();
            textureDescription.Width = width;
            textureDescription.Height = height;
            textureDescription.MipLevels = 1;
            textureDescription.ArraySize = 1;
            textureDescription.Format = DXGI.Format.B8G8R8A8_UNorm;
            textureDescription.Usage = D3D11.ResourceUsage.Default;
            textureDescription.SampleDescription = new DXGI.SampleDescription(1, 0);
            textureDescription.BindFlags = D3D11.BindFlags.ShaderResource;
            textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;

            return new D3D11.Texture2D(device, textureDescription);
        }

        /// <summary>
        /// Creates a render target texture with the given width and height.
        /// </summary>
        /// <param name="device">Graphics device.</param>
        /// <param name="width">Width of generated texture.</param>
        /// <param name="height">Height of generated texture.</param>
        public static D3D11.Texture2D CreateRenderTargetTexture(D3D11.Device device, int width, int height)
        {
            D3D11.Texture2DDescription textureDescription = new D3D11.Texture2DDescription();
            textureDescription.Width = width;
            textureDescription.Height = height;
            textureDescription.MipLevels = 1;
            textureDescription.ArraySize = 1;
            textureDescription.Format = DXGI.Format.R8G8B8A8_UNorm;
            textureDescription.Usage = D3D11.ResourceUsage.Default;
            textureDescription.SampleDescription = new DXGI.SampleDescription(1, 0);
            textureDescription.BindFlags = D3D11.BindFlags.ShaderResource | D3D11.BindFlags.RenderTarget;
            textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;

            return new D3D11.Texture2D(device, textureDescription);
        }

        /// <summary>
        /// Creates a depth buffer texture with given width and height.
        /// </summary>
        /// <param name="device">Graphics device.</param>
        /// <param name="width">Width of generated texture.</param>
        /// <param name="height">Height of generated texture.</param>
        public static D3D11.Texture2D CreateDepthBufferTexture(D3D11.Device device, int width, int height)
        {
            D3D11.Texture2DDescription textureDescription = new D3D11.Texture2DDescription();
            textureDescription.Width = width;
            textureDescription.Height = height;
            textureDescription.MipLevels = 1;
            textureDescription.ArraySize = 1;
            textureDescription.Format = GraphicsCore.Current.TargetHardware == TargetHardware.DirectX11 ? DXGI.Format.D32_Float : DXGI.Format.D24_UNorm_S8_UInt;
            textureDescription.Usage = D3D11.ResourceUsage.Default;
            textureDescription.SampleDescription = new DXGI.SampleDescription(1, 0);
            textureDescription.BindFlags = D3D11.BindFlags.DepthStencil;
            textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;

            return new D3D11.Texture2D(device, textureDescription);
        }

        /// <summary>
        /// Creates an immutable vertex buffer from the given vertex array.
        /// </summary>
        /// <typeparam name="T">Type of a vertex.</typeparam>
        /// <param name="device">Graphics device.</param>
        /// <param name="vertices">The vertex array.</param>
        public static Buffer CreateImmutableVertexBuffer<T>(D3D11.Device device, T[] vertices)
            where T : struct
        {
            Type vertexType = typeof(T);
            int vertexCount = vertices.Length;
            int vertexSize = Marshal.SizeOf(vertexType);
            DataStream outStream = new DataStream(
                vertexCount * vertexSize,
                true, true);

            outStream.WriteRange(vertices);
            outStream.Position = 0;

            D3D11.BufferDescription bufferDescription = new D3D11.BufferDescription();
            bufferDescription.BindFlags = D3D11.BindFlags.VertexBuffer;
            bufferDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            bufferDescription.OptionFlags = D3D11.ResourceOptionFlags.None;
            bufferDescription.SizeInBytes = vertexCount * vertexSize;
            bufferDescription.Usage = D3D11.ResourceUsage.Immutable;

            Buffer result = new Buffer(device, outStream, bufferDescription);
            outStream.Close();
            outStream.Dispose();

            return result;
        }

        /// <summary>
        /// Creates an immutable index buffer from the given index array.
        /// </summary>
        /// <param name="device">Graphics device.</param>
        /// <param name="indices">Source index array.</param>
        public static Buffer CreateImmutableIndexBuffer(D3D11.Device device, short[] indices)
        {
            DataStream outStreamIndex = new DataStream(indices.Length * Marshal.SizeOf(typeof(short)), true, true);

            outStreamIndex.WriteRange(indices);
            outStreamIndex.Position = 0;

            D3D11.BufferDescription bufferDescriptionIndex = new D3D11.BufferDescription();
            bufferDescriptionIndex.BindFlags = D3D11.BindFlags.IndexBuffer;
            bufferDescriptionIndex.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            bufferDescriptionIndex.OptionFlags = D3D11.ResourceOptionFlags.None;
            bufferDescriptionIndex.SizeInBytes = indices.Length * Marshal.SizeOf(typeof(short));
            bufferDescriptionIndex.Usage = D3D11.ResourceUsage.Immutable;

            Buffer result = new Buffer(device, outStreamIndex, bufferDescriptionIndex);

            outStreamIndex.Close();
            outStreamIndex.Dispose();

            return result;
        }

        /// <summary>
        /// Resizes the given bitmap to given size.
        /// </summary>
        /// <param name="bitmapToResize">The bitmap to resize.</param>
        /// <param name="newWidth">Width of the generated bitmap.</param>
        /// <param name="newHeight">Height of the genrated bitmap.</param>
        public static Bitmap ResizeGdiBitmap(Bitmap bitmapToResize, int newWidth, int newHeight)
        {
            Bitmap result = new Bitmap(newWidth, newHeight);
            using (GDI.Graphics g = GDI.Graphics.FromImage((Image)result))
            {
                g.DrawImage(bitmapToResize, 0, 0, newWidth, newHeight);
            }
            return result;
        }


        /// <summary>
        /// Loads a Direct2D bitmap from the given gdi resource.
        /// </summary>
        /// <param name="drawingBitmap">The source gdi bitmap.</param>
        /// <param name="renderTarget">The RenderTarget object for wich to create the resource.</param>
        public static D2D.Bitmap LoadBitmap(D2D.RenderTarget renderTarget, Bitmap drawingBitmap)
        {
            D2D.Bitmap result = null;

            //Lock the gdi resource
            BitmapData drawingBitmapData = drawingBitmap.LockBits(
                new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

            //Prepare loading the image from gdi resource
            DataStream dataStream = new DataStream(
                drawingBitmapData.Scan0,
                drawingBitmapData.Stride * drawingBitmapData.Height,
                true, false);
            D2D.BitmapProperties properties = new D2D.BitmapProperties();
            properties.PixelFormat = new D2D.PixelFormat(
                DXGI.Format.B8G8R8A8_UNorm,
                D2D.AlphaMode.Premultiplied);

            //Load the image from the gdi resource
            result = new D2D.Bitmap(
                renderTarget,
                new Size(drawingBitmap.Width, drawingBitmap.Height),
                dataStream, drawingBitmapData.Stride,
                properties);

            //Unlock the gdi resource
            drawingBitmap.UnlockBits(drawingBitmapData);

            return result;
        }

        /// <summary>
        /// Copies all contents of the given gdi bitmap into the given Direct2D bitmap.
        /// </summary>
        /// <param name="targetBitmap">Target Direct2D bitmap.</param>
        /// <param name="drawingBitmap">The source gdi bitmap.</param>
        public static void SetBitmapContents(D2D.Bitmap targetBitmap, Bitmap drawingBitmap)
        {
            //Lock the gdi resource
            BitmapData drawingBitmapData = drawingBitmap.LockBits(
                new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            targetBitmap.FromMemory(drawingBitmapData.Scan0, drawingBitmapData.Stride);
            drawingBitmap.UnlockBits(drawingBitmapData);
        }

        /// <summary>
        /// Disposes the given object and returns null.
        /// </summary>
        public static T DisposeGraphicsObject<T>(T objectToDispose)
            where T : class, IDisposable
        {
            if (objectToDispose == null) { return null; }

            try { objectToDispose.Dispose(); }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// Disposes all objects within the given enumeration.
        /// </summary>
        /// <param name="enumeration">Enumeration containing all disposable objects.</param>
        public static void DisposeGraphicsObjects<T>(IEnumerable<T> enumeration)
            where T : class, IDisposable
        {
            if (enumeration == null) { throw new ArgumentNullException("enumeration"); }

            foreach (T actItem in enumeration)
            {
                DisposeGraphicsObject(actItem);
            }
        }
    }
}
