using System;
using System.Runtime.InteropServices;

namespace Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4
    {
        public static readonly Matrix4 Empty = new Matrix4();
        public static readonly Matrix4 Identity = new Matrix4(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f);

        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        /// <summary>
        /// Creates a new Matrix structure
        /// </summary>
        public Matrix4(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        /// <summary>
        /// Creates a new matrix using the given rows
        /// </summary>
        public Matrix4(Vector4 first, Vector4 second, Vector4 third, Vector4 fourth)
        {
            M11 = first.X; M12 = first.Y; M13 = first.Z; M14 = first.W;
            M21 = second.X; M22 = second.Y; M23 = second.Z; M24 = second.W;
            M31 = third.X; M32 = third.Y; M33 = third.Z; M34 = third.W;
            M41 = fourth.X; M42 = fourth.Y; M43 = fourth.Z; M44 = fourth.W;
        }

        /// <summary>
        /// Creates a new Matrix4 structure out of the given 3x3 matrix.
        /// </summary>
        public Matrix4(Matrix3 matrix3)
        {
            M11 = matrix3.M11; M12 = matrix3.M12; M13 = matrix3.M13; M14 = 0;
            M21 = matrix3.M21; M22 = matrix3.M22; M23 = matrix3.M23; M24 = 0;
            M31 = matrix3.M31; M32 = matrix3.M32; M33 = matrix3.M33; M34 = 0;
            M41 = 0f; M42 = 0f; M43 = 0f; M44 = 1f;
        }

        ///// <summary>
        ///// Converts this matrix to a directx matrix
        ///// </summary>
        //internal SlimDX.Matrix ToDirectXMatrix()
        //{
        //    return new SlimDX.Matrix()
        //    {
        //        M11 = M11,
        //        M12 = M12,
        //        M13 = M13,
        //        M14 = M14,
        //        M21 = M21,
        //        M22 = M22,
        //        M23 = M23,
        //        M24 = M24,
        //        M31 = M31,
        //        M32 = M32,
        //        M33 = M33,
        //        M34 = M34,
        //        M41 = M41,
        //        M42 = M42,
        //        M43 = M43,
        //        M44 = M44
        //    };
        //}

        /// <summary>
        /// Converts this structure to a string
        /// </summary>
        public override string ToString()
        {
            return 
                "{" +
                M11.ToString("N2") + ", " + M12.ToString("N2") + ", " + M13.ToString("N2") + ", " + M14.ToString("N2") +
                M21.ToString("N2") + ", " + M22.ToString("N2") + ", " + M23.ToString("N2") + ", " + M24.ToString("N2") +
                M31.ToString("N2") + ", " + M32.ToString("N2") + ", " + M33.ToString("N2") + ", " + M34.ToString("N2") +
                M41.ToString("N2") + ", " + M42.ToString("N2") + ", " + M43.ToString("N2") + ", " + M44.ToString("N2") +
                "}";
        }

        /// <summary>
        /// Retrieves the hashcode for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return
                M11.GetHashCode() ^ M12.GetHashCode() ^ M31.GetHashCode() ^ M41.GetHashCode() ^
                M21.GetHashCode() ^ M22.GetHashCode() ^ M32.GetHashCode() ^ M42.GetHashCode() ^
                M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode() ^ M43.GetHashCode() ^
                M41.GetHashCode() ^ M42.GetHashCode() ^ M43.GetHashCode() ^ M44.GetHashCode();
        }

        /// <summary>
        /// Are both objects the same?
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Matrix4)
            {
                Matrix4 m = (Matrix4)obj;
                return
                    (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
                    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
                    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
                    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
            }
            return false;
        }

        /// <summary>
        /// Adds a matrix and a scalar.
        /// </summary>
        public void Add(float s)
        {
            M11 += s;
            M12 += s;
            M13 += s;
            M14 += s;

            M21 += s;
            M22 += s;
            M23 += s;
            M24 += s;

            M31 += s;
            M32 += s;
            M33 += s;
            M34 += s;

            M41 += s;
            M42 += s;
            M43 += s;
            M44 += s;
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        public void Add(Matrix4 other)
        {
            M11 += other.M11;
            M12 += other.M12;
            M13 += other.M13;
            M14 += other.M14;

            M21 += other.M21;
            M22 += other.M22;
            M23 += other.M23;
            M24 += other.M24;

            M31 += other.M31;
            M32 += other.M32;
            M33 += other.M33;
            M34 += other.M34;

            M41 += other.M41;
            M42 += other.M42;
            M43 += other.M43;
            M44 += other.M44;
        }

        /// <summary>
        /// Subtracts a scalar from a matrix.
        /// </summary>
        public void Subtract(float s)
        {
            M11 -= s;
            M12 -= s;
            M13 -= s;
            M14 -= s;

            M21 -= s;
            M22 -= s;
            M23 -= s;
            M24 -= s;

            M31 -= s;
            M32 -= s;
            M33 -= s;
            M34 -= s;

            M41 -= s;
            M42 -= s;
            M43 -= s;
            M44 -= s;
        }

        /// <summary>
        /// Subtracts a matrix from this matrix.
        /// </summary>
        public void Subtract(Matrix4 other)
        {
            M11 -= other.M11;
            M12 -= other.M12;
            M13 -= other.M13;
            M14 -= other.M14;

            M21 -= other.M21;
            M22 -= other.M22;
            M23 -= other.M23;
            M24 -= other.M24;

            M31 -= other.M31;
            M32 -= other.M32;
            M33 -= other.M33;
            M34 -= other.M34;

            M41 -= other.M41;
            M42 -= other.M42;
            M43 -= other.M43;
            M44 -= other.M44;
        }

        /// <summary>
        /// Multiplies the matrix with another
        /// </summary>
        /// <param name="other"></param>
        public void Multiply(Matrix4 other)
        {
            Matrix4 result = Matrix4.Empty;

            result.M11 = M11 * other.M11 + M12 * other.M21 + M13 * other.M31 + M14 * other.M41;  //M11
            result.M12 = M11 * other.M12 + M12 * other.M22 + M13 * other.M32 + M14 * other.M42;  //M12
            result.M13 = M11 * other.M13 + M12 * other.M23 + M13 * other.M33 + M14 * other.M43;  //M13
            result.M14 = M11 * other.M14 + M12 * other.M24 + M13 * other.M34 + M14 * other.M44;  //M14

            result.M21 = M21 * other.M11 + M22 * other.M21 + M23 * other.M31 + M24 * other.M41;  //M21
            result.M22 = M21 * other.M12 + M22 * other.M22 + M23 * other.M32 + M24 * other.M42;  //M22
            result.M23 = M21 * other.M13 + M22 * other.M23 + M23 * other.M33 + M24 * other.M43;  //M23
            result.M24 = M21 * other.M14 + M22 * other.M24 + M23 * other.M34 + M24 * other.M44;  //M24

            result.M31 = M31 * other.M11 + M32 * other.M21 + M33 * other.M31 + M34 * other.M41;  //M31
            result.M32 = M31 * other.M12 + M32 * other.M22 + M33 * other.M32 + M34 * other.M42;  //M32
            result.M33 = M31 * other.M13 + M32 * other.M23 + M33 * other.M33 + M34 * other.M43;  //M33
            result.M34 = M31 * other.M14 + M32 * other.M24 + M33 * other.M34 + M34 * other.M44;  //M34

            result.M41 = M41 * other.M11 + M42 * other.M21 + M43 * other.M31 + M44 * other.M41;  //M41
            result.M42 = M41 * other.M12 + M42 * other.M22 + M43 * other.M32 + M44 * other.M42;  //M42
            result.M43 = M41 * other.M13 + M42 * other.M23 + M43 * other.M33 + M44 * other.M43;  //M43
            result.M44 = M41 * other.M14 + M42 * other.M24 + M43 * other.M34 + M44 * other.M44;  //M44

            this.M11 = result.M11;
            this.M12 = result.M12;
            this.M13 = result.M13;
            this.M14 = result.M14;

            this.M21 = result.M21;
            this.M22 = result.M22;
            this.M23 = result.M23;
            this.M24 = result.M24;

            this.M31 = result.M31;
            this.M32 = result.M32;
            this.M33 = result.M33;
            this.M34 = result.M34;

            this.M41 = result.M41;
            this.M42 = result.M42;
            this.M43 = result.M43;
            this.M44 = result.M44;
        }

        /// <summary>
        /// Retrieves a matrix for translation
        /// </summary>
        public void Translate(float transX, float transY, float transZ)
        {
            Matrix4 other = Matrix4.Translation(transX, transY, transZ);
            Matrix4 result = Matrix4.Empty;

            result.M11 = M11 + M14 * other.M41;  //M11
            result.M12 = M12 + M14 * other.M42;  //M12
            result.M13 = M13 + M14 * other.M43;  //M13
            //M14 -> no change
         
            result.M21 = M21 + M24 * other.M41;  //M21
            result.M22 = M22 + M24 * other.M42;  //M22
            result.M23 = M23 + M24 * other.M43;  //M23
            //M24 -> no change

            result.M31 = M31 + M34 * other.M41;  //M31
            result.M32 = M32 + M34 * other.M42;  //M32
            result.M33 = M33 + M34 * other.M43;  //M33
            //M34 -> no change

            result.M41 = M41 + M44 * other.M41;  //M41
            result.M42 = M42 + M44 * other.M42;  //M42
            result.M43 = M43 + M44 * other.M43;  //M43
            //M44 -> no change

            this.M11 = result.M11;
            this.M12 = result.M12;
            this.M13 = result.M13;

            this.M21 = result.M21;
            this.M22 = result.M22;
            this.M23 = result.M23;

            this.M31 = result.M31;
            this.M32 = result.M32;
            this.M33 = result.M33;

            this.M41 = result.M41;
            this.M42 = result.M42;
            this.M43 = result.M43;
        }

        /// <summary>
        /// Retrieves a matrix for translation
        /// </summary>
        public void Translate(Vector3 transVector)
        {
            this.Translate(transVector.X, transVector.Y, transVector.Z);
        }

        /// <summary>
        /// Scales the matrix
        /// </summary>
        public void Scale(float xScale, float yScale, float zScale)
        {
            Matrix4 other = Matrix4.Scaling(xScale, yScale, zScale);
            Matrix4 result = Matrix4.Empty;

            result.M11 = M11 * other.M11;  //M11
            result.M12 = M12 * other.M22;  //M12
            result.M13 = M13 * other.M33;  //M13
            //M14 -> no change

            result.M21 = M21 * other.M11;  //M21
            result.M22 = M22 * other.M22;  //M22
            result.M23 = M23 * other.M33;  //M23
            //M24 -> no change

            result.M31 = M31 * other.M11;  //M31
            result.M32 = M32 * other.M22;  //M32
            result.M33 = M33 * other.M33;  //M33
            //M34 -> no change

            result.M41 = M41 * other.M11;  //M41
            result.M42 = M42 * other.M22;  //M42
            result.M43 = M43 * other.M33;  //M43
            //M44 -> no change

            this.M11 = result.M11;
            this.M12 = result.M12;
            this.M13 = result.M13;

            this.M21 = result.M21;
            this.M22 = result.M22;
            this.M23 = result.M23;

            this.M31 = result.M31;
            this.M32 = result.M32;
            this.M33 = result.M33;

            this.M41 = result.M41;
            this.M42 = result.M42;
            this.M43 = result.M43;
        }

        /// <summary>
        /// Inverts this matrix
        /// </summary>
        public void Invert()
        {
            float a0 = (this.M11 * this.M22) - (this.M12 * this.M21);
            float a1 = (this.M11 * this.M23) - (this.M13 * this.M21);
            float a2 = (this.M14 * this.M21) - (this.M11 * this.M24);
            float a3 = (this.M12 * this.M23) - (this.M13 * this.M22);
            float a4 = (this.M14 * this.M22) - (this.M12 * this.M24);
            float a5 = (this.M13 * this.M24) - (this.M14 * this.M23);

            float b0 = (this.M31 * this.M42) - (this.M32 * this.M41);
            float b1 = (this.M31 * this.M43) - (this.M33 * this.M41);
            float b2 = (this.M34 * this.M41) - (this.M31 * this.M44);
            float b3 = (this.M32 * this.M43) - (this.M33 * this.M42);
            float b4 = (this.M34 * this.M42) - (this.M32 * this.M44);
            float b5 = (this.M33 * this.M44) - (this.M34 * this.M43);

            float d11 = this.M22 * b5 + this.M23 * b4 + this.M24 * b3;
            float d12 = this.M21 * b5 + this.M23 * b2 + this.M24 * b1;
            float d13 = this.M21 * -b4 + this.M22 * b2 + this.M24 * b0;
            float d14 = this.M21 * b3 + this.M22 * -b1 + this.M23 * b0;

            float d21 = this.M12 * b5 + this.M13 * b4 + this.M14 * b3;
            float d22 = this.M11 * b5 + this.M13 * b2 + this.M14 * b1;
            float d23 = this.M11 * -b4 + this.M12 * b2 + this.M14 * b0;
            float d24 = this.M11 * b3 + this.M12 * -b1 + this.M13 * b0;

            float d31 = this.M42 * a5 + this.M43 * a4 + this.M44 * a3;
            float d32 = this.M41 * a5 + this.M43 * a2 + this.M44 * a1;
            float d33 = this.M41 * -a4 + this.M42 * a2 + this.M44 * a0;
            float d34 = this.M41 * a3 + this.M42 * -a1 + this.M43 * a0;

            float d41 = this.M32 * a5 + this.M33 * a4 + this.M34 * a3;
            float d42 = this.M31 * a5 + this.M33 * a2 + this.M34 * a1;
            float d43 = this.M31 * -a4 + this.M32 * a2 + this.M34 * a0;
            float d44 = this.M31 * a3 + this.M32 * -a1 + this.M33 * a0;

            float det = this.M11 * d11 - this.M12 * d12 + this.M13 * d13 - this.M14 * d14;

            if (Math.Abs(det) <= 0.000001f)
            {
                this.M11 = 0f; this.M12 = 0f; this.M13 = 0f; this.M14 = 0f;
                this.M21 = 0f; this.M22 = 0f; this.M23 = 0f; this.M24 = 0f;
                this.M31 = 0f; this.M32 = 0f; this.M33 = 0f; this.M34 = 0f;
                this.M41 = 0f; this.M42 = 0f; this.M43 = 0f; this.M44 = 0f;
            }
            else
            {
                this.M11 = +d11 * det; this.M12 = -d21 * det; this.M13 = +d31 * det; this.M14 = -d41 * det;
                this.M21 = -d12 * det; this.M22 = +d22 * det; this.M23 = -d32 * det; this.M24 = +d42 * det;
                this.M31 = +d13 * det; this.M32 = -d23 * det; this.M33 = +d33 * det; this.M34 = -d43 * det;
                this.M41 = -d14 * det; this.M42 = +d24 * det; this.M43 = -d34 * det; this.M44 = +d44 * det;
            }
        }

        /// <summary>
        /// Scales the matrix
        /// </summary>
        public void Scale(Vector3 scaleVector)
        {
            this.Scale(scaleVector.X, scaleVector.Y, scaleVector.Z);
        }

        /// <summary>
        /// Scales the matrix
        /// </summary>
        public void Scale(float scaleFactor)
        {
            this.Scale(scaleFactor, scaleFactor, scaleFactor);
        }

        /// <summary>
        /// Rotates the matrix using given horizontal and vertical rotations.
        /// </summary>
        /// <param name="rotation">Vector containing horizontal </param>
        public void RotateHV(Vector2 rotation)
        {
            RotateHV(rotation.X, rotation.Y);
        }

        /// <summary>
        /// Rotates the matrix using given horizontal and vertical rotations.
        /// </summary>
        /// <param name="hRotation"></param>
        /// <param name="vRotation"></param>
        public void RotateHV(float hRotation, float vRotation)
        {
            Matrix4 transformMatrix = RotationZ(vRotation) * RotationY(hRotation);
            Multiply(transformMatrix);
        }

        /// <summary>
        /// Rotates the matrix around x-axis
        /// </summary>
        public void RotateX(float angle)
        {
            Matrix4 other = Matrix4.RotationX(angle);
            Matrix4 result = Matrix4.Empty;

            //M11 -> no change
            result.M12 = M12 * other.M22 + M13 * other.M32;  //M12
            result.M13 = M12 * other.M23 + M13 * other.M33;  //M13
            //M14 -> no change

            //M21 -> no change
            result.M22 = M22 * other.M22 + M23 * other.M32;  //M22
            result.M23 = M22 * other.M23 + M23 * other.M33;  //M23
            //M24 -> no change

            //M31 -> no change
            result.M32 = M32 * other.M22 + M33 * other.M32;  //M32
            result.M33 = M32 * other.M23 + M33 * other.M33;  //M33
            //M34 -> no change

            //M41 -> no change
            result.M42 = M42 * other.M22 + M43 * other.M32;  //M42
            result.M43 = M42 * other.M23 + M43 * other.M33;  //M43
            //M44 -> no change

            this.M12 = result.M12;
            this.M13 = result.M13;

            this.M22 = result.M22;
            this.M23 = result.M23;

            this.M32 = result.M32;
            this.M33 = result.M33;

            this.M42 = result.M42;
            this.M43 = result.M43;
        }

        /// <summary>
        /// Rotates the matrix around y-axis
        /// </summary>
        public void RotateY(float angle)
        {
            Matrix4 other = Matrix4.RotationY(angle);
            Matrix4 result = Matrix4.Empty;

            result.M11 = M11 * other.M11 + M13 * other.M31;  //M11
            //M12 -> no change
            result.M13 = M11 * other.M13 + M13 * other.M33;  //M13
            //M14 -> no change

            result.M21 = M21 * other.M11 + M23 * other.M31;  //M21
            //M22 -> no change
            result.M23 = M21 * other.M13 + M23 * other.M33;  //M23
            //M24 -> no change

            result.M31 = M31 * other.M11 + M33 * other.M31;  //M31
            //M32 -> no change
            result.M33 = M31 * other.M13 + M33 * other.M33;  //M33
            //M34 -> no change

            result.M41 = M41 * other.M11 + M43 * other.M31;  //M41
            //M42 -> no change
            result.M43 = M41 * other.M13 + M43 * other.M33;  //M43
            //M44 -> no change

            this.M11 = result.M11;
            this.M13 = result.M13;

            this.M21 = result.M21;
            this.M23 = result.M23;

            this.M31 = result.M31;
            this.M33 = result.M33;

            this.M41 = result.M41;
            this.M43 = result.M43;
        }

        /// <summary>
        /// Rotates the matrix around z axis
        /// </summary>
        public void RotateZ(float angle)
        {
            Matrix4 other = Matrix4.RotationZ(angle);
            Matrix4 result = Matrix4.Empty;

            result.M11 = M11 * other.M11 + M12 * other.M21;  //M11
            result.M12 = M11 * other.M12 + M12 * other.M22;  //M12
            //M13 -> no change
            //M14 -> no change

            result.M21 = M21 * other.M11 + M22 * other.M21;  //M21
            result.M22 = M21 * other.M12 + M22 * other.M22;  //M22
            //M23 -> no change
            //M24 -> no change

            result.M31 = M31 * other.M11 + M32 * other.M21;  //M31
            result.M32 = M31 * other.M12 + M32 * other.M22;  //M32
            //M33 -> no change
            //M34 -> no change

            result.M41 = M41 * other.M11 + M42 * other.M21;  //M41
            result.M42 = M41 * other.M12 + M42 * other.M22;  //M42
            //M43 -> no change
            //M44 -> no change

            this.M11 = result.M11;
            this.M12 = result.M12;

            this.M21 = result.M21;
            this.M22 = result.M22;

            this.M31 = result.M31;
            this.M32 = result.M32;

            this.M41 = result.M41;
            this.M42 = result.M42;
        }

        /// <summary>
        /// Converts the given matrix to a native one
        /// </summary>
        public static unsafe void ToNative4x3(Matrix4 matrix, float* nativeMatrix)
        {
            //first row
            nativeMatrix[0] = matrix.M11;
            nativeMatrix[1] = matrix.M12;
            nativeMatrix[2] = matrix.M13;
            nativeMatrix[3] = matrix.M14;

            //second row
            nativeMatrix[4] = matrix.M21;
            nativeMatrix[5] = matrix.M22;
            nativeMatrix[6] = matrix.M23;
            nativeMatrix[7] = matrix.M24;

            //third row
            nativeMatrix[8] = matrix.M31;
            nativeMatrix[9] = matrix.M32;
            nativeMatrix[10] = matrix.M33;
            nativeMatrix[11] = matrix.M34;

            //Skip last row due to 4x3 target
        }

        /// <summary>
        /// Converts the given native matrix to a managed version
        /// </summary>
        public static unsafe Matrix4 FromNative4x3(float* nativeMatrix)
        {
            Matrix4 result = Matrix4.Identity;

            //first row
            result.M11 = nativeMatrix[0];
            result.M12 = nativeMatrix[1];
            result.M13 = nativeMatrix[2];
            result.M14 = nativeMatrix[3];

            //second row
            result.M21 = nativeMatrix[4];
            result.M22 = nativeMatrix[5];
            result.M23 = nativeMatrix[6];
            result.M24 = nativeMatrix[7];

            //third row
            result.M31 = nativeMatrix[8];
            result.M32 = nativeMatrix[9];
            result.M33 = nativeMatrix[10];
            result.M34 = nativeMatrix[11];

            //Skip last row due to 4x3 source

            return result;
        }

        ///// <summary>
        ///// Gets a matrix instance from the given directx matrix
        ///// </summary>
        //internal static Matrix4 FromDirectXMatrix(SlimDX.Matrix input)
        //{
        //    return new Matrix4(
        //        input.M11, input.M12, input.M13, input.M14,
        //        input.M21, input.M22, input.M23, input.M24,
        //        input.M31, input.M32, input.M33, input.M34,
        //        input.M41, input.M42, input.M43, input.M44);
        //}

        /// <summary>
        /// Gets a projection matrix for a left handed system
        /// </summary>
        public static Matrix4 PerspectiveLH(float width, float height, float zNear, float zFar)
        {
            return new Matrix4(
                width, 0f, 0f, 0f, 
                0f, height, 0f, 0f, 
                0f, 0f, zFar / (zFar - zNear), 1f, 
                0f, 0f, -((zNear * zFar) / (zFar - zNear)), 0f);
        }

        /// <summary>
        /// Gets a projection matrix for a left handed system
        /// </summary>
        public static Matrix4 PerspectiveFovLH(float fov, float aspectRatio, float zNear, float zFar)
        {
            float height = (float)(1.0 / Math.Tan(fov * 0.5));
            float width = height / aspectRatio;
            return PerspectiveLH(width, height, zNear, zFar);
        }

        /// <summary>
        /// Gets a projection matrix for a right handed system
        /// </summary>
        public static Matrix4 PerspectiveFovRH(float fov, float aspectRatio, float zNear, float zFar)
        {
            float height = (float)(1.0 / Math.Tan(fov * 0.5));
            float width = height / aspectRatio;
            return PerspectiveRH(width, height, zNear, zFar);
        }

        /// <summary>
        /// Gets a projection matrix for a right handed system
        /// </summary>
        public static Matrix4 PerspectiveRH(float width, float height, float zNear, float zFar)
        {
            return new Matrix4(
                width, 0f, 0f, 0f, 
                0f, height, 0f, 0f, 
                0f, 0f, zFar / (zNear - zFar), -1f, 
                0f, 0f, (zNear * zFar) / (zNear - zFar), 0f);
        }

        /// <summary>
        /// Gets a view matrix for a right handed system
        /// </summary>
        public static Matrix4 LookAtRH(Vector3 position, Vector3 target, Vector3 up)
        {
            Vector3 rhs = position - target;
            rhs.Normalize();
            Vector3 vector2 = Vector3.Cross(up, rhs);
            vector2.Normalize();
            Vector3 lhs = Vector3.Cross(rhs, vector2);
            return new Matrix4(
                vector2.X, lhs.X, rhs.X, 0f, 
                vector2.Y, lhs.Y, rhs.Y, 0f, 
                vector2.Z, lhs.Z, rhs.Z, 0f, 
                -Vector3.Dot(vector2, position), -Vector3.Dot(lhs, position), -Vector3.Dot(rhs, position), 1f);
        }

        /// <summary>
        /// Gets a view matrix for a left handed system
        /// </summary>
        public static Matrix4 LookAtLH(Vector3 position, Vector3 target, Vector3 up)
        {
            Vector3 rhs = position - target;
            rhs.Normalize();
            Vector3 vector2 = Vector3.Cross(up, rhs);
            vector2.Normalize();
            Vector3 lhs = Vector3.Cross(rhs, vector2);
            return new Matrix4(
                -vector2.X, lhs.X, -rhs.X, 0f,
                -vector2.Y, lhs.Y, -rhs.Y, 0f,
                -vector2.Z, lhs.Z, -rhs.Z, 0f,
                Vector3.Dot(vector2, position), -Vector3.Dot(lhs, position), Vector3.Dot(rhs, position), 1f);
        }

        /// <summary>
        /// Retrieves a matrix for scaling
        /// </summary>
        public static Matrix4 Scaling(float scaleX, float scaleY, float scaleZ)
        {
            Matrix4 result = Matrix4.Identity;
            result.M11 = scaleX;
            result.M22 = scaleY;
            result.M33 = scaleZ;
            return result;
        }

        /// <summary>
        /// Retrieves a matrix for scaling
        /// </summary>
        public static Matrix4 Scaling(Vector3 scaleVector)
        {
            return Matrix4.Scaling(scaleVector.X, scaleVector.Y, scaleVector.Z);
        }

        /// <summary>
        /// Retrieves a matrix for scaling
        /// </summary>
        public static Matrix4 Scaling(float scaleFactor)
        {
            return Matrix4.Scaling(scaleFactor, scaleFactor, scaleFactor);
        }

        /// <summary>
        /// Retrieves a matrix for translation
        /// </summary>
        public static Matrix4 Translation(float transX, float transY, float transZ)
        {
            Matrix4 result = Matrix4.Identity;
            result.M41 = transX;
            result.M42 = transY;
            result.M43 = transZ;
            return result;
        }

        /// <summary>
        /// Retrieves a matrix for translation
        /// </summary>
        public static Matrix4 Translation(Vector3 transVector)
        {
            return Matrix4.Translation(transVector.X, transVector.Y, transVector.Z);
        }

        /// <summary>
        /// Creates a rotation matrix from a quaternion.
        /// </summary>
        /// <param name="quaternion">The quaternion to use to build the matrix.</param>
        /// <param name="result">The created rotation matrix.</param>
        public static Matrix4 RotationQuaternion(Quaternion quaternion)
        {
            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;

            Matrix4 result = new Matrix4();
            result.M11 = 1.0f - (2.0f * (yy + zz));
            result.M12 = 2.0f * (xy + zw);
            result.M13 = 2.0f * (zx - yw);
            result.M21 = 2.0f * (xy - zw);
            result.M22 = 1.0f - (2.0f * (zz + xx));
            result.M23 = 2.0f * (yz + xw);
            result.M31 = 2.0f * (zx + yw);
            result.M32 = 2.0f * (yz - xw);
            result.M33 = 1.0f - (2.0f * (yy + xx));
            result.M44 = 1f;
            return result;
        }

        /// <summary>
        /// Creates a rotation matrix with a specified yaw, pitch, and roll.
        /// </summary>
        /// <param name="yaw">Yaw around the y-axis, in radians.</param>
        /// <param name="pitch">Pitch around the x-axis, in radians.</param>
        /// <param name="roll">Roll around the z-axis, in radians.</param>
        public static Matrix4 RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion quaternion = Quaternion.RotationYawPitchRoll(yaw, pitch, roll);
            return RotationQuaternion(quaternion);
        }
        
        /// <summary>
        /// Gets a matrix for given horizontal and vertical rotation.
        /// </summary>
        /// <param name="hRotation">Horizontal rotation angle.</param>
        /// <param name="vRotation">Vertical rotation angle.</param>
        public static Matrix4 RotationHV(float hRotation, float vRotation)
        {
            return Matrix4.RotationZ(vRotation) * Matrix4.RotationY(hRotation);
        }

        /// <summary>
        /// Gets a matrix for given horizontal and vertical rotation.
        /// </summary>
        /// <param name="rotation">Vector containing horizontal and vertical rotation angles.</param>
        public static Matrix4 RotationHV(Vector2 rotation)
        {
            return RotationHV(rotation.X, rotation.Y);
        }

        /// <summary>
        /// Retrieves a matrix for rotation around x-axis
        /// </summary>
        public static Matrix4 RotationX(float angle)
        {
            float cos = (float)(Math.Cos(angle));
            float sin = (float)(Math.Sin(angle));

            Matrix4 result = Matrix4.Identity;
            result.M22 = cos;
            result.M23 = sin;
            result.M32 = -sin;
            result.M33 = cos;
            return result;
        }

        /// <summary>
        /// Retrieves a matrix for rotation around y-axis
        /// </summary>
        public static Matrix4 RotationY(float angle)
        {
            float cos = (float)(Math.Cos(angle));
            float sin = (float)(Math.Sin(angle));

            Matrix4 result = Matrix4.Identity;
            result.M11 = cos;
            result.M13 = -sin;
            result.M31 = sin;
            result.M33 = cos;
            return result;
        }

        /// <summary>
        /// Retrieves a matrix for rotation around z-axis
        /// </summary>
        public static Matrix4 RotationZ(float angle)
        {
            float cos = (float)(Math.Cos(angle));
            float sin = (float)(Math.Sin(angle));

            Matrix4 result = Matrix4.Identity;
            result.M11 = cos;
            result.M12 = sin;
            result.M21 = -sin;
            result.M22 = cos;
            return result;
        }

        /// <summary>
        /// Retrieves the trace of the matrix
        /// </summary>
        public float Trace
        {
            get { return M11 + M22 + M33 + M44; }
        }

        /// <summary>
        /// == operator
        /// </summary>
        public static bool operator ==(Matrix4 a, Matrix4 b)
        {
            return
                (a.M11 == b.M11) && (a.M12 == b.M12) && (a.M13 == b.M13) && (a.M14 == b.M14) &&
                (a.M21 == b.M21) && (a.M22 == b.M22) && (a.M23 == b.M23) && (a.M24 == b.M24) &&
                (a.M31 == b.M31) && (a.M32 == b.M32) && (a.M33 == b.M33) && (a.M34 == b.M34) &&
                (a.M41 == b.M41) && (a.M42 == b.M42) && (a.M43 == b.M43) && (a.M44 == b.M44);

        }

        /// <summary>
        /// != operator
        /// </summary>
        public static bool operator !=(Matrix4 a, Matrix4 b)
        {
            return 
                !(
                (a.M11 == b.M11) && (a.M12 == b.M12) && (a.M13 == b.M13) && (a.M14 == b.M14) &&
                (a.M21 == b.M21) && (a.M22 == b.M22) && (a.M23 == b.M23) && (a.M24 == b.M24) &&
                (a.M31 == b.M31) && (a.M32 == b.M32) && (a.M33 == b.M33) && (a.M34 == b.M34) &&
                (a.M41 == b.M41) && (a.M42 == b.M42) && (a.M43 == b.M43) && (a.M44 == b.M44)
                );
        }

        /// <summary>
        /// + operator (Adding another matrix
        /// </summary>
        public static Matrix4 operator +(Matrix4 a, Matrix4 b)
        {
            a.Add(b);
            return a;
        }

        /// <summary>
        /// + operator (Adding scalar)
        /// </summary>
        public static Matrix4 operator +(Matrix4 a, float s)
        {
            a.Add(s);
            return a;
        }

        /// <summary>
        /// + operator (Adding scalar)
        /// </summary>
        public static Matrix4 operator +(float s, Matrix4 a)
        {
            a.Add(s);
            return a;
        }

        /// <summary>
        /// - operator (Subracting matrix)
        /// </summary>
        public static Matrix4 operator -(Matrix4 a, Matrix4 b)
        {
            a.Subtract(b);
            return a;
        }

        /// <summary>
        /// - operator (Subtracting scalar)
        /// </summary>
        public static Matrix4 operator -(Matrix4 a, float s)
        {
            a.Subtract(s);
            return a;
        }

        /// <summary>
        /// * operator
        /// </summary>
        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            a.Multiply(b);
            return a;
        }

        /// <summary>
        /// * operator (Transforming vector)
        /// </summary>
        public static Vector3 operator *(Matrix4 matrix, Vector3 vector)
        {
            vector.Transform(matrix);
            return vector;
        }
    }
}
