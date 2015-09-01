using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.D3DCompiler;
using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Drawing3D.Resources;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using Buffer = SlimDX.Direct3D11.Buffer;
using DXGI   = SlimDX.DXGI;
using D3D11  = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Objects
{
    public class SimpleCube : SceneSpacialObject
    {
        private Buffer m_vertexBuffer;
        private Buffer m_indexBuffer;
        private D3D11.InputLayout m_vertexLayout;
        private EffectResource m_effect;

        /// <summary>
        /// Loads all resources of the box.
        /// </summary>
        /// <param name="device">The graphics device</param>
        public override void LoadResources(D3D11.Device device)
        {
            //Create all vetices
            StandardVertex[] vertices = new StandardVertex[]
            {
                new StandardVertex(new Vector3(-1, -1, -1), Color4.Red.ToArgb()),
                new StandardVertex(new Vector3(1, -1, -1), Color4.LightBlue.ToArgb()),
                new StandardVertex(new Vector3(1, -1, 1), Color4.LightCyan.ToArgb()),
                new StandardVertex(new Vector3(-1, -1, 1), Color4.CadetBlue.ToArgb()),
                new StandardVertex(new Vector3(-1, 1, -1), Color4.Red.ToArgb()),
                new StandardVertex(new Vector3(1,1,-1), Color4.Orange.ToArgb()),
                new StandardVertex(new Vector3(1,1,1), Color4.Goldenrod.ToArgb()),
                new StandardVertex(new Vector3(-1, 1,1), Color4.Yellow.ToArgb())
            };

            //Create all indices
            short[] indices = new short[]
            {
                4,1,0,4,5,1,
                5,2,1,5,6,2,
                6,3,2,6,7,3,
                7,0,3,7,4,0,
                7,5,4,7,6,5,
                2,3,0,2,0,1
            };

            //Send vertices and indices to the graphics card
            m_vertexBuffer = GraphicsHelper.CreateImmutableVertexBuffer(device, vertices);
            m_indexBuffer = GraphicsHelper.CreateImmutableIndexBuffer(device, indices);

            //Load effect resource
            m_effect = StandardResources.AddAndLoadResource<EffectResource>(base.Resources, StandardResources.SimpleRenderingEffectName);

            //Create the vertex layout for current shader
            m_vertexLayout = new D3D11.InputLayout(device, m_effect.GetInputLayout("Render", "P0"), StandardVertex.InputElements);
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

            //Matrix4 test = Matrix4.RotationYawPitchRoll(0f, 0f, 0f);

            //Set shader state
            Matrix4 worldViewProj =
                m_transform *
                renderState.ViewProj;
            m_effect.SetVariableValue("WorldViewProj", worldViewProj);
            m_effect.SetVariableValue("Ambient", 1f);
            m_effect.SetVariableValue("StrongLightFactor", 1f);
            m_effect.ApplyPass(deviceContext, "Render", "P0");

            //Draw vertices
            deviceContext.DrawIndexed(36, 0, 0);
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
    }
}
