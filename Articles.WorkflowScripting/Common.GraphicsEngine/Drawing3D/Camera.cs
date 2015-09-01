using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.GraphicsEngine.Drawing3D
{
    public class Camera
    {
        private Vector3 m_position = new Vector3(0, 0, 0);
        private Vector3 m_lastBigStepPos = new Vector3(0, 0, 0);
        private Vector3 m_relativeTarget = new Vector3(0, 0, 1);
        private Vector3 m_upVector = new Vector3(0, 1, 0);

        private float m_fov = (float)Math.PI / 4.0f;

        private Vector3 m_up;
        private Vector3 m_right;
        private Vector3 m_look;

        private Matrix4 m_view;
        private Matrix4 m_project;
        private Matrix4 m_viewProj;

        private int m_screenWidth;
        private int m_screenHeight;

        private float m_hRotation = 0.0f;
        private float m_vRotation = 0.0f;

        /// <summary>
        /// Creates a new camera.
        /// </summary>
        /// <param name="width">Width of the renderwindow.</param>
        /// <param name="height">Height of the renderwindow.</param>
        public Camera(int width, int height)
        {
            m_screenWidth = width;
            m_screenHeight = height;

            UpdateCamera();
        }

        /// <summary>
        /// Updates the camera.
        /// </summary>
        public void UpdateCamera()
        {
            Vector3 newTarget = m_relativeTarget;
            newTarget = Vector3.Add(newTarget, m_position);

            m_view = Matrix4.LookAtLH(
                m_position,
                newTarget,
                m_upVector);
            m_project = Matrix4.PerspectiveFovLH(
                    m_fov,
                    (float)m_screenWidth / (float)m_screenHeight,
                    0.1f, 500f);
            m_viewProj = m_view * m_project;

            m_right.X = m_view.M11;
            m_right.Y = m_view.M21;
            m_right.Z = m_view.M31;
            m_up.X = m_view.M12;
            m_up.Y = m_view.M22;
            m_up.Z = m_view.M32;
            m_look.X = m_view.M13;
            m_look.Y = m_view.M23;
            m_look.Z = m_view.M33;
        }

        /// <summary>
        /// Sets the size of the screen.
        /// </summary>
        /// <param name="width">Width of the renderwindow.</param>
        /// <param name="height">Height of the renderwindow.</param>
        public void SetScreenSize(int width, int height)
        {
            m_screenWidth = width;
            m_screenHeight = height;

            UpdateCamera();
        }

        /// <summary>
        /// Rotates the target around the position of the camera.
        /// </summary>
        /// <param name="h">horizontal rotation.</param>
        /// <param name="v">vertical rotation.</param>
        public void Rotate(float HRot, float VRot)
        {
            m_hRotation += HRot;
            m_vRotation += VRot;

            if (m_vRotation >= (float)Math.PI / 2f) { m_vRotation = (float)Math.PI / 2f - 0.001f; }
            if (m_vRotation <= -(float)Math.PI / 2f) { m_vRotation = -(float)Math.PI / 2f + 0.001f; }

            m_relativeTarget.X = (float)(1f * Math.Cos(m_vRotation) * Math.Cos(m_hRotation));
            m_relativeTarget.Y = (float)(1f * Math.Sin(m_vRotation));
            m_relativeTarget.Z = (float)(1f * Math.Cos(m_vRotation) * Math.Sin(m_hRotation));

            UpdateCamera();
        }

        /// <summary>
        /// Zooms the camera into or out along the actual target-vector.
        /// </summary>
        /// <param name="dist">The Distance you want to zoom.</param>
        public void Zoom(float dist)
        {
            m_position.X += dist * m_relativeTarget.X;
            m_position.Y += dist * m_relativeTarget.Y;
            m_position.Z += dist * m_relativeTarget.Z;

            UpdateCamera();
        }

        /// <summary>
        /// Moves the camera position.
        /// </summary>
        /// <param name="x">moving in x direction.</param>
        /// <param name="z">moving in z direction.</param>
        public void Move(float x, float z)
        {
            m_position.X += x;
            m_position.Z += z;

            UpdateCamera();
        }

        /// <summary>
        /// Moves the Camera up and down.
        /// </summary>
        public void UpDown(float points)
        {
            //if (!(points * m_up.Y < 0))
            //{
            m_position.X = m_position.X + m_up.X * points;
            m_position.Y = m_position.Y + m_up.Y * points;
            m_position.Z = m_position.Z + m_up.Z * points;

            UpdateCamera();
            //}
        }

        /// <summary>
        /// Moves the camera up and down.
        /// </summary>
        public void UpDownWithoutMoving(float points)
        {
            //if (!(points * m_up.Y < 0))
            //{
            m_position.Y = m_position.Y + m_up.Y * points;

            UpdateCamera();
            //}
        }

        /// <summary>
        /// Straves the camera.
        /// </summary>
        public void Strave(float points)
        {
            m_position.X = m_position.X + m_right.X * points;
            m_position.Y = m_position.Y + m_right.Y * points;
            m_position.Z = m_position.Z + m_right.Z * points;

            UpdateCamera();
        }

        /// <summary>
        /// Streaves the camera.
        /// </summary>
        public void StraveAtPlane(float points)
        {
            m_position.X = m_position.X + m_right.X * points;
            m_position.Z = m_position.Z + m_right.Z * points;

            UpdateCamera();
        }

        /// <summary>
        /// Retrieves the view-matrix.
        /// </summary>
        public Matrix4 View
        {
            get { return m_view; }
        }

        /// <summary>
        /// Gets the view-projection matrix.
        /// </summary>
        public Matrix4 ViewProjection
        {
            get { return m_viewProj; }
        }

        /// <summary>
        /// Retrieves or sets the direction / target.
        /// </summary>
        public Vector3 Direction
        {
            get { return m_look; }
        }

        /// <summary>
        /// Retrieves a vector, wich is targeting right.
        /// </summary>
        public Vector3 Right
        {
            get { return m_right; }
        }

        /// <summary>
        /// Retrieves a vector, wich is targiting upwards.
        /// </summary>
        public Vector3 Up
        {
            get { return m_up; }
        }

        /// <summary>
        /// Retrieves or sets the rotation-angles of the target.
        /// The first element of the vector is hRot, the second is vRot.
        /// 
        /// The angles are radian values!
        /// </summary>
        public Vector2 TargetRotation
        {
            get { return new Vector2(m_hRotation, m_vRotation); }
            set
            {
                Vector2 v = value;

                m_hRotation = v.X;
                m_vRotation = v.Y;

                if (m_vRotation >= (float)Math.PI / 2f) { m_vRotation = (float)Math.PI / 2f - 0.001f; }
                if (m_vRotation <= -(float)Math.PI / 2f) { m_vRotation = -(float)Math.PI / 2f + 0.001f; }

                m_relativeTarget.X = (float)(1f * Math.Cos(m_vRotation) * Math.Cos(m_hRotation));
                m_relativeTarget.Y = (float)(1f * Math.Sin(m_vRotation));
                m_relativeTarget.Z = (float)(1f * Math.Cos(m_vRotation) * Math.Sin(m_hRotation));

                UpdateCamera();
            }
        }

        /// <summary>
        /// Gets or sets the target position.
        /// </summary>
        public Vector3 Target
        {
            get { return m_relativeTarget + m_position; }
            set { this.RelativeTarget = value - m_position; }
        }

        /// <summary>
        /// Gets or sets the relative target position.
        /// </summary>
        public Vector3 RelativeTarget
        {
            get { return m_relativeTarget; }
            set
            {
                m_relativeTarget = Vector3.Normalize(value);

                //Update horizontal and vertical rotation
                m_relativeTarget.ToHVRotation(out m_hRotation, out m_vRotation);

                UpdateCamera();
            }
        }

        /// <summary>
        /// Retrieves or sets the position of the camera.
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set
            {
                m_position = value;

                UpdateCamera();
            }
        }

        /// <summary>
        /// Retrieves projection-matrix.
        /// </summary>
        public Matrix4 Projection
        {
            get
            {
                return m_project;
            }
        }

        /// <summary>
        /// Width of the screen.
        /// </summary>
        public int ScreenWidth
        {
            get { return m_screenWidth; }
            set { m_screenWidth = value; }
        }

        /// <summary>
        /// Height of the screen.
        /// </summary>
        public int ScreenHeight
        {
            get { return m_screenHeight; }
            set { m_screenHeight = value; }
        }
    }
}
