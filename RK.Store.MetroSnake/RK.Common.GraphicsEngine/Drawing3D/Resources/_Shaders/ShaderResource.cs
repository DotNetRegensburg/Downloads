using System;
using System.IO;
using RK.Common.GraphicsEngine.Core;
using RK.Common.Util;

//Some namespace mappings

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public abstract class ShaderResource : Resource
    {
        //Generic members
        private string m_shaderProfile;
        private byte[] m_shaderBytecode;

        //private string m_filePath;
        private AssemblyResourceLink m_resourceLink;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ShaderResource"/> class.
        ///// </summary>
        ///// <param name="name">The name of the resource.</param>
        ///// <param name="shaderProfile">Shader profile used for compilation.</param>
        ///// <param name="filePath">Path to shader file.</param>
        //protected ShaderResource(string name, string shaderProfile, string filePath)
        //    : base(name)
        //{
        //    m_precompiled = false;
        //    m_shaderProfile = shaderProfile;
        //    m_filePath = filePath;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compilation.</param>
        /// <param name="resourceLink">Resource link for embedded resource.</param>
        protected ShaderResource(string name, string shaderProfile, AssemblyResourceLink resourceLink)
            : base(name)
        {
            m_shaderProfile = shaderProfile;
            m_resourceLink = resourceLink;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            //Load the shader itself
            if (m_shaderBytecode == null)
            {
                using (Stream inStream = m_resourceLink.OpenRead())
                {
                    m_shaderBytecode = inStream.ReadAllBytes();
                }
            }

            LoadShader(m_shaderBytecode);
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            UnloadShader();
        }

        /// <summary>
        /// Loads a shader using the given bytecode.
        /// </summary>
        /// <param name="inStream">A reading stream to the shader's bytecode.</param>
        protected abstract void LoadShader(byte[] shaderBytecode);

        /// <summary>
        /// Unloads the shader.
        /// </summary>
        protected abstract void UnloadShader();

        /// <summary>
        /// Gets the shader's raw bytecode.
        /// </summary>
        public byte[] ShaderBytecode
        {
            get { return m_shaderBytecode; }
        }
    }
}
