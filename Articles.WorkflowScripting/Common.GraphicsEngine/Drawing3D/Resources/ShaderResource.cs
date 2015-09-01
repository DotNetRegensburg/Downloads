using System;
using System.IO;
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
    public abstract class ShaderResource : Resource
    {
        //Resources for Direct3D 11 rendering
        private ShaderBytecode m_shaderBytecode;

        //Generic members
        private bool m_precompiled;
        private string m_shaderProfile;
        private string m_filePath;
        private AssemblyResourceLink m_resourceLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compilation.</param>
        /// <param name="filePath">Path to shader file.</param>
        protected ShaderResource(string name, string shaderProfile, string filePath)
            : base(name)
        {
            m_precompiled = false;
            m_shaderProfile = shaderProfile;
            m_filePath = filePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="shaderProfile">Shader profile used for compilation.</param>
        /// <param name="resourceLink">Resource link for embedded resource.</param>
        protected ShaderResource(string name, string shaderProfile, AssemblyResourceLink resourceLink)
            : base(name)
        {
            m_precompiled = false;
            m_shaderProfile = shaderProfile;
            m_resourceLink = resourceLink;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            //Load shader bytecode
            if (!m_precompiled)
            {
                string sourceCode = string.Empty;

                if (!string.IsNullOrEmpty(m_filePath)) { sourceCode = File.ReadAllText(m_filePath); }
                else if (m_resourceLink != null) { sourceCode = m_resourceLink.GetText(); }
                else { throw new ArgumentException("Unable to load shader resource: no source defined!"); }

                string compilationErrors = string.Empty;
                m_shaderBytecode = ShaderBytecode.Compile(sourceCode, m_shaderProfile, ShaderFlags.None, EffectFlags.None, null, null, out compilationErrors);
                if (!string.IsNullOrEmpty(compilationErrors))
                {
                    int blub = 0;
                }
            }
            else
            {
                throw new NotImplementedException("Loading of precompiled shaders is not supported at the moment!");
            }

            //Load the shader itself
            LoadShader(m_shaderBytecode);
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            UnloadShader();
            m_shaderBytecode = GraphicsHelper.DisposeGraphicsObject(m_shaderBytecode);
        }

        /// <summary>
        /// Loads a shader using the given bytecode.
        /// </summary>
        /// <param name="shaderBytecode">Bytecode of the shader.</param>
        protected virtual void LoadShader(ShaderBytecode shaderBytecode)
        {

        }

        /// <summary>
        /// Unloads the shader.
        /// </summary>
        protected virtual void UnloadShader()
        {
            
        }

        /// <summary>
        /// Gets the shader in raw format.
        /// </summary>
        public ShaderBytecode Bytecode
        {
            get { return m_shaderBytecode; }
        }
    }
}
