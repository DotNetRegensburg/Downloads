using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.GraphicsEngine
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Generates a slimdx color structure.
        /// </summary>
        internal static SlimDX.Color4 ToDirectXColor(this Color4 sourceColor)
        {
            return new SlimDX.Color4(sourceColor.A, sourceColor.R, sourceColor.G, sourceColor.B);
        }

        /// <summary>
        /// Generates a slimdx vector structure.
        /// </summary>
        internal static SlimDX.Vector3 ToDirectXVector(this Vector3 sourceVector)
        {
            return new SlimDX.Vector3(sourceVector.X, sourceVector.Y, sourceVector.Z);
        }

        /// <summary>
        /// Generates a slimdx matrix structure.
        /// </summary>
        internal static SlimDX.Matrix ToDirectXMatrix(this Matrix4 sourceMatrix)
        {
            return new SlimDX.Matrix()
            {
                M11 = sourceMatrix.M11,
                M12 = sourceMatrix.M12,
                M13 = sourceMatrix.M13,
                M14 = sourceMatrix.M14,
                M21 = sourceMatrix.M21,
                M22 = sourceMatrix.M22,
                M23 = sourceMatrix.M23,
                M24 = sourceMatrix.M24,
                M31 = sourceMatrix.M31,
                M32 = sourceMatrix.M32,
                M33 = sourceMatrix.M33,
                M34 = sourceMatrix.M34,
                M41 = sourceMatrix.M41,
                M42 = sourceMatrix.M42,
                M43 = sourceMatrix.M43,
                M44 = sourceMatrix.M44
            };
        }
    }
}
