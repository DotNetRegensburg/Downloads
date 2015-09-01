using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Core;

using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class ConstantBufferResource : Resource
    {
        private D3D11.Device m_device;
        private D3D11.Buffer m_constantBuffer;
        private int m_bufferSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBufferResource" /> class.
        /// </summary>
        public ConstantBufferResource(string resourceName, int bufferSize)
            : base(resourceName)
        {
            if (bufferSize < 1) { throw new ArgumentException("Invalid value for buffer size!", "bufferSize"); }
            m_bufferSize = bufferSize;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            if (m_device == null) { m_device = GraphicsCore.Current.HandlerD3D11.Device; }

            m_constantBuffer = CreateConstantBuffer(m_device);
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            m_constantBuffer = GraphicsHelper.DisposeGraphicsObject(m_constantBuffer);
        }

        /// <summary>
        /// Creates the constant buffer object.
        /// </summary>
        protected virtual D3D11.Buffer CreateConstantBuffer(D3D11.Device device)
        {
            return new D3D11.Buffer(
                device,
                m_bufferSize,
                D3D11.ResourceUsage.Default,
                D3D11.BindFlags.ConstantBuffer,
                D3D11.CpuAccessFlags.None,
                D3D11.ResourceOptionFlags.None,
                0);
        }

        /// <summary>
        /// Is the buffer loaded correctly?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_constantBuffer != null; }
        }

        /// <summary>
        /// Gets the buffer object.
        /// </summary>
        public D3D11.Buffer ConstantBuffer
        {
            get { return m_constantBuffer; }
        }

        /// <summary>
        /// Gets the total size of the constant buffer.
        /// </summary>
        public int BufferSize
        {
            get { return m_bufferSize; }
        }
    }
}
