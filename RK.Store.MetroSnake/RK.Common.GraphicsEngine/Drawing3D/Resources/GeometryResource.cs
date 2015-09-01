using System.Collections.Generic;
using System.Runtime.InteropServices;
using RK.Common.GraphicsEngine.Core;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.ObjectTypes;

//Some namespace mappings
using DXGI = SharpDX.DXGI;
using D3D = SharpDX.Direct3D;
using D3D11 = SharpDX.Direct3D11;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public class GeometryResource : Resource
    {
        //Resources for Direct3D 11 rendering
        private D3D11.Device m_device;
        private LoadedStructureInfo[] m_loadedStructures;

        //Generic resources
        private AxisAlignedBox m_boundingBox;
        private ObjectType m_objectType;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryResource"/> class.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public GeometryResource(string name, ObjectType objectType)
            : base(name)
        {
            m_objectType = objectType;

            m_loadedStructures = new LoadedStructureInfo[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryResource"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="vertexStructures">The vertex structures.</param>
        public GeometryResource(string name, params VertexStructure[] vertexStructures)
            : this(name, new GenericObjectType(vertexStructures))
        {

        }

        /// <summary>
        /// Renders this GeometryResource.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        public void Render(RenderState renderState)
        {
            D3D11.DeviceContext deviceContext = renderState.DeviceContext;

            for (int loop = 0; loop < m_loadedStructures.Length; loop++)
            {
                deviceContext.InputAssembler.InputLayout = m_loadedStructures[loop].InputLayout;
                deviceContext.InputAssembler.PrimitiveTopology = D3D.PrimitiveTopology.TriangleList;
                deviceContext.InputAssembler.SetIndexBuffer(m_loadedStructures[loop].IndexBuffer, DXGI.Format.R16_UInt, 0);
                deviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(m_loadedStructures[loop].VertexBuffer, m_loadedStructures[loop].SizePerVertex, 0));

                //renderState.ApplyMaterial(m_loadedStructures[loop].Material);
                renderState.ApplyMaterial(m_loadedStructures[loop].Material);

                deviceContext.DrawIndexed(m_loadedStructures[loop].IndexCount, 0, 0);
            }
        }

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void LoadResourceInternal(ResourceDictionary resources)
        {
            m_device = GraphicsCore.Current.HandlerD3D11.Device;

            //Build structures
            VertexStructure[] structures = m_objectType.BuildStructure();
            m_loadedStructures = new LoadedStructureInfo[structures.Length];

            //Load materials
            List<Vector3> vertexLocations = new List<Vector3>();
            for (int loop = 0; loop < structures.Length; loop++)
            {
                m_loadedStructures[loop] = new LoadedStructureInfo();
                m_loadedStructures[loop].VertexStructure = structures[loop];
                m_loadedStructures[loop].Material = resources.GetOrCreateMaterialResourceAndEnsureLoaded(structures[loop]); //resources.GetResourceAndEnsureLoaded<MaterialResource>(structures[loop].Material);

                foreach (Vertex actVertex in structures[loop].Vertices)
                {
                    vertexLocations.Add(actVertex.Position);
                }
            }
            m_boundingBox = new AxisAlignedBox(vertexLocations);

            //Build geometry
            BuildBuffers(structures, m_loadedStructures);
        }

        /// <summary>
        /// Unloads the resource.
        /// </summary>
        /// <param name="resources">Parent ResourceDictionary.</param>
        protected override void UnloadResourceInternal(ResourceDictionary resources)
        {
            for (int loop = 0; loop < m_loadedStructures.Length; loop++)
            {
                m_loadedStructures[loop].InputLayout = GraphicsHelper.DisposeGraphicsObject(m_loadedStructures[loop].InputLayout);
                m_loadedStructures[loop].VertexBuffer = GraphicsHelper.DisposeGraphicsObject(m_loadedStructures[loop].VertexBuffer);
                m_loadedStructures[loop].IndexBuffer = GraphicsHelper.DisposeGraphicsObject(m_loadedStructures[loop].IndexBuffer);
            }
            m_loadedStructures = new LoadedStructureInfo[0];

            m_device = null;
        }

        /// <summary>
        /// Builds vertex and index buffers.
        /// </summary>
        /// <param name="vertexBuffers">An array containing all vertex buffers.</param>
        /// <param name="indexBuffers">An array containing all index buffers.</param>
        protected virtual void BuildBuffers(VertexStructure[] structures, LoadedStructureInfo[] loadedStructures)
        {
            D3D11.Device device = GraphicsCore.Current.HandlerD3D11.Device;

            for (int loop = 0; loop < structures.Length; loop++)
            {
                StandardVertex[] vertices = StandardVertex.FromVertexStructure(structures[loop]);
                ushort[] indices = structures[loop].GetIndexArray();

                loadedStructures[loop].SizePerVertex = Marshal.SizeOf(typeof(StandardVertex));
                loadedStructures[loop].IndexCount = indices.Length;
                loadedStructures[loop].VertexBuffer = GraphicsHelper.CreateImmutableVertexBuffer(device, vertices);
                loadedStructures[loop].IndexBuffer = GraphicsHelper.CreateImmutableIndexBuffer(device, indices);
                loadedStructures[loop].InputLayout = loadedStructures[loop].Material.GenerateInputLayout(StandardVertex.InputElements);
            }
        }

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        public override bool IsLoaded
        {
            get { return m_loadedStructures.Length > 0; }
        }

        /// <summary>
        /// Gets the source of geometry data.
        /// </summary>
        public ObjectType ObjectType
        {
            get { return m_objectType; }
        }

        /// <summary>
        /// Gets the bounding box sourounding this object.
        /// </summary>
        public AxisAlignedBox BoundingBox
        {
            get { return m_boundingBox; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        protected class LoadedStructureInfo
        {
            public VertexStructure VertexStructure;
            public D3D11.Buffer VertexBuffer;
            public D3D11.Buffer IndexBuffer;
            public int SizePerVertex;
            public int IndexCount;
            public MaterialResource Material;
            public D3D11.InputLayout InputLayout;
        }
    }
}
