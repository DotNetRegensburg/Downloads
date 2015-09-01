using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX;

//Some namespace mappings
using Buffer = SharpDX.Direct3D11.Buffer;
using D2D = SharpDX.Direct2D1;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using WIC = SharpDX.WIC;

namespace RK.Common.GraphicsEngine.Core
{
    public static class GraphicsHelper
    {
        ///// <summary>
        ///// Converts a System.Drawing.Bitmap to a DirectX 11 texture object.
        ///// </summary>
        ///// <param name="device">Device on wich the resource should be created.</param>
        ///// <param name="bitmap">The source bitmap.</param>
        //public static D3D11.Texture2D CreateTextureFromBitmap(D3D11.Device device, Bitmap bitmap)
        //{
        //    return CreateTextureFromBitmap(device, bitmap, 1);
        //}

        ///// <summary>
        ///// Creates a default SwapChain for the given target control.
        ///// </summary>
        ///// <param name="targetControl">Target control of the swap chain.</param>
        ///// <param name="factory">Factory for SwapChain creation.</param>
        ///// <param name="renderDevice">Graphics device.</param>
        //public static DXGI.SwapChain CreateDefaultSwapChain(Control targetControl, DXGI.Factory factory, D3D11.Device renderDevice)
        //{
        //    //Create description of the swap chain
        //    DXGI.SwapChainDescription swapChainDesc = new DXGI.SwapChainDescription();
        //    swapChainDesc.OutputHandle = targetControl.Handle;
        //    swapChainDesc.IsWindowed = true;
        //    swapChainDesc.BufferCount = 1;
        //    swapChainDesc.Flags = DXGI.SwapChainFlags.AllowModeSwitch;
        //    swapChainDesc.ModeDescription = new DXGI.ModeDescription(
        //        targetControl.Width,
        //        targetControl.Height,
        //        new DXGI.Rational(60, 1),
        //        DXGI.Format.R8G8B8A8_UNorm);
        //    swapChainDesc.SampleDescription = new DXGI.SampleDescription(1, 0);
        //    swapChainDesc.SwapEffect = DXGI.SwapEffect.Discard;
        //    swapChainDesc.Usage = DXGI.Usage.RenderTargetOutput;

        //    //Create and return the swap chain and the render target
        //    return new DXGI.SwapChain(factory, renderDevice, swapChainDesc);
        //}

#if DESKTOP
        /// <summary>
        /// Converts a System.Drawing.Bitmap to a DirectX 11 texture object.
        /// </summary>
        /// <param name="device">Device on wich the resource should be created.</param>
        /// <param name="bitmap">The source bitmap.</param>
        public static D3D11.Texture2D CreateTextureFromBitmap(D3D11.Device device, System.Drawing.Bitmap bitmap)
        {
            return CreateTextureFromBitmap(device, bitmap, 1);
        }

        /// <summary>
        /// Converts a System.Drawing.Bitmap to a DirectX 11 texture object.
        /// </summary>
        /// <param name="device">Device on wich the resource should be created.</param>
        /// <param name="bitmap">The source bitmap.</param>
        /// <param name="mipLevels">Total count of levels for mipmapping.</param>
        public static D3D11.Texture2D CreateTextureFromBitmap(D3D11.Device device, System.Drawing.Bitmap bitmap, int mipLevels)
        {
            D3D11.Texture2D result = null;

            //Lock bitmap so it can be accessed for texture loading
            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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
            DataRectangle dataRectangle = new DataRectangle(bitmapData.Scan0, bitmap.Width * 4);
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
                bitmap.UnlockBits(bitmapData);
            }

            return result;
        }
#endif

        /// <summary>
        /// Creates a Direct3D 11 texture that can be shared between more devices.
        /// </summary>
        /// <param name="device">The Direct3D 11 device.</param>
        /// <param name="width">The width of the generated texture.</param>
        /// <param name="height">The height of the generated texture.</param>
        public static D3D11.Texture2D CreateSharedTexture(D3D11.Device device, int width, int height)
        {
            D3D11.Texture2DDescription textureDescription = new D3D11.Texture2DDescription
            {
                BindFlags = D3D11.BindFlags.RenderTarget | D3D11.BindFlags.ShaderResource,
                Format = DXGI.Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new DXGI.SampleDescription(1, 0),
                Usage = D3D11.ResourceUsage.Default,
                OptionFlags = D3D11.ResourceOptionFlags.Shared,
                CpuAccessFlags = D3D11.CpuAccessFlags.None,
                ArraySize = 1
            };
            return new D3D11.Texture2D(device, textureDescription);
        }

