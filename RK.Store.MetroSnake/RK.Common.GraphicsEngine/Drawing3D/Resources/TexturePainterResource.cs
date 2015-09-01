using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RK.Common.GraphicsEngine.Core;
using RK.Common.Util;

//Some namespace mappings
using DXGI  = SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;
using D3D = SharpDX.Direct3D;
using System.Diagnostics;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class TexturePainterResource : Resource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.SamplerState m_samplerState;
        private D3D11.InputLayout m_vertexLayout;
        private D3D11.Buffer m_vertexBuffer;
        private D3D11.Buffer m_indexBuffer;
        private D3D11.VertexBufferBinding m_vertexBufferBinding;

        //Loaded distributed resources
        private VertexShaderResource m_vertexShader;
        private PixelShaderResource m_pixelShader;
        private TypeSafeConstantBufferResource<ConstantBufferData> m_constantBuffer;

        //Some generic members
        private bool m_isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="TexturePainterResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public TexturePainterResource(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            if (!m_isLoaded)
            {
                D3D11.Device targetDevice = GraphicsCore.Current.HandlerD3D11.Device;

                //Build vertex array
                StandardVertex[] vertices = new StandardVertex[]
                {
                    new StandardVertex(new Vector3(-1f, -1f, 0f), new Vector2(0f, 1f)),
                    new StandardVertex(new Vector3(1f, -1f, 0f), new Vector2(1f, 1f)),
                    new StandardVertex(new Vector3(1f, 1f, 0f), new Vector2(1f, 0f)),
                    new StandardVertex(new Vector3(-1f, 1f, 0f), new Vector2(0f, 0f))
                };

                //Build index array
                ushort[] indices = new ushort[]
                {
                    2, 1, 0,
                    0, 3, 2
                };

                //Create VertexBuffer and IndexBuffer
                m_vertexBuffer = GraphicsHelper.CreateImmutableVertexBuffer(targetDevice, vertices);
                m_indexBuffer = GraphicsHelper.CreateImmutableIndexBuffer(targetDevice, indices);

                //Create a VertexBufferBinding for later rendering
                m_vertexBufferBinding = new D3D11.VertexBufferBinding(m_vertexBuffer, StandardVertex.Size, 0);

                //Load the shader
                m_constantBuffer = resources.GetResourceAndEnsureLoaded(
                    "TexturePainterConstants",
                    () => new TypeSafeConstantBufferResource<ConstantBufferData>("TexturePainterConstants"));
                m_vertexShader = resources.GetResourceAndEnsureLoaded(
                    "TexturePainterVertexShader",
                    () => new VertexShaderResource(
                        "TexturePainterVertexShader",
                        "vs_4_0_level_9_1",
                        new AssemblyResourceLink(
                            this.GetType().GetTypeInfo().Assembly,
                            "RK.Common.GraphicsEngine.Resources.VertexShaders.SimpleTransformedTexturedRendering.cso")));
                m_pixelShader = resources.GetResourceAndEnsureLoaded(
                    "TexturePainterPixelShader",
                    () => new PixelShaderResource(
                        "TexturePainterPixelShader",
                        "ps_4_0_level_9_1",
                        new AssemblyResourceLink(
                            this.GetType().GetTypeInfo().Assembly,
                            "RK.Common.GraphicsEngine.Resources.PixelShaders.SimpleTransformedTexturedRendering.cso")));

                //Create vertex layout based on target shader
                m_vertexLayout = new D3D11.InputLayout(targetDevice, m_vertexShader.ShaderBytecode, StandardVertex.InputElements);

                //Create the sampler state
                m_samplerState = new D3D11.SamplerState(targetDevice, new D3D11.SamplerStateDescription()
                {
                    Filter = D3D11.Filter.MinMagMipLinear,
                    AddressU = D3D11.TextureAddressMode.Wrap,
                    AddressV = D3D11.TextureAddressMode.Wrap,
                    AddressW = D3D11.TextureAddressMode.Wrap,
                    BorderColor = SharpDX.Color.Black,
                    ComparisonFunction = D3D11.Comparison.Never,
                    MaximumAnisotropy = 16,
                    MipLodBias = 0,
                    MinimumLod = -float.MaxValue,
                    MaximumLod = float.MaxValue
                });

                m_isLoaded = true;
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            if (m_isLoaded)
            {
                m_pixelShader = null;
                m_vertexShader = null;
                m_constantBuffer = null;

                m_samplerState = GraphicsHelper.DisposeGraphicsObject(m_samplerState);
                m_vertexLayout = GraphicsHelper.DisposeGraphicsObject(m_vertexLayout);

                m_vertexLayout = GraphicsHelper.DisposeGraphicsObject(m_vertexLayout);
                m_indexBuffer = GraphicsHelper.DisposeGraphicsObject(m_indexBuffer);
                m_vertexBuffer = GraphicsHelper.DisposeGraphicsObject(m_vertexBuffer);

                m_isLoaded = false;
            }
        }

        /// <summary>
        /// Draws the rectangle using the given RenderState object.
        /// </summary>
        /// <param name="renderState">RenderState for drawing.</param>
        public void Draw(RenderState renderState, D3D11.ShaderResourceView textureView)
        {
            this.Draw(renderState, textureView, 0.5f);
        }

        /// <summary>
        /// Draws the rectangle using the given RenderState object.
        /// </summary>
        /// <param name="renderState">RenderState for drawing.</param>
        public void Draw(RenderState renderState, D3D11.ShaderResourceView textureView, float scaling)
        {
            //Bind elements to context
            D3D11.DeviceContext deviceContext = renderState.DeviceContext;
            deviceContext.InputAssembler.InputLayout = m_vertexLayout;
            deviceContext.InputAssembler.PrimitiveTopology = D3D.PrimitiveTopology.TriangleList;
            deviceContext.InputAssembler.SetVertexBuffers(0, m_vertexBufferBinding);
            deviceContext.InputAssembler.SetIndexBuffer(m_indexBuffer, DXGI.Format.R16_UInt, 0);

            //Update constant buffer
            m_constantBuffer.SetData(deviceContext, new ConstantBufferData()
            {
                Scaling = scaling
            });

            //Setup rendering pipeline (depending on texture enabled or not)
            deviceContext.VertexShader.SetConstantBuffer(0, m_constantBuffer.ConstantBuffer);
            deviceContext.VertexShader.Set(m_vertexShader.VertexShader);
            deviceContext.PixelShader.SetConstantBuffer(0, m_constantBuffer.ConstantBuffer);
            deviceContext.PixelShader.SetShaderResource(0, textureView);
            deviceContext.PixelShader.SetSampler(0, m_samplerState);
            deviceContext.PixelShader.Set(m_pixelShader.PixelShader);

            //Execute draw call
            deviceContext.DrawIndexed(6, 0, 0);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_isLoaded; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        [StructLayout(LayoutKind.Sequential)]
        private struct ConstantBufferData
        {
            public float Scaling;
            public float Dummy1;
            public float Dummy2;
            public float Dummy3;
        }
    }
}
