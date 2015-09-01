using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RK.Common.GraphicsEngine.Core;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D
{
    public class Scene
    {
        public const string DEFAULT_LAYER_NAME = "Default";

        //Standard members
        private int m_lastRenderTime;
        private int m_lastUpdateTime;
        private List<SceneLayer> m_sceneLayers;
        private ReadOnlyCollection<SceneLayer> m_sceneLayersPublic;
        private ResourceDictionary m_resources;
        private Camera m_mainCamera;

        /// <summary>
        /// Raised before the renderer starts rendering.
        /// </summary>
        public event Rendering3DHandler BeforeRendering;

        /// <summary>
        /// Raises after the renderer finished rendering.
        /// </summary>
        public event Rendering3DHandler AfterRendering;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        public Scene(Camera camera)
        {
            m_resources = new ResourceDictionary();
            m_mainCamera = camera;
            m_sceneLayers = new List<SceneLayer>();
            m_sceneLayers.Add(new SceneLayer(DEFAULT_LAYER_NAME, this));
            m_sceneLayersPublic = new ReadOnlyCollection<SceneLayer>(m_sceneLayers);
        }

        /// <summary>
        /// Picks some objects within the scene.
        /// </summary>
        /// <param name="pickingRay">Ray used for picking.</param>
        public PickingInformation Pick(Ray pickingRay)
        {
            PickingInformation result = new PickingInformation();

            foreach (SceneLayer actLayer in m_sceneLayers)
            {
                foreach (SceneObject actObject in actLayer.Objects)
                {
                    
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the given object to the scene.
        /// </summary>
        /// <param name="sceneObject">Object to add.</param>
        public void Add(SceneObject sceneObject)
        {
            Add(sceneObject, DEFAULT_LAYER_NAME);
        }

        /// <summary>
        /// Adds the given object to the scene.
        /// </summary>
        /// <param name="sceneObject">Object to add.</param>
        /// <param name="layer">Layer on wich the object should be added.</param>
        public void Add(SceneObject sceneObject, string layer)
        {
            SceneLayer layerObject = GetLayer(layer);
            if (layerObject != null)
            {
                layerObject.Add(sceneObject);
            }
            else
            {
                throw new ArgumentException("Layer " + layer + " not found!", "layer");
            }
        }

        /// <summary>
        /// Adds all given scene objects.
        /// </summary>
        /// <param name="sceneObjects">All objects to add.</param>
        public void AddRange(IEnumerable<SceneObject> sceneObjects)
        {
            foreach (SceneObject actObject in sceneObjects)
            {
                this.Add(actObject);
            }
        }

        /// <summary>
        /// Adds all given scene objects.
        /// </summary>
        /// <param name="sceneObjects">All objects to add.</param>
        /// <param name="layer">Layer on wich the objects should be added.</param>
        public void AddRange(IEnumerable<SceneObject> sceneObjects, string layer)
        {
            foreach (SceneObject actObject in sceneObjects)
            {
                this.Add(actObject, layer);
            }
        }

        /// <summary>
        /// Adds a new layer with the given name.
        /// </summary>
        /// <param name="name">Name of the layer.</param>
        public SceneLayer AddLayer(string name)
        {
            SceneLayer currentLayer = GetLayer(name);
            if (currentLayer != null) { throw new ArgumentException("There is already a SceneLayer with the given name!", "name"); }

            SceneLayer newLayer = new SceneLayer(name, this);
            m_sceneLayers.Add(newLayer);
            return newLayer;
        }

        /// <summary>
        /// Removes the layer with the given name.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        public void RemoveLayer(string layerName)
        {
            SceneLayer layerToRemove = GetLayer(layerName);
            if (layerToRemove != null)
            {
                RemoveLayer(layerToRemove);
            }
        }

        /// <summary>
        /// Removes the given layer from the scene.
        /// </summary>
        /// <param name="layer">Layer to remove.</param>
        public void RemoveLayer(SceneLayer layer)
        {
            if (layer == null) { throw new ArgumentNullException("layer"); }
            if (layer.Scene != this) { throw new ArgumentException("Given layer does not belong to this scene!", "layer"); }
            if (layer.Name == DEFAULT_LAYER_NAME) { throw new ArgumentNullException("Unable to remove the default layer!", "layer"); }

            layer.UnloadResources();
            m_sceneLayers.Remove(layer);
        }

        /// <summary>
        /// Gets the layer with the given name.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        public SceneLayer GetLayer(string layerName)
        {
            if (string.IsNullOrEmpty(layerName)) { throw new ArgumentException("Given layer name is not valid!", "layerName"); }

            foreach (SceneLayer actLayer in m_sceneLayers)
            {
                if (actLayer.Name == layerName) { return actLayer; }
            }
            return null;
        }

        /// <summary>
        /// Clears the scene.
        /// </summary>
        public void Clear()
        {
            this.Clear(true);
        }

        /// <summary>
        /// Clears the scene.
        /// </summary>
        /// <param name="clearResources">Clear all resources too?</param>
        public void Clear(bool clearResources)
        {
            //Clear all layers
            for (int loop = 0; loop < m_sceneLayers.Count; loop++)
            {
                SceneLayer actLayer = m_sceneLayers[loop];
                actLayer.Clear();

                if (actLayer.Name != DEFAULT_LAYER_NAME)
                {
                    m_sceneLayers.RemoveAt(loop);
                    loop--;
                }
            }

            //Clear all resources
            if (clearResources)
            {
                m_resources.Clear();
            }
        }

        /// <summary>
        /// Removes the given object from the scene.
        /// </summary>
        /// <param name="sceneObject">Object to remove.</param>
        public void Remove(SceneObject sceneObject)
        {
            foreach (SceneLayer actLayer in m_sceneLayers)
            {
                actLayer.Remove(sceneObject);
            }
        }

        /// <summary>
        /// Removes the given object from the scene.
        /// </summary>
        /// <param name="sceneObject">Object to remove.</param>
        /// <param name="layerName">Layer on wich the scene object was added.</param>
        public void Remove(SceneObject sceneObject, string layerName)
        {
            SceneLayer layerObject = GetLayer(layerName);
            if (layerObject != null)
            {
                layerObject.Remove(sceneObject);
            }
            else
            {
                throw new ArgumentException("Layer " + layerName + " not found!", "layer");
            }
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="updateTime">Current update state.</param>
        public void Update(UpdateState updateState)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                //Render all renderable resources
                foreach (IRenderableResource actRenderableResource in m_resources.RenderableResources)
                {
                    if (actRenderableResource.IsLoaded)
                    {
                        actRenderableResource.Update(updateState);
                    }
                }

                //Update all standard object.
                foreach (SceneLayer actLayer in m_sceneLayers)
                {
                    actLayer.Update(updateState);
                }
            }
            finally
            {
                stopwatch.Stop();
                m_lastUpdateTime = (int)stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Preloads all resources needed for rendering.
        /// </summary>
        public void PrepareRendering()
        {
            //Prepare rendering on each layer
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;
            foreach (SceneLayer actLayer in m_sceneLayers)
            {
                actLayer.PrepareRendering(device);
            }
        }

        /// <summary>
        /// Renders the scene to the given context.
        /// </summary>
        /// <param name="deviceContext">The device context.</param>
        public void Render(RenderState renderState)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                bool pushedScene = false;
                if (renderState.Scene != this)
                {
                    pushedScene = true;
                    renderState.PushScene(this);
                }
                try
                {
                    //Prepare rendering on each layer
                    foreach (SceneLayer actLayer in m_sceneLayers)
                    {
                        actLayer.PrepareRendering(renderState.Device);
                    }

                    //Render all renderable resources
                    foreach (IRenderableResource actRenderableResource in m_resources.RenderableResources)
                    {
                        if (actRenderableResource.IsLoaded)
                        {
                            actRenderableResource.Render(renderState);
                        }
                    }

                    //Raise BeforeRendering event
                    RaiseBeforeRendering(renderState);

                    //Render all layers in current order
                    foreach (SceneLayer actLayer in m_sceneLayers)
                    {
                        actLayer.Render(renderState);
                    }

                    //Raise AfterRendering event
                    RaiseAfterRendering(renderState);
                }
                finally
                {
                    if (pushedScene) { renderState.PopScene(); }
                }
            }
            finally
            {
                stopwatch.Stop();
                m_lastRenderTime = (int)stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        internal void UnloadResources()
        {
            //Unload resources of all scene objects
            foreach (SceneLayer actLayer in m_sceneLayers)
            {
                actLayer.UnloadResources();
            }

            //Unload resources of all resources
            m_resources.UnloadResources();
        }

        /// <summary>
        /// Raises the BeforeRendering event.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        private void RaiseBeforeRendering(RenderState renderState)
        {
            if (BeforeRendering != null) { BeforeRendering(this, new Rendering3DArgs(renderState)); }
        }

        /// <summary>
        /// Raises the AfterRendering event.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        private void RaiseAfterRendering(RenderState renderState)
        {
            if (AfterRendering != null) { AfterRendering(this, new Rendering3DArgs(renderState)); }
        }

        /// <summary>
        /// Gets a collection containing all resources.
        /// </summary>
        public ResourceDictionary Resources
        {
            get { return m_resources; }
        }

        /// <summary>
        /// Gets a collection containing all layers.
        /// </summary>
        public ReadOnlyCollection<SceneLayer> Layers
        {
            get { return m_sceneLayersPublic; }
        }

        /// <summary>
        /// Gets the main camera of this scene.
        /// </summary>
        public Camera MainCamera
        {
            get { return m_mainCamera; }
        }

        /// <summary>
        /// Gets total count of objects within the scene.
        /// </summary>
        public int CountObjects
        {
            get
            {
                int result = 0;
                foreach (SceneLayer actLayer in m_sceneLayers)
                {
                    result += actLayer.CountObjects;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets total count of resources.
        /// </summary>
        public int CountResources
        {
            get { return m_resources.Count; }
        }

        /// <summary>
        /// Gets total count of layers.
        /// </summary>
        public int CountLayers
        {
            get { return m_sceneLayers.Count; }
        }
    }
}
