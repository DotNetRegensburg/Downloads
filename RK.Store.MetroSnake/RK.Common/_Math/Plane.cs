
namespace RK.Common
{
    public struct Plane
    {
        public Vector3 Normal;
        public float Distance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> struct.
        /// </summary>
        public Plane(Vector3 normal, float distance)
        {
            this.Normal = normal;
            this.Distance = distance;
        }
    }
}
