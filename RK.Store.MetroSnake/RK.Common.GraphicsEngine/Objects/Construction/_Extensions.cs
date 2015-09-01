using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.GraphicsEngine.Objects.Construction
{
    public static class ConstructionExtensions
    {
        /// <summary>
        /// Generates a bounding box out of given vertex structures.
        /// </summary>
        /// <param name="structures">The structures to buld the box for.</param>
        public static AxisAlignedBox GenerateBoundingBox(this IEnumerable<VertexStructure> structures)
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
        /// Generates a shadow volume structure out of the given vertex structures.
        /// </summary>
        /// <param name="structures">The structures for which to create the shadow volume.</param>
        /// <param name="lightDirection">The direction of the light.</param>
        public static VertexStructure GenerateShadowVolume(this IEnumerable<VertexStructure> structures, Vector3 lightDirection, float shadowVolumeLength)
        {
            VertexStructure result = new VertexStructure();

            //Find all shadow volume edges
            List<Line3D> shadowVolumeEdges = new List<Line3D>();
            List<Line3D> edgesToRemove = new List<Line3D>();
            foreach (VertexStructure actStructure in structures)
            {
                foreach (Triangle actTriangle in actStructure.Triangles)
                {
                    if (Vector3.Dot(lightDirection, actStructure.Vertices[actTriangle.Index1].Normal) >= 0)
                    {
                        Line3D[] actEdges = actTriangle.GetEdges(actStructure);
                        for (int loopEdge = 0; loopEdge < actEdges.Length; loopEdge++)
                        {
                            Line3D actEdge = actEdges[loopEdge];

                            //Was this edge already removed?
                            bool alreadyRemoved = false;
                            foreach (Line3D edgesRemoved in edgesToRemove)
                            {
                                if (edgesRemoved.EqualsWithTolerance(actEdge))
                                {
                                    alreadyRemoved = true;
                                    break;
                                }
                            }
                            if (alreadyRemoved) { continue; }

                            //Was this edge already added?
                            bool alreadyAdded = false;
                            for (int loopShadowEdge = 0; loopShadowEdge < shadowVolumeEdges.Count; loopShadowEdge++)
                            {
                                if (shadowVolumeEdges[loopShadowEdge].EqualsWithTolerance(actEdge))
                                {
                                    //Remove the edge because it can't be member of the contour when it is found twice
                                    alreadyAdded = true;
                                    shadowVolumeEdges.RemoveAt(loopShadowEdge);
                                    edgesToRemove.Add(actEdge);
                                }
                            }
                            if (alreadyAdded) { continue; }

                            //Add the edge to the result list finally
                            shadowVolumeEdges.Add(actEdge);
                        }
                    }
                }
            }

            //Build the structure based on the found edges
            Vector3 lightNormal = Vector3.Normalize(lightDirection);
            foreach (Line3D actEdge in shadowVolumeEdges)
            {
                Line3D targetEdge = new Line3D(
                    actEdge.StartPosition + lightNormal * shadowVolumeLength,
                    actEdge.EndPosition + lightNormal * shadowVolumeLength);

                result.AddTriangle(
                    new Vertex(actEdge.StartPosition, Color4.White),
                    new Vertex(actEdge.EndPosition, Color4.White),
                    new Vertex(targetEdge.EndPosition, Color4.White));
                result.AddTriangle(
                    new Vertex(targetEdge.EndPosition, Color4.White),
                    new Vertex(targetEdge.StartPosition, Color4.White),
                    new Vertex(actEdge.StartPosition, Color4.White));
            }

            return result;
        }

        public static void FitToCenteredCube(this IEnumerable<VertexStructure> structures)
        {
            structures.FitToCenteredCuboid(1f, 1f, 1f, FitToCuboidMode.MaintainAspectRatio, FitToCuboidOrigin.Center);
        }

        public static void FitToCenteredCube(this IEnumerable<VertexStructure> structures, float cubeSideLength)
        {
            structures.FitToCenteredCuboid(cubeSideLength, cubeSideLength, cubeSideLength, FitToCuboidMode.MaintainAspectRatio, FitToCuboidOrigin.Center);
        }

        public static void FitToCenteredCube(this IEnumerable<VertexStructure> structures, float cubeSideLength, FitToCuboidMode mode)
        {
            structures.FitToCenteredCuboid(cubeSideLength, cubeSideLength, cubeSideLength, mode, FitToCuboidOrigin.Center);
        }

        public static void FitToCenteredCube(this IEnumerable<VertexStructure> structures, float cubeSideLength, FitToCuboidMode mode, FitToCuboidOrigin fitOrigin)
        {
            structures.FitToCenteredCuboid(cubeSideLength, cubeSideLength, cubeSideLength, mode, fitOrigin);
        }

        public static void FitToCenteredCuboid(this IEnumerable<VertexStructure> structures, float cubeSideLengthX, float cubeSideLengthY, float cubeSideLengthZ, FitToCuboidMode fitMode, FitToCuboidOrigin fitOrigin)
        {
            //Get whole bounding box
            AxisAlignedBox wholeBoundingBox = structures.GenerateBoundingBox();
            if (wholeBoundingBox.IsEmpty) { return; }
            if (wholeBoundingBox.Size.X <= 0f) { return; }
            if (wholeBoundingBox.Size.Y <= 0f) { return; }
            if (wholeBoundingBox.Size.Z <= 0f) { return; }

            Vector3 targetCornerALocation = new Vector3(
                -wholeBoundingBox.Size.X / 2f,
                -wholeBoundingBox.Size.Y / 2f,
                -wholeBoundingBox.Size.Z / 2f);
            //Vector3 wholeRelocationVector = targetCornerALocation - wholeBoundingBox.CornerA;

            //Calculate resize factors
            float resizeFactorX = cubeSideLengthX / wholeBoundingBox.Size.X;
            float resizeFactorY = cubeSideLengthY / wholeBoundingBox.Size.Y;
            float resizeFactorZ = cubeSideLengthZ / wholeBoundingBox.Size.Z;
            if (fitMode == FitToCuboidMode.MaintainAspectRatio)
            {
                resizeFactorX = Math.Min(resizeFactorX, Math.Min(resizeFactorY, resizeFactorZ));
                resizeFactorY = resizeFactorX;
                resizeFactorZ = resizeFactorX;
            }

            targetCornerALocation.X = targetCornerALocation.X * resizeFactorX;
            targetCornerALocation.Y = targetCornerALocation.Y * resizeFactorY;
            targetCornerALocation.Z = targetCornerALocation.Z * resizeFactorZ;
            switch (fitOrigin)
            {
                case FitToCuboidOrigin.LowerCenter:
                    targetCornerALocation.Y = 0f;
                    break;
            }

            //Transform each single structure
            foreach (VertexStructure actStructure in structures)
            {
                AxisAlignedBox actPartBox = actStructure.GenerateBoundingBox();
                Vector3 localLocationOriginal = actPartBox.CornerA - wholeBoundingBox.CornerA;

                //Bring the structure to origin based location and then scale it
                actStructure.UpdateVerticesUsingRelocationBy(Vector3.Negate(actPartBox.CornerA));
                actStructure.UpdateVerticesUsingRelocationFunc((actPosition) => new Vector3(
                    actPosition.X * resizeFactorX,
                    actPosition.Y * resizeFactorY,
                    actPosition.Z * resizeFactorZ));

                Vector3 localLocation = new Vector3(
                    localLocationOriginal.X * resizeFactorX,
                    localLocationOriginal.Y * resizeFactorY,
                    localLocationOriginal.Z * resizeFactorZ);
                actStructure.UpdateVerticesUsingRelocationBy(targetCornerALocation + localLocation);
            }
        }
    }
}
