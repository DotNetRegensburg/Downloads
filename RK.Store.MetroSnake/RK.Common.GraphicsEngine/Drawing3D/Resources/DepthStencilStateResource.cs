using RK.Common.GraphicsEngine.Core;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class DepthStencilStateResource : Resource
    {
        //Direct3D 11 Resources
        private D3D11.DepthStencilState m_depthStencilState;

        //Standard members
        private bool m_enableZWrite;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilStateResource"/> class.
        /// </summary>
        public DepthStencilStateResource(string name)
            : base(name)
        {
            m_enableZWrite = true;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;
            m_depthStencilState = new D3D11.DepthStencilState(device, new D3D11.DepthStencilStateDescription()
            {
                BackFace = new D3D11.DepthStencilOperationDescription()
                {
                   Comparison = D3D11.Comparison.Never,
                   DepthFailOperation = D3D11.StencilOperation.Keep,
                   FailOperation = D3D11.StencilOperation.Keep,
                   PassOperation = D3D11.StencilOperation.Keep
                },
                FrontFace = new D3D11.DepthStencilOperationDescription()
                {
                    Comparison = D3D11.Comparison.Never,
                    DepthFailOperation = D3D11.StencilOperation.Keep,
                    FailOperation = D3D11.StencilOperation.Keep,
                    PassOperation = D3D11.StencilOperation.Keep
                },
                DepthComparison = D3D11.Comparison.Less,
                IsDepthEnabled = true,
                DepthWriteMask = m_enableZWrite ? D3D11.DepthWriteMask.All : D3D11.DepthWriteMask.Zero,
                IsStencilEnabled = false,
                StencilReadMask = 0,
                StencilWriteMask = 0,
            });
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_depthStencilState = GraphicsHelper.DisposeGraphicsObject(m_depthStencilState);
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        /// <value></value>
        public override bool IsLoaded
        {
            get { return m_depthStencilState != null; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable Z write].
        /// </summary>
        /// <value><c>true</c> if [enable Z write]; otherwise, <c>false</c>.</value>
        public bool EnableZWrite
        {
            get { return m_enableZWrite; }
            set { m_enableZWrite = value; }
        }

        /// <summary>
        /// Gets current DepthStencilState object.
        /// </summary>
        public D3D11.DepthStencilState State
        {
            get { return m_depthStencilState; }
        }
    }
}
