using System;
using System.Reflection;
using System.Runtime.InteropServices;
using RK.Common.GraphicsEngine.Core;
using RK.Common.Util;
using SharpDX;
using SharpDX.Direct3D;
//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class SimpleColoredDisplacedMaterialResource : MaterialResource
    {
        //Some configuration
        private string m_textureName;
        private string m_displaceTextureName;
        private float m_ambient;
        private float m_lightPower;
        private float m_displacementFactor;

        //Resource members
        private D3D11.SamplerState m_samplerState;
        private TextureResource m_textureResource;
        private TextureResource m_displaceTextureResource;
        private VertexShaderResource m_vertexShader;
        private PixelShaderResource m_pixelShader;
        private PixelShaderResource m_pixelShaderTextured;
        private TypeSafeConstantBufferResource<ConstantBufferData> m_constantBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleColoredMaterialResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <param name="textureName">The name of the texture to be rendered.</param>
        public SimpleColoredDisplacedMaterialResource(string name, string displaceTexture, string textureName = "")
            : base(name)
        {
            m_textureName = textureName;
            m_displaceTextureName = displaceTexture;

            m_ambient = 0f;
            m_lightPower = 0.8f;
        }

        /// <summary>
        /// Generates the requested input layout.
        /// </summary>
        /// <param name="inputElements">An array of InputElements describing vertex input structure.</param>
        public override D3D11.InputLayout GenerateInputLayout(D3D11.InputElement[] inputElements)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;
            return new D3D11.InputLayout(device, m_vertexShader.ShaderBytecode, StandardVertex.InputElements);
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            //Load the vertex shader (displacement effect only supported when shader model at least 4.0)
            if (device.FeatureLevel >= FeatureLevel.Level_10_0)
            {
                m_vertexShader = resources.GetResourceAndEnsureLoaded(
                    "SimpleRenderingVS",
                    () => new VertexShaderResource(
                        "SimpleRenderingVS",
                        "vs_4_0",
                        new AssemblyResourceLink(
                            this.GetType().GetTypeInfo().Assembly,
                            "RK.Common.GraphicsEngine.Resources.VertexShaders.SimpleDisplacedRendering.cso")));
            }
            else
            {
                //Fallback => Use standard textured/colored rendering without displacement
                m_vertexShader = resources.GetResourceAndEnsureLoaded(
                    "SimpleRenderingVS",
                    () => new VertexShaderResource(
                        "SimpleRenderingVS",
                        "vs_4_0",
                        new AssemblyResourceLink(
                            this.GetType().GetTypeInfo().Assembly,
                            "RK.Common.GraphicsEngine.Resources.VertexShaders.SimpleRendering.cso")));
            }

            //Load pixel shader resources
            m_pixelShader = resources.GetResourceAndEnsureLoaded(
                "SimpleRenderingPS",
                () => new PixelShaderResource(
                    "SimpleRenderingPS",
                    "ps_4_0_level_9_1",
                    new AssemblyResourceLink(
                        this.GetType().GetTypeInfo().Assembly,
                        "RK.Common.GraphicsEngine.Resources.PixelShaders.SimpleRendering.cso")));
            m_pixelShaderTextured = resources.GetResourceAndEnsureLoaded(
                "SimpleRenderingPSTextured",
                () => new PixelShaderResource(
                    "SimpleRenderingPSTextured",
                    "ps_4_0_level_9_1",
                    new AssemblyResourceLink(
                        this.GetType().GetTypeInfo().Assembly,
                        "RK.Common.GraphicsEngine.Resources.PixelShaders.SimpleRenderingTextured.cso")));

            //Load constant buffer
            m_constantBuffer = resources.GetResourceAndEnsureLoaded(
                "SimpleRenderingConstants",
                () => new TypeSafeConstantBufferResource<ConstantBufferData>("SimpleRenderingConstants"));

            //Load the displacement texture
            if (device.FeatureLevel >= FeatureLevel.Level_10_0)
            {
                m_displaceTextureResource = resources.GetResourceAndEnsureLoaded<TextureResource>(m_displaceTextureName);
            }

            //Create the sampler state
            m_samplerState = new D3D11.SamplerState(device, new D3D11.SamplerStateDescription()
            {
                Filter = D3D11.Filter.MinMagMipLinear,
                AddressU = D3D11.TextureAddressMode.Wrap,
                AddressV = D3D11.TextureAddressMode.Wrap,
                AddressW = D3D11.TextureAddressMode.Wrap,
                BorderColor = Color.Black,
                ComparisonFunction = D3D11.Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = -float.MaxValue,
                MaximumLod = float.MaxValue
            });

            //Load the texture if any configured.
            if (!string.IsNullOrEmpty(m_textureName))
            {
                //Get texture resource
                m_textureResource = resources.GetResourceAndEnsureLoaded<TextureResource>(m_textureName);
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_samplerState = GraphicsHelper.DisposeGraphicsObject(m_samplerState);

            m_vertexShader = null;
            m_pixelShader = null;
            m_pixelShaderTextured = null;
            m_constantBuffer = null;
            m_textureResource = null;
            m_displaceTextureResource = null;
        }

        /// <summary>
        /// Applies the material to the given render state.
        /// </summary>
        /// <param name="renderState">Current render state</param>
        public override void Apply(RenderState renderState, MaterialApplyMode applyMode)
        {
            D3D11.DeviceContext deviceContext = renderState.DeviceContext;

            //Get transformation matrices
            Matrix4 world = renderState.World.Top;
            Matrix4 viewProj = renderState.ViewProj;
            Matrix4 worldViewProj = world * viewProj;
            world.Transpose();
            worldViewProj.Transpose();

            //Update constant buffer
            m_constantBuffer.SetData(deviceContext, new ConstantBufferData()
            {
                WorldViewProj = worldViewProj,
                World = world,
                Ambient = m_ambient,
                LightPosition = renderState.Camera.Position,
                LightPower = m_lightPower,
                StrongLightFactor = 1.6f,
                DisplaceFactor = m_displacementFactor
            });

            if (applyMode == MaterialApplyMode.Full)
            {
                //Setup rendering pipeline (depending on texture enabled or not)
                if (m_textureResource != null)
                {
                    deviceContext.VertexShader.SetConstantBuffer(0, m_constantBuffer.ConstantBuffer);
                    deviceContext.VertexShader.Set(m_vertexShader.VertexShader);
                    if (renderState.Device.FeatureLevel >= FeatureLevel.Level_10_0)
                    {
                        deviceContext.VertexShader.SetShaderResource(0, m_displaceTextureResource.TextureView);
                        deviceContext.VertexShader.SetSampler(0, m_samplerState);
                    }

                    deviceContext.PixelShader.SetConstantBuffer(0, m_constantBuffer.ConstantBuffer);
                    deviceContext.PixelShader.SetShaderResource(0, m_textureResource.TextureView);
                    deviceContext.PixelShader.SetSampler(0, m_samplerState);
                    deviceContext.PixelShader.Set(m_pixelShaderTextured.PixelShader);
                }
                else
                {
                    deviceContext.VertexShader.SetConstantBuffer(0, m_constantBuffer.ConstantBuffer);
                    deviceContext.VertexShader.Set(m_vertexShader.VertexShader);
                    if (renderState.Device.FeatureLevel >= FeatureLevel.Level_10_0)
                    {
                        deviceContext.VertexShader.SetShaderResource(0, m_displaceTextureResource.TextureView);
                        deviceContext.VertexShader.SetSampler(0, m_samplerState);
                    }

                    deviceContext.PixelShader.SetConstantBuffer(0, m_constantBuffer.ConstantBuffer);
                    deviceContext.PixelShader.Set(m_pixelShader.PixelShader);
                }
            }
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_vertexShader != null; }
        }

        /// <summary>
        /// Gets or sets the ambient lightning factor.
        /// </summary>
        public float Ambient
        {
            get { return m_ambient; }
            set
            {
                if (value < 0f) { m_ambient = 0f; }
                else if (value > 1f) { m_ambient = 1f; }
                else { m_ambient = value; }
            }
        }

        /// <summary>
        /// Gets or sets the current displacement factor.
        /// </summary>
        public float DisplaceFactor
        {
            get { return m_displacementFactor; }
            set { m_displacementFactor = value; }
        }

        /// <summary>
        /// Gets or sets current light power value.
        /// </summary>
        public float LightPower
        {
            get { return m_lightPower; }
            set
            {
                if (value < 0f) { m_lightPower = 0f; }
                else if (value > 1f) { m_lightPower = 1f; }
                else { m_lightPower = value; }
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        [StructLayout(LayoutKind.Sequential)]
        private struct ConstantBufferData
        {
            public Matrix4 WorldViewProj;
            public Matrix4 World;
            public Vector3 LightPosition;
            public float LightPower;
            public float StrongLightFactor;
            public float Ambient;
            public float DisplaceFactor;
            public float Dummy2;
        }
    }
}
