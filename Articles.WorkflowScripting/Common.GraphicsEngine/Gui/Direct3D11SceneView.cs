using System;
using System.ComponentModel;
using System.Drawing;
using Common.GraphicsEngine.Drawing3D;

namespace Common.GraphicsEngine.Gui
{
    public partial class Direct3D11SceneView : Direct3D11Canvas
    {
        //Some members needed for 3D display
        //private BackbufferCopiedTextureSource m_backBufferSource;
        private Scene m_scene;
        private Camera m_camera;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct3D11SceneView"/> class.
        /// </summary>
        public Direct3D11SceneView()
        {
            m_camera = new Camera(100, 100);
            m_scene = new Scene(m_camera);
        }

        /// <summary>
        /// Gets the object on the given pixel location (location local to this control).
        /// </summary>
        public SceneObject PickObject(Point localLocation)
        {
            SceneObject result = null;

            if ((m_scene != null) && (m_camera != null))
            {
                Matrix4 projectionMatrix = m_camera.Projection;

                Vector3 v;
                v.X = (((2.0f * localLocation.X) / this.ClientRectangle.Width) - 1) / projectionMatrix.M11;
                v.Y = -(((2.0f * localLocation.Y) / this.ClientRectangle.Height) - 1) / projectionMatrix.M22;
                v.Z = 1f;

                Matrix4 worldMatrix = Matrix4.Identity;
                Matrix4 viewWorld = m_camera.View * worldMatrix;
                Matrix4 inversionViewWorld = viewWorld;
                inversionViewWorld.Invert();

                //Calculate picking-ray start and direction
                Vector3 rayDirection = new Vector3(
                    v.X * inversionViewWorld.M11 + v.Y * inversionViewWorld.M21 + v.Z * inversionViewWorld.M31,
                    v.X * inversionViewWorld.M12 + v.Y * inversionViewWorld.M22 + v.Z * inversionViewWorld.M32,
                    v.X * inversionViewWorld.M13 + v.Y * inversionViewWorld.M23 + v.Z * inversionViewWorld.M33);
                Vector3 rayStart = new Vector3(
                    inversionViewWorld.M41,
                    inversionViewWorld.M42,
                    inversionViewWorld.M43);

                //Perform pick operation
                PickingInformation pickingInfo = m_scene.Pick(new Ray(rayStart, rayDirection));
            }

            return result;
        }

        protected internal override void OnDirect3DPaint(RenderState renderState, UpdateState updateState)
        {
            //Update RenderState object
            renderState.ViewProj = m_camera.ViewProjection;

            renderState.PushScene(m_scene);
            try
            {
                //Perform updating
                m_scene.Update(updateState);

                //Perform rendering
                m_scene.Render(renderState);
            }
            finally
            {
                renderState.PopScene();
            }
        }

        /// <summary>
        /// Called when the handle way created.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (m_camera != null)
            {
                //Update properties on camera
                m_camera.ScreenWidth = this.Width;
                m_camera.ScreenHeight = this.Height;
                m_camera.UpdateCamera();
            }
        }

        /// <summary>
        /// Called when the control changed its size.
        /// </summary>
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            if (m_camera != null)
            {
                //Update properties on camera
                m_camera.ScreenWidth = this.Width;
                m_camera.ScreenHeight = this.Height;
                m_camera.UpdateCamera();
            }
        }

        /// <summary>
        /// Gets current scene.
        /// </summary>
        [Browsable(false)]
        public Scene Scene
        {
            get { return m_scene; }
        }

        /// <summary>
        /// Gets current camera.
        /// </summary>
        [Browsable(false)]
        public Camera Camera
        {
            get { return m_camera; }
        }
    }
}
