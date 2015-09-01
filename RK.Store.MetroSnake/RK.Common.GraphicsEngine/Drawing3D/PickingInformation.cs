

namespace RK.Common.GraphicsEngine.Drawing3D
{
    public class PickingInformation
    {


        /// <summary>
        /// The picked object.
        /// </summary>
        public SceneObject PickedObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the distance to the picked object.
        /// </summary>
        public float Distance
        {
            get;
            private set;
        }
    }
}
