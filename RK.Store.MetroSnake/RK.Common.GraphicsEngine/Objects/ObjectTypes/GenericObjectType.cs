using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public class GenericObjectType : ObjectType
    {
        private VertexStructure[] m_vertexStructures;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObjectType"/> class.
        /// </summary>
        /// <param name="vertexStructures">The vertex structures.</param>
        public GenericObjectType(VertexStructure[] vertexStructures)
        {
            m_vertexStructures = vertexStructures;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObjectType"/> class.
        /// </summary>
        /// <param name="vertexStructure">The vertex structure.</param>
        public GenericObjectType(VertexStructure vertexStructure)
            : this(new VertexStructure[]{ vertexStructure })
        {

        }

        /// <summary>
        /// Builds the structure.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            VertexStructure[] result = new VertexStructure[m_vertexStructures.Length];
            for (int loop = 0; loop < result.Length; loop++)
            {
                result[loop] = m_vertexStructures[loop].Clone();
            }
            return result;
        }

        /// <summary>
        /// Applies the given material to all contained vertex structures.
        /// </summary>
        /// <param name="materialToApply">The materials to apply.</param>
        public void ApplyMaterialForAll(string materialToApply)
        {
            for (int loop = 0; loop < m_vertexStructures.Length; loop++)
            {
                m_vertexStructures[loop].Material = materialToApply;
            }
        }

        public void ConvertMaterial(string materialNameOld, string materialNameNew)
        {
            for (int loop = 0; loop < m_vertexStructures.Length; loop++)
            {
                if (m_vertexStructures[loop].Material == materialNameOld)
                {
                    m_vertexStructures[loop].Material = materialNameNew;
                }
            }
        }

        /// <summary>
        /// Gets the array containing all loaded vertex structures.
        /// </summary>
        public VertexStructure[] VertexStructures
        {
            get { return m_vertexStructures; }
        }
    }
}
