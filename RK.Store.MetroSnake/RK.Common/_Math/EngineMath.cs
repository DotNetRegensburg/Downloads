using System;

namespace RK.Common
{
    public static class EngineMath
    {
        public const float RAD_45DEG = (float)Math.PI * 0.25f;
        public const float RAD_90DEG = (float)Math.PI * 0.5f;
        public const float RAD_135DEG = (float)Math.PI * 0.75f;
        public const float RAD_180DEG = (float)Math.PI;
        public const float RAD_225DEG = (float)Math.PI * 1.15f;
        public const float RAD_270DEG = (float)Math.PI * 1.5f;
        public const float RAD_315DEG = (float)Math.PI * 1.75f;
        public const float RAD_360DEG = (float)Math.PI * 2f;

        /// <summary>
        /// Converts the given angle value to an absolute value (e. g. -10° to 350°).
        /// </summary>
        /// <param name="angle">The angle to convert.</param>
        public static float GetAbsoluteAngleRadian(float angle)
        {
            float result = angle;

            result = result % ((float)Math.PI * 2f);
            if (result < 0) { result = ((float)Math.PI * 2f) + result; }

            return result;
        }

        /// <summary>
        /// Converts the given angle value to an absolute value (e. g. -10° to 350°).
        /// </summary>
        /// <param name="angle">The angle to convert.</param>
        public static float GetAboluteAngleDegree(float angle)
        {
            float result = angle;

            result = result % 360f;
            if (result < 0) { result = 360f + result; }

            return result;
        }

        /// <summary>
        /// Converts the given degree value to radian.
        /// </summary>
        /// <param name="degreeValue">A angle in degree.</param>
        /// <returns>The radian value of the angle.</returns>
        public static float DegreeToRadian(float degreeValue)
        {
            return (degreeValue / 360f) * RAD_360DEG;
        }

        /// <summary>
        /// Converts the given degree value to radian.
        /// </summary>
        /// <param name="degreeValue">A angle in degree.</param>
        /// <returns>The radian value of the angle.</returns>
        public static float DegreeToRadian(int degreeValue)
        {
            return (degreeValue / 360f) * RAD_360DEG;
        }

        /// <summary>
        /// Converts the given radian vlaue to degree.
        /// </summary>
        /// <param name="radianValue">A angle in radian.</param>
        /// <returns>The degree value of the angle.</returns>
        public static float RadianToDegree(float radianValue)
        {
            return (radianValue / RAD_360DEG) * 360f;
        }
    }
}
