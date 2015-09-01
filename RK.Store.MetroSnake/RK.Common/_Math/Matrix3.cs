
namespace RK.Common
{
    public struct Matrix3
    {
        public static Matrix3 Identity = new Matrix3(
            1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 1f);

        public float M11;
        public float M12;
        public float M13;

        public float M21;
        public float M22;
        public float M23;

        public float M31;
        public float M32;
        public float M33;

        /// <summary>
        /// Creates a new matrix with the given values
        /// </summary>
        public Matrix3(
            float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;

            M21 = m21;
            M22 = m22;
            M23 = m23;

            M31 = m31;
            M32 = m32;
            M33 = m33;
        }

        /// <summary>
        /// Converts the given matrix to a native one
        /// </summary>
        public static unsafe void ToNative3x3(Matrix3 matrix, float* nativeMatrix)
        {
            //first row
            nativeMatrix[0] = matrix.M11;
            nativeMatrix[1] = matrix.M12;
            nativeMatrix[2] = matrix.M13;

            //second row
            nativeMatrix[3] = matrix.M21;
            nativeMatrix[4] = matrix.M22;
            nativeMatrix[5] = matrix.M23;

            //third row
            nativeMatrix[6] = matrix.M31;
            nativeMatrix[7] = matrix.M32;
            nativeMatrix[8] = matrix.M33;
        }

        /// <summary>
        /// Converts the given native matrix to a managed version
        /// </summary>
        public static unsafe Matrix3 FromNative3x3(float* nativeMatrix)
        {
            Matrix3 result = Matrix3.Identity;

            //first row
            result.M11 = nativeMatrix[0];
            result.M12 = nativeMatrix[1];
            result.M13 = nativeMatrix[2];

            //second row
            result.M21 = nativeMatrix[3];
            result.M22 = nativeMatrix[4];
            result.M23 = nativeMatrix[5];

            //third row
            result.M31 = nativeMatrix[6];
            result.M32 = nativeMatrix[7];
            result.M33 = nativeMatrix[8];

            return result;
        }
    }
}
