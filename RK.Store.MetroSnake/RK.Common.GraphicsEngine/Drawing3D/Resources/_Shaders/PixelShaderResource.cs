using System.IO;
using RK.Common.GraphicsEngine.Core;
using RK.Common.Util;
using RK.Common;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class PixelShaderResource : ShaderResource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.PixelShader m_pixelShader;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compiling.</param>
        /// <param name="resourceLink">The resource link.</param>
        public PixelShaderResource(string name, string shaderProfile, AssemblyResourceLink resourceLink)
            : base(name, shaderProfile, resourceLink)
        {

        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        protected override void LoadShader(byte[] shaderBytecode)
        {
            if (m_pixelShader == null)
            {
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

                m_pixelShader = new D3D11.PixelShader(device, shaderBytecode);
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        protected override void UnloadShader()
        {
            m_pixelShader = GraphicsHelper.DisposeGraphicsObject(m_pixelShader);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_pixelShader != null; }
        }

        /// <summary>
        /// Gets the loaded VertexShader object.
        /// </summary>
        public D3D11.PixelShader PixelShader
        {
            get { return m_pixelShader; }
        }
    }
}
