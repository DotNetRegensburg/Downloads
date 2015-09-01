using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Core;

using D2D = SharpDX.Direct2D1;

namespace RK.Common.GraphicsEngine.Drawing2D
{
    public class Graphics2DCache : IDisposable
    {
        private D2D.RenderTarget m_renderTarget;

        private Dictionary<Color4, D2D.Brush> m_solidBrushes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graphics2DCache" /> class.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public Graphics2DCache(D2D.RenderTarget renderTarget)
        {
            m_renderTarget = renderTarget;

            m_solidBrushes = new Dictionary<Color4, D2D.Brush>();
        }

        /// <summary>
        /// Gets the brush for the given color.
        /// </summary>
        /// <param name="color">The color for which to get the brush.</param>
        public D2D.Brush GetSolidColorBrush(Color4 color)
        {
            if (m_renderTarget == null) { throw new ObjectDisposedException(typeof(Graphics2DCache).FullName); }

            if (m_solidBrushes.ContainsKey(color)) { return m_solidBrushes[color]; }
            else
            {
                D2D.SolidColorBrush newBrush = new D2D.SolidColorBrush(m_renderTarget, color.ToDXColor());
                m_solidBrushes[color] = newBrush;
                return newBrush;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            m_renderTarget = null;
            foreach (var actPair in m_solidBrushes)
            {
                GraphicsHelper.DisposeGraphicsObject(actPair.Value);
            }
            m_solidBrushes.Clear();
            m_solidBrushes = null;
        }
    }
}
