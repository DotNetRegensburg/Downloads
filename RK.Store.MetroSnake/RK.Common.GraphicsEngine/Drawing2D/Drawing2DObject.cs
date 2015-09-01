using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Drawing3D;
using D2D = SharpDX.Direct2D1;

namespace RK.Common.GraphicsEngine.Drawing2D
{
    public abstract class Drawing2DObject
    {

        /// <summary>
        /// Renders this object using the given render target.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        /// <param name="updateState">Current update state.</param>
        public abstract void Render(RenderState2D renderState, UpdateState updateState);
    }
}
