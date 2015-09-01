using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Objects.Construction;

namespace Common.GraphicsEngine.Objects.ObjectTypes
{
    public class ComplexPalletType : ObjectType
    {
        //Members for pallet
        private float m_width;
        private float m_depth;
        private float m_palletHeight;
        private float m_smallFooterWidth;
        private float m_bigFooterWidth;
        private float m_boardHeight;
        private Color4 m_palletColor;
        private string m_palletMaterial;

        //Members for content
        private Dictionary<string, BoxTypeInfo> m_boxTypes;
        private List<PalletLayer> m_layerTemplates;
        private int m_boxCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexPalletType"/> class.
        /// </summary>
        public ComplexPalletType()
        {
            //Set measures of the pallet
            m_width = 0.8f;
            m_depth = 1.2f;
            m_palletHeight = 0.144f;
            m_smallFooterWidth = 0.10f;
            m_bigFooterWidth = 0.145f;
            m_boardHeight = 0.022f;
            m_palletMaterial = string.Empty;
            m_palletColor = Color4.DarkGoldenrod;

            //Initialize boxtype dictionary
            m_boxTypes = new Dictionary<string, BoxTypeInfo>();
            m_boxTypes.Add("Empty", new BoxTypeInfo(Vector3.Empty, string.Empty));

            m_layerTemplates = new List<PalletLayer>();
            m_boxCount = -1;
        }

        /// <summary>
        /// Adds a new box type.
        /// </summary>
        /// <param name="typeName">Name of the type object.</param>
        /// <param name="size">Size of the box.</param>
        public void AddBoxType(string typeName, Vector3 size, string material)
        {
            if (typeName == "Empty") { throw new ArgumentException("Empty is not possible as type name!", "typeName"); }
            m_boxTypes[typeName] = new BoxTypeInfo(size, material);
        }

        public void AddBarrelType(string typeName, Vector3 size, string material)
        {
            if (typeName == "Empty") { throw new ArgumentException("Empty is not possible as type name!", "typeName"); }
            m_boxTypes[typeName] = new BoxTypeInfo(size, material) { Structure = BoxStructure.Barrel };
        }

        /// <summary>
        /// Defines all possible layer types in order bottom-up.
        /// </summary>
        /// <param name="layerDescriptions">An array where each string describes a layer of the pallet</param>
        public void DefineLayers(params string[] layerDescriptions)
        {
            m_layerTemplates.Clear();
            if (layerDescriptions == null) { return; }

            //Generate all layers
            foreach (string actLayer in layerDescriptions)
            {
                m_layerTemplates.Add(new PalletLayer(actLayer, m_boxTypes)); 
            }
        }

