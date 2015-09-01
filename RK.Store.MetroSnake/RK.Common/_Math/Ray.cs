using System;
using System.Runtime.InteropServices;

namespace RK.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        /// <summary>
        /// Creates a new ray
        /// </summary>
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        /// <summary>
        /// Does this ray intersect the given box?
        /// </summary>
        /// <param name="box">Box for testing.</param>
        /// <param name="distance">Distance to intersection point. If no intersection, then float.NaN.</param>
        public bool Intersects(AxisAlignedBox box, out float distance)
        {
            distance = float.NaN;

            float d = 0f;
            float maxValue = float.MaxValue;

            //Text x component
            if (Math.Abs(Direction.X) < 0.0000001f)
            {
                if ((Origin.X < box.Minimum.X) || (Origin.X > box.Maximum.X))
                {
                    return false;
                }
            }
            else
            {
                float inv = (float)(1.0 / ((double)Direction.X));
                float min = (box.Minimum.X - Origin.X) * inv;
                float max = (box.Maximum.X - Origin.X) * inv;
                if (min > max)
                {
                    float temp = min;
                    min = max;
                    max = temp;
                }
                d = Math.Max(min, 0f);
                maxValue = Math.Min(max, float.MaxValue);
                if (d > maxValue)
                {
                    return false;
                }
            }

            //Test y component
            if (Math.Abs(Direction.Y) < 0.0000001f)
            {
                if ((Origin.Y < box.Minimum.Y) || (Origin.Y > box.Maximum.Y))
                {
                    return false;
                }
            }
            else
            {
                float inv = (float)(1.0 / ((double)Direction.Y));
                float min = (box.Minimum.Y - Origin.Y) * inv;
                float max = (box.Maximum.Y - Origin.Y) * inv;
                if (min > max)
                {
                    float temp = min;
                    min = max;
                    max = temp;
                }
                d = Math.Max(min, d);
                maxValue = Math.Min(max, maxValue);
                if (d > maxValue)
                {
                    return false;
                }
            }

            //Test z component
            if (Math.Abs(Direction.Z) < 0.0000001f)
            {
                if ((Origin.Z < box.Minimum.Z) || (Origin.Z > box.Maximum.Z))
                {
                    return false;
                }
            }
            else
            {
                float inv = (float)(1.0 / ((double)Direction.Z));
                float min = (box.Minimum.Z - Origin.Z) * inv;
                float max = (box.Maximum.Z - Origin.Z) * inv;
                if (min > max)
                {
                    float temp = min;
                    min = max;
                    max = temp;
                }
                d = Math.Max(min, d);
                maxValue = Math.Min(max, maxValue);
                if (d > maxValue)
                {
                    return false;
                }
            }

            distance = d;
            return true;
        }

        /// <summary>
        /// Transforms the ray
        /// </summary>
        public void Transform(Matrix4 transformMatrix)
        {
            Origin.Transform(transformMatrix);
            Direction.TransformNormal(transformMatrix);
        }
    }
}