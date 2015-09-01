using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Objects.Construction;

namespace Common.GraphicsEngine.Objects.ObjectTypes
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
    }
}
