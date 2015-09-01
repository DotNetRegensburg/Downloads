using System;

namespace RK.Common
{
    public struct BoundingSphere
    {
        public Vector3 Location;
        public float Radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingSphere"/> struct.
        /// </summary>
        /// <param name="location">The location of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        public BoundingSphere(Vector3 location, float radius)
        {
            this.Location = location;
            this.Radius = radius;
        }

        /// <summary>
        /// Does this sphere intersect with the other one?
        /// </summary>
        /// <param name="otherSphere">The other spehre to check.</param>
        public bool Intersects(BoundingSphere otherSphere)
        {
            if (Math.Abs((otherSphere.Location - this.Location).Length) <= (otherSphere.Radius + this.Radius)) { return true; }
            else { return false; }
        }
    }
}
