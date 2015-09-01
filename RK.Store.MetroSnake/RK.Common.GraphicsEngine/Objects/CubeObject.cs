using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Core;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
using RK.Common.Util;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;
using D3D = SharpDX.Direct3D;
using DXGI = SharpDX.DXGI;
using System.Reflection;

namespace RK.Common.GraphicsEngine.Objects
{
    [AssemblyResourceFile("RK.Common.GraphicsEngine.Resources.VertexShaders.SimpleDemoRendering.cso", "VertexShader")]
    [AssemblyResourceFile("RK.Common.GraphicsEngine.Resources.PixelShaders.SimpleDemoRendering.cso", "PixelShader")]
    public class CubeObject : SceneSpacialObject
    {
        private D3D11.Buffer m_constantBuffer;
        private D3D11.Buffer m_vertexBuffer;
        private D3D11.InputLayout m_layout;
        private D3D11.VertexBufferBinding m_vertexBufferBinding;
        private D3D11.VertexShader m_vertexShader;
        private D3D11.PixelShader m_pixelShader;

        /// <summary>
        /// Loads all resources of the object.
        /// </summary>
        /// <param name="device">Current DirectX device.</param>
        public override void LoadResources(D3D11.Device device)
        {
            AssemblyResourceReader resourceReader = new AssemblyResourceReader(this.GetType());

            //Create vertex and pixel shaders
            byte[] vertexShaderByteCode = resourceReader.GetBytes("VertexShader");
            m_vertexShader = new D3D11.VertexShader(device, vertexShaderByteCode);
            m_pixelShader = new D3D11.PixelShader(device, resourceReader.GetBytes("PixelShader"));

            //Define layout for vertexshader input signature
            m_layout = new D3D11.InputLayout(device, vertexShaderByteCode, new[]
            {
                new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32B32A32_Float, 0, 0),
                new D3D11.InputElement("COLOR", 0, DXGI.Format.R32G32B32A32_Float, 16, 0)
            });

            //Create vertexbuffer data
            m_vertexBuffer = D3D11.Buffer.Create(device, D3D11.BindFlags.VertexBuffer, new[]
            {
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), // Front
                new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),

                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), // BACK
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),

                new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), // Top
                new Vector4(-1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                new Vector4( 1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),

                new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), // Bottom
                new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                new Vector4( 1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),

                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), // Left
                new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),

                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), // Right
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
            });

            m_vertexBufferBinding = new D3D11.VertexBufferBinding(m_vertexBuffer, SharpDX.Utilities.SizeOf<Vector4>() * 2, 0);

            // Create Constant Buffer
            m_constantBuffer = new D3D11.Buffer(
                device, 
                SharpDX.Utilities.SizeOf<Matrix4>(), 
                D3D11.ResourceUsage.Default, 
                D3D11.BindFlags.ConstantBuffer,
                D3D11.CpuAccessFlags.None, 
                D3D11.ResourceOptionFlags.None, 0);
        }

        /// <summary>
        /// Unloads all resources of the object.
        /// </summary>
        public override void UnloadResources()
        {
            m_constantBuffer = GraphicsHelper.DisposeGraphicsObject(m_constantBuffer);
            m_vertexBuffer = GraphicsHelper.DisposeGraphicsObject(m_vertexBuffer);
            m_layout = GraphicsHelper.DisposeGraphicsObject(m_layout);
            m_pixelShader = GraphicsHelper.DisposeGraphicsObject(m_pixelShader);
            m_vertexShader = GraphicsHelper.DisposeGraphicsObject(m_vertexShader);
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void RenderInternal(RenderState renderState)
        {
            D3D11.DeviceContext deviceContext = renderState.DeviceContext;

            //Setup rendering pipeline
            deviceContext.InputAssembler.SetVertexBuffers(0, m_vertexBufferBinding);
            deviceContext.InputAssembler.InputLayout = m_layout;
            deviceContext.InputAssembler.PrimitiveTopology = D3D.PrimitiveTopology.TriangleList;
            deviceContext.VertexShader.SetConstantBuffer(0, m_constantBuffer);
            deviceContext.VertexShader.Set(m_vertexShader);
            deviceContext.PixelShader.Set(m_pixelShader);

            //Update local world-view-projection matrix
            Matrix4 worldViewProj = renderState.GenerateWorldViewProj(this.Transform);
            worldViewProj.Transpose();
           
            //Update shader constant
            deviceContext.UpdateSubresource(ref worldViewProj, m_constantBuffer, 0); 

            //Draws the object finally
            deviceContext.Draw(36, 0);
        }

        /// <summary>
        /// Are resources loaded?
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool IsLoaded
        {
            get { return m_vertexBuffer != null; }
        }
    }
}
