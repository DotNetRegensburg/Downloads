using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RK.Common.GraphicsEngine.Objects.Construction;

namespace RK.Common.GraphicsEngine.Objects.ObjectTypes
{
    public class RackType : ObjectType
    {
        private List<RackRowProperties> m_rowProperties;
        private List<RackColumnProperties> m_columnProperties;
        private ReadOnlyCollection<RackRowProperties> m_rowPropertiesPublic;
        private ReadOnlyCollection<RackColumnProperties> m_columnPropertiesPublic;
        private float m_rackWidth;
        private float m_groundHeight;
        private float m_columnWidth;
        private string m_columnMaterial;
        private string m_groundMaterial;

        /// <summary>
        /// Initializes a new instance of the <see cref="RackType"/> class.
        /// </summary>
        public RackType(int rowCount, int columnCount)
            : this(rowCount, columnCount, 1.3f, 0.9f)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RackType"/> class.
        /// </summary>
        public RackType(int rowCount, int columnCount, float rowHeight, float columnDepth)
        {
            if (rowCount <= 0) { throw new ArgumentException("Illegal row count!", "rowCount"); }
            if (columnCount <= 0) { throw new ArgumentException("Illegal column count!", "columnCount"); }

            m_rackWidth = 1f;
            m_columnMaterial = string.Empty;
            m_groundMaterial = string.Empty;

            m_rowProperties = new List<RackRowProperties>();
            m_columnProperties = new List<RackColumnProperties>();
            m_rowPropertiesPublic = new ReadOnlyCollection<RackRowProperties>(m_rowProperties);
            m_columnPropertiesPublic = new ReadOnlyCollection<RackColumnProperties>(m_columnProperties);

            for (int loop = 0; loop < rowCount; loop++)
            {
                m_rowProperties.Add(new RackRowProperties() { Height = rowHeight });
            }
            for (int loop = 0; loop < columnCount; loop++)
            {
                m_columnProperties.Add(new RackColumnProperties() { Depth = columnDepth });
            }
        }

        /// <summary>
        /// Gets total height of the rack.
        /// </summary>
        public float GetTotalHeight()
        {
            float result = 0f;
            foreach (RackRowProperties actRow in m_rowProperties)
            {
                result += actRow.Height;
            }
            return result;
        }

        /// <summary>
        /// Gets total width of the rack.
        /// </summary>
        public float GetTotalWidth()
        {
            return m_rackWidth;
        }

        /// <summary>
        /// Gets total depth of the rack.
        /// </summary>
        public float GetTotalDepth()
        {
            float result = 0f;
            foreach (RackColumnProperties actColumn in m_columnProperties)
            {
                result += actColumn.Depth; 
            }
            return result;
        }

        /// <summary>
        /// Gets bounds of the bin described by given coordinates.
        /// </summary>
        /// <param name="column">The column of the bin (zero-based).</param>
        /// <param name="row">The row of the bin (zero-based).</param>
        public AxisAlignedBox GetBinBounds(int column, int row)
        {
            //Check parameters
            if (column >= m_columnProperties.Count) { throw new ArgumentOutOfRangeException("column"); }
            if (column < 0) { throw new ArgumentOutOfRangeException("column"); }
            if (row >= m_rowProperties.Count) { throw new ArgumentOutOfRangeException("row"); }
            if (row < 0) { throw new ArgumentOutOfRangeException("row"); }

            //Calculate column location
            float columnLocation = 0f;
            for (int loopColumn = 0; loopColumn < column; loopColumn++)
            {
                columnLocation += m_columnProperties[loopColumn].Depth;
            }

            //Calculate row location
            float rowLocation = 0f;
            for (int loopRow = 0; loopRow < row; loopRow++)
            {
                rowLocation += m_rowProperties[loopRow].Height;
            }

            Vector3 bottomMiddle = new Vector3(GetTotalWidth() / 2f, 0f, GetTotalDepth() / 2f);

            //Create and return the box
            AxisAlignedBox result = new AxisAlignedBox(
                new Vector3(0f, rowLocation, columnLocation) - bottomMiddle,
                new Vector3(m_rackWidth, m_rowProperties[row].Height, m_columnProperties[column].Depth));
            return result;
        }

