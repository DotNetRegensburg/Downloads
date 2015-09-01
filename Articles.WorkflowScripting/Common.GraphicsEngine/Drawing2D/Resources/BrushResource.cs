using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D2D = SlimDX.Direct2D;

namespace Common.GraphicsEngine.Drawing2D.Resources
{
    public class BrushResource
    {
        private D2D.Brush m_brush;

        /// <summary>
        /// Creates a new BrushResource object using the given brush.
        /// </summary>
        internal BrushResource(D2D.Brush brush)
        {
            m_brush = brush;
        }

        /// <summary>
        /// Creates a new BrushResource object using the given solid color.
        /// </summary>
        /// <param name="solidColor">The solid color to use.</param>
        /// <param name="opacity">The opacity factor used for the brush.</param>
        internal BrushResource(Color solidColor, float opacity)
        {
            D2D.RenderTarget renderTarget = ResourceStorage2D.Current.RenderTarget;
            if (renderTarget == null) { throw new GraphicsLibraryException("Unable to create resource: ResourceStorage2D not correctly initialized!"); }

            m_brush = new D2D.SolidColorBrush(renderTarget, new SlimDX.Color4(solidColor), new D2D.BrushProperties()
            {
                Opacity = opacity
            });
        }

        /// <summary>
        /// Creates a new BrushResource object using the given solid color.
        /// </summary>
        /// <param name="solidColor">The solid color to use.</param>
        internal BrushResource(Color solidColor)
        {
            D2D.RenderTarget renderTarget = ResourceStorage2D.Current.RenderTarget;
            if (renderTarget == null) { throw new GraphicsLibraryException("Unable to create resource: ResourceStorage2D not correctly initialized!"); }

            m_brush = new D2D.SolidColorBrush(renderTarget, new SlimDX.Color4(solidColor));
        }

        /// <summary>
        /// Gets the created brush.
        /// </summary>
        internal D2D.Brush Brush
        {
            get { return m_brush; }
        }
    }
}
