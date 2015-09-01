using System.Collections.Generic;
using System.Diagnostics;
using RK.Common.GraphicsEngine.Animations;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
//Some namespace mappings
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D
{
    public abstract class SceneObject
    {
        //Some information about parent containers
        private Scene m_scene;
        private SceneLayer m_sceneLayer;

        //Members for animations
        private AnimationHandler m_animationHandler;

        //Generic data
        //private List<ObjectAnimation> m_animations;
        private int m_lastRenderTime;
        private List<string> m_visibilityLayers;

        public event Rendering3DHandler Rendered;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneObject"/> class.
        /// </summary>
        protected SceneObject()
        {
            //m_animations = new List<ObjectAnimation>();
            m_visibilityLayers = new List<string>();
            m_animationHandler = new AnimationHandler(this);
        }

        /// <summary>
        /// Starts a new AnimationSequence.
        /// </summary>
        public AnimationSequenceBuilder BeginAnimationSequence()
        {
            return new AnimationSequenceBuilder(m_animationHandler);
        }

        /// <summary>
        /// Performs a simple picking-test.
        /// </summary>
        /// <param name="pickingRay">The picking ray.</param>
        public virtual PickingInformation Pick(Ray pickingRay)
        {
            return null;
        }

        /// <summary>
        /// Loads all resources of the object.
        /// </summary>
        /// <param name="device">Current DirectX device.</param>
        public abstract void LoadResources(D3D11.Device device);

        /// <summary>
        /// Updates this object.
        /// </summary>
        /// <param name="updateState">State of update process.</param>
        public void Update(UpdateState updateState)
        {
            //Update current animation state
            if (m_animationHandler != null)
            {
                m_animationHandler.Update(updateState);
            }

            //Update the object
            UpdateInternal(updateState);
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        public void Render(RenderState renderState)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                //Render the object
                RenderInternal(renderState);

                //Raise Rendered event
                RaiseRendered(renderState);
            }
            finally
            {
                stopwatch.Stop();
                m_lastRenderTime = (int)stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Unloads all resources of the object.
        /// </summary>
        public abstract void UnloadResources();

        /// <summary>
        /// Are resources loaded?
        /// </summary>
        public abstract bool IsLoaded { get; }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        protected abstract void RenderInternal(RenderState renderState);

        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="updateState">Current update state.</param>
        protected abstract void UpdateInternal(UpdateState updateState);

        /// <summary>
        /// Raises the Rendered event.
        /// </summary>
        /// <param name="renderState">Current RenderState object.</param>
        private void RaiseRendered(RenderState renderState)
        {
            if (Rendered != null) { Rendered(this, new Rendering3DArgs(renderState)); }
        }

        /// <summary>
        /// Gets current scene.
        /// </summary>
        public Scene Scene
        {
            get { return m_scene; }
            internal set { m_scene = value; }
        }

        /// <summary>
        /// Gets or sets the scene layer.
        /// </summary>
        public SceneLayer SceneLayer
        {
            get { return m_sceneLayer; }
            internal set { m_sceneLayer = value; }
        }

        /// <summary>
        /// Gets a list containing all visibility layers wich this object belongs to.
        /// </summary>
        public List<string> VisibilityLayers
        {
            get { return m_visibilityLayers; }
        }

        /// <summary>
        /// Gets a dictionary containing all resources.
        /// </summary>
        public ResourceDictionary Resources
        {
            get
            {
                if (m_scene == null) { return null; }
                return m_scene.Resources;
            }
        }

        /// <summary>
        /// Gets current AnimationHandler object.
        /// </summary>
        public AnimationHandler AnimationHandler
        {
            get { return m_animationHandler; }
        }

        /// <summary>
        /// Gets or sets some additional methadata object.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the last measured render time.
        /// </summary>
        public int LastRenderTime
        {
            get { return m_lastRenderTime; }
        }
    }
}
