using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using RK.Common.GraphicsEngine.Animations;
using RK.Common;
using RK.Common.GraphicsEngine.Drawing3D;
using RK.Common.GraphicsEngine.Drawing3D.Resources;
using RK.Common.GraphicsEngine.Objects;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.ObjectTypes;
using RK.Common.GraphicsEngine.Core;
using System.IO;

namespace RK.Store.MetroSnake.Game
{
    public class GraphicsContainer
    {
        public const string MATERIAL_PLAIN = "PlainMaterial";

        private Scene m_scene;
        private MapProperties m_mapProperties;
        private Random m_randomizer;

        //Global texture resources
        private SimpleColoredMaterialResource[] m_tileMaterials;

        //Global geometry resources
        private GeometryResource[] m_gemModelResources;
        private GeometryResource m_wormHeadResource;
        private GeometryResource m_wormElementResource;
        private GeometryResource m_groundResource;

        //Global objects
        private GenericObject m_wormHeadObject;
        private GenericObject m_groundObject;
        private GenericObject m_gemObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsContainer" /> class.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="mapProperties">The map properties.</param>
        public GraphicsContainer(Scene scene, MapProperties mapProperties)
        {
            m_randomizer = new Random(Environment.TickCount);
            m_mapProperties = mapProperties;
            m_scene = scene;

            //Define materials for the tilemap
            m_tileMaterials = new SimpleColoredMaterialResource[3];
            for (int loopTex = 1; loopTex <= m_tileMaterials.Length; loopTex++)
            {
                var actTexture = scene.Resources.AddTexture("TextureTile" + loopTex, "Assets\\Textures\\Tile" + loopTex + ".png");
                m_tileMaterials[loopTex - 1] = scene.Resources.AddSimpleTexturedMaterial("MaterialTile" + loopTex, actTexture.Name);
                m_tileMaterials[loopTex - 1].Ambient = 0.1f;
            }
            var tileBackgroundMaterial = scene.Resources.AddSimpleColoredMaterial("MaterialTileBackground");
            tileBackgroundMaterial.Ambient = 0.6f;  

            scene.Resources.AddTexture("WormBodyTexture", "Assets\\Textures\\SnakeBody.png");
            scene.Resources.AddSimpleTexturedMaterial("WormBodyMaterial", "WormBodyTexture");
            scene.Resources.AddTexture("AppleSkinTexture", "Assets\\Textures\\AppleSkin.jpg");
            scene.Resources.AddSimpleTexturedMaterial("AppleSkinMaterial", "AppleSkinTexture");

            //Create background texture painter on the scene
            scene.Resources.AddTexture("BackgroundTexture", "Assets\\Textures\\Background.png");
            scene.Add(new TexturePainter("BackgroundTexture"));

            //Define all materials
            scene.Resources.AddSimpleColoredMaterial("AC3D-Material");
            var materialPlain = scene.Resources.AddSimpleColoredMaterial(MATERIAL_PLAIN);
            var materialAc3D = scene.Resources.AddSimpleColoredMaterial("ac3dmat14");
            materialAc3D.Ambient = 0.8f;

            //Define models for worm heads and elements
            GenericObjectType objTypeWormElement = ObjectType.FromACFile("Assets\\Models\\WormElement.ac") as GenericObjectType;
            GenericObjectType objTypeWormHead = ObjectType.FromACFile("Assets\\Models\\WormHead.ac") as GenericObjectType;
            objTypeWormElement.ApplyMaterialForAll("WormBodyMaterial");
            objTypeWormHead.ConvertMaterial("ac3dmat6", "WormBodyMaterial");
            objTypeWormHead.ConvertMaterial("ac3dmat1", "AC3D-Material");
            objTypeWormHead.ConvertMaterial("ac3dmat3", "AC3D-Material");
            m_wormHeadResource = scene.Resources.AddGeometry("WormHeadGeometry", objTypeWormHead);
            m_wormElementResource = scene.Resources.AddGeometry("WormElementGeometry", objTypeWormElement);

            //Define models for gems
            string[] gemModelPaths = new string[]
            {
                "Assets\\Models\\Banana.ac",
                "Assets\\Models\\Apple.ac",
                "Assets\\Models\\Donut.ac"
            };
            m_gemModelResources = new GeometryResource[gemModelPaths.Length];
            for (int loop = 0; loop < gemModelPaths.Length; loop++)
            {
                GenericObjectType actGemObjectType = ObjectType.FromACFile(gemModelPaths[loop]) as GenericObjectType;
                actGemObjectType.ConvertMaterial("ac3dmat2", "AC3D-Material");
                actGemObjectType.ConvertMaterial("ac3dmat3", "AppleSkinMaterial");
                actGemObjectType.ConvertMaterial("ac3dmat5", "AC3D-Material");
                actGemObjectType.ConvertMaterial("ac3dmat4", "AC3D-Material");
                actGemObjectType.ConvertMaterial("ac3dmat0", "AC3D-Material");
                actGemObjectType.VertexStructures.FitToCenteredCuboid(0.6f, 0.6f, 0.6f, FitToCuboidMode.MaintainAspectRatio, FitToCuboidOrigin.LowerCenter);
                m_gemModelResources[loop] = scene.Resources.AddGeometry("GemResource:" + Path.GetFileName(gemModelPaths[loop]), actGemObjectType);
            }

            //Load resources for the ground
            m_groundResource = scene.Resources.AddGeometry("GroundGeometry", BuildGroundStructures(m_mapProperties, Vector3.Empty));

            //Prepare all standard objects
            m_wormHeadObject = new GenericObject(m_wormHeadResource.Name);
            m_wormHeadObject.RotationType = RotationType.HWAngles;
            m_groundObject = new GenericObject(m_groundResource.Name);
            m_gemObject = new GenericObject(m_gemModelResources[0].Name);

            //Loads the title resource and displays it
            var titleResource = scene.Resources.AddGeometry("TitleText3D", ObjectType.FromACFile("Assets\\Models\\Title3D.ac"));
            var titleObject = new GenericObject(titleResource.Name);
            titleObject.Position = new Vector3(m_mapProperties.TilesX * -0.5f, -0.2f, m_mapProperties.TilesY * 0.5f + 0.2f);
            scene.Add(titleObject);

            //Add standard objects to the scene
            scene.Add(m_groundObject);
            scene.Add(m_gemObject);

            var additionalField = scene.Resources.AddGeometry("GroundGeometryTemp", BuildGroundStructures(
                new MapProperties() { TilesX = 4, TilesY = m_mapProperties.TilesY },
                new Vector3(-8f, 0f, 0f)));
            scene.Add(new GenericObject(additionalField.Name));
            additionalField = scene.Resources.AddGeometry("GroundGeometryTemp2", BuildGroundStructures(
                new MapProperties() { TilesX = 4, TilesY = m_mapProperties.TilesY },
                new Vector3(-13f, 0f, 0f)));
            scene.Add(new GenericObject(additionalField.Name));

            additionalField = scene.Resources.AddGeometry("GroundGeometryTemp3", BuildGroundStructures(
                new MapProperties() { TilesX = 4, TilesY = m_mapProperties.TilesY },
                new Vector3(8f, 0f, 0f)));
            scene.Add(new GenericObject(additionalField.Name));
            additionalField = scene.Resources.AddGeometry("GroundGeometryTemp4", BuildGroundStructures(
                new MapProperties() { TilesX = 4, TilesY = m_mapProperties.TilesY },
                new Vector3(13f, 0f, 0f)));
            scene.Add(new GenericObject(additionalField.Name));

            //Define endles animation for gems
            m_gemObject.BeginAnimationSequence()
                .Scale3D(new Vector3(0.8f, 0.8f, 0.8f), TimeSpan.FromMilliseconds(500))
                .WaitFinished()
                .Scale3D(new Vector3(1f / 0.8f, 1f / 0.8f, 1f / 0.8f), TimeSpan.FromMilliseconds(500))
                .WaitFinished()
                .FinishAndRewind();
        }

