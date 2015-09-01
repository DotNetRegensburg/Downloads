using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Line3D
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;

        public Line3D(Vector3 start, Vector3 end)
        {
            this.StartPosition = start;
            this.EndPosition = end;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "From " + StartPosition + " to " + EndPosition;
        }

        /// <summary>
        /// Equality test with a small tolerance.
        /// </summary>
        /// <param name="otherLine">The other line to check.</param>
        public bool EqualsWithTolerance(Line3D otherLine)
        {
            //Check in both directions
            return
                (StartPosition.EqualsWithTolerance(otherLine.StartPosition) && EndPosition.EqualsWithTolerance(otherLine.EndPosition)) ||
                (EndPosition.EqualsWithTolerance(otherLine.StartPosition) && StartPosition.EqualsWithTolerance(otherLine.EndPosition));
        }
    }
}
