using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Core;
using SharpDX;

//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class TypeSafeConstantBufferResource<T> : ConstantBufferResource
        where T : struct
    {
        private T m_initialData;
        private int m_structureSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSafeConstantBufferResource{T}" /> class.
        /// </summary>
        public TypeSafeConstantBufferResource(string resourceName)
            : base(resourceName, Utilities.SizeOf<T>())
        {
            m_initialData = new T();
            m_structureSize = Utilities.SizeOf<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSafeConstantBufferResource{T}" /> class.
        /// </summary>
        public TypeSafeConstantBufferResource(string resourceName, T initialData)
            : base(resourceName, Utilities.SizeOf<T>())
        {
            m_initialData = initialData;
            m_structureSize = Utilities.SizeOf<T>();
        }

        /// <summary>
        /// Sets given content to the constant buffer.
        /// </summary>
        /// <param name="deviceContext">The context used for updating the constant buffer.</param>
        /// <param name="dataToSet">The data to set.</param>
        public void SetData(D3D11.DeviceContext deviceContext, T dataToSet)
        {
            //deviceContext.UpdateSubresourceSafe<T>(
            //    ref dataToSet, base.ConstantBuffer, m_structureSize);
            deviceContext.UpdateSubresource<T>(ref dataToSet, base.ConstantBuffer);
        }

        /// <summary>
        /// Creates the constant buffer object.
        /// </summary>
        protected override D3D11.Buffer CreateConstantBuffer(D3D11.Device device)
        {
            using (SharpDX.DataStream dataStream = new SharpDX.DataStream(
                SharpDX.Utilities.SizeOf<T>(),
                true, true))
            {
                dataStream.Write(m_initialData);
                dataStream.Position = 0;

                return new D3D11.Buffer(
                    device,
                    dataStream,
                    new D3D11.BufferDescription(
                        m_structureSize,
                        D3D11.ResourceUsage.Default,
                        D3D11.BindFlags.ConstantBuffer,
                        D3D11.CpuAccessFlags.None,
                        D3D11.ResourceOptionFlags.None,
                        0));
            }
        }
    }
}
