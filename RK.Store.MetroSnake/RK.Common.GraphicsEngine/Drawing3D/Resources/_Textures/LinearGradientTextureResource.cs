using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Some namespace mappings
using GDI = System.Drawing;
using GDI2D = System.Drawing.Drawing2D;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class LinearGradientTextureResource : DrawingBrushTextureResource
    {
        /// <summary>
        /// Creates a linear gradient texture.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        /// <param name="start">Starting color.</param>
        /// <param name="destination">Destination color.</param>
        /// <param name="gradientDirection">Direction of the gradient.</param>
        public LinearGradientTextureResource(
            string name,
            Color4 start,
            Color4 destination,
            GradientDirection gradientDirection,
            int widht, int height)
            : base(
                name, 
                new GDI2D.LinearGradientBrush(GetStartPoint(gradientDirection, widht, height), GetTargetPoint(gradientDirection, widht, height), start.ToGdiColor(), destination.ToGdiColor()),
                widht, height)
        {

        }

        /// <summary>
        /// Creates a linear gradient texture.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        /// <param name="start">Starting color.</param>
        /// <param name="destination">Destination color.</param>
        /// <param name="gradientDirection">Direction of the gradient.</param>
        public LinearGradientTextureResource(
            string name,
            Color4 start,
            Color4 destination,
            GradientDirection gradientDirection)
            : base(
                name,
                new GDI2D.LinearGradientBrush(GetStartPoint(gradientDirection, 32, 32), GetTargetPoint(gradientDirection, 32, 32), start.ToGdiColor(), destination.ToGdiColor()),
                32, 32)
        {

        }

        /// <summary>
        /// Gets the start-point of the gradient.
        /// </summary>
        /// <param name="direction">Direction of the gradient.</param>
        private static GDI.Point GetStartPoint(GradientDirection direction, int width, int height)
        {
            switch (direction)
            {
                case GradientDirection.LeftToRight:
                    return new GDI.Point(0, 0);

                case GradientDirection.TopToBottom:
                    return new GDI.Point(0, 0);

                case GradientDirection.Directional:
                    return new GDI.Point(0, 0);
            }
            return new GDI.Point(0, 0);
        }

        /// <summary>
        /// Gets the target-point of the gradient.
        /// </summary>
        /// <param name="direction">Direction of the gradient.</param>
        private static GDI.Point GetTargetPoint(GradientDirection direction, int width, int height)
        {
            switch (direction)
            {
                case GradientDirection.LeftToRight:
                    return new GDI.Point(width, 0);

                case GradientDirection.TopToBottom:
                    return new GDI.Point(0, height);

                case GradientDirection.Directional:
                    return new GDI.Point(width, height);
            }
            return new GDI.Point(width, height);
        }
    }
}