        /// <summary>
        /// Changes the current gem model.
        /// </summary>
        public void ChangeGemModel()
        {
            m_gemObject.ChangeGeometry(m_gemModelResources[m_randomizer.Next(0, m_gemModelResources.Length)].Name);
        }

        /// <summary>
        /// Gets the index of the current gem model.
        /// </summary>
        public int GetCurrentGemModelIndex()
        {
            for (int loop = 0; loop < m_gemModelResources.Length; loop++)
            {
                if (m_gemModelResources[loop].Name == m_gemObject.GeometryResource.Name) { return loop; }
            }
            return 0;
        }

        /// <summary>
        /// Creates a new worm element on the location of the given one.
        /// </summary>
        /// <param name="lastWormelement"></param>
        /// <returns></returns>
        public GenericObject CreateWormElement(WormElement lastWormelement)
        {
            GenericObject result = new GenericObject(m_wormElementResource.Name)
            {
                RotationType = RotationType.HWAngles,
                Position = lastWormelement.Object3D.Position
            };
            result.Scaling = new Vector3(0.5f, 0.5f, 0.5f);
            result.RotationHV = lastWormelement.Object3D.RotationHV;//new Vector2((float)Math.PI / 2f, 0f);
            result.BeginAnimationSequence()
                .Scale3D(new Vector3(2f, 2f, 2f), TimeSpan.FromMilliseconds(500))
                .Finish();
            m_scene.Add(result);
            return result;
        }

