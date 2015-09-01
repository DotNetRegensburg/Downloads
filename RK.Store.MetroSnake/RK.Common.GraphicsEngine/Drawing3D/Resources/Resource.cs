using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public abstract class Resource
    {
        private string m_name;
        private ResourceDictionary m_parentDictionary;
        private bool m_markedForReloading;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        protected Resource(string name)
        {
            m_name = name;
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        public void LoadResource(ResourceDictionary resources)
        {
            try
            {
                LoadResourceInternal(resources);
            }
            finally
            {
                m_markedForReloading = false;
            }
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        public void UnloadResource(ResourceDictionary resources)
        {
            UnloadResourceInternal(resources);
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected abstract void LoadResourceInternal(ResourceDictionary resources);

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected abstract void UnloadResourceInternal(ResourceDictionary resources);

        /// <summary>
        /// Triggers reloading of the resource.
        /// </summary>
        protected void ReloadResource()
        {
            m_markedForReloading = true;
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public abstract bool IsLoaded
        {
            get;
        }

        /// <summary>
        /// Is this resource marked for reloading.
        /// </summary>
        public bool IsMarkedForReloading
        {
            get
            {
                if (!IsLoaded) { return false; }
                return m_markedForReloading;
            }
        }

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// Gets the parent ResourceDictionary object.
        /// </summary>
        public ResourceDictionary Dictionary
        {
            get { return m_parentDictionary; }
            internal set { m_parentDictionary = value; }
        }
    }
}
