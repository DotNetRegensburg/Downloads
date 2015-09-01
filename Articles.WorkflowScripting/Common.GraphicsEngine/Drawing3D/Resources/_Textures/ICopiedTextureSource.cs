using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public interface ICopiedTextureSource
    {
        /// <summary>
        /// Occurs when texture contents have changed.
        /// </summary>
        event TextureChangedHandler TextureChanged;

        /// <summary>
        /// Gets the texture.
        /// </summary>
        D3D11.Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        int TextureWidth
        {
            get;
        }

        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        int TextureHeight
        {
            get;
        }
    }
}
