using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Core;
using SlimDX;

//Some namespace mappings
using DXGI  = SlimDX.DXGI;
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class TexturePainterResource : Resource
    {
        //Resources for Direct3D 11 rendering
        private EffectResource m_effect;
        private D3D11.InputLayout m_vertexLayout;
        private D3D11.Buffer m_vertexBuffer;
        private D3D11.Buffer m_indexBuffer;
        private D3D11.VertexBufferBinding m_vertexBufferBinding;

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
                short[] indices = new short[]
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
                m_effect = StandardResources.AddAndLoadResource<EffectResource>(resources, StandardResources.SimpleTransformedTexturedRenderingName);
                m_vertexLayout = new D3D11.InputLayout(targetDevice, m_effect.GetInputLayout("Render", "P0"), StandardVertex.InputElements);

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
            deviceContext.InputAssembler.PrimitiveTopology = D3D11.PrimitiveTopology.TriangleList;
            deviceContext.InputAssembler.SetVertexBuffers(0, m_vertexBufferBinding);
            deviceContext.InputAssembler.SetIndexBuffer(m_indexBuffer, DXGI.Format.R16_UInt, 0);

            //Set shader resources
            m_effect.SetVariableValue("ObjectTexture", textureView);
            m_effect.SetVariableValue("Scaling", scaling);
            m_effect.ApplyPass(deviceContext, "Render", "P0");

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
    }
}
