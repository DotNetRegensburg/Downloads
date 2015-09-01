using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace RK.Common.GraphicsEngine.Gui
{
    /// <summary>
    /// Interaction logic for WpfModelViewerControl.xaml
    /// </summary>
    public partial class WpfModelViewerControl : UserControl
    {
        public static readonly DependencyProperty CameraDistanceProperty =
            DependencyProperty.Register("CameraDistance", typeof(double), typeof(WpfModelViewerControl), new PropertyMetadata(3.0, OnCameraPropertyChanged));
        public static readonly DependencyProperty CameraHorizontalRotationProperty =
            DependencyProperty.Register("CameraHorizontalRotation", typeof(double), typeof(WpfModelViewerControl), new PropertyMetadata(45.0, OnCameraPropertyChanged));
        public static readonly DependencyProperty CameraMaximumDistanceProperty =
            DependencyProperty.Register("CameraMaximumDistance", typeof(double), typeof(WpfModelViewerControl), new PropertyMetadata(20.0));
        public static readonly DependencyProperty CameraMinimumDistanceProperty =
            DependencyProperty.Register("CameraMinimumDistance", typeof(double), typeof(WpfModelViewerControl), new PropertyMetadata(2.0));
        public static readonly DependencyProperty CameraVerticalRotationProperty =
            DependencyProperty.Register("CameraVerticalRotation", typeof(double), typeof(WpfModelViewerControl), new PropertyMetadata(45.0, OnCameraPropertyChanged));
        public static readonly DependencyProperty ControlsVisibilityProperty =
            DependencyProperty.Register("ControlsVisibility", typeof(Visibility), typeof(WpfModelViewerControl), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowEditorGridProperty =
            DependencyProperty.Register("ShowEditorGrid", typeof(bool), typeof(WpfModelViewerControl), new PropertyMetadata(true, OnShowEditorGridChanged));

        private bool m_isDragging;
        private Point m_lastDragPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfModelViewerControl" /> class.
        /// </summary>
        public WpfModelViewerControl()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            //Register all events needed for mouse camera dragging
            m_viewportGrid.MouseDown += OnViewportGridMouseDown;
            m_viewportGrid.MouseUp += OnViewportGridMouseUp;
            m_viewportGrid.MouseMove += OnViewportGridMouseMove;
            m_viewportGrid.MouseLeave += OnViewportGridMouseLeave;
            m_viewportGrid.LostFocus += OnViewportGridLostFocus;
            m_viewportGrid.PreviewMouseUp += OnViewportGridPreviewMouseUp;
        }

        public double CameraDistance
        {
            get { return (double)GetValue(CameraDistanceProperty); }
            set { SetValue(CameraDistanceProperty, value); }
        }

        public double CameraHorizontalRotation
        {
            get { return (double)GetValue(CameraHorizontalRotationProperty); }
            set { SetValue(CameraHorizontalRotationProperty, value); }
        }

        public double CameraMaximumDistance
        {
            get { return (double)GetValue(CameraMaximumDistanceProperty); }
            set { SetValue(CameraMaximumDistanceProperty, value); }
        }

        public double CameraMinimumDistance
        {
            get { return (double)GetValue(CameraMinimumDistanceProperty); }
            set { SetValue(CameraMinimumDistanceProperty, value); }
        }

        public double CameraVerticalRotation
        {
            get { return (double)GetValue(CameraVerticalRotationProperty); }
            set { SetValue(CameraVerticalRotationProperty, value); }
        }

        public Visibility ControlsVisibility
        {
            get { return (Visibility)GetValue(ControlsVisibilityProperty); }
            set { SetValue(ControlsVisibilityProperty, value); }
        }

        public bool ShowEditorGrid
        {
            get { return (bool)GetValue(ShowEditorGridProperty); }
            set { SetValue(ShowEditorGridProperty, value); }
        }

        /// <summary>
        /// Gets a collection containing all 3D children.
        /// </summary>
        public Visual3DCollection Visual3DChildren
        {
            get { return m_mainModelContainer.Children; }
        }

        /// <summary>
        /// Gets the main children collection
        /// </summary>
        public Visual3DCollection Visual3DChildrenViewport
        {
            get { return m_viewport3D.Children; }
        }

        /// <summary>
        /// Called when the configured distance of the camera to the origin has changed.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="eArgs">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        public static void OnCameraPropertyChanged(DependencyObject sourceObject, DependencyPropertyChangedEventArgs eArgs)
        {
            WpfModelViewerControl source = sourceObject as WpfModelViewerControl;
            if (source != null)
            {
                source.ResetCameraOrientation();
            }
        }

        /// <summary>
        /// Shows/Hides the editor grid.
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="eArgs">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        public static void OnShowEditorGridChanged(DependencyObject sourceObject, DependencyPropertyChangedEventArgs eArgs)
        {
            WpfModelViewerControl source = sourceObject as WpfModelViewerControl;
            if (source != null)
            {
                if (source.ShowEditorGrid) { source.m_viewport3D.Children.Add(source.m_grid3D); }
                else { source.m_viewport3D.Children.Remove(source.m_grid3D); }
            }
        }

        /// <summary>
        /// Standard load event handling.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ResetCameraOrientation();
        }

        /// <summary>
        /// Called when user uses the mouse wheel for zooming.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Calculate new distance from camera to origin
            double distanceToOrigin = this.CameraDistance - (e.Delta / 100.0);
            if (distanceToOrigin < CameraMinimumDistance) { distanceToOrigin = CameraMinimumDistance; }
            if (distanceToOrigin > CameraMaximumDistance) { distanceToOrigin = CameraMaximumDistance; }
            this.CameraDistance = distanceToOrigin;
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
                Point newDragPoint = e.GetPosition(m_viewportGrid);
                Vector2 moveDistance = new Vector2(
                    (float)(newDragPoint.X - m_lastDragPoint.X),
                    (float)(newDragPoint.Y - m_lastDragPoint.Y));
                this.CameraHorizontalRotation = this.CameraHorizontalRotation + -(double)moveDistance.X / (this.ActualWidth / 200);
                this.CameraVerticalRotation = this.CameraVerticalRotation + (double)moveDistance.Y / (this.ActualHeight / 200);
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

        /// <summary>
        /// Recalculates position and orientation of the current camera.
        /// </summary>
        /// <param name="lookDirection">The look direction to set.</param>
        /// <param name="distanceToOrigin">The distance to the origin to set.</param>
        private void ResetCameraOrientation()
        {
            PerspectiveCamera currentCamera = m_viewport3D.Camera as PerspectiveCamera;
            if (currentCamera == null) { return; }

            //Get current camera distance
            double distanceToOrigin = this.CameraDistance;
            if (distanceToOrigin < CameraMinimumDistance) { distanceToOrigin = CameraMinimumDistance; }
            if (distanceToOrigin > CameraMaximumDistance) { distanceToOrigin = CameraMaximumDistance; }
            if (distanceToOrigin != this.CameraDistance)
            {
                CameraDistance = distanceToOrigin;
                return;
            }

            //Get current camera rotation
            double horizontalRotation = this.CameraHorizontalRotation;
            if (horizontalRotation < 0) { horizontalRotation = 360.0 + (horizontalRotation % 360.0); }
            if (horizontalRotation > 360.0) { horizontalRotation = horizontalRotation % 360.0; }
            if (horizontalRotation != this.CameraHorizontalRotation)
            {
                CameraHorizontalRotation = horizontalRotation;
                return;
            }

            double verticalRotation = this.CameraVerticalRotation;
            if (verticalRotation < 0) { verticalRotation = 0; }
            if (verticalRotation > 89.9) { verticalRotation = 89.9; }
            if (verticalRotation != this.CameraVerticalRotation)
            {
                CameraVerticalRotation = verticalRotation;
                return;
            }

            //Calculate camera look direction
            Vector3 cameraLookDirection = new Vector3(0f, 0f, 1f);
            cameraLookDirection.TransformNormal(Matrix4.RotationX(EngineMath.DegreeToRadian((float)verticalRotation)));
            cameraLookDirection.TransformNormal(Matrix4.RotationY(EngineMath.DegreeToRadian((float)horizontalRotation)));
            cameraLookDirection.Normalize();
            Vector3D lookDirection = cameraLookDirection.ToWpfVector();

            //Recalculate position
            Vector3D negatedLookDirection = lookDirection;
            negatedLookDirection.Negate();
            currentCamera.Position = new Point3D() + negatedLookDirection * distanceToOrigin;
            currentCamera.LookDirection = lookDirection;

            m_mainLight.Transform = new TranslateTransform3D(currentCamera.Position.X, currentCamera.Position.Y, currentCamera.Position.Z);
            m_mainLight.Direction = lookDirection;
        }

        private void StartCameraDragging(MouseButtonEventArgs e)
        {
            m_isDragging = true;
            m_viewport3D.Cursor = Cursors.Cross;
            m_lastDragPoint = e.GetPosition(m_viewportGrid);
        }
        private void StopCameraDragging()
        {
            m_isDragging = false;
            m_viewport3D.Cursor = Cursors.Hand;
        }
    }
}