        /// <summary>
        /// Loads the texture2 D from stream.
        /// </summary>
        /// <param name="factory">The WIC factory object used for loading.</param>
        /// <param name="device">The device on wich to create the texture.</param>
        /// <param name="inStream">The source stream.</param>
        /// <returns></returns>
        public static D3D11.Texture2D LoadTexture2D(WIC.ImagingFactory factory, D3D11.Device device, Stream inStream)
        {
            return LoadTexture2DFromBitmap(
                device,
                LoadBitmap(factory, inStream));
        }

        /// <summary>
        /// Loads a new texture from the given file path.
        /// </summary>
        /// <param name="factory">The WIC factory object used for loading.</param>
        /// <param name="device">The device on wich to create the texture.</param>
        /// <param name="fileName">The source file</param>
        /// <returns></returns>
        public static D3D11.Texture2D LoadTexture2D(WIC.ImagingFactory factory, D3D11.Device device, string fileName)
        {
            return LoadTexture2DFromBitmap(
                device,
                LoadBitmap(factory, fileName));
        }

        /// <summary>
        /// Loads a bitmap using WIC.
        /// </summary>
        /// <param name="factory">The WIC factory object.</param>
        /// <param name="inStream">The stream from wich to load the texture file.</param>
        public static WIC.BitmapSource LoadBitmap(WIC.ImagingFactory factory, Stream inStream)
        {
            var bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                factory,
                inStream,
                SharpDX.WIC.DecodeOptions.CacheOnDemand
                );

