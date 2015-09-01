using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using RK.Common.GraphicsEngine.Objects.Construction;
using RK.Common.GraphicsEngine.Objects.ObjectTypes;

namespace RK.Common.GraphicsEngine.Objects.Loaders
{
    public static class ACFileLoader
    {
        /// <summary>
        /// Imports an object-type form given raw model file.
        /// </summary>
        /// <param name="rawBytes">Raw model file.</param>
        public static ObjectType ImportObjectType(byte[] rawBytes)
        {
            using (MemoryStream inStream = new MemoryStream(rawBytes))
            {
                return ImportObjectType(inStream);
            }
        }

        /// <summary>
        /// Loads an object from the given uri
        /// </summary>
        public static ObjectType ImportObjectType(Stream inStream)
        {
            ACFileInfo fileInfo = LoadFile(inStream);

            VertexStructure[] structures = GenerateStructures(fileInfo);
            return new GenericObjectType(structures);
        }

        /// <summary>
        /// Generates all vertex structures needed for this object
        /// </summary>
        private static VertexStructure[] GenerateStructures(ACFileInfo fileInfo)
        {
            List<VertexStructure> result = new List<VertexStructure>();

            for (int loop = 0; loop < fileInfo.Materials.Count; loop++)
            {
                VertexStructure actStructure = new VertexStructure();
                ACMaterialInfo actMaterial = fileInfo.Materials[loop];
                Matrix4Stack transformStack = new Matrix4Stack();
                foreach (ACObjectInfo actObject in fileInfo.Objects)
                {
                    FillVertexStructure(actStructure, loop, actMaterial, actObject, transformStack);
                }

                actStructure.Material = actMaterial.Name;
                actStructure.MaterialProperties.DiffuseColor = actMaterial.Diffuse;
                actStructure.MaterialProperties.AmbientColor = actMaterial.Ambient;
                actStructure.MaterialProperties.EmissiveColor = actMaterial.Emissive;
                actStructure.MaterialProperties.Specular = actMaterial.Specular;
                actStructure.MaterialProperties.Shininess = actMaterial.Shininess;

                if (actStructure.CountIndexes > 0)
                {
                    result.Add(actStructure);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Fills the given vertex structure 
        /// </summary>
        private static void FillVertexStructure(VertexStructure structure, int matIndex, ACMaterialInfo materialInfo, ACObjectInfo objInfo, Matrix4Stack transformStack)
        {
            transformStack.Push();
            {
                transformStack.TransformLocal(objInfo.Rotation);
                transformStack.TranslateLocal(objInfo.Translation);

                //Initialize local index table
                ushort[] localIndices = new ushort[objInfo.Vertices.Count];
                for (int loop = 0; loop < localIndices.Length; loop++)
                {
                    localIndices[loop] = 0;
                }

                foreach (ACSurface actSurface in objInfo.Surfaces)
                {
                    if (actSurface.Material == matIndex)
                    {
                        for (int loop = 0; loop < actSurface.VertexReferences.Count; loop++)
                        {
                            //Build vertex information
                            int actReference = actSurface.VertexReferences[loop];
                            Vector2 actTexCoord = actSurface.TextureCoordinates[loop];

                            Vector3 actPosition = objInfo.Vertices[actReference].Position;
                            actPosition.Transform(transformStack.Top);

                            localIndices[actReference] = structure.AddVertex(new Vertex(
                                actPosition, materialInfo.Diffuse, actTexCoord, Vector3.Empty));
                        }

                        //Build triangles
                        switch (actSurface.VertexReferences.Count)
                        {
                            case 3:
                                structure.AddTriangleAndCalculateNormals(
                                    localIndices[actSurface.VertexReferences[0]],
                                    localIndices[actSurface.VertexReferences[1]],
                                    localIndices[actSurface.VertexReferences[2]]);
                                ////back side
                                //structure.AddTriangleAndCalculateNormals(
                                //    localIndices[actSurface.VertexReferences[2]],
                                //    localIndices[actSurface.VertexReferences[1]],
                                //    localIndices[actSurface.VertexReferences[0]]);
                                break;

                            case 4:
                                structure.AddTriangleAndCalculateNormals(
                                    localIndices[actSurface.VertexReferences[0]],
                                    localIndices[actSurface.VertexReferences[1]],
                                    localIndices[actSurface.VertexReferences[2]]);
                                structure.AddTriangleAndCalculateNormals(
                                    localIndices[actSurface.VertexReferences[2]],
                                    localIndices[actSurface.VertexReferences[3]],
                                    localIndices[actSurface.VertexReferences[0]]);
                                ////back side
                                //structure.AddTriangleAndCalculateNormals(
                                //    localIndices[actSurface.VertexReferences[2]],
                                //    localIndices[actSurface.VertexReferences[1]],
                                //    localIndices[actSurface.VertexReferences[0]]);
                                //structure.AddTriangleAndCalculateNormals(
                                //    localIndices[actSurface.VertexReferences[0]],
                                //    localIndices[actSurface.VertexReferences[3]],
                                //    localIndices[actSurface.VertexReferences[2]]);
                                break;

                            default:
                                structure.AddPolygonByCuttingEarsAndCalculateNormals(actSurface.VertexReferences);
                                break;
                        }
                    }
                }

                //Fill in all child object data
                foreach (ACObjectInfo actObjInfo in objInfo.Childs)
                {
                    FillVertexStructure(structure, matIndex, materialInfo, actObjInfo, transformStack);
                }
            }
            transformStack.Pop();
        }

        /// <summary>
        /// Loads a ac file from the given uri
        /// </summary>
        private static ACFileInfo LoadFile(Stream inStream)
        {
            ACFileInfo result = null;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(inStream);

                //Check for correct header
                string header = reader.ReadLine();
                if (!header.StartsWith("AC3D")) { throw new GraphicsEngineException("Header of AC3D file not found!"); }

                //Create file information object
                result = new ACFileInfo();

                //Create a loaded objects stack
                Stack<ACObjectInfo> loadedObjects = new Stack<ACObjectInfo>();
                Stack<ACObjectInfo> parentObjects = new Stack<ACObjectInfo>();
                ACSurface currentSurface = null;

                //Read the file
                while (!reader.EndOfStream)
                {
                    string actLine = reader.ReadLine().Trim();

                    string firstWord = string.Empty;
                    int spaceIndex = actLine.IndexOf(' ');
                    if (spaceIndex == -1) { firstWord = actLine; }
                    else { firstWord = firstWord = actLine.Substring(0, spaceIndex); }

                    switch (firstWord)
                    {
                        //New Material info
                        case "MATERIAL":
                            ACMaterialInfo materialInfo = new ACMaterialInfo();
                            {
                                //Get the name of the material
                                string[] materialData = actLine.Split(' ');
                                if (materialData.Length > 1) { materialInfo.Name = materialData[1].Trim(' ', '"'); }

                                //Parse components
                                for (int loop = 0; loop < materialData.Length; loop++)
                                {
                                    switch (materialData[loop])
                                    {
                                        case "rgb":
                                            Color4 diffuseColor = new Color4();
                                            diffuseColor.A = 1f;
                                            diffuseColor.R = Single.Parse(materialData[loop + 1], CultureInfo.InvariantCulture);
                                            diffuseColor.G = Single.Parse(materialData[loop + 2], CultureInfo.InvariantCulture);
                                            diffuseColor.B = Single.Parse(materialData[loop + 3], CultureInfo.InvariantCulture);
                                            materialInfo.Diffuse = diffuseColor;
                                            break;

                                        case "amb":
                                            Color4 ambientColor = new Color4();
                                            ambientColor.R = Single.Parse(materialData[loop + 1], CultureInfo.InvariantCulture);
                                            ambientColor.G = Single.Parse(materialData[loop + 2], CultureInfo.InvariantCulture);
                                            ambientColor.B = Single.Parse(materialData[loop + 3], CultureInfo.InvariantCulture);
                                            materialInfo.Ambient = ambientColor;
                                            break;

                                        case "emis":
                                            Color4 emissiveColor = new Color4();
                                            emissiveColor.R = Single.Parse(materialData[loop + 1], CultureInfo.InvariantCulture);
                                            emissiveColor.G = Single.Parse(materialData[loop + 2], CultureInfo.InvariantCulture);
                                            emissiveColor.B = Single.Parse(materialData[loop + 3], CultureInfo.InvariantCulture);
                                            materialInfo.Emissive = emissiveColor;
                                            break;

                                        case "spec":
                                            Color4 specularColor = new Color4();
                                            specularColor.R = Single.Parse(materialData[loop + 1], CultureInfo.InvariantCulture);
                                            specularColor.G = Single.Parse(materialData[loop + 2], CultureInfo.InvariantCulture);
                                            specularColor.B = Single.Parse(materialData[loop + 3], CultureInfo.InvariantCulture);
                                            materialInfo.Specular = specularColor;
                                            break;

                                        case "shi":
                                            materialInfo.Shininess = Single.Parse(materialData[loop + 1], CultureInfo.InvariantCulture);
                                            break;

                                        case "trans":
                                            materialInfo.Transparency = Single.Parse(materialData[loop + 1], CultureInfo.InvariantCulture);
                                            break;
                                    }
                                }
                                result.Materials.Add(materialInfo);
                            }
                            break;

                        //New object starts here
                        case "OBJECT":
                            {
                                ACObjectInfo newObject = new ACObjectInfo();

                                string[] lineData = actLine.Split(' ');
                                if (lineData[1] == "poly") { newObject.Type = ACObjectType.Poly; }
                                else if (lineData[1] == "group") { newObject.Type = ACObjectType.Group; }
                                else if (lineData[1] == "world") { newObject.Type = ACObjectType.World; }

                                loadedObjects.Push(newObject);
                            }
                            break;

                        //End of an object, kids following
                        case "kids":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                //Parse kid count
                                int kidCount = 0;
                                string[] lineData = actLine.Split(' ');
                                if ((lineData != null) && (lineData.Length >= 1))
                                {
                                    Int32.TryParse(lineData[1], out kidCount);
                                }

                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    //Add object to parent object, if any related
                                    bool addedToParent = false;
                                    if (parentObjects.Count > 0)
                                    {
                                        ACObjectInfo currentParent = parentObjects.Peek();
                                        if (currentParent.Childs.Count < currentParent.KidCount)
                                        {
                                            currentParent.Childs.Add(currentObject);
                                            addedToParent = true;
                                        }
                                        else
                                        {
                                            while (parentObjects.Count > 0)
                                            {
                                                parentObjects.Pop();
                                                if (parentObjects.Count == 0) { break; }

                                                currentParent = parentObjects.Peek();
                                                if (currentParent == null) { break; }
                                                if (currentParent.Childs.Count < currentParent.KidCount) { break; }
                                            }
                                            if ((currentParent != null) &&
                                                (currentParent.Childs.Count < currentParent.KidCount))
                                            {
                                                currentParent.Childs.Add(currentObject);
                                                addedToParent = true;
                                            }
                                        }
                                    }

                                    //Enable this object as parent object
                                    currentObject.KidCount = kidCount;
                                    if (currentObject.KidCount > 0) { parentObjects.Push(currentObject); }

                                    //Add to scene root if this object has no parent
                                    loadedObjects.Pop();
                                    if (!addedToParent)
                                    {
                                        if (loadedObjects.Count == 0)
                                        {
                                            result.Objects.Add(currentObject);
                                        }
                                        else
                                        {
                                            loadedObjects.Peek().Childs.Add(currentObject);
                                        }
                                    }
                                    currentObject = null;
                                }
                            }
                            break;

                        //Current object's name
                        case "name":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    currentObject.Name = actLine.Replace("name ", "").Replace("\"", "");
                                }
                            }
                            break;

                        case "data":
                            break;

                        case "texture":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');
                                    currentObject.Texture = lineData[1].Trim('"');
                                }
                            }
                            break;

