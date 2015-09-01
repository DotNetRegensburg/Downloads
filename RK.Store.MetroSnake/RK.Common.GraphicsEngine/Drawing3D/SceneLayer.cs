using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RK.Common.GraphicsEngine.Core;

using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D
{
    public class SceneLayer
    {
        //private List<OcclusionGroup> m_occlusionGroups;
        private List<SceneObject> m_sceneObjects;
        private ReadOnlyCollection<SceneObject> m_sceneObjectsPublic;
        private Scene m_scene;
        private string m_name;

        public event Rendering3DHandler BeginRender;
        public event Rendering3DHandler FinishRender;

        /// <summary>
        /// Creates a new SceneLayer object for the given scene.
        /// </summary>
        /// <param name="parentScene">Parent scene.</param>
        internal SceneLayer(string name, Scene parentScene)
        {
            m_name = name;
            m_scene = parentScene;
            //m_occlusionGroups = new List<OcclusionGroup>();
            m_sceneObjects = new List<SceneObject>();
            m_sceneObjectsPublic = new ReadOnlyCollection<SceneObject>(m_sceneObjects);
        }

        /// <summary>
        /// Adds the given object to the layer.
        /// </summary>
        /// <param name="sceneObject">Object to add.</param>
        public void Add(SceneObject sceneObject)
        {
            if (sceneObject == null) { throw new ArgumentNullException("sceneObject"); }
            if (sceneObject.Scene == m_scene) { return; }
            if (sceneObject.Scene != null) { throw new ArgumentException("Given object does already belong to another scene!", "sceneObject"); }
            if (sceneObject.SceneLayer == this) { return; }
            if (sceneObject.SceneLayer != null) { throw new ArgumentException("Given object does already belong to another scene layer!", "sceneObject"); }

            if (!m_sceneObjects.Contains(sceneObject))
            {
                m_sceneObjects.Add(sceneObject);
                sceneObject.Scene = m_scene;
                sceneObject.SceneLayer = this;
            }
        }

        /// <summary>
        /// Clears this layer.
        /// </summary>
        public void Clear()
        {
            foreach (SceneObject actObject in m_sceneObjects)
            {
                if (actObject.IsLoaded) { actObject.UnloadResources(); }
                actObject.Scene = null;
            }
            m_sceneObjects.Clear();
        }

        /// <summary>
        /// Removes the given object from the layer.
        /// </summary>
        /// <param name="sceneObject">Object to remove.</param>
        public void Remove(SceneObject sceneObject)
        {
            if (m_sceneObjects.Contains(sceneObject))
            {
                if (sceneObject.IsLoaded) { sceneObject.UnloadResources(); }
                m_sceneObjects.Remove(sceneObject);
                sceneObject.Scene = null;
            }
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        internal void UnloadResources()
        {
            //Unload resources of all scene objects
            foreach (SceneObject actObject in m_sceneObjects)
            {
                if (actObject.IsLoaded)
                {
                    actObject.UnloadResources();
                }
            }
        }

        /// <summary>
        /// Prepares rendering (Loads all needed resources).
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        internal void PrepareRendering(D3D11.Device device)
        {
            List<SceneObject> invalidObjects = null;

            //Load all resources
            foreach (SceneObject actObject in m_sceneObjects)
            {
                try
                {
                    //Load all resources of the object
                    if (!actObject.IsLoaded)
                    {
                        actObject.LoadResources(device);
                    }
                }
                catch (Exception ex)
                {
                    //Build list of invalid objects
                    if (invalidObjects == null) { invalidObjects = new List<SceneObject>(); }
                    invalidObjects.Add(actObject);
                }
            }

            //Remove all invalid objects
            if (invalidObjects != null)
            {
                HandleInvalidObjects(invalidObjects);
            }
        }

        /// <summary>
        /// Updates the layer.
        /// </summary>
        /// <param name="updateState">Current update state.</param>
        internal void Update(UpdateState updateState)
        {
            SceneObject[] updateList = m_sceneObjects.ToArray();
            for (int loop = 0; loop < updateList.Length; loop++)
            {
                updateList[loop].Update(updateState);
            }
        }

        /// <summary>
        /// Renders the scene to the given context.
        /// </summary>
        /// <param name="deviceContext">The device context.</param>
        internal void Render(RenderState renderState)
        {
            List<SceneObject> invalidObjects = null;

            RaiseBeginRender(renderState);

            //Render all objects
            foreach (SceneObject actObject in m_sceneObjects)
            {
                //Load all resources of the object
                try
                {
                    //Render the object
                    if (actObject.IsLoaded)
                    {
                        actObject.Render(renderState);
                    }
                }
                catch (Exception ex)
                {
                    //Build list of invalid objects
                    if (invalidObjects == null) { invalidObjects = new List<SceneObject>(); }
                    invalidObjects.Add(actObject);
                }
            }

            //Remove all invalid objects
            if (invalidObjects != null)
            {
                HandleInvalidObjects(invalidObjects);
            }

            RaiseFinishRender(renderState);
        }

        /// <summary>
        /// Handles invalid objects.
        /// </summary>
        /// <param name="invalidObjects">List containing all invalid objects to handle.</param>
        private void HandleInvalidObjects(List<SceneObject> invalidObjects)
        {
            foreach (SceneObject actObject in invalidObjects)
            {
                //Unload the object if it is loaded
                if (actObject.IsLoaded)
                {
                    try { actObject.UnloadResources(); }
                    catch (Exception) { }
                }

                m_sceneObjects.Remove(actObject);
            }
        }

        /// <summary>
        /// Raises the BeginRender event.
        /// </summary>
        private void RaiseBeginRender(RenderState renderState)
        {
            if (BeginRender != null) { BeginRender(this, new Rendering3DArgs(renderState)); }
        }

        /// <summary>
        /// Raises the FinishRender event.
        /// </summary>
        private void RaiseFinishRender(RenderState renderState)
        {
            if (FinishRender != null) { FinishRender(this, new Rendering3DArgs(renderState)); }
        }

        /// <summary>
        /// Gets the name of this layer.
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// Gets parent scene.
        /// </summary>
        public Scene Scene
        {
            get { return m_scene; }
        }

        /// <summary>
        /// Gets a collection containing all objects.
        /// </summary>
        public ReadOnlyCollection<SceneObject> Objects
        {
            get { return m_sceneObjectsPublic; }
        }

        /// <summary>
        /// Gets total count of objects within the scene.
        /// </summary>
        public int CountObjects
        {
            get { return m_sceneObjects.Count; }
        }

    }
}
