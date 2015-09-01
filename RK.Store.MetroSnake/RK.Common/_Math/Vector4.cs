using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace RK.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4
    {
        public const float TOLERANCE = 0.00001f;
        public static readonly Vector4 Empty = new Vector4();

        public float X;
        public float Y;
        public float Z;
        public float W;

        /// <summary>
        /// Creates a new Vector4 with the given values
        /// </summary>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Parses the given string
        /// </summary>
        public Vector4(string text)
        {
            try
            {
                string[] elements = text.Split(';');

                X = Single.Parse(elements[0], CultureInfo.InvariantCulture);
                Y = Single.Parse(elements[1], CultureInfo.InvariantCulture);
                Z = Single.Parse(elements[2], CultureInfo.InvariantCulture);
                W = Single.Parse(elements[3], CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse given string to Vector2: " + ex.Message, ex);
            }
        }

        ///// <summary>
        ///// Converts this vector to a directX vector
        ///// </summary>
        //internal SlimDX.Vector4 ToDirectXVector()
        //{
        //    return new SlimDX.Vector4(X, Y, Z, W);
        //}

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return X.ToString("F3", CultureInfo.InvariantCulture) + ";" + 
                   Y.ToString("F3", CultureInfo.InvariantCulture) + ";" + 
                   Z.ToString("F3", CultureInfo.InvariantCulture) + ";" + 
                   W.ToString("F3", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (obj is Vector4)
            {
                Vector4 other = (Vector4)obj;
                return this == other;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Equality test with tolerance
        /// </summary>
        public bool EqualsWithTolerance(Vector4 other)
        {
            return
                (this.X <= other.X + TOLERANCE) && (this.X >= other.X - TOLERANCE) &&
                (this.Y <= other.Y + TOLERANCE) && (this.Y >= other.Y - TOLERANCE) &&
                (this.Z <= other.Z + TOLERANCE) && (this.Z >= other.Z - TOLERANCE) &&
                (this.W <= other.W + TOLERANCE) && (this.W >= other.W - TOLERANCE);
        }

        /// <summary>
        /// != operator
        /// </summary>
        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z) || (left.W != right.W);
        }

        /// <summary>
        /// == operator
        /// </summary>
        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z) && (left.W != right.W);
        }
    }
}
