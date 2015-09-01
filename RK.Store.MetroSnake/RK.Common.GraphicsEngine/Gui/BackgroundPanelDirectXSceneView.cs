using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Drawing2D;
using RK.Common.GraphicsEngine.Drawing3D;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using D2D = SharpDX.Direct2D1;

namespace RK.Common.GraphicsEngine.Gui
{
    public class BackgroundPanelDirectXSceneView : BackgroundPanelDirectXCanvas
    {
        private Scene m_scene;
        private Camera m_camera;
        private List<Drawing2DObject> m_2dObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct3D11SceneView"/> class.
        /// </summary>
        public BackgroundPanelDirectXSceneView(SwapChainBackgroundPanel target)
            : base(target)
        {
            m_camera = new Camera(base.Width, base.Height);
            m_scene = new Scene(m_camera);
            m_2dObjects = new List<Drawing2DObject>();
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
                v.X = (((2.0f * (float)localLocation.X) / base.Width) - 1) / projectionMatrix.M11;
                v.Y = -(((2.0f * (float)localLocation.Y) / base.Height) - 1) / projectionMatrix.M22;
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

        /// <summary>
        /// Called when Direct2D rendering should be done.
        /// </summary>
        /// <param name="renderState">Current state of the renderer.</param>
        /// <param name="updateState">Current state for updating.</param>
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
        /// Called when Direct2D rendering should be done.
        /// </summary>
        /// <param name="renderState">The current render state.</param>
        /// <param name="updateState">Current update state.</param>
        protected internal override void OnDirect2DPaintAfter3DRendering(RenderState2D renderState, UpdateState updateState)
        {
            for (int loop = 0; loop < m_2dObjects.Count; loop++)
            {
                m_2dObjects[loop].Render(renderState, updateState);
            }
        }

        /// <summary>
        /// Called when the size of the target panel has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.SizeChangedEventArgs" /> instance containing the event data.</param>
        protected override void OnTargetPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnTargetPanelSizeChanged(sender, e);

            if (m_camera != null)
            {
                //Update properties on camera
                m_camera.ScreenWidth = base.Width;
                m_camera.ScreenHeight = base.Height;
                m_camera.UpdateCamera();
            }
        }

        /// <summary>
        /// Gets current scene.
        /// </summary>
        public Scene Scene
        {
            get { return m_scene; }
        }

        /// <summary>
        /// Gets current camera.
        /// </summary>
        public Camera Camera
        {
            get { return m_camera; }
        }
    }
}
