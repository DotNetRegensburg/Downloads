using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Common.GraphicsEngine.Objects.Construction;

namespace Common.GraphicsEngine.Objects.ObjectTypes
{
    public class PalletType : ObjectType
    {
        private float m_width;
        private float m_depth;
        private float m_palletHeight;
        private float m_contentHeight;
        private float m_smallFooterWidth;
        private float m_bigFooterWidth;
        private float m_boardHeight;
        private Color4 m_contentColor;
        private Color4 m_palletColor;
        private string m_palletMaterial;
        private string m_contentMaterial;

        /// <summary>
        /// Creates a new pallet type with default size
        /// </summary>
        public PalletType(string palletMaterial, string contentMaterial)
            : this(palletMaterial, contentMaterial, 0.8f, 1.2f, 0.144f, 1f, 0.10f, 0.145f, 0.022f)
        {

        }

        /// <summary>
        /// Creates a new pallet type with given size settings
        /// </summary>
        public PalletType(string palletMaterial, string contentMaterial, float width, float depth, float palletHeight, float contentHeight, float smallFooterWidth, float bigFooterWidth, float boardHeight)
        {
            m_width = width;
            m_depth = depth;
            m_palletHeight = palletHeight;
            m_contentHeight = contentHeight;
            m_bigFooterWidth = bigFooterWidth;
            m_smallFooterWidth = smallFooterWidth;
            m_boardHeight = boardHeight;

            m_palletMaterial = palletMaterial;
            m_contentMaterial = contentMaterial;

            m_palletColor = Color4.DarkGoldenrod;
            m_contentColor = Color4.White;
        }

        /// <summary>
        /// Builds the structure needed for the pallet
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            VertexStructure[] result = new VertexStructure[3];

            //Build pallet
            #region -----------------------------------------------------------
            float middleFront = m_width / 2f;
            float middleSide = m_depth / 2f;
            float middleFrontBegin = middleFront - m_bigFooterWidth / 2f;
            float middleSideBegin = middleSide - m_bigFooterWidth / 2f;
            float lastBeginSmall = m_width - m_smallFooterWidth;
            float lastBeginBig = m_depth - m_bigFooterWidth;
            float footerHeight = m_palletHeight - m_boardHeight * 3f;
            float quarterFrontBegin = ((m_bigFooterWidth / 2f) + ((middleFront - (m_bigFooterWidth / 2f)) / 2f)) - (m_smallFooterWidth / 2f);// +(middleFront / 2f - m_smallFooterWidth / 2f);
            float threeQuarterFrontBegin = middleFront + (middleFront - quarterFrontBegin - m_smallFooterWidth);//(middleFront / 2f) * 3f - m_smallFooterWidth / 2f;

            result[0] = new VertexStructure(408, 612);
            result[0].Material = m_palletMaterial;

            //Build 3 board on bottom
            result[0].BuildCube24V(new Vector3(0f, 0f, 0f), new Vector3(m_smallFooterWidth, m_boardHeight, m_depth), m_palletColor);
            result[0].BuildCube24V(new Vector3(middleFrontBegin, 0f, 0f), new Vector3(m_bigFooterWidth, m_boardHeight, m_depth), m_palletColor);
            result[0].BuildCube24V(new Vector3(lastBeginSmall, 0f, 0f), new Vector3(m_smallFooterWidth, m_boardHeight, m_depth), m_palletColor);

