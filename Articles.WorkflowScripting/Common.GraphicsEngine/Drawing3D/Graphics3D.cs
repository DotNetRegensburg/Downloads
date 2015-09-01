using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Drawing3D.Resources;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Drawing3D
{
    public class Graphics3D
    {
        private ResourceDictionary m_resourceDictionary;
        private RenderState m_renderState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graphics3D"/> class.
        /// </summary>
        /// <param name="renderState">Current state of the renderer.</param>
        public Graphics3D(RenderState renderState)
        {
            m_resourceDictionary = new ResourceDictionary();
            m_renderState = renderState;
        }

        /// <summary>
        /// Draws the given line.
        /// </summary>
        /// <param name="start">Start-Point of the line.</param>
        /// <param name="destination">Destination-Point of the line.</param>
        /// <param name="lineColor">Color of the line.</param>
        public void DrawLine(Vector3 start, Vector3 destination, Color4 lineColor)
        {
            DrawLines(new Vector3[] { start, destination }, lineColor);
        }

        /// <summary>
        /// Draws lines created by the given line creator function (e. g. VertexStructure.BuildLineListForFaceNormals).
        /// </summary>
        /// <param name="lineListCreator">Delegate targeting a line creator function.</param>
        /// <param name="lineColor">Color of the result.</param>
        public void DrawLines(Func<List<Vector3>> lineListCreator, Color4 lineColor)
        {
            DrawLines(lineListCreator().ToArray(), lineColor);
        }

        /// <summary>
        /// Draws the given list of lines.
        /// </summary>
        /// <param name="lines">List containing all lines.</param>
        /// <param name="lineColor">Color of the line.</param>
        public void DrawLines(Vector3[] lines, Color4 lineColor)
        {
            D3D11.Device device = m_renderState.Device;

            D3D11.Buffer vertexBuffer = null;
            D3D11.InputLayout inputLayout = null;

            try
            {
                //Load resources
                #region -------------------------------------------------------
                //Load vertex buffer
                StandardVertex[] vertices = StandardVertex.FromLineList(lineColor, lines);
                vertexBuffer = GraphicsHelper.CreateImmutableVertexBuffer(device, vertices);

                //Load effect resource
                EffectResource effectResource = StandardResources.AddAndLoadResource<EffectResource>(m_resourceDictionary, StandardResources.SimpleRenderingEffectName);
                if (effectResource == null) { return; }

                //Create input layout
                inputLayout = new D3D11.InputLayout(device, effectResource.GetInputLayout("Render", "P0"), StandardVertex.InputElements);
                #endregion

                //Draw lines
                #region -------------------------------------------------------
                D3D11.DeviceContext deviceContext = m_renderState.DeviceContext;

                //Set device state
                deviceContext.InputAssembler.InputLayout = inputLayout;
                deviceContext.InputAssembler.PrimitiveTopology = D3D11.PrimitiveTopology.LineList;
                deviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(vertexBuffer, Marshal.SizeOf(typeof(StandardVertex)), 0));

                //Set shader state
                Matrix4 worldViewProj =
                    m_renderState.World.Top *
                    m_renderState.ViewProj;
                effectResource.SetVariableValue("WorldViewProj", worldViewProj);
                effectResource.SetVariableValue("Ambient", 1f);
                effectResource.SetVariableValue("StrongLightFactor", 1f);
                effectResource.ApplyPass(deviceContext, "Render", "P0");

                D3D11.RasterizerState rasterState = null;
                if (GraphicsCore.Current.TargetHardware == TargetHardware.SoftwareRenderer)
                {
                    rasterState = D3D11.RasterizerState.FromDescription(device, new D3D11.RasterizerStateDescription()
                    {
                        CullMode = D3D11.CullMode.Back,
                        FillMode = D3D11.FillMode.Solid,
                        IsFrontCounterclockwise = false,
                        DepthBias = 0,
                        SlopeScaledDepthBias = 0f,
                        DepthBiasClamp = 0f,
                        IsDepthClipEnabled = true,
                        IsAntialiasedLineEnabled = false,
                        IsMultisampleEnabled = false,
                        IsScissorEnabled = false
                    });
                }
                else
                {
                    rasterState = D3D11.RasterizerState.FromDescription(device, new D3D11.RasterizerStateDescription()
                    {
                        CullMode = D3D11.CullMode.Back,
                        FillMode = D3D11.FillMode.Solid,
                        IsFrontCounterclockwise = false,
                        DepthBias = 0,
                        SlopeScaledDepthBias = 0f,
                        DepthBiasClamp = 0f,
                        IsDepthClipEnabled = true,
                        IsAntialiasedLineEnabled = true,
                        IsMultisampleEnabled = false,
                        IsScissorEnabled = false
                    });
                }
                D3D11.RasterizerState oldOne = deviceContext.Rasterizer.State;

                deviceContext.Rasterizer.State = rasterState;
                //Draw vertices
                deviceContext.Draw(vertices.Length, 0);

                deviceContext.Rasterizer.State = oldOne;
                GraphicsHelper.DisposeGraphicsObject(rasterState);
                #endregion
            }
            finally
            {

                vertexBuffer = GraphicsHelper.DisposeGraphicsObject(vertexBuffer);
                inputLayout = GraphicsHelper.DisposeGraphicsObject(inputLayout);
            }
        }
    }
}