        /// <summary>
        /// Builds the structure.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            VertexStructure[] result = new VertexStructure[2];
            result[0] = new VertexStructure();
            result[0].Material = m_columnMaterial;
            result[1] = new VertexStructure();
            result[1].Material = m_groundMaterial;

            //Gets origin coordinate
            float totalHeight = GetTotalHeight();
            float totalDepth = GetTotalDepth();
            Vector3 bottomMiddle = new Vector3(GetTotalWidth() / 2f, 0f, totalDepth / 2f);

            //Enable transformation of generated vertices
            result[0].EnableBuildTimeTransform(Matrix4.Translation(-bottomMiddle));
            result[1].EnableBuildTimeTransform(Matrix4.Translation(-bottomMiddle));

            //Build all columns
            float currentDepth = 0f;
            float lastRowHeight = m_rowProperties[m_rowProperties.Count - 1].Height;
            for (int loop = 0; loop <= m_columnProperties.Count; loop++)
            {
                if (loop > 0) { currentDepth += m_columnProperties[loop - 1].Depth; }

                result[0].BuildCube24V(
                    new Vector3(0f, 0f, currentDepth),
                    m_columnWidth, totalHeight - lastRowHeight +0.01f,
                    Color4.SteelBlue);
                result[0].BuildCube24V(
                    new Vector3(m_rackWidth, 0f, currentDepth),
                    m_columnWidth, totalHeight - lastRowHeight + 0.01f,
                    Color4.SteelBlue);
            }

            //Build all grounds
            float currentHeight = 0f;
            for (int loop = 1; loop < m_rowProperties.Count; loop++)
            {
                if (loop > 0) { currentHeight += m_rowProperties[loop - 1].Height; }

                result[1].BuildCube24V(
                    new Vector3(0, currentHeight - m_groundHeight, 0),
                    new Vector3(m_rackWidth, m_groundHeight, totalDepth),
                    Color4.LightSteelBlue);
            }

            //Disable transformation of generated vertices
            result[0].DisableBuildTimeTransform();
            result[1].DisableBuildTimeTransform();

            return result;
        }

        /// <summary>
        /// Gets a collection containing all row properties.
        /// </summary>
        public ReadOnlyCollection<RackRowProperties> Rows
        {
            get { return m_rowPropertiesPublic; }
        }

        /// <summary>
        /// Gets a collection containing all column properties.
        /// </summary>
        public ReadOnlyCollection<RackColumnProperties> Columns
        {
            get { return m_columnPropertiesPublic; }
        }

        /// <summary>
        /// Gets or sets the width of the rack.
        /// </summary>
        public float Width
        {
            get { return m_rackWidth; }
            set { m_rackWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of grounds.
        /// </summary>
        public float GroundHeight
        {
            get { return m_groundHeight; }
            set { m_groundHeight = value; }
        }

        /// <summary>
        /// Gets or sets the width of columns.
        /// </summary>
        public float ColumnWidth
        {
            get { return m_columnWidth; }
            set { m_columnWidth = value; }
        }

        /// <summary>
        /// Gets or sets the column material.
        /// </summary>
        public string ColumnMaterial
        {
            get { return m_columnMaterial; }
            set { m_columnMaterial = value; }
        }

        /// <summary>
        /// Gets or sets the ground material.
        /// </summary>
        public string GroundMaterial
        {
            get { return m_groundMaterial; }
            set { m_groundMaterial = value; }
        }

        /// <summary>
        /// Gets or sets the width of the rack.
        /// </summary>
        public float RackWidth
        {
            get { return m_rackWidth; }
            set { m_rackWidth = value; }
        }
    }
}
