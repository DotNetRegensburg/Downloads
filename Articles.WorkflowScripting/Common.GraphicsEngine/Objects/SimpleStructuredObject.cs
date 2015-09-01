using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Objects.Construction;
using Common.GraphicsEngine.Drawing3D.Resources;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using Buffer = SlimDX.Direct3D11.Buffer;
using DXGI   = SlimDX.DXGI;
using D3D11  = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Objects
{
    public class SimpleStructuredObject : SceneObject
    {
        private VertexStructure m_vertexStructure;
        private Vector3 m_position;
        private Buffer m_vertexBuffer;
        private Buffer m_indexBuffer;
        private string m_texture;
        private string m_displaceTexture;
        private float m_displaceFactor;
        private D3D11.InputLayout m_vertexLayout;
        private EffectResource m_effect;
        private TextureResource m_textureResource;
        private TextureResource m_displaceTextureResource;
        private Camera m_camera;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTexturedCube"/> class.
        /// </summary>
        /// <param name="texture">Name of the texture resource.</param>
        public SimpleStructuredObject(string texture, string displaceTexture, float displaceFactor, VertexStructure vertexStructure, Camera camera)
        {
            m_vertexStructure = vertexStructure;
            m_camera = camera;
            m_texture = texture;
            m_displaceTexture = displaceTexture;
            m_displaceFactor = displaceFactor;
        }

        /// <summary>
        /// Loads all resources of the box.
        /// </summary>
        /// <param name="device">The graphics device</param>
        public override void LoadResources(D3D11.Device device)
        {
            StandardVertex[] vertices = StandardVertex.FromVertexStructure(m_vertexStructure);
            short[] indices = m_vertexStructure.GetIndexArray();

            //Send vertices and indices to the graphics card
            m_vertexBuffer = GraphicsHelper.CreateImmutableVertexBuffer(device, vertices);
            m_indexBuffer = GraphicsHelper.CreateImmutableIndexBuffer(device, indices);

            //Load texture resource
            m_textureResource = base.Resources.GetResourceAndEnsureLoaded<TextureResource>(m_texture);
            m_displaceTextureResource = base.Resources.GetResourceAndEnsureLoaded<TextureResource>(m_displaceTexture);

            //Load effect resource
            m_effect = StandardResources.AddAndLoadResource<EffectResource>(base.Resources, StandardResources.SimpleDisplacedRenderingEffectName);

            //Create the vertex layout for current shader
            m_vertexLayout = new D3D11.InputLayout(device, m_effect.GetInputLayout("Render", "P1"), StandardVertex.InputElements);
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="updateState">Current update state.</param>
        protected override void UpdateInternal(UpdateState updateState)
        {
           
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        protected override void RenderInternal(RenderState renderState)
        {
            D3D11.DeviceContext deviceContext = renderState.DeviceContext;

            //Set device state
            deviceContext.InputAssembler.InputLayout = m_vertexLayout;
            deviceContext.InputAssembler.PrimitiveTopology = D3D11.PrimitiveTopology.TriangleList;
            deviceContext.InputAssembler.SetIndexBuffer(m_indexBuffer, DXGI.Format.R16_UInt, 0);
            deviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(m_vertexBuffer, Marshal.SizeOf(typeof(StandardVertex)), 0));

            //Set shader state
            Matrix4 worldViewProj =
                Matrix4.Translation(m_position) *
                renderState.ViewProj;
            m_effect.SetVariableValue("WorldViewProj", worldViewProj);
            m_effect.SetVariableValue("World", Matrix.Translation(m_position.ToDirectXVector()));
            m_effect.SetVariableValue("ObjectTexture", m_textureResource.TextureView);
            m_effect.SetVariableValue("DisplacementTexture", m_displaceTextureResource.TextureView);
            m_effect.SetVariableValue("DisplaceFactor", m_displaceFactor);
            m_effect.SetVariableValue("Ambient", 0.5f);
            m_effect.SetVariableValue("LightPosition", m_camera.Position);
            m_effect.SetVariableValue("LightPower", 0.8f);
            m_effect.ApplyPass(deviceContext, "Render", "P1");

            //Draw vertices
            deviceContext.DrawIndexed(m_vertexStructure.CountIndexes, 0, 0);
        }

        /// <summary>
        /// Unloads all resources of the object.
        /// </summary>
        public override void UnloadResources()
        {
            if (m_vertexLayout != null) { m_vertexLayout.Dispose(); }
            if (m_indexBuffer != null) { m_indexBuffer.Dispose(); }
            if (m_vertexBuffer != null) { m_vertexBuffer.Dispose(); }

            m_vertexLayout = null;
            m_indexBuffer = null;
            m_vertexBuffer = null;

            m_effect = null;
        }

        /// <summary>
        /// Are resources loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_vertexBuffer != null; }
        }

        /// <summary>
        /// Gets or sets the displacement factor.
        /// </summary>
        public float DisplacementFactor
        {
            get { return m_displaceFactor; }
            set { m_displaceFactor = value; }
        }

        /// <summary>
        /// Gets or sets the object's position.
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }
    }
}
