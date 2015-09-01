using RK.Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class RasterizerStateResource : Resource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.RasterizerState m_rasterizerState;

        //Standard members

        public RasterizerStateResource(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            m_rasterizerState = new D3D11.RasterizerState(
                GraphicsCore.Current.HandlerD3D11.Device,
                new D3D11.RasterizerStateDescription()
            {
                CullMode = D3D11.CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0f,
                FillMode = D3D11.FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0f
            });
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_rasterizerState = GraphicsHelper.DisposeGraphicsObject(m_rasterizerState);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_rasterizerState != null; }
        }

        /// <summary>
        /// Gets the RasterizerState object.
        /// </summary>
        public D3D11.RasterizerState State
        {
            get { return m_rasterizerState; }
        }
    }
}
