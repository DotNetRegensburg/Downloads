using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.D3DCompiler;
using Common.Util;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class VertexShaderResource : ShaderResource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.VertexShader m_vertexShader;
        private ShaderSignature m_inputSignature;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compiling.</param>
        /// <param name="filePath">Path to shader file.</param>
        public VertexShaderResource(string name, string shaderProfile, string filePath)
            : base(name, shaderProfile, filePath)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compiling.</param>
        /// <param name="resourceLink">The resource link.</param>
        public VertexShaderResource(string name, string shaderProfile, AssemblyResourceLink resourceLink)
            : base(name, shaderProfile, resourceLink)
        {

        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        protected override void LoadShader(ShaderBytecode shaderBytecode)
        {
            if (m_vertexShader == null)
            {
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

                m_vertexShader = new D3D11.VertexShader(device, shaderBytecode);
                m_inputSignature = ShaderSignature.GetInputSignature(shaderBytecode);
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        protected override void UnloadShader()
        {
            m_inputSignature = GraphicsHelper.DisposeGraphicsObject(m_inputSignature);
            m_vertexShader = GraphicsHelper.DisposeGraphicsObject(m_vertexShader);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_vertexShader != null; }
        }

        /// <summary>
        /// Gets the loaded VertexShader object.
        /// </summary>
        public D3D11.VertexShader VertexShader
        {
            get { return m_vertexShader; }
        }

        /// <summary>
        /// Gets the input signature of the shader.
        /// </summary>
        public ShaderSignature InputSignature
        {
            get { return m_inputSignature; }
        }
    }
}
