using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public class LabelType : ObjectType
    {
        private float m_width;
        private float m_height;
        private string m_material;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelType"/> class.
        /// </summary>
        /// <param name="width">The width of the Label.</param>
        /// <param name="height">The height of the Label.</param>
        /// <param name="material">The material of the Label.</param>
        public LabelType(float width, float height, string material)
        {
            m_width = width;
            m_height = height;
            m_material = material;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelType"/> class.
        /// </summary>
        /// <param name="width">The width of the Label.</param>
        /// <param name="height">The height of the Label.</param>
        public LabelType(float width, float height)
            : this(width, height, string.Empty)
        {

        }

        /// <summary>
        /// Builds the structure.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            VertexStructure[] result = new VertexStructure[1];

            //Build the label
            result[0] = new VertexStructure();
            result[0].Material = m_material;
            result[0].BuildRect4V(
                new Vector3(-m_width / 2f, 0f, -m_height / 2f),
                new Vector3(m_width / 2f, 0f, -m_height / 2f),
                new Vector3(m_width / 2f, 0f, m_height / 2f),
                new Vector3(-m_width / 2f, 0f, m_height / 2f));

            return result;
        }
    }
}
