using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;
using DXGI  = SlimDX.DXGI;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public abstract class TextureResource : Resource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        protected TextureResource(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Generates a bitmap that contains all data from the texture.
        /// </summary>
        public Bitmap ToDrawingBitmap()
        {
            if (IsLoaded)
            {
                //Generate result bitmap
                Bitmap drawingBitmap = new Bitmap(Texture.Description.Width, Texture.Description.Height);

                //Fill result bitmap
                DXGI.Surface surface = this.Texture.AsSurface();
                try
                {
                    DataRectangle dataRectangle = surface.Map(DXGI.MapFlags.Read);

                    BitmapData rawBitmap = drawingBitmap.LockBits(
                        new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppPArgb);
                    try
                    {

                    }
                    finally
                    {
                        drawingBitmap.UnlockBits(rawBitmap);
                    }
                }
                finally
                {
                    GraphicsHelper.DisposeGraphicsObject(surface);
                }

                return drawingBitmap;
            }
            return null;
        }

        /// <summary>
        /// Gets the texture object.
        /// </summary>
        public abstract D3D11.Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Gets a ShaderResourceView targeting the texture.
        /// </summary>
        public abstract D3D11.ShaderResourceView TextureView
        {
            get;
        }
    }
}
