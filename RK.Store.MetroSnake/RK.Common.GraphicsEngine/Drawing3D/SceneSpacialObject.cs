
namespace RK.Common.GraphicsEngine.Drawing3D
{
    public abstract class SceneSpacialObject : SceneObject
    {
        protected RotationType m_rotationType;
        protected Vector3 m_position;
        protected Vector3 m_rotation;
        protected Vector2 m_rotationHW;
        protected Vector3 m_scaling;
        protected Matrix4 m_transform;

        private Vector3 m_up;
        private Vector3 m_right;
        private Vector3 m_look;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneSpacialObject"/> class.
        /// </summary>
        public SceneSpacialObject()
        {
            m_rotationType = RotationType.EulerAngles;
            m_position = Vector3.Empty;
            m_rotationHW = Vector2.Empty;
            m_rotation = Vector3.Empty;
            m_scaling = new Vector3(1f, 1f, 1f);
            m_transform = Matrix4.Identity;

            //Define standard look vectors
            m_look.TransformNormal(m_transform);
            m_up.TransformNormal(m_transform);
            m_right.TransformNormal(m_transform);
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="updateState">Current update state.</param>
        protected override void UpdateInternal(UpdateState updateState)
        {
            //Calculates local transform matrix (transforms local space to world space)
            if (m_rotationType == RotationType.EulerAngles)
            {
                m_transform =
                    Matrix4.RotationYawPitchRoll(m_rotation.Y, m_rotation.X, m_rotation.Z) *
                    Matrix4.Scaling(m_scaling) *
                    Matrix4.Translation(m_position) *
                    updateState.World.Top;
            }
            else if (m_rotationType == RotationType.HWAngles)
            {
                m_transform =
                    Matrix4.RotationHV(m_rotationHW) *
                    Matrix4.Scaling(m_scaling) *
                    Matrix4.Translation(m_position) *
                    updateState.World.Top;
            }

            //Define standard look vectors
            m_look = new Vector3(0f, 0f, 1f);
            m_look.TransformNormal(m_transform);
            m_up = new Vector3(0f, 1f, 0f);
            m_up.TransformNormal(m_transform);
            m_right = new Vector3(1f, 0f, 0f);
            m_right.TransformNormal(m_transform);
        }

        /// <summary>
        /// Zooms the camera into or out along the actual target-vector.
        /// </summary>
        /// <param name="dist">The Distance you want to zoom.</param>
        public void MoveForward(float dist)
        {
            m_position.X += dist * m_look.X;
            m_position.Y += dist * m_look.Y;
            m_position.Z += dist * m_look.Z;
        }

        /// <summary>
        /// Moves the object position.
        /// </summary>
        /// <param name="x">moving in x direction.</param>
        /// <param name="z">moving in z direction.</param>
        public void Move(float x, float z)
        {
            m_position.X += x;
            m_position.Z += z;
        }

        /// <summary>
        /// Moves the object up and down.
        /// </summary>
        public void UpDown(float points)
        {
            m_position.X = m_position.X + m_up.X * points;
            m_position.Y = m_position.Y + m_up.Y * points;
            m_position.Z = m_position.Z + m_up.Z * points;
        }

        /// <summary>
        /// Moves the object up and down.
        /// </summary>
        public void UpDownWithoutMoving(float points)
        {
            m_position.Y = m_position.Y + m_up.Y * points;
        }

        /// <summary>
        /// Straves the object.
        /// </summary>
        public void Strave(float points)
        {
            m_position.X = m_position.X + m_right.X * points;
            m_position.Y = m_position.Y + m_right.Y * points;
            m_position.Z = m_position.Z + m_right.Z * points;
        }

        /// <summary>
        /// Streaves the object.
        /// </summary>
        public void StraveAtPlane(float points)
        {
            m_position.X = m_position.X + m_right.X * points;
            m_position.Z = m_position.Z + m_right.Z * points;
        }

        /// <summary>
        /// Gets or sets current position.
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        /// <summary>
        /// Gets or sets current rotation.
        /// </summary>
        public Vector3 Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        /// <summary>
        /// Gets or sets horizontal and vertical rotation values.
        /// </summary>
        public Vector2 RotationHV
        {
            get { return m_rotationHW; }
            set { m_rotationHW = value; }
        }

        /// <summary>
        /// Gets or sets the used rotation type.
        /// </summary>
        public RotationType RotationType
        {
            get { return m_rotationType; }
            set { m_rotationType = value; }
        }

        /// <summary>
        /// Gets or sets current scaling.
        /// </summary>
        public Vector3 Scaling
        {
            get { return m_scaling; }
            set { m_scaling = value; }
        }

        /// <summary>
        /// Gets a matrix that transforms local space to world space.
        /// </summary>
        public Matrix4 Transform
        {
            get { return m_transform; }
        }

        /// <summary>
        /// Gets the vector that looks up.
        /// </summary>
        public Vector3 Up
        {
            get { return m_up; }
        }

        /// <summary>
        /// Gets the vector that looks into front.
        /// </summary>
        public Vector3 Look
        {
            get { return m_look; }
        }

        /// <summary>
        /// Gets the vector that looks to the right.
        /// </summary>
        public Vector3 Right
        {
            get { return m_right; }
        }
    }
}
