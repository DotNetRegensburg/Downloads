using System;
using System.Collections.Generic;

namespace Common.GraphicsEngine.Objects.Construction
{
    public partial class VertexStructure : ICloneable
    {
        private List<Vertex> m_vertices;
        private List<short> m_indices;
        private VertexCollection m_vertexCollection;
        private TriangleCollection m_triangleCollection;
        private string m_description;
        private string m_name;

        private string m_material;

        /// <summary>
        /// Creates a new Vertex structure object
        /// </summary>
        public VertexStructure()
            : this(0, 0)
        {

        }

        /// <summary>
        /// Gets a vector to the middle center of given structures.
        /// </summary>
        public static Vector3 GetMiddleCenter(VertexStructure[] structures)
        {
            AxisAlignedBox box = GetBoundingBox(structures);
            return box.GetMiddleCenter();
        }

        /// <summary>
        /// Gets a vector to the bottom center of given structures.
        /// </summary>
        public static Vector3 GetBottomCenter(VertexStructure[] structures)
        {
            AxisAlignedBox box = GetBoundingBox(structures);
            return box.GetBottomCenter();
        }

        /// <summary>
        /// Gets a bounding box for given vertex structure array.
        /// </summary>
        /// <param name="structures">Array of structures.</param>
        public static AxisAlignedBox GetBoundingBox(VertexStructure[] structures)
        {
            AxisAlignedBox result = new AxisAlignedBox();

            if (structures != null)
            {
                for (int loop = 0; loop < structures.Length; loop++)
                {
                    if (structures[loop] != null)
                    {
                        result.MergeWith(structures[loop].GenerateBoundingBox());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new Vertex structure object
        /// </summary>
        public VertexStructure(int verticesCapacity, int triangleCapacity)
        {
            m_vertices = new List<Vertex>(verticesCapacity);
            m_indices = new List<short>(triangleCapacity * 3);
          
            m_vertexCollection = new VertexCollection(m_vertices);
            m_triangleCollection = new TriangleCollection(m_indices, m_vertices);

            m_material = string.Empty;

            m_name = string.Empty;
            m_description = string.Empty;
        }

        /// <summary>
        /// Generates a bounding box surrounding all given structures.
        /// </summary>
        /// <param name="structures">Structures to generate a bounding box for.</param>
        public static AxisAlignedBox GenerateBoundingBox(params VertexStructure[] structures)
        {
            AxisAlignedBox result = new AxisAlignedBox();

            foreach (VertexStructure actStructure in structures)
            {
                AxisAlignedBox actBoundingBox = actStructure.GenerateBoundingBox();
                result.MergeWith(actBoundingBox);
            }

            return result;
        }

        /// <summary>
        /// Generates a boundbox around this structure
        /// </summary>
        public AxisAlignedBox GenerateBoundingBox()
        {
            Vector3 maximum = Vector3.MinValue;
            Vector3 minimum = Vector3.MaxValue;

            foreach (Vertex actVertex in m_vertices)
            {
                Vector3 actPosition = actVertex.Position;

                //Update minimum vector
                if (actPosition.X < minimum.X) { minimum.X = actPosition.X; }
                if (actPosition.Y < minimum.Y) { minimum.Y = actPosition.Y; }
                if (actPosition.Z < minimum.Z) { minimum.Z = actPosition.Z; }

                //Update maximum vector
                if (actPosition.X > maximum.X) { maximum.X = actPosition.X; }
                if (actPosition.Y > maximum.Y) { maximum.Y = actPosition.Y; }
                if (actPosition.Z > maximum.Z) { maximum.Z = actPosition.Z; }
            }

            return new AxisAlignedBox(minimum, maximum - minimum);
        }

        /// <summary>
        /// Build a line list containing all lines for wireframe display.
        /// </summary>
        public List<Vector3> BuildLineListForWireframeView()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Triangle actTriangle in this.Triangles)
            {
                //Get all vertices of current face
                Vertex vertex1 = m_vertices[actTriangle.Index1];
                Vertex vertex2 = m_vertices[actTriangle.Index2];
                Vertex vertex3 = m_vertices[actTriangle.Index3];

                //first line (c)
                result.Add(vertex1.Position);
                result.Add(vertex2.Position);

                //second line (a)
                result.Add(vertex2.Position);
                result.Add(vertex3.Position);

                //third line (b)
                result.Add(vertex3.Position);
                result.Add(vertex1.Position);
            }

            return result;
        }

        /// <summary>
        /// Builds a line list containing a line for each face normal.
        /// </summary>
        public List<Vector3> BuildLineListForFaceNormals()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Triangle actTriangle in this.Triangles)
            {
                //Get all vertices of current face
                Vertex vertex1 = m_vertices[actTriangle.Index1];
                Vertex vertex2 = m_vertices[actTriangle.Index2];
                Vertex vertex3 = m_vertices[actTriangle.Index3];

                //Get average values for current face
                Vector3 averageNormal = Vector3.Normalize(Vector3.Average(vertex1.Normal, vertex2.Normal, vertex3.Normal));
                Vector3 averagePosition = Vector3.Average(vertex1.Position, vertex2.Position, vertex3.Position);
                averageNormal *= 0.2f;

                //Generate a line
                if (averageNormal.Length > 0.1f)
                {
                    result.Add(averagePosition);
                    result.Add(averagePosition + averageNormal);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a line list containing a line for each face tangent.
        /// </summary>
        public List<Vector3> BuildLineListForFaceTangents()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Triangle actTriangle in this.Triangles)
            {
                //Get all vertices of current face
                Vertex vertex1 = m_vertices[actTriangle.Index1];
                Vertex vertex2 = m_vertices[actTriangle.Index2];
                Vertex vertex3 = m_vertices[actTriangle.Index3];

                //Get average values for current face
                Vector3 averageTangent = Vector3.Normalize(Vector3.Average(vertex1.Tangent, vertex2.Tangent, vertex3.Tangent));
                Vector3 averagePosition = Vector3.Average(vertex1.Position, vertex2.Position, vertex3.Position);
                averageTangent *= 0.2f;

                //Generate a line
                if (averageTangent.Length > 0.1f)
                {
                    result.Add(averagePosition);
                    result.Add(averagePosition + averageTangent);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a line list containing a line for each face binormal.
        /// </summary>
        public List<Vector3> BuildLineListForFaceBinormals()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Triangle actTriangle in this.Triangles)
            {
                //Get all vertices of current face
                Vertex vertex1 = m_vertices[actTriangle.Index1];
                Vertex vertex2 = m_vertices[actTriangle.Index2];
                Vertex vertex3 = m_vertices[actTriangle.Index3];

                //Get average values for current face
                Vector3 averageBinormal = Vector3.Normalize(Vector3.Average(vertex1.Binormal, vertex2.Binormal, vertex3.Binormal));
                Vector3 averagePosition = Vector3.Average(vertex1.Position, vertex2.Position, vertex3.Position);
                averageBinormal *= 0.2f;

                //Generate a line
                if (averageBinormal.Length > 0.1f)
                {
                    result.Add(averagePosition);
                    result.Add(averagePosition + averageBinormal);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a list list containing a list for each vertex normal.
        /// </summary>
        public List<Vector3> BuildLineListForVertexNormals()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Vertex actVertex in m_vertices)
            {
                if (actVertex.Normal.Length > 0.1f)
                {
                    result.Add(actVertex.Position);
                    result.Add(actVertex.Position + actVertex.Normal * 0.2f);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a list list containing a list for each vertex tangent.
        /// </summary>
        public List<Vector3> BuildLineListForVertexTangents()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Vertex actVertex in m_vertices)
            {
                if (actVertex.Tangent.Length > 0.1f)
                {
                    result.Add(actVertex.Position);
                    result.Add(actVertex.Position + actVertex.Tangent * 0.2f);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a list list containing a list for each vertex binormal.
        /// </summary>
        public List<Vector3> BuildLineListForVertexBinormals()
        {
            List<Vector3> result = new List<Vector3>();

            //Generate all lines
            foreach (Vertex actVertex in m_vertices)
            {
                if (actVertex.Binormal.Length > 0.1f)
                {
                    result.Add(actVertex.Position);
                    result.Add(actVertex.Position + actVertex.Binormal * 0.2f);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates tangents for all vectors.
        /// </summary>
        public void CalculateTangentsAndBinormals()
        {
            for (int loop = 0; loop < this.CountTriangles; loop += 1)
            {
                Triangle actTriangle = this.Triangles[loop];

                //Get all vertices of current face
                Vertex vertex1 = m_vertices[actTriangle.Index1];
                Vertex vertex2 = m_vertices[actTriangle.Index2];
                Vertex vertex3 = m_vertices[actTriangle.Index3];

                //Perform some precalculations
                Vector2 w1 = vertex1.TexCoord;
                Vector2 w2 = vertex2.TexCoord;
                Vector2 w3 = vertex3.TexCoord;
                float x1 = vertex2.Position.X - vertex1.Position.X;
                float x2 = vertex3.Position.X - vertex1.Position.X;
                float y1 = vertex2.Position.Y - vertex1.Position.Y;
                float y2 = vertex3.Position.Y - vertex1.Position.Y;
                float z1 = vertex2.Position.Z - vertex1.Position.Z;
                float z2 = vertex3.Position.Z - vertex1.Position.Z;
                float s1 = w2.X - w1.X;
                float s2 = w3.X - w1.X;
                float t1 = w2.Y - w1.Y;
                float t2 = w3.Y - w1.Y;
                float r = 1f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                //Create the tangent vector (assumes that each vertex normal within the face are equal)
                Vector3 tangent = sdir - vertex1.Normal * Vector3.Dot(vertex1.Normal, sdir);
                tangent.Normalize();

                //Create the binormal using the tangent
                float tangentDir = (Vector3.Dot(Vector3.Cross(vertex1.Normal, sdir), tdir) >= 0.0f) ? 1f : -1f;
                Vector3 binormal = Vector3.Cross(vertex1.Normal, tangent) * tangentDir;

                //Seting binormals and tangents to each vertex of current face
                vertex1.Tangent = tangent;
                vertex1.Binormal = binormal;
                vertex2.Tangent = tangent;
                vertex2.Binormal = binormal;
                vertex3.Tangent = tangent;
                vertex3.Binormal = binormal;

                //Overtake changes made in vertex structures
                m_vertices[actTriangle.Index1] = vertex1;
                m_vertices[actTriangle.Index2] = vertex2;
                m_vertices[actTriangle.Index3] = vertex3;
            }
        }

        /// <summary>
        /// Gets an index array
        /// </summary>
        public short[] GetIndexArray()
        {
            return m_indices.ToArray();
        }

        /// <summary>
        /// Clones this object
        /// </summary>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Clones this object
        /// </summary>
        public VertexStructure Clone()
        {
            int vertexCount = m_vertices.Count;
            int indexCount = m_indices.Count;

            VertexStructure result = new VertexStructure(vertexCount, indexCount / 3);
            for (int loop = 0; loop < vertexCount; loop++)
            {
                result.m_vertices.Add(m_vertices[loop]);
            }
            for (int loop = 0; loop < indexCount; loop++)
            {
                result.m_indices.Add(m_indices[loop]);
            }
            result.m_material = m_material;
            result.m_description = m_description;
            result.m_name = m_name;

            return result;
        }

        /// <summary>
        /// Relocates all vertices by the given vector
        /// </summary>
        public void RelocateVerticesBy(Vector3 relocateVector)
        {
            int length = m_vertices.Count;
            for (int loop = 0; loop < length; loop++)
            {
                m_vertices[loop] = m_vertices[loop].Copy(Vector3.Add(m_vertices[loop].Geometry.Position, relocateVector));
            }
        }

        /// <summary>
        /// Transforms positions and normals of all vertices using the given transform matrix
        /// </summary>
        /// <param name="transformMatrix"></param>
        public void TransformVertices(Matrix4 transformMatrix)
        {
            int length = m_vertices.Count;
            for (int loop = 0; loop < length; loop++)
            {
                m_vertices[loop] = m_vertices[loop].Copy(
                    Vector3.Transform(m_vertices[loop].Position, transformMatrix),
                    Vector3.TransformNormal(m_vertices[loop].Normal, transformMatrix));
            }
        }

        /// <summary>
        /// Adds a vertex to the structure
        /// </summary>
        /// <param name="vertex"></param>
        public short AddVertex(Vertex vertex)
        {
            //Transform vertex on build-time
            if (m_buildTimeTransformEnabled)
            {
                if (m_buildTimeTransformFunc != null) { vertex = m_buildTimeTransformFunc(vertex); }
                else 
                { 
                    vertex.Position = Vector3.Transform(vertex.Position, m_buildTransformMatrix);
                    vertex.Normal = Vector3.TransformNormal(vertex.Normal, m_buildTransformMatrix);
                }
            }

            //Add the vertex and return the index
            m_vertices.Add(vertex);
            return (short)(m_vertices.Count - 1);
        }

        /// <summary>
        /// Adds a triangle
        /// </summary>
        /// <param name="index1">Index of the first vertex</param>
        /// <param name="index2">Index of the second vertex</param>
        /// <param name="index3">Index of the third vertex</param>
        public void AddTriangle(short index1, short index2, short index3)
        {
            m_indices.Add(index1);
            m_indices.Add(index2);
            m_indices.Add(index3);
        }

        /// <summary>
        /// Adds a triangle
        /// </summary>
        /// <param name="v1">First vertex</param>
        /// <param name="v2">Second vertex</param>
        /// <param name="v3">Third vertex</param>
        public void AddTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            m_indices.Add(AddVertex(v1));
            m_indices.Add(AddVertex(v2));
            m_indices.Add(AddVertex(v3));
        }

        /// <summary>
        /// Adds a triangle
        /// </summary>
        /// <param name="v1">First vertex</param>
        /// <param name="v2">Second vertex</param>
        /// <param name="v3">Third vertex</param>
        public void AddTriangleAndCalculateNormals(Vertex v1, Vertex v2, Vertex v3)
        {
            short index1 = AddVertex(v1);
            short index2 = AddVertex(v2);
            short index3 = AddVertex(v3);

            AddTriangleAndCalculateNormals(index1, index2, index3);
        }

        /// <summary>
        /// Adds a triangle and calculates normals for it.
        /// </summary>
        /// <param name="index1">Index of the first vertex</param>
        /// <param name="index2">Index of the second vertex</param>
        /// <param name="index3">Index of the third vertex</param>
        public void AddTriangleAndCalculateNormals(short index1, short index2, short index3)
        {
            AddTriangle(index1, index2, index3);
            CalculateNormalsForTriangle(new Triangle(index1, index2, index3));
        }

        /// <summary>
        /// Recalculates all normals
        /// </summary>
        public void CalulateNormals()
        {
            foreach (Triangle actTriangle in m_triangleCollection)
            {
                CalculateNormalsForTriangle(actTriangle);
            }
        }

        /// <summary>
        /// Calculates normals for the given treeangle.
        /// </summary>
        public void CalculateNormalsForTriangle(Triangle actTriangle)
        {
            Vertex v1 = m_vertices[actTriangle.Index1];
            Vertex v2 = m_vertices[actTriangle.Index2];
            Vertex v3 = m_vertices[actTriangle.Index3];

            Vector3 normal = Vector3.CalculateTriangleNormal(v1.Geometry.Position, v2.Geometry.Position, v3.Geometry.Position);

            v1 = v1.Copy(v1.Geometry.Position, normal);
            v2 = v2.Copy(v2.Geometry.Position, normal);
            v3 = v3.Copy(v3.Geometry.Position, normal);

            m_vertices[actTriangle.Index1] = v1;
            m_vertices[actTriangle.Index2] = v2;
            m_vertices[actTriangle.Index3] = v3;
        }

        /// <summary>
        /// Retrieves a collection of vertices
        /// </summary>
        public VertexCollection Vertices
        {
            get { return m_vertexCollection; }
        }

        /// <summary>
        /// Retrieves a collection of triangles
        /// </summary>
        public TriangleCollection Triangles
        {
            get { return m_triangleCollection; }
        }

        /// <summary>
        /// Retrieves total count of all vertices within this structure
        /// </summary>
        public int CountVertices
        {
            get { return m_vertices.Count; }
        }

        /// <summary>
        /// Retrieves total count of all triangles within this structure
        /// </summary>
        public int CountTriangles
        {
            get { return m_indices.Count / 3; }
        }

        /// <summary>
        /// Retrieves total count of all indexes within this structure
        /// </summary>
        internal int CountIndexes
        {
            get { return m_indices.Count; }
        }

        /// <summary>
        /// A short description for the use of this structure
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /// <summary>
        /// The name of this structure
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set 
            { 
                m_name = value;
                if (m_name == null) { m_name = string.Empty; }
            }
        }

        /// <summary>
        /// Is this structure empty?
        /// </summary>
        public bool IsEmpty
        {
            get { return (m_vertices.Count == 0) && (m_indices.Count == 0); }
        }

        /// <summary>
        /// Gets the name of the material.
        /// </summary>
        public string Material
        {
            get { return m_material; }
            set { m_material = value; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Contains all vertices of a VertexStructure object
        /// </summary>
        public class VertexCollection : IEnumerable<Vertex>
        {
            List<Vertex> m_vertices;

            /// <summary>
            /// 
            /// </summary>
            internal VertexCollection(List<Vertex> vertices)
            {
                m_vertices = vertices;
            }

            /// <summary>
            /// Adds a vertex to the structure
            /// </summary>
            public void Add(Vertex vertex)
            {
                m_vertices.Add(vertex);
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerator<Vertex> GetEnumerator()
            {
                return m_vertices.GetEnumerator();
            }

            /// <summary>
            /// 
            /// </summary>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return m_vertices.GetEnumerator();
            }

            /// <summary>
            /// Returns the vertex at ghe given index
            /// </summary>
            public Vertex this[int index]
            {
                get { return m_vertices[index]; }
            }
        }
        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        /// <summary>
        /// Contains all triangles of a VertexStructure object
        /// </summary>
        public class TriangleCollection : IEnumerable<Triangle>
        {
            private List<short> m_indices;
            private List<Vertex> m_vertices;

            /// <summary>
            /// 
            /// </summary>
            internal TriangleCollection(List<short> indices, List<Vertex> vertices)
            {
                m_indices = indices;
                m_vertices = vertices;
            }

            /// <summary>
            /// Adds a treangle to this vertex structure
            /// </summary>
            /// <param name="index1">Index of the first vertex</param>
            /// <param name="index2">Index of the second vertex</param>
            /// <param name="index3">Index of the third vertex</param>
            public int Add(short index1, short index2, short index3)
            {
                int result = m_indices.Count / 3;

                m_indices.Add(index1);
                m_indices.Add(index2);
                m_indices.Add(index3);

                return result;
            }

            /// <summary>
            /// Adds a treangle to this vertex structure
            /// </summary>
            /// <param name="triangle"></param>
            public int Add(Triangle triangle)
            {
                return this.Add(triangle.Index1, triangle.Index2, triangle.Index3);
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerator<Triangle> GetEnumerator()
            {
                return new Enumerator(m_indices);
            }

            /// <summary>
            /// 
            /// </summary>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new Enumerator(m_indices);
            }

            /// <summary>
            /// Retrieves the triangle at the given index
            /// </summary>
            public Triangle this[int index]
            {
                get
                {
                    int startIndex = index * 3;
                    return new Triangle(m_indices[startIndex], m_indices[startIndex + 1], m_indices[startIndex + 2]);
                }
            }

            //*****************************************************************
            //*****************************************************************
            //*****************************************************************
            /// <summary>
            /// Enumerator of TriangleCollection
            /// </summary>
            private class Enumerator : IEnumerator<Triangle>
            {
                private int m_maxIndex;
                private int m_startIndex;
                private List<short> m_indices;

                /// <summary>
                /// 
                /// </summary>
                public Enumerator(List<short> indices)
                {
                    m_startIndex = -3;
                    m_maxIndex = indices.Count - 3;
                    m_indices = indices;
                }

                /// <summary>
                /// 
                /// </summary>
                public void Dispose()
                {
                    m_startIndex = -3;
                    m_indices = null;
                }
 
                /// <summary>
                /// 
                /// </summary>
                public bool MoveNext()
                {
                    m_startIndex += 3;
                    return m_startIndex <= m_maxIndex;
                }

                /// <summary>
                /// 
                /// </summary>
                public void Reset()
                {
                    m_startIndex = -3;
                    m_maxIndex = m_indices.Count - 3;
                }

                /// <summary>
                /// 
                /// </summary>
                public Triangle Current
                {
                    get { return new Triangle(m_indices[m_startIndex], m_indices[m_startIndex + 1], m_indices[m_startIndex + 2]); }
                }

                /// <summary>
                /// 
                /// </summary>
                object System.Collections.IEnumerator.Current
                {
                    get { return new Triangle(m_indices[m_startIndex], m_indices[m_startIndex + 1], m_indices[m_startIndex + 2]); }
                }
            }
        }
    }
}
