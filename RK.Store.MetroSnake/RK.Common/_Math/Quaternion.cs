using System;

namespace RK.Common
{
    public struct Quaternion
    {
        public const float TOLERANCE = 0.00001f;

        public static readonly Quaternion Empty = new Quaternion();

        public float X;
        public float Y;
        public float Z;
        public float W;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        ///// <summary>
        ///// Convers this quaternion to a directx quaternion
        ///// </summary>
        //internal SlimDX.Quaternion ToDirectXQuaternion()
        //{
        //    return new SlimDX.Quaternion(X, Y, Z, W);
        //}

        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value.
        /// </summary>
        /// <param name="yaw">The yaw of rotation.</param>
        /// <param name="pitch">The pitch of rotation.</param>
        /// <param name="roll">The roll of rotation.</param>
        public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            float halfRoll = roll * 0.5f;
            float halfPitch = pitch * 0.5f;
            float halfYaw = yaw * 0.5f;

            float sinRoll = (float)Math.Sin(halfRoll);
            float cosRoll = (float)Math.Cos(halfRoll);
            float sinPitch = (float)Math.Sin(halfPitch);
            float cosPitch = (float)Math.Cos(halfPitch);
            float sinYaw = (float)Math.Sin(halfYaw);
            float cosYaw = (float)Math.Cos(halfYaw);

            Quaternion result = new Quaternion();
            result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
            return result;
        }

        /// <summary>
        /// Conjugates the quaternion.
        /// </summary>
        public void Conjugate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Gets the angle of the quaternion.
        /// </summary>
        public float Angle
        {
            get
            {
                float length = (X * X) + (Y * Y) + (Z * Z);
                if (length < TOLERANCE) { return 0.0f; }

                return (float)(2.0 * Math.Acos(W));
            }
        }

        /// <summary>
        /// Gets the axis of the quaternion.
        /// </summary>
        /// <value>The axis of the quaternion.</value>
        public Vector3 Axis
        {
            get
            {
                float length = (X * X) + (Y * Y) + (Z * Z);
                if (length < TOLERANCE) { return Vector3.AxisX; }

                float inv = 1.0f / length;
                return new Vector3(X * inv, Y * inv, Z * inv);
            }
        }

        /// <summary>
        /// Gets the length of the quaternion.
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
            }
        }
    }
}
