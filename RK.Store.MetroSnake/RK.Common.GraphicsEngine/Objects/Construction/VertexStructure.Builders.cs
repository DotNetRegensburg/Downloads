using System;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Objects.Construction
{
    public partial class VertexStructure
    {
        private Vector2 m_tileSize = Vector2.Empty;
        private bool m_buildTimeTransformEnabled;
        private Matrix4 m_buildTransformMatrix;
        private Func<Vertex, Vertex> m_buildTimeTransformFunc;

        /// <summary>
        /// Enables texture tile mode.
        /// </summary>
        public void EnableTextureTileMode(Vector2 tileSize)
        {
            m_tileSize = tileSize;
        }

        /// <summary>
        /// Disables texture tile mode.
        /// </summary>
        public void DisableTextureTileMode()
        {
            m_tileSize = Vector2.Empty;
        }

        /// <summary>
        /// Enables build-time transform using the given matrix.
        /// </summary>
        /// <param name="transformMatrix">Transform matrix.</param>
        public void EnableBuildTimeTransform(Matrix4 transformMatrix)
        {
            m_buildTimeTransformEnabled = true;
            m_buildTransformMatrix = transformMatrix;
            m_buildTimeTransformFunc = null;
        }

        /// <summary>
        /// Enables build-time transform using given transform method.
        /// </summary>
        public void EnableBuildTimeTransform(Func<Vertex, Vertex> transformFunc)
        {
            m_buildTimeTransformEnabled = true;
            m_buildTransformMatrix = Matrix4.Identity;
            m_buildTimeTransformFunc = transformFunc;
        }

        /// <summary>
        /// Disables build-time transform.
        /// </summary>
        public void DisableBuildTimeTransform()
        {
            m_buildTimeTransformEnabled = false;
            m_buildTransformMatrix = Matrix4.Identity;
        }

        /// <summary>
        /// Performs a simple picking test against all triangles of this object.
        /// </summary>
        /// <param name="pickingRay">The picking ray.</param>
        public PickingInformation Pick(Ray pickingRay)
        {
            return null;
        }

        /// <summary>
        /// Builds a cone into the structure with correct texture coordinates and normals.
        /// </summary>
        /// <param name="bottomMiddle">Coordinate of bottom middle.</param>
        /// <param name="radius">The radius of the cone.</param>
        /// <param name="height">The height of the cone.</param>
        /// <param name="countOfSegments">Total count of segments to generate.</param>
        public void BuildConeFullV(Vector3 bottomMiddle, float radius, float height, int countOfSegments, Color4 color)
        {
            if (countOfSegments < 5) { throw new ArgumentException("Segment count of " + countOfSegments + " is too small!", "coundOfSegments"); }
            float diameter = radius * 2f;

            //Get texture offsets
            float texX = 1f;
            float texY = 1f;
            if (m_tileSize != Vector2.Empty)
            {
                texX = diameter / m_tileSize.X;
                texY = diameter / m_tileSize.Y;
            }

            //Specify bottom and top middle coordinates
            Vector3 bottomCoordinate = bottomMiddle;
            Vector3 topCoordinate = new Vector3(bottomMiddle.X, bottomMiddle.Y + height, bottomMiddle.Z);

            //Create bottom and top vertices
            Vertex bottomVertex = new Vertex(bottomCoordinate, color, new Vector2(texX / 2f, texY / 2f), new Vector3(0f, -1f, 0f));

            //Add bottom and top vertices to the structure
            ushort bottomVertexIndex = AddVertex(bottomVertex);

            //Generate all segments
            float fullRadian = EngineMath.RAD_360DEG;
            float countOfSegmentsF = (float)countOfSegments;
            for (int loop = 0; loop < countOfSegments; loop++)
            {
                //Calculate rotation values for each segment border
                float startRadian = fullRadian * ((float)loop / (float)countOfSegmentsF);
                float targetRadian = fullRadian * ((float)(loop + 1) / (float)countOfSegmentsF);
                float normalRadian = startRadian + (targetRadian - startRadian) / 2f;

                //Generate all normals
                Vector3 sideNormal = Vector3.NormalFromHVRotation(normalRadian, 0f);
                Vector3 sideLeftNormal = Vector3.NormalFromHVRotation(startRadian, 0f);
                Vector3 sideRightNormal = Vector3.NormalFromHVRotation(targetRadian, 0f);

                //Calculate border texture coordinates
                Vector2 sideLeftTexCoord = new Vector2(0.5f + sideLeftNormal.X * radius, 0.5f + sideLeftNormal.Z * radius);
                Vector2 sideRightTexCoord = new Vector2(0.5f + sideRightNormal.X * radius, 0.5f + sideRightNormal.Z * radius);

                //Generate all points
                Vector3 sideLeftBottomCoord = bottomCoordinate + sideLeftNormal * radius;
                Vector3 sideRighBottomtCoord = bottomCoordinate + sideRightNormal * radius;
                Vector3 sideMiddleBottomCoord = bottomCoordinate + sideNormal * radius;

                //Add segment bottom triangle
                Vertex segmentBottomLeft = bottomVertex.Copy(sideLeftBottomCoord);
                Vertex segmentBottomRight = bottomVertex.Copy(sideRighBottomtCoord);
                AddTriangle(
                    bottomVertexIndex,
                    AddVertex(segmentBottomLeft),
                    AddVertex(segmentBottomRight));

                //Generate side normal
                Vector3 vectorToTop = topCoordinate - sideMiddleBottomCoord;
                Vector2 vectorToTopRotation = vectorToTop.ToHVRotation();
                vectorToTopRotation.Y = vectorToTopRotation.Y + EngineMath.RAD_90DEG;
                Vector3 topSideNormal = Vector3.NormalFromHVRotation(vectorToTopRotation);

                //Add segment top triangle
                Vertex topVertex = new Vertex(topCoordinate, color, new Vector2(texX / 2f, texY / 2f), topSideNormal);
                Vertex segmentTopLeft = topVertex.Copy(sideLeftBottomCoord);
                Vertex segmentTopRight = topVertex.Copy(sideRighBottomtCoord);
                AddTriangle(
                    AddVertex(topVertex),
                    AddVertex(segmentTopRight),
                    AddVertex(segmentTopLeft));
            }
        }

        /// <summary>
        /// Builds a cylinder into the structure with correct texture coordinates and normals.
        /// </summary>
        /// <param name="bottomMiddle">Coordinate of bottom middle.</param>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="height">The height of the cylinder.</param>
        /// <param name="countOfSegments">Total count of segments to generate.</param>
        public void BuildCylinderFullV(Vector3 bottomMiddle, float radius, float height, int countOfSegments, Color4 color)
        {
            if (countOfSegments < 5) { throw new ArgumentException("Segment count of " + countOfSegments + " is too small!", "coundOfSegments"); }
            float diameter = radius * 2f;

            //Get texture offsets
            float texX = 1f;
            float texY = 1f;
            float texSegmentY = 1f;
            float texSegmentX = 1f;
            if (m_tileSize != Vector2.Empty)
            {
                texX = diameter / m_tileSize.X;
                texY = diameter / m_tileSize.Y;
                texSegmentY = height / m_tileSize.Y;
                texSegmentX = (EngineMath.RAD_180DEG * diameter) / m_tileSize.X;
            }

            //Specify bottom and top middle coordinates
            Vector3 bottomCoordinate = bottomMiddle;
            Vector3 topCoordinate = new Vector3(bottomMiddle.X, bottomMiddle.Y + height, bottomMiddle.Z);

            //Create bottom and top vertices
            Vertex bottomVertex = new Vertex(bottomCoordinate, color, new Vector2(texX / 2f, texY / 2f), new Vector3(0f, -1f, 0f));
            Vertex topVertex = new Vertex(topCoordinate, color, new Vector2(texX / 2f, texY / 2f), new Vector3(0f, 1f, 0f));

            //Add bottom and top vertices to the structure
            ushort bottomVertexIndex = AddVertex(bottomVertex);
            ushort topVertexIndex = AddVertex(topVertex);

            //Generate all segments
            float fullRadian = EngineMath.RAD_360DEG;
            float countOfSegmentsF = (float)countOfSegments;
            for (int loop = 0; loop < countOfSegments; loop++)
            {
                //Calculate rotation values for each segment border
                float startRadian = fullRadian * ((float)loop / (float)countOfSegmentsF);
                float targetRadian = fullRadian * ((float)(loop + 1) / (float)countOfSegmentsF);
                float normalRadian = startRadian + (targetRadian - startRadian) / 2f;

                //Generate all normals
                Vector3 sideNormal = Vector3.NormalFromHVRotation(normalRadian, 0f);
                Vector3 sideLeftNormal = Vector3.NormalFromHVRotation(startRadian, 0f);
                Vector3 sideRightNormal = Vector3.NormalFromHVRotation(targetRadian, 0f);

                //
                Vector2 sideLeftTexCoord = new Vector2(0.5f + sideLeftNormal.X * radius, 0.5f + sideLeftNormal.Z * radius);
                Vector2 sideRightTexCoord = new Vector2(0.5f + sideRightNormal.X * radius, 0.5f + sideRightNormal.Z * radius);

                //Generate all points
                Vector3 sideLeftBottomCoord = bottomCoordinate + sideLeftNormal * radius;
                Vector3 sideRighBottomtCoord = bottomCoordinate + sideRightNormal * radius;
                Vector3 sideLeftTopCoord = new Vector3(sideLeftBottomCoord.X, sideLeftBottomCoord.Y + height, sideLeftBottomCoord.Z);
                Vector3 sideRightTopCoord = new Vector3(sideRighBottomtCoord.X, sideRighBottomtCoord.Y + height, sideRighBottomtCoord.Z);

                //Add segment bottom triangle
                Vertex segmentBottomLeft = bottomVertex.Copy(sideLeftBottomCoord);
                Vertex segmentBottomRight = bottomVertex.Copy(sideRighBottomtCoord);
                AddTriangle(
                    bottomVertexIndex,
                    AddVertex(segmentBottomLeft),
                    AddVertex(segmentBottomRight));

                //Add segment top triangle
                Vertex segmentTopLeft = topVertex.Copy(sideLeftTopCoord);
                Vertex segmentTopRight = topVertex.Copy(sideRightTopCoord);
                AddTriangle(
                    topVertexIndex,
                    AddVertex(segmentTopRight),
                    AddVertex(segmentTopLeft));

                //Calculate texture coords for side segment
                Vector2 texCoordSegmentStart = new Vector2(texSegmentX * (loop / countOfSegments), 0f);
                Vector2 texCoordSegmentTarget = new Vector2(texSegmentX * ((loop + 1) / countOfSegments), texSegmentY);

                //Add segment side
                BuildRect4V(sideLeftBottomCoord, sideRighBottomtCoord, sideRightTopCoord, sideLeftTopCoord, sideNormal, color, texCoordSegmentStart, texCoordSegmentTarget);
            }
        }

        /// <summary>
        /// Builds a sphere geometry.
        /// </summary>
        public void BuildShpere(int tDiv, int pDiv, double radius, Color4 color)
        {
            double dt = (Math.PI * 2) / tDiv;
            double dp = Math.PI / pDiv;

            for (int pi = 0; pi <= pDiv; pi++)
            {
                double phi = pi * dp;

                for (int ti = 0; ti <= tDiv; ti++)
                {
                    // we want to start the mesh on the x axis
                    double theta = ti * dt;

                    Vector3 position = SphereGetPosition(theta, phi, radius);
                    Vertex vertex = new Vertex(
                        position,
                        color,
                        SphereGetTextureCoordinate(theta, phi),
                        Vector3.Normalize(position));
                    this.Vertices.Add(vertex);
                }
            }

            for (int pi = 0; pi < pDiv; pi++)
            {
                for (int ti = 0; ti < tDiv; ti++)
                {
                    int x0 = ti;
                    int x1 = (ti + 1);
                    int y0 = pi * (tDiv + 1);
                    int y1 = (pi + 1) * (tDiv + 1);

                    this.Triangles.Add(
                        (ushort)(x0 + y0),
                        (ushort)(x0 + y1),
                        (ushort)(x1 + y0));

                    this.Triangles.Add(
                        (ushort)(x1 + y0),
                        (ushort)(x0 + y1),
                        (ushort)(x1 + y1));
                }
            }
        }

        /// <summary>
        /// Builds a cube into a vertex structure (this cube is built up of just 8 vertices, so not texturing is supported)
        /// </summary>
        /// <param name="vs">Target VertexStructure object</param>
        /// <param name="start">Start point of the cube (left-lower-front point)</param>
        /// <param name="size">Size of the cube</param>
        public void BuildCube8V(Vector3 start, Vector3 size)
        {
            BuildCube8V(start, size, Color4.White);
        }

        /// <summary>
        /// Builds a cube into a vertex structure (this cube is built up of just 8 vertices, so no texturing is supported)
        /// </summary>
        /// <param name="vs">Target VertexStructure object</param>
        /// <param name="start">Start point of the cube (left-lower-front point)</param>
        /// <param name="size">Size of the cube</param>
        /// <param name="color">Color of the cube</param>
        public void BuildCube8V(Vector3 start, Vector3 size, Color4 color)
        {
            Vector3 dest = start + size;
            Vertex vertex = new Vertex(start, color, new Vector2());

            ushort a = this.AddVertex(vertex);
            ushort b = this.AddVertex(vertex.Copy(new Vector3(dest.X, start.Y, start.Z)));
            ushort c = this.AddVertex(vertex.Copy(new Vector3(dest.X, start.Y, dest.Z)));
            ushort d = this.AddVertex(vertex.Copy(new Vector3(start.X, start.Y, dest.Z)));
            ushort e = this.AddVertex(vertex.Copy(new Vector3(start.X, dest.Y, start.Z)));
            ushort f = this.AddVertex(vertex.Copy(new Vector3(dest.X, dest.Y, start.Z)));
            ushort g = this.AddVertex(vertex.Copy(new Vector3(dest.X, dest.Y, dest.Z)));
            ushort h = this.AddVertex(vertex.Copy(new Vector3(start.X, dest.Y, dest.Z)));

            this.AddTriangle(a, e, f);  //front side
            this.AddTriangle(f, b, a);
            this.AddTriangle(b, f, g);  //right side
            this.AddTriangle(g, c, b); 
            this.AddTriangle(c, g, h);  //back side
            this.AddTriangle(h, d, c);
            this.AddTriangle(d, h, e);  //left side
            this.AddTriangle(e, a, d);
            this.AddTriangle(e, h, g);  //top side
            this.AddTriangle(g, f, e);
            this.AddTriangle(a, b, c);  //botton side
            this.AddTriangle(c, d, a);
        }

        /// <summary>
        /// Builds a cube into this VertexStructure (this cube is built up of 24 vertices, so texture coordinates and normals are set)
        /// </summary>
        /// <param name="start">Start point of the cube</param>
        /// <param name="size">Size of the cube</param>
        /// <param name="color">Color of the cube</param>
        public void BuildCube24V(Vector3 start, Vector3 size, Color4 color)
        {
            this.BuildCubeSides16V(start, size, color);
            this.BuildCubeTop4V(start, size, color);
            this.BuildCubeBottom4V(start, size, color);
        }

        /// <summary>
        /// Builds a cube into this VertexStructure (this cube is built up of 24 vertices, so texture coordinates and normals are set)
        /// </summary>
        /// <param name="box">Box defining bounds of generated cube.</param>
        /// <param name="color">Color of generated vertices.</param>
        public void BuildCube24V(AxisAlignedBox box, Color4 color)
        {
            BuildCube24V(box.Location, box.Size, color);
        }

        /// <summary>
        /// Builds a cube into this VertexStructure (this cube is built up of 24 vertices, so texture coordinates and normals are set)
        /// </summary>
        /// <param name="box">Box defining bounds of generated cube.</param>
        public void BuildCube24V(AxisAlignedBox box)
        {
            BuildCube24V(box, Color4.White);
        }

        /// <summary>
        /// Builds a cube into this VertexStructure (this cube is built up of 24 vertices, so texture coordinates and normals are set)
        /// </summary>
        /// <param name="bottomCenter">Bottom center point of the cube.</param>
        /// <param name="width">Width (and depth) of the cube.</param>
        /// <param name="height">Height of the cube.</param>
        /// <param name="color">Color of the cube</param>
        public void BuildCube24V(Vector3 bottomCenter, float width, float height, Color4 color)
        {
            Vector3 start = new Vector3(
                bottomCenter.X - width / 2f,
                bottomCenter.Y,
                bottomCenter.Z - width / 2f);
            Vector3 size = new Vector3(width, height, width);
            BuildCube24V(start, size, color);
        }

        /// <summary>
        /// Builds the top side of a cube into this VertexStructure (Built up of 4 vertices, so texture coordinates and normals are set)
        /// </summary>
        public void BuildCubeTop4V(Vector3 start, Vector3 size, Color4 color)
        {
            Vector3 dest = start + size;

            this.BuildRect4V(
                new Vector3(start.X, dest.Y, start.Z),
                new Vector3(dest.X, dest.Y, start.Z),
                new Vector3(dest.X, dest.Y, dest.Z),
                new Vector3(start.X, dest.Y, dest.Z),
                new Vector3(0f, 1f, 0f),
                color);
        }

        /// <summary>
        /// Builds the bottom side of a cube into this VertexStructure (Built up of 4 vertices, so texture coordinates and normals are set)
        /// </summary>
        public void BuildCubeBottom4V(Vector3 start, Vector3 size, Color4 color)
        {
            Vector3 dest = start + size;

            this.BuildRect4V(
                new Vector3(start.X, start.Y, start.Z),
                new Vector3(start.X, start.Y, dest.Z),
                new Vector3(dest.X, start.Y, dest.Z),
                new Vector3(dest.X, start.Y, start.Z),
                new Vector3(0f, -1f, 0f),
                color);
        }

        /// <summary>
        /// Builds cube sides into this VertexStructure (these sides are built up of  16 vertices, so texture coordinates and normals are set)
        /// </summary>
        /// <param name="start">Start poiint of the cube</param>
        /// <param name="size">Size of the cube</param>
        /// <param name="color">Color of the cube</param>
        public void BuildCubeSides16V(Vector3 start, Vector3 size, Color4 color)
        {
            Vector3 dest = start + size;

            float texX = 1f;
            float texY = 1f;
            float texZ = 1f;
            if (m_tileSize != Vector2.Empty)
            {
                texX = size.X / m_tileSize.X;
                texY = size.Y / m_tileSize.Y;
                texZ = size.Z / m_tileSize.X;
            }

            //Front side
            Vertex vertex = new Vertex(start, color, new Vector2(0f, texY), new Vector3(0f, 0f, -1f));
            ushort a = this.AddVertex(vertex);
            ushort b = this.AddVertex(vertex.Copy(new Vector3(dest.X, start.Y, start.Z), new Vector2(texX, texY)));
            ushort c = this.AddVertex(vertex.Copy(new Vector3(dest.X, dest.Y, start.Z), new Vector2(texX, 0f)));
            ushort d = this.AddVertex(vertex.Copy(new Vector3(start.X, dest.Y, start.Z), new Vector2(0f, 0f)));
            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);

            //Right side
            a = this.AddVertex(vertex.Copy(new Vector3(dest.X, start.Y, start.Z), new Vector3(1f, 0f, 0f), new Vector2(0f, texY)));
            b = this.AddVertex(vertex.Copy(new Vector3(dest.X, start.Y, dest.Z), new Vector3(1f, 0f, 0f), new Vector2(texZ, texY)));
            c = this.AddVertex(vertex.Copy(new Vector3(dest.X, dest.Y, dest.Z), new Vector3(1f, 0f, 0f), new Vector2(texZ, 0f)));
            d = this.AddVertex(vertex.Copy(new Vector3(dest.X, dest.Y, start.Z), new Vector3(1f, 0f, 0f), new Vector2(0f, 0f)));
            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);

            //Back side
            a = this.AddVertex(vertex.Copy(new Vector3(dest.X, start.Y, dest.Z), new Vector3(0f, 0f, 1f), new Vector2(0f, texY)));
            b = this.AddVertex(vertex.Copy(new Vector3(start.X, start.Y, dest.Z), new Vector3(0f, 0f, 1f), new Vector2(texX, texY)));
            c = this.AddVertex(vertex.Copy(new Vector3(start.X, dest.Y, dest.Z), new Vector3(0f, 0f, 1f), new Vector2(texX, 0f)));
            d = this.AddVertex(vertex.Copy(new Vector3(dest.X, dest.Y, dest.Z), new Vector3(0f, 0f, 1f), new Vector2(0f, 0f)));
            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);

            //Left side
            a = this.AddVertex(vertex.Copy(new Vector3(start.X, start.Y, dest.Z), new Vector3(-1f, 0f, 0f), new Vector2(0f, texY)));
            b = this.AddVertex(vertex.Copy(new Vector3(start.X, start.Y, start.Z), new Vector3(-1f, 0f, 0f), new Vector2(texZ, texY)));
            c = this.AddVertex(vertex.Copy(new Vector3(start.X, dest.Y, start.Z), new Vector3(-1f, 0f, 0f), new Vector2(texZ, 0f)));
            d = this.AddVertex(vertex.Copy(new Vector3(start.X, dest.Y, dest.Z), new Vector3(-1f, 0f, 0f), new Vector2(0f, 0f)));
            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);
        }

        /// <summary>
        /// Build a single rectangle into the vertex structure
        /// </summary>
        public void BuildRect4V(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD)
        {
            BuildRect4V(pointA, pointB, pointC, pointD, Color4.White);
        }

        /// <summary>
        /// Build a single rectangle into the vertex structure (Supports texturing)
        /// </summary>
        public void BuildRect4V(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Color4 color)
        {
            float texX = 1f;
            float texY = 1f;
            if (m_tileSize != Vector2.Empty)
            {
                texX = (pointB - pointA).Length / m_tileSize.X;
                texY = (pointC - pointB).Length / m_tileSize.Y;
            }

            Vertex vertex = new Vertex(pointA, color, new Vector2(0f, texY));

            ushort a = this.AddVertex(vertex);
            ushort b = this.AddVertex(vertex.Copy(pointB, new Vector2(texX, texY)));
            ushort c = this.AddVertex(vertex.Copy(pointC, new Vector2(texX, 0f)));
            ushort d = this.AddVertex(vertex.Copy(pointD, new Vector2(0f, 0f)));

            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);
        }

        /// <summary>
        /// Build a single rectangle into the vertex structure (Supports texturing and normal vectors)
        /// </summary>
        public void BuildRect4V(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Vector3 normal, Color4 color)
        {
            float texX = 1f;
            float texY = 1f;
            if (m_tileSize != Vector2.Empty)
            {
                texX = (pointB - pointA).Length / m_tileSize.X;
                texY = (pointC - pointB).Length / m_tileSize.Y;
            }

            Vertex vertex = new Vertex(pointA, color, new Vector2(0f, texY), normal);

            ushort a = this.AddVertex(vertex);
            ushort b = this.AddVertex(vertex.Copy(pointB, new Vector2(texX, texY)));
            ushort c = this.AddVertex(vertex.Copy(pointC, new Vector2(texX, 0f)));
            ushort d = this.AddVertex(vertex.Copy(pointD, new Vector2(0f, 0f)));

            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);
        }

        /// <summary>
        /// Build a single rectangle into the vertex structure (Supports texturing and normal vectors)
        /// </summary>
        public void BuildRect4V(Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD, Vector3 normal, Color4 color, Vector2 minTexCoord, Vector2 maxTexCoord)
        {
            Vertex vertex = new Vertex(pointA, color, new Vector2(minTexCoord.X, maxTexCoord.Y), normal);

            ushort a = this.AddVertex(vertex);
            ushort b = this.AddVertex(vertex.Copy(pointB, new Vector2(maxTexCoord.X, maxTexCoord.Y)));
            ushort c = this.AddVertex(vertex.Copy(pointC, new Vector2(maxTexCoord.X, minTexCoord.Y)));
            ushort d = this.AddVertex(vertex.Copy(pointD, new Vector2(minTexCoord.X, minTexCoord.Y)));

            this.AddTriangle(a, c, b);
            this.AddTriangle(a, d, c);
        }

        /// <summary>
        /// Helper method for spehere creation.
        /// </summary>
        private Vector3 SphereGetPosition(double theta, double phi, double radius)
        {
            double x = radius * Math.Sin(theta) * Math.Sin(phi);
            double y = radius * Math.Cos(phi);
            double z = radius * Math.Cos(theta) * Math.Sin(phi);

            return new Vector3((float)x, (float)y, (float)z);
        }

        /// <summary>
        /// Helper method for spehere creation.
        /// </summary>
        private Vector2 SphereGetTextureCoordinate(double theta, double phi)
        {
            return new Vector2(
                (float)(theta / (2 * Math.PI)),
                (float)(phi / Math.PI));
        }

        /// <summary>
        /// Changes the index order of each triangle.
        /// </summary>
        public void RecorderTriangleIndices()
        {
            for (int loop = 2; loop < m_indices.Count; loop+=3)
            {
                ushort index1 = m_indices[loop - 2];
                ushort index2 = m_indices[loop - 1];
                ushort index3 = m_indices[loop];
                m_indices[loop] = index1;
                m_indices[loop - 1] = index2;
                m_indices[loop - 2] = index3;
            }
        }
    }
}
