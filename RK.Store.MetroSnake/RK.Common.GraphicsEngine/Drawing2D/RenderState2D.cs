using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D2D = SharpDX.Direct2D1;

namespace RK.Common.GraphicsEngine.Drawing2D
{
    public class RenderState2D
    {
        private D2D.RenderTarget m_renderTarget;
        private Graphics2DCache m_graphicsCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderState2D" /> class.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="graphicsCache">The graphics cache.</param>
        public RenderState2D(D2D.RenderTarget renderTarget, Graphics2DCache graphicsCache)
        {
            m_renderTarget = renderTarget;
            m_graphicsCache = graphicsCache;
        }

        public D2D.RenderTarget RenderTarget
        {
            get { return m_renderTarget; }
        }

        public Graphics2DCache GraphicsCache
        {
            get { return m_graphicsCache; }
        }
    }
}
