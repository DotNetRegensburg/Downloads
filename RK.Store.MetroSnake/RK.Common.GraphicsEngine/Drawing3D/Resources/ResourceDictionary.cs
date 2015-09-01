using System;
using System.Collections;
using System.Collections.Generic;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.ObjectTypes;
using RK.Common.Util;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class ResourceDictionary : IEnumerable<Resource>
    {
        private const string DEFAULT_RES_PREFIX = "Default.";
        public string DEFAULT_TEXTURE = "Default.Texture";

        private List<IRenderableResource> m_renderableResources;
        private WriteProtectedCollection<IRenderableResource> m_renderableResourcesPublic;
        private Dictionary<string, ResourceInfo> m_resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDictionary"/> class.
        /// </summary>
        public ResourceDictionary()
        {
            m_renderableResources = new List<IRenderableResource>();
            m_renderableResourcesPublic = new WriteProtectedCollection<IRenderableResource>(m_renderableResources);

            m_resources = new Dictionary<string, ResourceInfo>();
        }

        /// <summary>
        /// Creates a default resource and registers it using the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceName"></param>
        public void CreateDefaultResource<T>(string resourceName)
            where T : Resource
        {
            Type resourceType = typeof(T);

            //if (resourceType == typeof(MaterialResource)) { AddResource(new SimpleColoredMaterialResource(resourceName)); }
        }

        /// <summary>
        /// Clears all resources.
        /// </summary>
        public void Clear()
        {
            foreach (ResourceInfo actResource in m_resources.Values)
            {
                if (actResource.Resource.IsLoaded)
                {
                    actResource.Resource.UnloadResource(this);
                }
            }
            m_renderableResources.Clear();
            m_resources.Clear();
        }

        /// <summary>
        /// Adds the given resource to the dictionary.
        /// </summary>
        /// <param name="resource">The resource to add.</param>
        public ResourceType AddResource<ResourceType>(ResourceType resource)
            where ResourceType : Resource
        {
            //Perform some checks 
            if (resource == null) { throw new ArgumentNullException("resource"); }
            if (resource.Dictionary != null)
            {
                if (resource.Dictionary == this) { return resource; }
                if (resource.Dictionary != this) { throw new ArgumentException("Given resource belongs to another ResourceDictionary!", "resource"); }
            }

            //Remove another resource with the same name
            RemoveResource(resource.Name);

            //Add the resource
            ResourceInfo newResource = new ResourceInfo(resource);
            m_resources[resource.Name] = newResource;
            if (newResource.RenderableResource != null) { m_renderableResources.Add(newResource.RenderableResource); }

            //Register this dictionary on the resource
            resource.Dictionary = this;

            return resource;
        }

        /// <summary>
        /// Adds a new geometry resource.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="structures">All structures for the generated geometry resource.</param>
        public GeometryResource AddGeometry(string resourceName, params VertexStructure[] structures)
        {
            return this.AddResource(new GeometryResource(resourceName, structures));
        }

        /// <summary>
        /// Adds a new geometry resource.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="objectType">A ObjectType describing the generated structure.</param>
        public GeometryResource AddGeometry(string resourceName, ObjectType objectType)
        {
            return this.AddResource(new GeometryResource(resourceName, objectType));
        }

        /// <summary>
        /// Adds a new texture resource pointing to the given texture file name.
        /// </summary>
        /// <param name="textureName">The name of the generated texture resource.</param>
        /// <param name="textureFileName">The path to the texture's file.</param>
        public StandardTextureResource AddTexture(string textureName, string textureFileName)
        {
            return this.AddResource(new StandardTextureResource(textureName, textureFileName));
        }

#if DESKTOP
        /// <summary>
        /// Adds a new texture resource pointing to the given texture file name.
        /// </summary>
        /// <param name="textureName">The name of the generated texture resource.</param>
        /// <param name="textureResourceUri">The resource uri of the texture.</param>
        public StandardTextureResource AddTexture(string textureName, Uri textureResourceUri)
        {
            return this.AddResource(new StandardTextureResource(textureName, textureResourceUri));
        }
#endif

        /// <summary>
        /// Gets or creates the material resource for the given VertexStructure object.
        /// </summary>
        /// <param name="targetStructure">The target structure.</param>
        public MaterialResource GetOrCreateMaterialResourceAndEnsureLoaded(VertexStructure targetStructure)
        {
            MaterialResource materialResource = GetOrCreateMaterialResource(targetStructure);
            materialResource.LoadResource(this);
            return materialResource;
        }

        /// <summary>
        /// Gets or creates the material resource for the given VertexStructure object.
        /// </summary>
        /// <param name="targetStructure">The target structure.</param>
        public MaterialResource GetOrCreateMaterialResource(VertexStructure targetStructure)
        {
            string materialName = targetStructure.Material;

            if (this.ContainsResource(materialName)) { return this.GetResource<MaterialResource>(materialName); }
            else
            {
                if (string.IsNullOrEmpty(targetStructure.MaterialProperties.TextureName))
                {
                    return AddAndLoadResource<SimpleColoredMaterialResource>(new SimpleColoredMaterialResource(materialName));
                }
                else
                {
                    return AddAndLoadResource<SimpleColoredMaterialResource>(new SimpleColoredMaterialResource(materialName, targetStructure.MaterialProperties.TextureName));
                }
            }
        }

        /// <summary>
        /// Adds a new material for texture drawing pointing to the given texture resource.
        /// </summary>
        /// <param name="materialName">The name of the generated material.</param>
        /// <param name="textureResourceName">The name of the texture the material is pointing to.</param>
        public SimpleColoredMaterialResource AddSimpleTexturedMaterial(string materialName, string textureResourceName)
        {
            return this.AddResource(new SimpleColoredMaterialResource(materialName, textureResourceName));
        }

        /// <summary>
        /// Adds a new material for drawing a simple colored mesh.
        /// </summary>
        /// <param name="materialName">The name of the material.</param>
        public SimpleColoredMaterialResource AddSimpleColoredMaterial(string materialName)
        {
            return this.AddResource(new SimpleColoredMaterialResource(materialName));
        }

        /// <summary>
        /// Adds the given resource to the dictionary and loads it directly.
        /// </summary>
        /// <param name="resource">The resource to add.</param>
        public ResourceType AddAndLoadResource<ResourceType>(ResourceType resource)
            where ResourceType : Resource
        {
            AddResource(resource);
            if (!resource.IsLoaded) { resource.LoadResource(this); }

            return resource;
        }

        /// <summary>
        /// Removes the resource with the given name.
        /// </summary>
        /// <param name="resourceName"></param>
        public void RemoveResource(string resourceName)
        {
            if (m_resources.ContainsKey(resourceName))
            {
                ResourceInfo resourceInfo = m_resources[resourceName];

                //Unload the resource
                if (resourceInfo.Resource.IsLoaded) { resourceInfo.Resource.UnloadResource(this); }
                if (resourceInfo.RenderableResource != null) { m_renderableResources.Remove(resourceInfo.RenderableResource); }

                //Remove the resource
                m_resources.Remove(resourceName);
                resourceInfo.Resource.Dictionary = null;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Resource> GetEnumerator()
        {
            return new ResourceEnumerator(m_resources.Values.GetEnumerator());
        }

        /// <summary>
        /// Gets the resource with the given name. CreateMethod will be called to create
        /// the resource if it is not available yet.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="createMethod">Method wich creates the resource.</param>
        public T GetResource<T>(string resourceName, Func<T> createMethod)
            where T : Resource
        {
            if (m_resources.ContainsKey(resourceName)) { return m_resources[resourceName].Resource as T; }
            else
            {
                T newResource = createMethod();
                if (newResource == null) { return null; }
                if (newResource.Name != resourceName) { throw new GraphicsEngineException("Name of created resource does not equal the requested resource name!"); }

                AddResource(newResource);
                return newResource;
            }
        }

        /// <summary>
        /// Gets the resource with the gien name.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="resourceName">Name of the resource.</param>
        public T GetResource<T>(string resourceName)
            where T : Resource
        {
            if (!ContainsResource(resourceName)) { CreateDefaultResource<T>(resourceName); }
            T result = m_resources[resourceName].Resource as T;
            if (result == null) { CreateDefaultResource<T>(resourceName); }
            return m_resources[resourceName].Resource as T; 
        }

        /// <summary>
        /// Gets the resource with the given name. CreateMethod will be called to create
        /// the resource if it is not available yet.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="createMethod">Method wich creates the resource.</param>
        public T GetResourceAndEnsureLoaded<T>(string resourceName, Func<T> createMethod)
            where T : Resource
        {
            T resource = GetResource(resourceName, createMethod);
            if (!resource.IsLoaded) { resource.LoadResource(this); }
            return resource;
        }

        /// <summary>
        /// Gets the resource with the given name.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="resourceName">Name of the resource.</param>
        public T GetResourceAndEnsureLoaded<T>(string resourceName)
            where T : Resource
        {
            T resource = GetResource<T>(resourceName);
            if (!resource.IsLoaded) { resource.LoadResource(this); }
            return resource;
        }

        /// <summary>
        /// Loads all resources.
        /// </summary>
        public void LoadResources()
        {
            foreach (ResourceInfo actResourceInfo in m_resources.Values)
            {
                //Load the resource
                if (!actResourceInfo.Resource.IsLoaded)
                {
                    actResourceInfo.Resource.LoadResource(this);
                }

                //Reload the resource
                if (actResourceInfo.Resource.IsMarkedForReloading)
                {
                    actResourceInfo.Resource.UnloadResource(this);
                    actResourceInfo.Resource.LoadResource(this);
                }
            }
        }

        /// <summary>
        /// Unloads all resources.
        /// </summary>
        public void UnloadResources()
        {
            foreach (ResourceInfo actResourceInfo in m_resources.Values)
            {
                if (actResourceInfo.Resource.IsLoaded)
                {
                    actResourceInfo.Resource.UnloadResource(this);
                }
            }
        }

        /// <summary>
        /// Is there a resource with the given name?
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        public bool ContainsResource(string name)
        {
            return m_resources.ContainsKey(name);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ResourceEnumerator(m_resources.Values.GetEnumerator());
        }

        /// <summary>
        /// Gets the resource with the given name.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        public Resource this[string name]
        {
            get { return m_resources[name].Resource; }
        }

        /// <summary>
        /// Gets an enumeration containing all renderable resources.
        /// </summary>
        public WriteProtectedCollection<IRenderableResource> RenderableResources
        {
            get { return m_renderableResourcesPublic; }
        }

        /// <summary>
        /// Gets total count of resource.
        /// </summary>
        public int Count
        {
            get { return m_resources.Count; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ResourceInfo
        {
            public Resource Resource;
            public IRenderableResource RenderableResource;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceInfo"/> class.
            /// </summary>
            /// <param name="resource">The resource.</param>
            public ResourceInfo(Resource resource)
            {
                this.Resource = resource;
                this.RenderableResource = resource as IRenderableResource;
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ResourceEnumerator : IEnumerator<Resource>
        {
            private IEnumerator<ResourceInfo> m_resourceInfoEnumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceEnumerator"/> class.
            /// </summary>
            /// <param name="resourceInfoEnumerator">The resource info enumerator.</param>
            public ResourceEnumerator(IEnumerator<ResourceInfo> resourceInfoEnumerator)
            {
                m_resourceInfoEnumerator = resourceInfoEnumerator;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                m_resourceInfoEnumerator.Dispose();
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                return m_resourceInfoEnumerator.MoveNext();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public void Reset()
            {
                m_resourceInfoEnumerator.Reset();
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public Resource Current
            {
                get { return m_resourceInfoEnumerator.Current.Resource; }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