                        case "texrep":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');

                                    Vector2 repetition = new Vector2();
                                    repetition.X = Single.Parse(lineData[1], CultureInfo.InvariantCulture);
                                    repetition.Y = Single.Parse(lineData[2], CultureInfo.InvariantCulture);

                                    currentObject.TextureRepeat = repetition;
                                }
                            }
                            break;

                        case "texoff":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');

                                    Vector2 offset = new Vector2();
                                    offset.X = Single.Parse(lineData[1], CultureInfo.InvariantCulture);
                                    offset.Y = Single.Parse(lineData[2], CultureInfo.InvariantCulture);

                                    currentObject.TextureRepeat = offset;
                                }
                            }
                            break;

                        case "rot":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');

                                    Matrix3 rotation = new Matrix3();
                                    rotation.M11 = !string.IsNullOrEmpty(lineData[1]) ? Single.Parse(lineData[1], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M12 = !string.IsNullOrEmpty(lineData[2]) ? Single.Parse(lineData[2], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M13 = !string.IsNullOrEmpty(lineData[3]) ? Single.Parse(lineData[3], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M21 = !string.IsNullOrEmpty(lineData[4]) ? Single.Parse(lineData[4], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M22 = !string.IsNullOrEmpty(lineData[5]) ? Single.Parse(lineData[5], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M23 = !string.IsNullOrEmpty(lineData[6]) ? Single.Parse(lineData[6], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M31 = !string.IsNullOrEmpty(lineData[7]) ? Single.Parse(lineData[7], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M32 = !string.IsNullOrEmpty(lineData[8]) ? Single.Parse(lineData[8], CultureInfo.InvariantCulture) : 0f;
                                    rotation.M33 = !string.IsNullOrEmpty(lineData[9]) ? Single.Parse(lineData[9], CultureInfo.InvariantCulture) : 0f;

                                    currentObject.Rotation = rotation;
                                }
                            }
                            break;

                        case "url":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');
                                    currentObject.Url = lineData[1].Trim('"');
                                }
                            }
                            break;

                        //Current object's location
                        case "loc":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');

                                    Vector3 location = new Vector3();
                                    location.X = Single.Parse(lineData[1], CultureInfo.InvariantCulture);
                                    location.Y = Single.Parse(lineData[2], CultureInfo.InvariantCulture);
                                    location.Z = Single.Parse(lineData[3], CultureInfo.InvariantCulture);

                                    currentObject.Translation = location;
                                }
                            }
                            break;


                        case "numvert":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    string[] lineData = actLine.Split(' ');
                                    int numberOfVertices = Int32.Parse(lineData[1], CultureInfo.InvariantCulture);
                                    for (int loop = 0; loop < numberOfVertices; loop++)
                                    {
                                        string actInnerLine = reader.ReadLine().Trim();
                                        string[] splittedVertex = actInnerLine.Split(' ');

                                        Vector3 position = new Vector3();
                                        position.X = Single.Parse(splittedVertex[0], CultureInfo.InvariantCulture);
                                        position.Y = Single.Parse(splittedVertex[1], CultureInfo.InvariantCulture);
                                        position.Z = Single.Parse(splittedVertex[2], CultureInfo.InvariantCulture);

                                        currentObject.Vertices.Add(new ACVertex() { Position = position });
                                    }
                                }
                            }
                            break;

                        //Start of a list of surfaces
                        case "numsurf":
                            break;

                        //New surface starts here
                        case "SURF":
                            {
                                if (currentSurface == null) { currentSurface = new ACSurface(); }

                                string[] lineData = actLine.Split(' ');
                                lineData[1] = lineData[1].Substring(2);
                                currentSurface.Flags = Int32.Parse(lineData[1], NumberStyles.HexNumber);
                            }
                            break;

                        //Current surface's material
                        case "mat":
                            {
                                if (currentSurface == null) { currentSurface = new ACSurface(); }

                                string[] lineData = actLine.Split(' ');
                                currentSurface.Material = Int32.Parse(lineData[1], CultureInfo.InvariantCulture);
                            }
                            break;

                        //Current surface's indices
                        case "refs":
                            if (loadedObjects.Count == 0) { break; }
                            {
                                if (currentSurface == null) { currentSurface = new ACSurface(); }

                                string[] lineData = actLine.Split(' ');
                                int numberOfRefs = Int32.Parse(lineData[1], CultureInfo.InvariantCulture);
                                for (int loop = 0; loop < numberOfRefs; loop++)
                                {
                                    string actInnerLine = reader.ReadLine().Trim();
                                    string[] splittedRef = actInnerLine.Split(' ');

                                    Vector2 texCoord = new Vector2();
                                    ushort vertexReference = UInt16.Parse(splittedRef[0], CultureInfo.InvariantCulture);
                                    texCoord.X = Single.Parse(splittedRef[1], CultureInfo.InvariantCulture);
                                    texCoord.Y = Single.Parse(splittedRef[2], CultureInfo.InvariantCulture);

                                    currentSurface.TextureCoordinates.Add(texCoord);
                                    currentSurface.VertexReferences.Add(vertexReference);
                                }

                                ACObjectInfo currentObject = loadedObjects.Peek();
                                if (currentObject != null)
                                {
                                    currentObject.Surfaces.Add(currentSurface);
                                }
                                currentSurface = null;
                            }
                            break;

                        default:
                            break;
                    }

                }
            }
            finally
            {
                if (reader != null) { reader.Dispose(); }
            }

            return result;
        }

        /// <summary>
        /// Gets the default extension (e. g. ".ac").
        /// </summary>
        public static string DefaultExtension
        {
            get { return ".ac"; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ACFileInfo
        {
            public List<ACMaterialInfo> Materials;
            public List<ACObjectInfo> Objects;

            public ACFileInfo()
            {
                this.Materials = new List<ACMaterialInfo>();
                this.Objects = new List<ACObjectInfo>();
            }

            /// <summary>
            /// Counts all objects within this file
            /// </summary>
            public int CountAllObjects()
            {
                int result = 0;

                foreach (ACObjectInfo actObj in Objects)
                {
                    result++;
                    result += actObj.CountAllChildObjects();
                }

                return result;
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ACMaterialInfo
        {
            public string Name;
            public Color4 Diffuse;
            public Color4 Ambient;
            public Color4 Emissive;
            public Color4 Specular;
            public float Shininess;
            public float Transparency;
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ACObjectInfo
        {
            public List<ACObjectInfo> Childs;
            public List<ACSurface> Surfaces;
            public List<ACVertex> Vertices;
            public string Texture;
            public Vector2 TextureRepeat;
            public Vector2 TextureOffset;
            public Vector3 Translation;
            public Matrix3 Rotation;
            public string Name;
            public string Url;
            public ACObjectType Type;
            public int KidCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="ACObjectInfo"/> class.
            /// </summary>
            public ACObjectInfo()
            {
                this.Childs = new List<ACObjectInfo>();
                this.Surfaces = new List<ACSurface>();
                this.Vertices = new List<ACVertex>();
                this.Rotation = Matrix3.Identity;
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return this.Name;
            }

            /// <summary>
            /// Gets total count of all child ojects
            /// </summary>
            public int CountAllChildObjects()
            {
                int result = 0;

                foreach (ACObjectInfo actObj in Childs)
                {
                    result += actObj.CountAllChildObjects();
                }

                return result;
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ACSurface
        {
            public List<ushort> VertexReferences;
            public List<Vector2> TextureCoordinates;
            public int Flags;
            public int Material;

            /// <summary>
            /// Initializes a new instance of the <see cref="ACSurface"/> class.
            /// </summary>
            public ACSurface()
            {
                VertexReferences = new List<ushort>();
                TextureCoordinates = new List<Vector2>();
            }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class ACVertex
        {
            public Vector3 Position;
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private enum ACObjectType
        {
            World,
            Poly,
            Group
        }
    }
}
