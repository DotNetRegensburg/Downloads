using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Drawing2D.Resources;

//Some namespace mappings
using D2D = SlimDX.Direct2D;

namespace Common.GraphicsEngine.Drawing2D
{
    public class Graphics2D
    {
        private D2D.RenderTarget m_renderTarget;

        /// <summary>
        /// Creates a new Graphics2D object.
        /// </summary>
        internal Graphics2D(D2D.RenderTarget renderTarget)
        {
            m_renderTarget = renderTarget;
        }

        /// <summary>
        /// Draws the given line.
        /// </summary>
        /// <param name="brush">The brush to use for drawing.</param>
        /// <param name="start">The point to start from.</param>
        /// <param name="destination">The destination point.</param>
        public void DrawLine(BrushResource brush, PointF start, PointF destination)
        {
            m_renderTarget.DrawLine(brush.Brush, start, destination);
        }

        /// <summary>
        /// Draws the given line.
        /// </summary>
        /// <param name="brush">The brush to use for drawing.</param>
        /// <param name="start">The point to start from.</param>
        /// <param name="destination">The destination point.</param>
        public void DrawLine(BrushResource brush, PointF start, PointF destination, float strokeSize)
        {
            m_renderTarget.DrawLine(brush.Brush, start, destination, strokeSize);
        }

        /// <summary>
        /// Draws the given line.
        /// </summary>
        /// <param name="brush">The brush to use for drawing.</param>
        /// <param name="start">The point to start from.</param>
        /// <param name="destination">The destination point.</param>
        public void DrawLine(BrushResource brush, float startX, float startY, float targetX, float targetY)
        {
            m_renderTarget.DrawLine(brush.Brush, startX, startY, targetX, targetY);
        }

        /// <summary>
        /// Draws the given line.
        /// </summary>
        /// <param name="brush">The brush to use for drawing.</param>
        /// <param name="start">The point to start from.</param>
        /// <param name="destination">The destination point.</param>
        public void DrawLine(BrushResource brush, float startX, float startY, float targetX, float targetY, float strokeSize)
        {
            m_renderTarget.DrawLine(brush.Brush, startX, startY, targetX, targetY, strokeSize);
        }
    }
}
