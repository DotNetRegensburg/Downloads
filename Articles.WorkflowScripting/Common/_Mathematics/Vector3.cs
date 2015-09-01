using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

//Some namespace mappings
using GDI = System.Drawing;

namespace Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public const float TOLERANCE = 0.00001f;
        public const int NATIVE_SIZE = 12;
        
        public static readonly Vector3 Empty = new Vector3();
        public static readonly Vector3 AxisX = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 AxisY = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 AxisZ = new Vector3(0f, 0f, 1f);
        public static readonly Vector3 MinValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        public static readonly Vector3 MaxValue = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public float X;
        public float Y;
        public float Z;

        /// <summary>
        /// Creates a new Vector3 structure
        /// </summary>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Parses the given string
        /// </summary>
        public Vector3(string text)
        {
            try
            {
                string[] elements = text.Split(';');

                X = Single.Parse(elements[0], CultureInfo.InvariantCulture);
                Y = Single.Parse(elements[1], CultureInfo.InvariantCulture);
                Z = Single.Parse(elements[2], CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse given string to Vector2: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates a new Vector3 structure
        /// </summary>
        public Vector3(Vector3 original)
        {
            X = original.X;
            Y = original.Y;
            Z = original.Z;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return X.ToString("F3",CultureInfo.InvariantCulture) + ";" + 
                   Y.ToString("F3",CultureInfo.InvariantCulture) + ";" + 
                   Z.ToString("F3",CultureInfo.InvariantCulture);
        }

        ///// <summary>
        ///// Converts this vector to a directx vector
        ///// </summary>
        //internal SlimDX.Vector3 ToDirectXVector()
        //{
        //    return new SlimDX.Vector3(X, Y, Z);
        //}

        /// <summary>
        /// Converts this vector to a vector containing horizontal and vertical rotation values.
        /// </summary>
        public Vector2 ToHVRotation()
        {
            Vector3 normal = Vector3.Normalize(this);

            Vector2 result = new Vector2();
            result.X = (float)Math.Atan2(normal.Z, normal.X);
            result.Y = (float)Math.Atan2(normal.Y, new Vector2(normal.Z, normal.X).Length);
            return result;
        }

        /// <summary>
        /// Writes horizontal and vertical rotation values to given parameters.
        /// </summary>
        /// <param name="hRotation">Parameter for horizontal rotation.</param>
        /// <param name="vRotation">Parameter for vertical rotation.</param>
        public void ToHVRotation(out float hRotation, out float vRotation)
        {
            Vector3 normal = Vector3.Normalize(this);

            hRotation = (float)Math.Atan2(normal.Z, normal.X);
            vRotation = (float)Math.Atan2(normal.Y, new Vector2(normal.Z, normal.X).Length);
        }

        /// <summary>
        /// Generates a normal out of given horizontal and vertical rotation.
        /// </summary>
        /// <param name="horizontalRotation">Horizontal rotation value.</param>
        /// <param name="verticalRotation">Vertical rotation value.</param>
        public static Vector3 NormalFromHVRotation(float horizontalRotation, float verticalRotation)
        {
            Vector3 result = Vector3.Empty;

            //Generate vector 
            result.X = (float)(1f * Math.Cos(verticalRotation) * Math.Cos(horizontalRotation));
            result.Y = (float)(1f * Math.Sin(verticalRotation));
            result.Z = (float)(1f * Math.Cos(verticalRotation) * Math.Sin(horizontalRotation));

            //Normalize the generated vector
            result.Normalize();

            return result;
        }

        /// <summary>
        /// Generates a normal out of given horizontal and vertical rotation.
        /// </summary>
        /// <param name="rotation">Vector containing horizontal and vertical rotations.</param>
        public static Vector3 NormalFromHVRotation(Vector2 rotation)
        {
            return NormalFromHVRotation(rotation.X, rotation.Y);
        }

        /// <summary>
        /// Creates the cross product between this vector and the given one
        /// </summary>
        public void Cross(Vector3 vector)
        {
            float oldX = X;
            float oldY = Y;
            float oldZ = Z;

            X = (oldY * vector.Z) - (oldZ * vector.Y);
            Y = (oldZ * vector.X) - (oldX * vector.Z);
            Z = (oldX * vector.Y) - (oldY * vector.X);
        }

        /// <summary>
        /// Creates the dot product between this vector and the given one
        /// </summary>
        public float Dot(Vector3 vector)
        {
            return
                X * vector.X +
                Y * vector.Y +
                Z * vector.Z;
        }

        /// <summary>
        /// Scales the vector
        /// </summary>
        public void Scale(float scaling)
        {
            X *= scaling;
            Y *= scaling;
            Z *= scaling;
        }

        /// <summary>
        /// Transforms the vector
        /// </summary>
        public void Transform(Matrix4 transformMatrix)
        {
            Vector3 tmp = new Vector3(X, Y, Z);

            X = tmp.X * transformMatrix.M11 + tmp.Y * transformMatrix.M21 + tmp.Z * transformMatrix.M31 + transformMatrix.M41;
            Y = tmp.X * transformMatrix.M12 + tmp.Y * transformMatrix.M22 + tmp.Z * transformMatrix.M32 + transformMatrix.M42;
            Z = tmp.X * transformMatrix.M13 + tmp.Y * transformMatrix.M23 + tmp.Z * transformMatrix.M33 + transformMatrix.M43;
        }

        /// <summary>
        /// Transforms this normal.
        /// </summary>
        public void TransformNormal(Matrix4 transformMatrix)
        {
            float oldX = X;
            float oldY = Y;
            float oldZ = Z;

            X = (oldX * transformMatrix.M11) + (oldY * transformMatrix.M21) + (oldZ * transformMatrix.M31);
            Y = (oldX * transformMatrix.M12) + (oldY * transformMatrix.M22) + (oldZ * transformMatrix.M32);
            Z = (oldX * transformMatrix.M13) + (oldY * transformMatrix.M23) + (oldZ * transformMatrix.M33);
        }

        /// <summary>
        /// Transforms the vector
        /// </summary>
        public static Vector3 Transform(Vector3 toTransform, Matrix4 transformMatrix)
        {
            toTransform.Transform(transformMatrix);
            return toTransform;
        }

        /// <summary>
        /// Transforms the normal
        /// </summary>
        public static Vector3 TransformNormal(Vector3 toTransform, Matrix4 transformMatrix)
        {
            toTransform.TransformNormal(transformMatrix);
            return toTransform;
        }

        /// <summary>
        /// Is any component negative?
        /// </summary>
        public bool AnyComponentNegative()
        {
            return (X < 0f) ||
                   (Y < 0f) ||
                   (Z < 0f);
        }

        /// <summary>
        /// Is any component negative?
        /// </summary>
        public bool AnyComponentPositive()
        {
            return (X > 0f) ||
                   (Y > 0f) ||
                   (Z > 0f);
        }

        /// <summary>
        /// Is this vector empty?
        /// </summary>
        public bool IsEmpty()
        {
            return this == Empty; 
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        public void Normalize()
        {
            float lenght = this.Length;
            float multiplier = 1f / lenght;

            X *= multiplier;
            Y *= multiplier;
            Z *= multiplier;
        }

        /// <summary>
        /// Adds the given vector
        /// </summary>
        public void Add(Vector3 vector)
        {
            X += vector.X;
            Y += vector.Y;
            Z += vector.Z;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                Vector3 other = (Vector3)obj;
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
        public bool EqualsWithTolerance(Vector3 other)
        {
            return
                (this.X <= other.X + TOLERANCE) && (this.X >= other.X - TOLERANCE) &&
                (this.Y <= other.Y + TOLERANCE) && (this.Y >= other.Y - TOLERANCE) &&
                (this.Z <= other.Z + TOLERANCE) && (this.Z >= other.Z - TOLERANCE);
        }

        /// <summary>
        /// Calculates the normal of the given triangle
        /// </summary>
        public static Vector3 CalculateTriangleNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 result = new Vector3();

            result.X = (b.Y - a.Y) * (c.Z - a.Z) - (c.Y - a.Y) * (b.Z - a.Z);
            result.Y = (b.Z - a.Z) * (c.X - a.X) - (c.Z - a.Z) * (b.X - a.X);
            result.Z = (b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y);
            float length = (float)Math.Sqrt(result.X * result.X + result.Y * result.Y + result.Z * result.Z);

            if (length > 0)
            {
                result.X /= length;
                result.Y /= length;
                result.Z /= length;
            }

            return result;
        }

        ///// <summary>
        ///// Calculates rotation axis and angle between two vectors.
        ///// </summary>
        ///// <param name="_vector1">Source vector.</param>
        ///// <param name="_vector2">Target vector.</param>
        ///// <param name="_planeWhenOrthogonal">Vector for determining rotation plane if vector 1 and 2 are aligned.</param>
        //public static float AxisAngle(Vector3 _vector1, Vector3 _vector2, Vector3 _planeWhenAligned, out Vector3 axis)
        //{
        //    Vector3 vector1 = Vector3.Normalize(_vector1);
        //    Vector3 vector2 = Vector3.Normalize(_vector2);
        //    Vector3 planeWhenOrthogonal = Vector3.Normalize(_planeWhenAligned);
        //    Vector3 rotationAxis = Vector3.Cross(vector1, vector2);
        //    float sinAngle = rotationAxis.Length;
        //    float cosAngle = Vector3.Dot(vector1, vector2);

        //    if (Math.Abs(vector1.X - vector2.X) < 0.001 &&
        //        Math.Abs(vector1.Y - vector2.Y) < 0.001 &&
        //        Math.Abs(vector1.Z - vector2.Z) < 0.001
        //        )
        //    {
        //        axis = Vector3.Normalize(Vector3.Cross(vector1, planeWhenOrthogonal));
        //        return 0;
        //    }

        //    Vector3 invertedVector2 = Vector3.Scale(vector2, -1.0f);
        //    if (Math.Abs(vector1.X - invertedVector2.X) < 0.001 &&
        //        Math.Abs(vector1.Y - invertedVector2.Y) < 0.001 &&
        //        Math.Abs(vector1.Z - invertedVector2.Z) < 0.001
        //        )
        //    {
        //        axis = Vector3.Normalize(Vector3.Cross(vector1, planeWhenOrthogonal));
        //        return (float)Math.PI;
        //    }

        //    float angle = 0;
        //    if (cosAngle >= 0)
        //    {
        //        angle = (float)Math.Asin(sinAngle);
        //    }
        //    if (cosAngle < 0)
        //    {
        //        angle = ((float)(Math.PI - Math.Asin(sinAngle)));
        //    }
        //    if (sinAngle < 0)
        //    {
        //        rotationAxis.Scale(-1.0f);
        //    }
        //    rotationAxis.Normalize();
        //    axis = rotationAxis;
        //    return angle;
        //}

        /// <summary>
        /// Builds a vector using lowest components of given vectors
        /// </summary>
        public static Vector3 Minimize(params Vector3[] vectors)
        {
            if(vectors.Length==0){ return new Vector3(); }

            Vector3 result = vectors[0];
            for(int loop=1 ; loop<vectors.Length ; loop++)
            {
                if (vectors[loop].X < result.X) { result.X = vectors[loop].X; }
                if (vectors[loop].Y < result.Y) { result.Y = vectors[loop].Y; }
                if (vectors[loop].Z < result.Z) { result.Z = vectors[loop].Z; }
            }

            return result;
        }

        /// <summary>
        /// Builds a vector using highest components of given vectors
        /// </summary>
        public static Vector3 Maximize(params Vector3[] vectors)
        {
            if (vectors.Length == 0) { return new Vector3(); }

            Vector3 result = vectors[0];
            for (int loop = 1; loop < vectors.Length; loop++)
            {
                if (vectors[loop].X > result.X) { result.X = vectors[loop].X; }
                if (vectors[loop].Y > result.Y) { result.Y = vectors[loop].Y; }
                if (vectors[loop].Z > result.Z) { result.Z = vectors[loop].Z; }
            }

            return result;
        }

        /// <summary>
        /// Builds a normal out of the given vector
        /// </summary>
        public static Vector3 Normalize(Vector3 source)
        {
            source.Normalize();
            return source;
        }

        /// <summary>
        /// Adds the given vectors and returns the result
        /// </summary>
        public static Vector3 Add(Vector3 left, Vector3 right)
        {
            left.Add(right);
            return left;
        }

        /// <summary>
        /// Builds the cross product of the given vectors
        /// </summary>
        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            left.Cross(right);
            return left;
        }

        /// <summary>
        /// Builds the dot product of the given vectors
        /// </summary>
        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.Dot(right);
        }

        /// <summary>
        /// Scales the given vector and returns the result
        /// </summary>
        public static Vector3 Scale(Vector3 vector, float scaling)
        {
            vector.Scale(scaling);
            return vector;
        }

        /// <summary>
        /// Gets an average vector.
        /// </summary>
        public static Vector3 Average(params Vector3[] vectors)
        {
            if (vectors.Length == 0) { return Vector3.Empty; }
            Vector3 result = Vector3.Sum(vectors);

            result.X = result.X / (float)vectors.Length;
            result.Y = result.Y / (float)vectors.Length;
            result.Z = result.Z / (float)vectors.Length;

            return result;
        }

        /// <summary>
        /// Gets the a vector containing the sum of each given vector.
        /// </summary>
        /// <param name="vectors">The vectors to add one by one.</param>
        public static Vector3 Sum(params Vector3[] vectors)
        {
            Vector3 result = Vector3.Empty;
            for (int loop = 0; loop < vectors.Length; loop++)
            {
                result = result + vectors[loop];
            }
            return result;
        }

        /// <summary>
        /// + operator
        /// </summary>
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// - operator
        /// </summary>
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// - operator
        /// </summary>
        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3(-vector.X, -vector.Y, -vector.Z);
        }

        /// <summary>
        /// * operator (Vector3 * float)
        /// </summary>
        public static Vector3 operator *(Vector3 left, float right)
        {
            return new Vector3(left.X * right, left.Y * right, left.Z * right);
        }

        /// <summary>
        /// / operator (Vector3 / float).
        /// </summary>
        public static Vector3 operator /(Vector3 left, float right)
        {
            return new Vector3(left.X / right, left.Y / right, left.Z / right);
        }

        /// <summary>
        /// != operator
        /// </summary>
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z);
        }

        /// <summary>
        /// == operator
        /// </summary>
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z);
        }

        /// <summary>
        /// Retrieves the length of the vector
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }
    }
}
