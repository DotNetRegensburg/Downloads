using Common.GraphicsEngine.Drawing3D;
using Common.GraphicsEngine.Drawing3D.Resources;
using Common.GraphicsEngine.Objects.ObjectTypes;
//Some namespace mappings
using D3D11 = SlimDX.Direct3D11;

namespace Common.GraphicsEngine.Objects
{
    public class GenericObject : SceneSpacialObject
    {
        //Standard members
        private string m_geometry;

        //Resources
        private GeometryResource m_geometryResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObject"/> class.
        /// </summary>
        /// <param name="geometryResource">The geometry resource.</param>
        public GenericObject(string geometryResource)
        {
            m_geometry = geometryResource;
        }

        /// <summary>
        /// Performs a simple picking-test.
        /// </summary>
        /// <param name="pickingRay">The picking ray.</param>
        /// <returns></returns>
        public override PickingInformation Pick(Ray pickingRay)
        {
            ObjectType objType = GetObjectType();
            
            //Transfrom given ray to object space
            Matrix4 localTransform = base.Transform;
            localTransform.Invert();
            pickingRay.Transform(localTransform);

            

            return null;
        }

        /// <summary>
        /// Tries to get the ObjectType.
        /// </summary>
        public ObjectType GetObjectType()
        {
            if (m_geometryResource != null) { return m_geometryResource.ObjectType; }
            else
            {
                if ((base.Resources != null) && (base.Resources.ContainsResource(m_geometry)))
                {
                    GeometryResource geometryResource = base.Resources[m_geometry] as GeometryResource;
                    if (geometryResource != null)
                    {
                        return geometryResource.ObjectType;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Loads all resources of the object.
        /// </summary>
        /// <param name="device">Current DirectX device.</param>
        public override void LoadResources(D3D11.Device device)
        {
            //Load geometry resource
            m_geometryResource = base.Resources.GetResourceAndEnsureLoaded<GeometryResource>(m_geometry);
        }

        /// <summary>
        /// Unloads all resources of the object.
        /// </summary>
        public override void UnloadResources()
        {
            m_geometryResource = null;
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        protected override void RenderInternal(RenderState renderState)
        {
            Matrix4Stack matrixStack = renderState.World;
            matrixStack.Push(m_transform);
            try
            {
                //Render geometry
                m_geometryResource.Render(renderState);
            }
            finally
            {
                matrixStack.Pop();
            }
        }

        /// <summary>
        /// Are resources loaded?
        /// </summary>
        /// <value></value>
        public override bool IsLoaded
        {
            get { return m_geometryResource != null; }
        }
    }
}
