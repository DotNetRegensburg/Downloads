using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.GraphicsEngine.Drawing3D
{
    public abstract class SceneSpacialObject : SceneObject
    {
        protected RotationType m_rotationType;
        protected Vector3 m_position;
        protected Vector3 m_rotation;
        protected Vector2 m_rotationHW;
        protected Vector3 m_scaling;
        protected Matrix4 m_transform;

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
        public Vector2 RotationHW
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
    }
}