            //Build 9 footers
            result[0].BuildCubeSides16V(new Vector3(0f, m_boardHeight, 0f), new Vector3(m_smallFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(0f, m_boardHeight, middleSideBegin), new Vector3(m_smallFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(0f, m_boardHeight, lastBeginBig), new Vector3(m_smallFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(middleFrontBegin, m_boardHeight, 0f), new Vector3(m_bigFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(middleFrontBegin, m_boardHeight, middleSideBegin), new Vector3(m_bigFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(middleFrontBegin, m_boardHeight, lastBeginBig), new Vector3(m_bigFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(lastBeginSmall, m_boardHeight, 0f), new Vector3(m_smallFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(lastBeginSmall, m_boardHeight, middleSideBegin), new Vector3(m_smallFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCubeSides16V(new Vector3(lastBeginSmall, m_boardHeight, lastBeginBig), new Vector3(m_smallFooterWidth, footerHeight, m_bigFooterWidth), m_palletColor);

            //Build boards above footers
            result[0].BuildCube24V(new Vector3(0f, m_boardHeight + footerHeight, 0f), new Vector3(m_width, m_boardHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCube24V(new Vector3(0f, m_boardHeight + footerHeight, middleSideBegin), new Vector3(m_width, m_boardHeight, m_bigFooterWidth), m_palletColor);
            result[0].BuildCube24V(new Vector3(0f, m_boardHeight + footerHeight, lastBeginBig), new Vector3(m_width, m_boardHeight, m_bigFooterWidth), m_palletColor);

            //Build top boards
            float localYPos = m_palletHeight - m_boardHeight;
            result[0].BuildCube24V(new Vector3(0f, localYPos, 0f), new Vector3(m_bigFooterWidth, m_boardHeight, m_depth), m_palletColor);
            result[0].BuildCube24V(new Vector3(middleFrontBegin, localYPos, 0f), new Vector3(m_bigFooterWidth, m_boardHeight, m_depth), m_palletColor);
            result[0].BuildCube24V(new Vector3(m_width - m_bigFooterWidth, localYPos, 0f), new Vector3(m_bigFooterWidth, m_boardHeight, m_depth), m_palletColor);
            result[0].BuildCube24V(new Vector3(quarterFrontBegin, localYPos, 0f), new Vector3(m_smallFooterWidth, m_boardHeight, m_depth), m_palletColor);
            result[0].BuildCube24V(new Vector3(threeQuarterFrontBegin, localYPos, 0f), new Vector3(m_smallFooterWidth, m_boardHeight, m_depth), m_palletColor);
            #endregion

            //Build content sides
            #region -----------------------------------------------------------
            result[1] = new VertexStructure();
            result[1].Material = m_contentMaterial;
            result[1].BuildCubeSides16V(new Vector3(0f, m_palletHeight, 0f), new Vector3(m_width, m_contentHeight, m_depth), m_contentColor);
            #endregion

            //Build content top and bottom
            #region -----------------------------------------------------------
            result[2] = new VertexStructure();
            result[2].Material = m_contentMaterial;
            result[2].BuildCubeTop4V(new Vector3(0f, m_palletHeight, 0f), new Vector3(m_width, m_contentHeight, m_depth), m_contentColor);
            result[2].BuildCubeBottom4V(new Vector3(0f, m_palletHeight, 0f), new Vector3(m_width, m_contentHeight, m_depth), m_contentColor);
            #endregion

            //Relocate center point to bottom middle
            result[0].RelocateVerticesBy(new Vector3(-m_width / 2f, 0f, -m_depth / 2f));
            result[1].RelocateVerticesBy(new Vector3(-m_width / 2f, 0f, -m_depth / 2f));
            result[2].RelocateVerticesBy(new Vector3(-m_width / 2f, 0f, -m_depth / 2f));

            //Calculate all tangents and binormals
            result[0].CalculateTangentsAndBinormals();
            result[1].CalculateTangentsAndBinormals();
            result[2].CalculateTangentsAndBinormals();

            return result;
        }

        /// <summary>
        /// Gets or sets the width of the object
        /// </summary>
        public float Width
        {
            get { return m_width; }
            set
            {
                if (m_width != value)
                {
                    m_width = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the depth of the object
        /// </summary>
        public float Depth
        {
            get { return m_depth; }
            set
            {
                if (m_depth != value)
                {
                    m_depth = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of a pallet
        /// </summary>
        public float PalletHeight
        {
            get { return m_palletHeight; }
            set
            {
                if (m_palletHeight != value)
                {
                    m_palletHeight = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the content
        /// </summary>
        public float ContentHeight
        {
            get { return m_contentHeight; }
            set
            {
                if (m_contentHeight != value)
                {
                    m_contentHeight = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the small footer
        /// </summary>
        public float SmallFooterWidth
        {
            get { return m_smallFooterWidth; }
            set
            {
                if (m_smallFooterWidth != value)
                {
                    m_smallFooterWidth = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the big footer
        /// </summary>
        public float BigFooterWidth
        {
            get { return m_bigFooterWidth; }
            set
            {
                if (m_bigFooterWidth != value)
                {
                    m_bigFooterWidth = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of a board
        /// </summary>
        public float BoardHeight
        {
            get { return m_boardHeight; }
            set
            {
                if (m_boardHeight != value)
                {
                    m_boardHeight = value;
                    //base.RefreshStructure();
                }
            }
        }

        /// <summary>
        /// Gets or sets the material of the pallet.
        /// </summary>
        public string PalletMaterial
        {
            get { return m_palletMaterial; }
            set { m_palletMaterial = value; }
        }

        /// <summary>
        /// Gets or sets the material of the content.
        /// </summary>
        public string ContentMaterial
        {
            get { return m_contentMaterial; }
            set { m_contentMaterial = value; }
        }
    }
}