        /// <summary>
        /// Builds the structure.
        /// </summary>
        public override VertexStructure[] BuildStructure()
        {
            //Get count of boxes
            int boxCount = BoxCount;
            if (boxCount == -1)
            {
                boxCount = 0;
                foreach (PalletLayer actLayer in m_layerTemplates)
                {
                    boxCount += actLayer.BoxCount;
                }
            }

            //Prepare result structure
            List<VertexStructure> result = new List<VertexStructure>();

            //Build pallet structure
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

            result.Add(new VertexStructure(408, 612));
            result[0].EnableTextureTileMode(new Vector2(0.2f, 0.2f));
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
            result[0].RelocateVerticesBy(new Vector3(-m_width / 2f, 0f, -m_depth / 2f));
            #endregion

            //Build content structures
            #region -----------------------------------------------------------
            List<string> materialIndices = new List<string>();
            if ((m_layerTemplates.Count > 0) && (boxCount > 0))
            {
                float currentHeight = m_palletHeight;
                int actLayerIndex = 0;
                int placedBoxes = 0;
                while ((boxCount > 0) && (placedBoxes < boxCount))
                {
                    //Get current layer object
                    PalletLayer actLayer = m_layerTemplates[actLayerIndex];

                    //Add all boxes to corresponding vertex structure
                    IEnumerable<BoxInfo> layerBoxes = actLayer.GetBoxes();
                    foreach (BoxInfo actBox in layerBoxes)
                    {
                        //Get material index (controls wich vertexstructure to use)
                        int materialIndex = -1;
                        if (materialIndices.Contains(actBox.BoxType.Material)) { materialIndex = materialIndices.IndexOf(actBox.BoxType.Material); }
                        else
                        {
                            materialIndices.Add(actBox.BoxType.Material);
                            materialIndex = materialIndices.Count - 1;
                            result.Add(new VertexStructure() { Material = actBox.BoxType.Material });
                        }
          
                        //Add new box to the structure
                        switch (actBox.BoxType.Structure)
                        {
                            case BoxStructure.Normal:
                                result[1 + materialIndex].BuildCube24V(
                                    new AxisAlignedBox(
                                        new Vector3(actBox.Location.X, currentHeight, actBox.Location.Z),
                                        actBox.BoxType.Bounds));
                                break;

                                //Builds a barrel on current location
                            case BoxStructure.Barrel:
                                Vector3 barrelSize = actBox.BoxType.Bounds;
                                Vector3 barrelHalfSize = barrelSize / 2f;
                                result[1 + materialIndex].BuildCylinderFullV(
                                    new Vector3(actBox.Location.X + barrelHalfSize.X, currentHeight, actBox.Location.Z + barrelHalfSize.Z),
                                    (barrelHalfSize.X < barrelHalfSize.Z ? barrelHalfSize.X : barrelHalfSize.Z) * 1.05f,
                                    barrelSize.Y * 0.05f,
                                    15,
                                    Color4.LightSteelBlue * 0.8f);
                                result[1 + materialIndex].BuildCylinderFullV(
                                    new Vector3(actBox.Location.X + barrelHalfSize.X, currentHeight + barrelSize.Y * 0.05f, actBox.Location.Z + barrelHalfSize.Z),
                                    barrelHalfSize.X < barrelHalfSize.Z ? barrelHalfSize.X : barrelHalfSize.Z,
                                    barrelSize.Y * 0.90f,
                                    15,
                                    Color4.LightSteelBlue * 0.8f);
                                result[1 + materialIndex].BuildCylinderFullV(
                                    new Vector3(actBox.Location.X + barrelHalfSize.X, currentHeight + barrelSize.Y * 0.95f, actBox.Location.Z + barrelHalfSize.Z),
                                    (barrelHalfSize.X < barrelHalfSize.Z ? barrelHalfSize.X : barrelHalfSize.Z) * 1.05f,
                                    barrelSize.Y * 0.05f,
                                    15,
                                    Color4.LightSteelBlue * 0.8f);
                                //result[1 + materialIndex].BuildCylinderFullV(
                                //    new Vector3(actBox.Location.X + barrelHalfSize.X, currentHeight + barrelSize.Y * 0.55f, actBox.Location.Z + barrelHalfSize.Z),
                                //    barrelHalfSize.X < barrelHalfSize.Z ? barrelHalfSize.X : barrelHalfSize.Z,
                                //    barrelSize.Y * 0.4f,
                                //    20,
                                //    Color4.LightSteelBlue);
                                //result[1 + materialIndex].BuildCylinderFullV(
                                //    new Vector3(actBox.Location.X + barrelHalfSize.X, currentHeight + barrelSize.Y * 0.95f, actBox.Location.Z + barrelHalfSize.Z),
                                //    (barrelHalfSize.X < barrelHalfSize.Z ? barrelHalfSize.X : barrelHalfSize.Z) * 1.05f,
                                //    barrelSize.Y * 0.05f,
                                //    20,
                                    //Color4.LightSteelBlue);
                                break;
                        }

                        placedBoxes++;
                        if (placedBoxes >= boxCount) { break; }
                    }

                    //Switch to next layer
                    currentHeight += actLayer.Height;
                    actLayerIndex++;
                    if (actLayerIndex >= m_layerTemplates.Count) { actLayerIndex = 0; }
                }
            }
            #endregion

            //Calculate normals and tangents for each structure
            foreach (VertexStructure actStructure in result)
            {
                actStructure.CalculateTangentsAndBinormals();
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets or sets the count of boxes to generate.
        /// </summary>
        public int BoxCount
        {
            get { return m_boxCount; }
            set { m_boxCount = value; }
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
        /// Gets or sets the color of the pallet.
        /// </summary>
        public Color4 PalletColor
        {
            get { return m_palletColor; }
            set { m_palletColor = value; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Class defining one layer of the pallet.
        /// </summary>
        private class PalletLayer
        {
            private string m_layerString;
            private List<BoxInfo> m_boxes;
            private float m_height;

            /// <summary>
            /// Initializes a new instance of the <see cref="PalletLayer"/> class.
            /// </summary>
            /// <param name="layerString">The layer string.</param>
            /// <param name="boxTypes">The box types.</param>
            public PalletLayer(string layerString, Dictionary<string, BoxTypeInfo> boxTypes)
            {
                m_layerString = layerString;
                m_boxes = new List<BoxInfo>();

                ApplyLayerString(boxTypes);
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return m_layerString;
            }

            /// <summary>
            /// Gets all boxes of this structure.
            /// </summary>
            public IEnumerable<BoxInfo> GetBoxes()
            {
                return m_boxes; 
            }

            /// <summary>
            /// Applies current layer string
            /// </summary>
            private void ApplyLayerString(Dictionary<string, BoxTypeInfo> boxTypes)
            {
                m_boxes.Clear();
                List<string[]> lines = new List<string[]>();

                //Read all lines
                StringReader stringReader = new StringReader(m_layerString);
                string actLine = string.Empty;
                while ((actLine = stringReader.ReadLine()) != null)
                {
                    actLine = actLine.Trim();
                    string[] splittedLine = actLine.Split(' ');

                    lines.Add(splittedLine);
                }
                stringReader.Close();

                //Analyse each line
                List<BoxTypeInfo[]> analysedLines = new List<BoxTypeInfo[]>();
                for (int loop = 0; loop < lines.Count; loop++)
                {
                    string[] actLineItems = lines[loop];
                    BoxTypeInfo[] analysedItems = new BoxTypeInfo[actLineItems.Length];

                    for (int innerLoop = 0; innerLoop < actLineItems.Length; innerLoop++)
                    {
                        if (!boxTypes.ContainsKey(actLineItems[innerLoop]))
                        {
                            throw new InvalidOperationException("Unable to build complex pallet: BoxType " + actLineItems[innerLoop] + " not found!"); 
                        }
                        analysedItems[innerLoop] = boxTypes[actLineItems[innerLoop]];
                    }

                    analysedLines.Add(analysedItems);
                }

                //Calculate bounds of the layer
                Vector3 layerSize = Vector3.MinValue;
                layerSize.X = 0f;
                foreach (BoxTypeInfo[] actLayerLine in analysedLines)
                {
                    float maxZ = 0f;
                    float maxX = 0f;
                    float maxY = 0f;

                    foreach (BoxTypeInfo actBox in actLayerLine)
                    {
                        maxZ += actBox.Bounds.Z;
                        if (maxX < actBox.Bounds.X) { maxX = actBox.Bounds.X; }
                        if (maxY < actBox.Bounds.Y) { maxY = actBox.Bounds.Y; }
                    }

                    layerSize.X += maxX;
                    if (maxZ > layerSize.Z) { layerSize.Z = maxZ; }
                    if (maxY > layerSize.Y) { layerSize.Y = maxY; }
                }

                //Build layer structure
                m_height = layerSize.Y;
                Vector3 startLocation = new Vector3(-layerSize.X / 2f, 0f, -layerSize.Z / 2f);
                Vector3 currentLocation = startLocation;
                foreach (BoxTypeInfo[] actLayerLine in analysedLines)
                {
                    //Count empty boxes within this line
                    int emptyBoxCount = 0;
                    float fullBoxDepth = 0f;
                    foreach (BoxTypeInfo actBoxType in actLayerLine) 
                    {
                        if (actBoxType.IsEmpty) { emptyBoxCount++; }
                        else { fullBoxDepth += actBoxType.Bounds.Z; }
                    }

                    //Calculate empty box size
                    float emptyBoxDepth = emptyBoxCount > 0 ? (layerSize.Z - fullBoxDepth) / (float)emptyBoxCount : 0f;

                    float maxX = 0f;
                    foreach (BoxTypeInfo actBoxType in actLayerLine)
                    {
                        if (actBoxType.IsEmpty)
                        {
                            //Handle empty space
                            currentLocation.Z = currentLocation.Z + emptyBoxDepth;
                        }
                        else
                        {
                            BoxInfo boxInfo = new BoxInfo(actBoxType, currentLocation);
                            m_boxes.Add(boxInfo);

                            //Prepare next location
                            currentLocation.Z = currentLocation.Z + actBoxType.Bounds.Z;
                            if (maxX < actBoxType.Bounds.X) { maxX = actBoxType.Bounds.X; }
                        }
                    }

                    //Prepare location of next line
                    currentLocation.X = currentLocation.X + maxX;
                    currentLocation.Z = startLocation.Z;
                }
            }

            /// <summary>
            /// Gets the height of this layer.
            /// </summary>
            public float Height
            {
                get { return m_height; }
            }

            /// <summary>
            /// Gets total count of boxes within this layer.
            /// </summary>
            public int BoxCount
            {
                get { return m_boxes.Count; }
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Structure describing a box type.
        /// </summary>
        private struct BoxTypeInfo
        {
            public BoxStructure Structure;
            public Vector3 Bounds;
            public string Material;

            /// <summary>
            /// Initializes a new instance of the <see cref="BoxTypeInfo"/> struct.
            /// </summary>
            /// <param name="bounds">The bounds of the box.</param>
            /// <param name="material">The material.</param>
            public BoxTypeInfo(Vector3 bounds, string material)
            {
                this.Bounds = bounds;
                this.Material = material;
                this.Structure = BoxStructure.Normal;
            }

            /// <summary>
            /// Is this type an empty type?
            /// </summary>
            public bool IsEmpty
            {
                get { return Bounds.IsEmpty() && string.IsNullOrEmpty(Material); }
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Structure describing a box within a layer.
        /// </summary>
        private struct BoxInfo
        {
            public BoxTypeInfo BoxType;
            public Vector3 Location;

            /// <summary>
            /// Initializes a new instance of the <see cref="BoxInfo"/> struct.
            /// </summary>
            /// <param name="boxType">Type of the box.</param>
            /// <param name="location">The location.</param>
            public BoxInfo(BoxTypeInfo boxType, Vector3 location)
            {
                this.BoxType = boxType;
                this.Location = location;
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private enum BoxStructure
        {
            Normal,

            Barrel
        }
    }
}
