using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Objects
{
    public class TexturePainter : SceneObject
    {
        private TexturePainterResource m_painterResource;
        private TextureResource m_textureResource;
        private DepthStencilStateResource m_depthStencilStateResource;
        private BlendStateResource m_blendStateResource;
        private string m_texture;
        private float m_scaling;

        /// <summary>
        /// Initializes a new instance of the <see cref="TexturePainter"/> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        public TexturePainter(string texture)
        {
            m_texture = texture;
            m_scaling = 1f;
        }

        /// <summary>
        /// Loads all resources of the object.
        /// </summary>
        /// <param name="device">Current DirectX device.</param>
        public override void LoadResources(D3D11.Device device)
        {
            //Load painter resource
            m_painterResource = base.Resources.GetResourceAndEnsureLoaded<TexturePainterResource>(
                "TexturePainter",
                () => new TexturePainterResource("TexturePainter"));

            //Load the texture resource
            m_textureResource = base.Resources.GetResourceAndEnsureLoaded<TextureResource>(m_texture);

            //Loads the depth-stencil resource
            m_depthStencilStateResource = base.Resources.GetResourceAndEnsureLoaded(
                "DisableZWrites",
                () => new DepthStencilStateResource("DisableZWrites") { EnableZWrite = false });

            m_blendStateResource = base.Resources.GetResourceAndEnsureLoaded(
                "Blending",
                () => new BlendStateResource("Blending"));
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
            renderState.DeviceContext.OutputMerger.DepthStencilState = m_depthStencilStateResource.State;
            //renderState.DeviceContext.OutputMerger.BlendState = m_blendStateResource.BlendState;
            //renderState.DeviceContext.OutputMerger.BlendFactor = new SharpDX.Color4(0.7f, 0.7f, 0.7f, 1.0f);
            m_painterResource.Draw(
                renderState,
                m_textureResource.TextureView,
                m_scaling);
            //renderState.DeviceContext.OutputMerger.BlendState = null;
            renderState.DeviceContext.OutputMerger.DepthStencilState = null;
        }

        /// <summary>
        /// Unloads all resources of the object.
        /// </summary>
        public override void UnloadResources()
        {
            m_painterResource = null;
            m_textureResource = null;
            m_depthStencilStateResource = null;
            m_blendStateResource = null;
        }

        /// <summary>
        /// Are resources loaded?
        /// </summary>
        /// <value></value>
        public override bool IsLoaded
        {
            get { return (m_painterResource != null) && (m_texture != null); }
        }

        /// <summary>
        /// Gets or sets the scaling factor.
        /// </summary>
        public float Scaling
        {
            get { return m_scaling; }
            set { m_scaling = value; }
        }
    }
}
