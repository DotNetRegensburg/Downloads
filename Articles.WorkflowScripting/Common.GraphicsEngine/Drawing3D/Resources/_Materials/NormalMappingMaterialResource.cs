using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class NormalMappingMaterialResource : MaterialResource
    {
        //Standard members
        private string m_diffuseTexture;
        private string m_normalMapTexture;
        private bool m_isLoaded;

        //Resource members
        private TextureResource m_diffuseTextureResource;
        private TextureResource m_normalMapTextureResource;
        private EffectResource m_effectResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplacementMaterialResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public NormalMappingMaterialResource(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplacementMaterialResource"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="diffuseTexture">The diffuse texture.</param>
        /// <param name="displacementTexture">The displacement texture.</param>
        /// <param name="displacementFactor">The displacement factor.</param>
        public NormalMappingMaterialResource(string name, string diffuseTexture, string normalMapTexture)
            : this(name)
        {
            m_diffuseTexture = diffuseTexture;
            m_normalMapTexture = normalMapTexture;
        }

        /// <summary>
        /// Applies the material to the given render state.
        /// </summary>
        /// <param name="renderState">Current render state</param>
        public override void Apply(RenderState renderState)
        {
            D3D11.DeviceContext deviceContext = renderState.DeviceContext;

            //Get transformation matrices
            Matrix4 world = renderState.World.Top;
            Matrix4 viewProj = renderState.ViewProj;
            Matrix4 worldViewProj = world * viewProj;

            //Apply effect values
            m_effectResource.SetVariableValue("WorldViewProj", worldViewProj);
            m_effectResource.SetVariableValue("World", world);
            m_effectResource.SetVariableValue("ObjectTexture", m_diffuseTextureResource.TextureView);
            m_effectResource.SetVariableValue("NormalMapTexture", m_normalMapTextureResource.TextureView);
            m_effectResource.SetVariableValue("LightPosition", renderState.Camera.Position);
            m_effectResource.SetVariableValue("LightPower", 1f);
            m_effectResource.SetVariableValue("StrongLightFactor", 1.5f);
            m_effectResource.SetVariableValue("Ambient", 1f);
            m_effectResource.ApplyPass(deviceContext, "Render", "P1");
        }

        /// <summary>
        /// Generates the requested input layout.
        /// </summary>
        /// <param name="inputElements">An array of InputElements describing vertex input structure.</param>
        public override D3D11.InputLayout GenerateInputLayout(D3D11.InputElement[] inputElements)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            return new D3D11.InputLayout(device, m_effectResource.GetInputLayout("Render", "P1"), inputElements);
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            //Get texture resources
            m_diffuseTextureResource = resources.GetResourceAndEnsureLoaded<TextureResource>(m_diffuseTexture);
            m_normalMapTextureResource = resources.GetResourceAndEnsureLoaded<TextureResource>(m_normalMapTexture);

            //Get effect resource
            m_effectResource = StandardResources.AddAndLoadResource<EffectResource>(resources, StandardResources.SimpleNormalMappedRenderingEffectName);

            //Update loaded flag
            m_isLoaded = true;
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            try
            {
                m_diffuseTextureResource = null;
                m_normalMapTextureResource = null;
                m_effectResource = null;
            }
            finally
            {
                m_isLoaded = false;
            }
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_isLoaded; }
        }

        /// <summary>
        /// Gets or sets the diffuse texture.
        /// </summary>
        public string DiffuseTexture
        {
            get { return m_diffuseTexture; }
            set
            {
                if (m_diffuseTexture != value)
                {
                    m_diffuseTexture = value;
                    base.ReloadResource();
                }
            }
        }

        /// <summary>
        /// Gets or sets the normal map texture.
        /// </summary>
        public string NormalMapTexture
        {
            get { return m_normalMapTexture; }
            set
            {
                if (m_normalMapTexture != value)
                {
                    m_normalMapTexture = value;
                    base.ReloadResource();
                }
            }
        }
    }
}
