using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class SimpleColoredMaterialResource : MaterialResource
    {
        //Standard members
        private string m_texture;

        //Resource members
        private TextureResource m_textureResource;
        private EffectResource m_effectResource;
     
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleColoredMaterialResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public SimpleColoredMaterialResource(string name)
            : this(name, string.Empty)
        {
       
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleColoredMaterialResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="diffuseColor">Diffuse color component.</param>
        /// <param name="texture">Diffuse texture.</param>
        public SimpleColoredMaterialResource(string name, string texture)
            : base(name)
        {
            m_texture = texture;
        }

        /// <summary>
        /// Generates the requested input layout.
        /// </summary>
        /// <param name="inputElements">An array of InputElements describing vertex input structure.</param>
        public override D3D11.InputLayout GenerateInputLayout(D3D11.InputElement[] inputElements)
        {
            if (!string.IsNullOrEmpty(m_texture))
            {
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

                return new D3D11.InputLayout(device, m_effectResource.GetInputLayout("Render", "P1"), inputElements);
            }
            else
            {
                D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

                return new D3D11.InputLayout(device, m_effectResource.GetInputLayout("Render", "P0"), inputElements);
            }
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            if (!string.IsNullOrEmpty(m_texture))
            {
                //Load texture resource
                m_textureResource = resources.GetResourceAndEnsureLoaded<TextureResource>(m_texture);
            }

            //Load effect resource
            m_effectResource = StandardResources.AddAndLoadResource<EffectResource>(resources, StandardResources.SimpleRenderingEffectName);
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_textureResource = null;
            m_effectResource = null;
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

            if (m_textureResource != null)
            {
                m_effectResource.SetVariableValue("WorldViewProj", worldViewProj);
                m_effectResource.SetVariableValue("World", world);
                m_effectResource.SetVariableValue("ObjectTexture", m_textureResource.TextureView);
                m_effectResource.SetVariableValue("LightPosition", renderState.Camera.Position);
                m_effectResource.SetVariableValue("Ambient", 0f);
                m_effectResource.SetVariableValue("LightPower", 0.8f);
                m_effectResource.SetVariableValue("StrongLightFactor", 1.6f);
                m_effectResource.ApplyPass(deviceContext, "Render", "P1");
            }
            else
            {
                m_effectResource.SetVariableValue("WorldViewProj", worldViewProj);
                m_effectResource.SetVariableValue("World", world);
                m_effectResource.SetVariableValue("Ambient", 0f);
                m_effectResource.SetVariableValue("LightPosition", renderState.Camera.Position);
                m_effectResource.SetVariableValue("LightPower", 0.8f);
                m_effectResource.SetVariableValue("StrongLightFactor", 1.6f);
                m_effectResource.ApplyPass(deviceContext, "Render", "P0");
            }
        }

        /// <summary>
        /// Gets or sets the texture for rendering.
        /// </summary>
        public string Texture
        {
            get { return m_texture; }
            set
            {
                if (m_texture != value)
                {
                    m_texture = value;
                    base.ReloadResource();
                }
            }
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_effectResource != null; }
        }
    }
}