            var formatConverter = new WIC.FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                WIC.PixelFormat.Format32bppPRGBA,
                WIC.BitmapDitherType.None,
                null,
                0.0,
                WIC.BitmapPaletteType.Custom);

            return formatConverter;
        }

        /// <summary>
        /// Loads a bitmap using WIC.
        /// </summary>
        /// <param name="factory">The WIC factory object.</param>
        /// <param name="inStream">The file from wich to load the texture.</param>
        public static WIC.BitmapSource LoadBitmap(WIC.ImagingFactory factory, string filename)
        {
            var bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                factory,
                filename,
                SharpDX.WIC.DecodeOptions.CacheOnDemand
                );

            var formatConverter = new WIC.FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                WIC.PixelFormat.Format32bppPRGBA,
                WIC.BitmapDitherType.None,
                null,
                0.0,
                WIC.BitmapPaletteType.Custom);

            return formatConverter;
        }

        /// <summary>
        /// Creates a <see cref="SharpDX.Direct3D11.Texture2D"/> from a WIC <see cref="SharpDX.WIC.BitmapSource"/>
        /// </summary>
        /// <param name="device">The Direct3D11 device</param>
        /// <param name="bitmapSource">The WIC bitmap source</param>
        /// <returns>A Texture2D</returns>
        public static D3D11.Texture2D LoadTexture2DFromBitmap(D3D11.Device device, WIC.BitmapSource bitmapSource)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(stride, buffer);
                return new SharpDX.Direct3D11.Texture2D(device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Immutable,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                }, new SharpDX.DataRectangle(buffer.DataPointer, stride));
            }
        }

        ///// <summary>
        ///// Converts a System.Drawing.Bitmap to a DirectX 11 texture object.
        ///// </summary>
        ///// <param name="device">Device on wich the resource should be created.</param>
        ///// <param name="bitmap">The source bitmap.</param>
        ///// <param name="mipLevels">Total count of levels for mipmapping.</param>
        //public static D3D11.Texture2D CreateTextureFromBitmap(D3D11.Device device, WIC.Bitmap bitmap, int mipLevels)
        //{
        //    D3D11.Texture2D result = null;

        //    //Lock bitmap so it can be accessed for texture loading
        //    using (WIC.BitmapLock bitmapData = bitmap.Lock(
        //        new DrawingRectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
        //        WIC.BitmapLockFlags.Read))
        //    {
        //        DataRectangle dataRectangle = bitmapData.Data;

        //        //Load the texture
        //        result = new D3D11.Texture2D(device, new D3D11.Texture2DDescription()
        //        {
        //            BindFlags = D3D11.BindFlags.ShaderResource | D3D11.BindFlags.RenderTarget,
        //            CpuAccessFlags = D3D11.CpuAccessFlags.None,
        //            Format = DXGI.Format.R8G8B8A8_UNorm,
        //            OptionFlags = D3D11.ResourceOptionFlags.None | D3D11.ResourceOptionFlags.GenerateMipMaps,
        //            MipLevels = 0,
        //            Usage = D3D11.ResourceUsage.Default,
        //            Width = bitmap.Size.Width,
        //            Height = bitmap.Size.Height,
        //            ArraySize = 1,
        //            SampleDescription = new DXGI.SampleDescription(1, 0)
        //        }, new DataRectangle[] { dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle });

        //        //Workaround for now... auto generate mip-levels
        //        D3D11.ShaderResourceView shaderResourceView = new D3D11.ShaderResourceView(device, result);
        //        device.ImmediateContext.GenerateMips(shaderResourceView);
        //        shaderResourceView.Dispose();

        //    }
            
        //    //    //new GDI.Rectangle(0, 0, bitmap.Width, bitmap.Height),
        //    //    //ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        //    //unsafe
        //    //{
        //    //    //Convert pixel format
        //    //    byte* startPointer = (byte*)bitmapData.Scan0.ToPointer();
        //    //    for (int loop = 0; loop < (bitmapData.Stride / 4) * bitmapData.Height; loop++)
        //    //    {
        //    //        byte blueValue = startPointer[loop * 4];
        //    //        byte greenValue = startPointer[loop * 4 + 1];
        //    //        byte redValue = startPointer[loop * 4 + 2];
        //    //        byte alphaValue = startPointer[loop * 4 + 3];
        //    //        startPointer[loop * 4] = redValue;
        //    //        startPointer[loop * 4 + 1] = greenValue;
        //    //        startPointer[loop * 4 + 2] = blueValue;
        //    //        startPointer[loop * 4 + 3] = alphaValue;
        //    //    }
        //    //}

        //    //Open a reading stream for bitmap memory
        //    DataRectangle dataRectangle = new DataRectangle(bitmapData.Scan0, bitmap.Width * 4);
        //    try
        //    {
        //        //Load the texture
        //        result = new D3D11.Texture2D(device, new D3D11.Texture2DDescription()
        //        {
        //            BindFlags = D3D11.BindFlags.ShaderResource | D3D11.BindFlags.RenderTarget,
        //            CpuAccessFlags = D3D11.CpuAccessFlags.None,
        //            Format = DXGI.Format.R8G8B8A8_UNorm,
        //            OptionFlags = D3D11.ResourceOptionFlags.None | D3D11.ResourceOptionFlags.GenerateMipMaps,
        //            MipLevels = 0,
        //            Usage = D3D11.ResourceUsage.Default,
        //            Width = bitmap.Width,
        //            Height = bitmap.Height,
        //            ArraySize = 1,
        //            SampleDescription = new DXGI.SampleDescription(1, 0)
        //        }, new DataRectangle[] { dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle, dataRectangle });

        //        //Workaround for now... auto generate mip-levels
        //        D3D11.ShaderResourceView shaderResourceView = new D3D11.ShaderResourceView(device, result);
        //        device.ImmediateContext.GenerateMips(shaderResourceView);
        //        shaderResourceView.Dispose();
        //    }
        //    finally
        //    {
        //        //Free bitmap-access resources
        //        bitmap.UnlockBits(bitmapData);
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Creates a default viewport for the given control.
        ///// </summary>
        ///// <param name="targetControl">Target control object.</param>
        //public static D3D11.Viewport CreateDefaultViewport(Control targetControl)
        //{
        //    return CreateDefaultViewport(
        //        targetControl.Width,
        //        targetControl.Height);
        //}

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

            if (device.FeatureLevel == SharpDX.Direct3D.FeatureLevel.Level_11_0)
            {
                textureDescription.Width = width;
                textureDescription.Height = height;
                textureDescription.MipLevels = 1;
                textureDescription.ArraySize = 1;
                textureDescription.Format = DXGI.Format.B8G8R8A8_UNorm;
                textureDescription.Usage = D3D11.ResourceUsage.Default;
                textureDescription.SampleDescription = new DXGI.SampleDescription(4, 0);
                textureDescription.BindFlags = D3D11.BindFlags.ShaderResource | D3D11.BindFlags.RenderTarget;
                textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
                textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;
            }
            else
            {
                textureDescription.Width = width;
                textureDescription.Height = height;
                textureDescription.MipLevels = 1;
                textureDescription.ArraySize = 1;
                textureDescription.Format = DXGI.Format.B8G8R8A8_UNorm;
                textureDescription.Usage = D3D11.ResourceUsage.Default;
                textureDescription.SampleDescription = new DXGI.SampleDescription(1, 0);
                textureDescription.BindFlags = D3D11.BindFlags.ShaderResource | D3D11.BindFlags.RenderTarget;
                textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
                textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;
            }

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

            if (device.FeatureLevel == SharpDX.Direct3D.FeatureLevel.Level_11_0)
            {
                textureDescription.Width = width;
                textureDescription.Height = height;
                textureDescription.MipLevels = 1;
                textureDescription.ArraySize = 1;
                textureDescription.Usage = D3D11.ResourceUsage.Default;
                textureDescription.SampleDescription = new DXGI.SampleDescription(4, 0);
                textureDescription.BindFlags = D3D11.BindFlags.DepthStencil;
                textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
                textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;
            }
            else
            {
                textureDescription.Width = width;
                textureDescription.Height = height;
                textureDescription.MipLevels = 1;
                textureDescription.ArraySize = 1;
                textureDescription.Usage = D3D11.ResourceUsage.Default;
                textureDescription.SampleDescription = new DXGI.SampleDescription(1, 0);
                textureDescription.BindFlags = D3D11.BindFlags.DepthStencil;
                textureDescription.CpuAccessFlags = D3D11.CpuAccessFlags.None;
                textureDescription.OptionFlags = D3D11.ResourceOptionFlags.None;
            }

            if (device.CheckFormatSupport(DXGI.Format.D32_Float) != D3D11.FormatSupport.None) { textureDescription.Format = DXGI.Format.D32_Float; }
            else { textureDescription.Format = DXGI.Format.D24_UNorm_S8_UInt; }

            return new D3D11.Texture2D(device, textureDescription);
        }

        /// <summary>
        /// Creates a dynamic vertex buffer for the given vertex type and maximum capacity.
        /// </summary>
        /// <typeparam name="T">Type of the vertices.</typeparam>
        /// <param name="device">Graphics device.</param>
        /// <param name="vertexCount">Maximum count of vertices within the buffer.</param>
        public static Buffer CreateDynamicVertexBuffer<T>(D3D11.Device device, int vertexCount)
            where T : struct
        {
            Type vertexType = typeof(T);
            int vertexSize = Marshal.SizeOf(vertexType);

            D3D11.BufferDescription bufferDescription = new D3D11.BufferDescription();
            bufferDescription.BindFlags = D3D11.BindFlags.VertexBuffer;
            bufferDescription.CpuAccessFlags = D3D11.CpuAccessFlags.Write;
            bufferDescription.OptionFlags = D3D11.ResourceOptionFlags.None;
            bufferDescription.SizeInBytes = vertexCount * vertexSize;
            bufferDescription.Usage = D3D11.ResourceUsage.Dynamic;
            bufferDescription.StructureByteStride = vertexCount * vertexSize;

            return new Buffer(device, bufferDescription);
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
            bufferDescription.StructureByteStride = vertexCount * vertexSize;

            Buffer result = new Buffer(device, outStream, bufferDescription);
            outStream.Dispose();

            return result;
        }

        /// <summary>
        /// Creates an immutable index buffer from the given index array.
        /// </summary>
        /// <param name="device">Graphics device.</param>
        /// <param name="indices">Source index array.</param>
        public static Buffer CreateImmutableIndexBuffer(D3D11.Device device, ushort[] indices)
        {
            DataStream outStreamIndex = new DataStream(indices.Length * Marshal.SizeOf(typeof(ushort)), true, true);

            outStreamIndex.WriteRange(indices);
            outStreamIndex.Position = 0;

            D3D11.BufferDescription bufferDescriptionIndex = new D3D11.BufferDescription();
            bufferDescriptionIndex.BindFlags = D3D11.BindFlags.IndexBuffer;
            bufferDescriptionIndex.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            bufferDescriptionIndex.OptionFlags = D3D11.ResourceOptionFlags.None;
            bufferDescriptionIndex.SizeInBytes = indices.Length * Marshal.SizeOf(typeof(ushort));
            bufferDescriptionIndex.Usage = D3D11.ResourceUsage.Immutable;

            Buffer result = new Buffer(device, outStreamIndex, bufferDescriptionIndex);

            outStreamIndex.Dispose();

            return result;
        }

        ///// <summary>
        ///// Resizes the given bitmap to given size.
        ///// </summary>
        ///// <param name="bitmapToResize">The bitmap to resize.</param>
        ///// <param name="newWidth">Width of the generated bitmap.</param>
        ///// <param name="newHeight">Height of the genrated bitmap.</param>
        //public static Bitmap ResizeGdiBitmap(Bitmap bitmapToResize, int newWidth, int newHeight)
        //{
        //    Bitmap result = new Bitmap(newWidth, newHeight);
        //    using (GDI.Graphics g = GDI.Graphics.FromImage((Image)result))
        //    {
        //        g.DrawImage(bitmapToResize, 0, 0, newWidth, newHeight);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Loads a Direct2D bitmap from the given gdi resource.
        ///// </summary>
        ///// <param name="drawingBitmap">The source gdi bitmap.</param>
        ///// <param name="renderTarget">The RenderTarget object for wich to create the resource.</param>
        //public static D2D.Bitmap LoadBitmap(D2D.RenderTarget renderTarget, Bitmap drawingBitmap)
        //{
        //    D2D.Bitmap result = null;

        //    //Lock the gdi resource
        //    BitmapData drawingBitmapData = drawingBitmap.LockBits(
        //        new GDI.Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height),
        //        ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

        //    //Prepare loading the image from gdi resource
        //    DataStream dataStream = new DataStream(
        //        drawingBitmapData.Scan0,
        //        drawingBitmapData.Stride * drawingBitmapData.Height,
        //        true, false);
        //    D2D.BitmapProperties properties = new D2D.BitmapProperties();
        //    properties.PixelFormat = new D2D.PixelFormat(
        //        DXGI.Format.B8G8R8A8_UNorm,
        //        D2D.AlphaMode.Premultiplied);

        //    //Load the image from the gdi resource
        //    result = new D2D.Bitmap(
        //        renderTarget,
        //        new Size(drawingBitmap.Width, drawingBitmap.Height),
        //        dataStream, drawingBitmapData.Stride,
        //        properties);

        //    //Unlock the gdi resource
        //    drawingBitmap.UnlockBits(drawingBitmapData);

        //    return result;
        //}

        ///// <summary>
        ///// Copies all contents of the given gdi bitmap into the given Direct2D bitmap.
        ///// </summary>
        ///// <param name="targetBitmap">Target Direct2D bitmap.</param>
        ///// <param name="drawingBitmap">The source gdi bitmap.</param>
        //public static void SetBitmapContents(D2D.Bitmap targetBitmap, Bitmap drawingBitmap)
        //{
        //    //Lock the gdi resource
        //    BitmapData drawingBitmapData = drawingBitmap.LockBits(
        //        new GDI.Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height),
        //        ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
        //    targetBitmap.FromMemory(drawingBitmapData.Scan0, drawingBitmapData.Stride);
        //    drawingBitmap.UnlockBits(drawingBitmapData);
        //}

        /// <summary>
        /// Disposes the given object.
        /// </summary>
        public static void SafeDispose<T>(ref T toDispose)
            where T : class, IDisposable
        {
            toDispose = DisposeGraphicsObject(toDispose);
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