        /// <summary>
        /// Builds all structures for the worm's head.
        /// </summary>
        private VertexStructure[] BuildWormHeadStructures()
        {
            //Vertex structures for worm head
            VertexStructure[] wormHeadStructures = new VertexStructure[3];
            wormHeadStructures[0] = new VertexStructure();
            wormHeadStructures[0].BuildShpere(10, 50, 0.4, Color4.Blue);
            wormHeadStructures[0].Material = MATERIAL_PLAIN;
            wormHeadStructures[0].CalulateNormals();

            wormHeadStructures[1] = new VertexStructure();
            wormHeadStructures[1].BuildShpere(10, 10, 0.05, Color4.Yellow);
            wormHeadStructures[1].Material = MATERIAL_PLAIN;
            wormHeadStructures[1].UpdateVerticesUsingRelocationBy(new Vector3(0.15f, 0.3f, 0.15f));
            wormHeadStructures[1].CalulateNormals();

            wormHeadStructures[2] = new VertexStructure();
            wormHeadStructures[2].BuildShpere(10, 10, 0.05, Color4.Yellow);
            wormHeadStructures[2].Material = MATERIAL_PLAIN;
            wormHeadStructures[2].UpdateVerticesUsingRelocationBy(new Vector3(-0.15f, 0.3f, 0.15f));
            wormHeadStructures[2].CalulateNormals();

            return wormHeadStructures;
        }

        /// <summary>
        /// Builds the structure of one of the worm's elements.
        /// </summary>
        private VertexStructure[] BuildWormElementStructure()
        {
            VertexStructure[] wormElementStructures = new VertexStructure[1];
            wormElementStructures[0] = new VertexStructure();
            wormElementStructures[0].BuildShpere(10, 20, 0.38, Color4.CornflowerBlue);
            wormElementStructures[0].Material = MATERIAL_PLAIN;
            wormElementStructures[0].CalulateNormals();

            return wormElementStructures;
        }

        /// <summary>
        /// Builds all structures for the main ground.
        /// </summary>
        private VertexStructure[] BuildGroundStructures(MapProperties mapProperties, Vector3 center)
        {
            bool[,] tilesPlaced = new bool[mapProperties.TilesX, mapProperties.TilesY];

            Random mapRandomizer = m_randomizer;

            //Prepare all vertex structures
            VertexStructure[] groundStructures = new VertexStructure[m_tileMaterials.Length + 1];
            for (int loop = 0; loop < groundStructures.Length; loop++)
            {
                groundStructures[loop] = new VertexStructure();
            }

            //Create tilemap
            int halfX = mapProperties.TilesX / 2;
            int halfZ = mapProperties.TilesY / 2;
            for (int loopX = -halfX; loopX < halfX; loopX++)
            {
                int indexX = loopX + halfX;
                for (int loopZ = -halfZ; loopZ < halfZ; loopZ++)
                {
                    int indexZ = loopZ + halfZ;

                    if (tilesPlaced[indexX, indexZ]) { continue; }
                    tilesPlaced[indexX, indexZ] = true;

                    //Make a double tile?
                    float tileWidth = 0.9f;
                    if ((indexX % 2 == 0) && (loopX < halfX - 1))
                    {
                        if (mapRandomizer.Next(0, 1000) > 600) 
                        {
                            tileWidth = 1.9f;
                            tilesPlaced[indexX + 1, indexZ] = true;
                        }
                    }
                    
                    //Place current tile on the map
                    Vector3 origin = new Vector3(
                        loopX * 1f + 0.05f, 0f, loopZ * 1f + 0.05f);
                    origin = origin + center;
                    int actTileTexture = mapRandomizer.Next(0, m_tileMaterials.Length);
                    groundStructures[actTileTexture].BuildRect4V(
                        origin,
                        origin + new Vector3(tileWidth, 0f, 0f),
                        origin + new Vector3(tileWidth, 0f, .9f),
                        origin + new Vector3(0f, 0f, .9f),
                        Color4.White);
                }
            }

            groundStructures[groundStructures.Length - 1].BuildRect4V(
                new Vector3(-mapProperties.TilesX / 2f, -0.01f, -mapProperties.TilesY / 2f) + center,
                new Vector3(mapProperties.TilesX / 2f, -0.01f, -mapProperties.TilesY / 2f) + center,
                new Vector3(mapProperties.TilesX / 2f, -0.01f, mapProperties.TilesY / 2f) + center,
                new Vector3(-mapProperties.TilesX / 2f, -0.01f, mapProperties.TilesY / 2f) + center,
                CommonUtil.MixColors(Color4.CornflowerBlue, Color4.LightSteelBlue));
            groundStructures[groundStructures.Length - 1].Material = "MaterialTileBackground";

            //Postprocess all vertex structures
            for (int loop = 0; loop < groundStructures.Length; loop++)
            {
                groundStructures[loop].UpdateVerticesUsingRelocationBy(new Vector3(0f, -0.15f, 0f));
                if (loop < m_tileMaterials.Length)
                {
                    groundStructures[loop].Material = m_tileMaterials[loop].Name;
                }
                groundStructures[loop].CalulateNormals();
            }

            return groundStructures;
        }

        /// <summary>
        /// Gets the object of the worm's head.
        /// </summary>
        public GenericObject WormHead
        {
            get { return m_wormHeadObject; }
        }

        /// <summary>
        /// Gets the object of a gem.
        /// </summary>
        public GenericObject Gem
        {
            get { return m_gemObject; }
        }
    }
}
