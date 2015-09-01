using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Common.GraphicsEngine.Objects.Construction
{
    /// <summary>
    /// A Treangle inside a VertexStructure object
    /// </summary>
    public struct Triangle
    {
        public short Index1;
        public short Index2;
        public short Index3;

        /// <summary>
        /// Creates a new triangle
        /// </summary>
        /// <param name="index1">Index of the first vertex</param>
        /// <param name="index2">Index of the second vertex</param>
        /// <param name="index3">Index of the third vertex</param>
        public Triangle(short index1, short index2, short index3)
        {
            Index1 = index1;
            Index2 = index2;
            Index3 = index3;
        }
    }
}
