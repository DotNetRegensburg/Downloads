using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Objects.Construction;

namespace Common.GraphicsEngine.Objects.ObjectTypes
{
    public class CoordinateAxesType : ObjectType
    {
        private string m_material;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateAxesType"/> class.
        /// </summary>
        /// <param name="material">The material.</param>
        public CoordinateAxesType(string material)
        {
            m_material = material;
        }

        /// <summary>
        /// Builds the structure.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            VertexStructure[] result = new VertexStructure[1];

            result[0] = new VertexStructure();
            result[0].Material = m_material;

            //Build x axis
            result[0].EnableBuildTimeTransform(Matrix4.RotationHV(0f, -EngineMath.RAD_90DEG));
            BuildArrow(result[0], Color4.Red);

            //Build z axis
            result[0].EnableBuildTimeTransform(Matrix4.RotationHV(-EngineMath.RAD_90DEG, -EngineMath.RAD_90DEG));
            BuildArrow(result[0], Color4.Blue);

            //Build y axis
            result[0].DisableBuildTimeTransform();
            BuildArrow(result[0], Color4.Green);

            return result;
        }

        /// <summary>
        /// Builds an arrow into the given structure using the given color.
        /// </summary>
        private void BuildArrow(VertexStructure structure, Color4 color)
        {
            structure.BuildCylinderFullV(
                Vector3.Empty,
                0.15f, 1f, 15, color);
            structure.BuildConeFullV(
                new Vector3(0f, 1f, 0f),
                0.3f, 0.6f, 15, color);
        }
    }
}
