using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

//Some namespace mappings
using GDI = System.Drawing;

namespace Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public const float TOLERANCE = 0.00001f;

        public static readonly Vector2 Empty = new Vector2();
        public static readonly Vector2 AxisX = new Vector2(1f, 0f);
        public static readonly Vector2 AxisY = new Vector2(0f, 1f);

        public float X;
        public float Y;

        /// <summary>
        /// Creates a new vector2 structure.
        /// </summary>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new vector2 structure.
        /// </summary>
        public Vector2(GDI.Point point)
        {
            X = (float)point.X;
            Y = (float)point.Y;
        }

        /// <summary>
        /// Creates a new vector2 structure.
        /// </summary>
        public Vector2(GDI.PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        /// <summary>
        /// Parses the given string
        /// </summary>
        public Vector2(string text)
        {
            try
            {
                string[] elements = text.Split(';');

                X = Single.Parse(elements[0], CultureInfo.InvariantCulture);
                Y = Single.Parse(elements[1], CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse given string to Vector2: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates a new vector2 structure
        /// </summary>
        public Vector2(Vector2 original)
        {
            X = original.X;
            Y = original.Y;
        }

        ///// <summary>
        ///// Converts this vector to a directx vector
        ///// </summary>
        //internal SlimDX.Vector2 ToDirectXVector()
        //{
        //    return new SlimDX.Vector2(X, Y);
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
                   Y.ToString("F3", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Normalizes this vector
        /// </summary>
        public void Normalize()
        {
            float lenght = this.Length;
            float multiplier = 1f / lenght;

            X *= multiplier;
            Y *= multiplier;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                Vector2 other = (Vector2)obj;
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
        /// <param name="other">The other vector.</param>
        public bool EqualsWithTolerance(Vector2 other)
        {
            return
                (this.X <= other.X + TOLERANCE) && (this.X >= other.X - TOLERANCE) &&
                (this.Y <= other.Y + TOLERANCE) && (this.Y >= other.Y - TOLERANCE);
        }

        /// <summary>
        /// Normalizes the given vector
        /// </summary>
        public static Vector2 Normalize(Vector2 vector)
        {
            vector.Normalize();
            return vector;
        }

        /// <summary>
        /// - operator
        /// </summary>
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.X - right.X,
                left.Y - right.Y);
        }

        /// <summary>
        /// + operator
        /// </summary>
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.X + right.X,
                left.Y + right.Y);
        }

        /// <summary>
        /// * operator (Vector2 * float)
        /// </summary>
        public static Vector2 operator *(Vector2 left, float right)
        {
            return new Vector2(left.X * right, left.Y * right);
        }

        /// <summary>
        /// != operator
        /// </summary>
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return (left.X != right.X) || (left.Y != right.Y);
        }

        /// <summary>
        /// == operator
        /// </summary>
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return (left.X == right.X) && (left.Y == right.Y);
        }

        /// <summary>
        /// Gets the length of this vector
        /// </summary>
        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }
    }
}
