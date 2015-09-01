using RK.Common.GraphicsEngine.Core;
using SharpDX;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
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
