using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Gui
{
    public class Direct3D11SceneImage : Direct3D11Image
    {
        public static readonly DependencyProperty UpdateOnRenderProperty =
            DependencyProperty.Register("UpdateOnRender", typeof(bool), typeof(Direct3D11SceneImage), new UIPropertyMetadata(true));

        private Scene m_scene;
        private Camera m_camera;
        private bool m_isDragging;
        private Point m_lastDragPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direct3D11SceneImage"/> class.
        /// </summary>
        public Direct3D11SceneImage()
        {
            m_camera = new Camera(100, 100);
            m_scene = new Scene(m_camera);

            this.Loaded += new RoutedEventHandler(OnLoaded);
            this.Unloaded += OnUnloaded;

            //Register all events needed for mouse camera dragging
            this.MouseWheel += OnMouseWheel;
            this.MouseDown += OnViewportGridMouseDown;
            this.MouseUp += OnViewportGridMouseUp;
            this.MouseMove += OnViewportGridMouseMove;
            this.MouseLeave += OnViewportGridMouseLeave;
            this.LostFocus += OnViewportGridLostFocus;
            this.PreviewMouseUp += OnViewportGridPreviewMouseUp;
        }

        /// <summary>
        /// Called when this object gets unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            //Unload all 3d resources
            m_scene.UnloadResources();
            m_scene.Clear(true);
        }

        /// <summary>
        /// Called when user uses the mouse wheel for zooming.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            m_camera.Zoom((float)(e.Delta / 100.0));
        }

        private void OnViewportGridLostFocus(object sender, RoutedEventArgs e)
        {
            StopCameraDragging();
        }

        private void OnViewportGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            StartCameraDragging(e);
        }

        private void OnViewportGridMouseLeave(object sender, MouseEventArgs e)
        {
            StopCameraDragging();
        }

        private void OnViewportGridMouseMove(object sender, MouseEventArgs e)
        {
            if (m_isDragging)
            {
                Point newDragPoint = e.GetPosition(this);
                Vector2 moveDistance = new Vector2(
                    (float)(newDragPoint.X - m_lastDragPoint.X),
                    (float)(newDragPoint.Y - m_lastDragPoint.Y));

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    m_camera.Strave((float)((double)moveDistance.X / 50));
                    m_camera.UpDown((float)(-(double)moveDistance.Y / 50));
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                {
                    m_camera.Rotate(
                        (float)(-(double)moveDistance.X / 300),
                        (float)(-(double)moveDistance.Y / 300));
                }

                m_lastDragPoint = newDragPoint;
            }
        }

        private void OnViewportGridMouseUp(object sender, MouseButtonEventArgs e)
        {
            StopCameraDragging();
        }

        private void OnViewportGridPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            StopCameraDragging();
        }

        private void StartCameraDragging(MouseButtonEventArgs e)
        {
            m_isDragging = true;
            this.Cursor = Cursors.Cross;
            m_lastDragPoint = e.GetPosition(this);
        }
        private void StopCameraDragging()
        {
            m_isDragging = false;
            this.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Called when Direct3D rendering should be done.
        /// </summary>
        /// <param name="renderState"></param>
        /// <param name="updateState"></param>
        protected internal override void OnDirect3DPaint(RenderState renderState, UpdateState updateState)
        {
            //Update RenderState object
            renderState.ViewProj = m_camera.ViewProjection;

            renderState.PushScene(m_scene);
            try
            {
                //Perform updating
                if (this.UpdateOnRender)
                {
                    m_scene.Update(updateState);
                }

                //Perform rendering
                m_scene.Render(renderState);
            }
            finally
            {
                renderState.PopScene();
            }
        }

        /// <summary>
        /// Called when the image is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (m_camera != null)
            {
                //Update properties on camera
                m_camera.ScreenWidth = (int)this.ActualWidth;
                m_camera.ScreenHeight = (int)this.ActualHeight;
                m_camera.UpdateCamera();
            }
        }

        /// <summary>
        /// Called when the render size changes.
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (m_camera != null)
            {
                //Update properties on camera
                m_camera.ScreenWidth = (int)this.ActualWidth;
                m_camera.ScreenHeight = (int)this.ActualHeight;
                m_camera.UpdateCamera();
            }
        }

        /// <summary>
        /// Update scene automatically before rendering?
        /// </summary>
        public bool UpdateOnRender
        {
            get { return (bool)GetValue(UpdateOnRenderProperty); }
            set { SetValue(UpdateOnRenderProperty, value); }
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
        /// Gets current resource dictionary.
        /// </summary>
        [Browsable(false)]
        public RK.Common.GraphicsEngine.Drawing3D.Resources.ResourceDictionary Resources
        {
            get { return m_scene.Resources; }
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