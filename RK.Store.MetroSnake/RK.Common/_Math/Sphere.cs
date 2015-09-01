
namespace RK.Common
{
    public class Sphere
    {
        public Vector3 Center;
        public float Radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        /// <param name="center">The center corrdinate.</param>
        /// <param name="radius">The radius.</param>
        public Sphere(Vector3 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class.
        /// </summary>
        /// <param name="x">X-Component of the center coordinate.</param>
        /// <param name="y">Y-Component of the center coordinate.</param>
        /// <param name="z">Z-Component of the center coordinate.</param>
        /// <param name="radius">The radius.</param>
        public Sphere(float x, float y, float z, float radius)
        {
            this.Center = new Vector3(x, y, z);
            this.Radius = radius;
        }

        /// <summary>
        /// Inflates the sphere by the given value.
        /// </summary>
        public void Inflate(float value)
        {
            this.Radius = this.Radius + value;
        }
    }
}
