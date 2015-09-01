using System.Collections.Generic;

namespace RK.Common
{
    public struct AxisAlignedBox
    {
        public static readonly AxisAlignedBox Empty = new AxisAlignedBox(Vector3.Empty, Vector3.Empty);

        public Vector3 Location;
        public Vector3 Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisAlignedBox"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        public AxisAlignedBox(Vector3 location, Vector3 size)
        {
            this.Location = location;
            this.Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisAlignedBox"/> struct.
        /// </summary>
        /// <param name="containedLocations">The contained locations.</param>
        public AxisAlignedBox(IEnumerable<Vector3> containedLocations)
        {
            Vector3 minimum = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 maximum = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            bool anyInteration = false;
            foreach(Vector3 actContainedLocation in containedLocations)
            {
                anyInteration = true;

                if (minimum.X > actContainedLocation.X) { minimum.X = actContainedLocation.X; }
                if (minimum.Y > actContainedLocation.Y) { minimum.Y = actContainedLocation.Y; }
                if (minimum.Z > actContainedLocation.Z) { minimum.Z = actContainedLocation.Z; }

                if (maximum.X < actContainedLocation.X) { maximum.X = actContainedLocation.X; }
                if (maximum.Y < actContainedLocation.Y) { maximum.Y = actContainedLocation.Y; }
                if (maximum.Z < actContainedLocation.Z) { maximum.Z = actContainedLocation.Z; }
            }

            if (!anyInteration) { throw new CommonLibraryException("No vectors given!"); }

            this.Location = minimum;
            this.Size = maximum - minimum;
        }

        /// <summary>
        /// Does this box intersect the given one?
        /// </summary>
        /// <param name="otherBox">The other box to check.</param>
        public bool Intersects(AxisAlignedBox otherBox)
        {
            Vector3 ownMinimum = this.Minimum;
            Vector3 ownMaximum = this.Maximum;
            Vector3 otherMinimum = otherBox.Minimum;
            Vector3 otherMaximum = otherBox.Maximum;

            //Build corner arrays
            Vector3[] ownCorners = new Vector3[]
            {
                this.CornerA, this.CornerB, this.CornerC, this.CornerD,
                this.CornerE, this.CornerF, this.CornerG, this.CornerH,
            };
            Vector3[] otherCorners = new Vector3[]
            {
                otherBox.CornerA, otherBox.CornerB, otherBox.CornerC, otherBox.CornerD,
                otherBox.CornerE, otherBox.CornerF, otherBox.CornerG, otherBox.CornerH,
            };

            //Perform intersection test
            for (int loop = 0; loop < 8; loop++)
            {
                if ((otherCorners[loop] >= ownMinimum) && (otherCorners[loop] <= ownMaximum))
                {
                    return true;
                }
                else if ((ownCorners[loop] >= otherMinimum) && (ownCorners[loop] <= otherMaximum))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Expands this AxisAlignedBox so that it contains the given location.
        /// </summary>
        /// <param name="newLocation">New location to be merged to this AxisAlignedBox.</param>
        public void MergeWith(Vector3 newLocation)
        {
            //Handle x axis
            if (newLocation.X < Location.X)
            {
                Size.X = Size.X + (Location.X - newLocation.X);
                Location.X = newLocation.X;
            }
            else if (newLocation.X > Location.X + Size.X)
            {
                Size.X = Size.X + (newLocation.X - (Location.X + Size.X));
            }

            //Handle y axis
            if (newLocation.Y < Location.Y)
            {
                Size.Y = Size.Y + (Location.Y - newLocation.Y);
                Location.Y = newLocation.Y;
            }
            else if (newLocation.Y > Location.Y + Size.Y)
            {
                Size.Y = Size.Y + (newLocation.Y - (Location.Y + Size.Y));
            }

            //Handle z axis
            if (newLocation.Z < Location.Z)
            {
                Size.Z = Size.Z + (Location.Z - newLocation.Z);
                Location.Z = newLocation.Z;
            }
            else if (newLocation.Z > Location.Z + Size.Z)
            {
                Size.Z = Size.Z + (newLocation.Z - (Location.Z + Size.Z));
            }
        }

        /// <summary>
        /// Gets the corrdinate of middle of bottom rectangle.
        /// </summary>
        public Vector3 GetBottomMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Minimum.Y;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of bottom-left border.
        /// </summary>
        public Vector3 GetBottomLeftMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Minimum.Y;
            result.X = Minimum.X;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of bottom-right border.
        /// </summary>
        public Vector3 GetBottomRightMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Minimum.Y;
            result.X = Maximum.X;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of bottom-front border.
        /// </summary>
        public Vector3 GetBottomFrontMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Minimum.Y;
            result.Z = Minimum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of bottom-back border.
        /// </summary>
        public Vector3 GetBottomBackMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Minimum.Y;
            result.Z = Maximum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of the middle of the box.
        /// </summary>
        public Vector3 GetMiddleCoordinate()
        {
            return Minimum + (Maximum - Minimum) / 2f;
        }

        /// <summary>
        /// Gets the coordinate of middle of top rectangle.
        /// </summary>
        public Vector3 GetTopMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Maximum.Y;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of top-left border.
        /// </summary>
        public Vector3 GetTopLeftMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Maximum.Y;
            result.X = Minimum.X;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of top-right border.
        /// </summary>
        public Vector3 GetTopRightMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Maximum.Y;
            result.X = Maximum.X;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of top-front border.
        /// </summary>
        public Vector3 GetTopFrontMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Maximum.Y;
            result.Z = Minimum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of top-back border.
        /// </summary>
        public Vector3 GetTopBackMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Y = Maximum.Y;
            result.Z = Maximum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of front rectangle.
        /// </summary>
        public Vector3 GetFrontMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Z = Minimum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of back rectangle.
        /// </summary>
        public Vector3 GetBackMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.Z = Maximum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of left rectangle.
        /// </summary>
        public Vector3 GetLeftMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.X = Minimum.X;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of left-front border.
        /// </summary>
        public Vector3 GetLeftFrontMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.X = Minimum.X;
            result.Z = Minimum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of left-back border.
        /// </summary>
        public Vector3 GetLeftBackMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.X = Minimum.X;
            result.Z = Maximum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of right rectangle.
        /// </summary>
        public Vector3 GetRightMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.X = Maximum.X;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of right-front border.
        /// </summary>
        public Vector3 GetRightFrontMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.X = Maximum.X;
            result.Z = Minimum.Z;
            return result;
        }

        /// <summary>
        /// Gets the coordinate of middle of right-back border.
        /// </summary>
        public Vector3 GetRightBackMiddleCoordinate()
        {
            Vector3 result = Minimum + (Maximum - Minimum) / 2f;
            result.X = Maximum.X;
            result.Z = Maximum.Z;
            return result;
        }

        /// <summary>
        /// Builds a line list containing lines for all borders of this box.
        /// </summary>
        public List<Vector3> BuildLineListForBorders()
        {
            List<Vector3> result = new List<Vector3>();

            //Add front face
            result.Add(Location);
            result.Add(Location + new Vector3(Size.X, 0f, 0f));
            result.Add(Location + new Vector3(Size.X, 0f, 0f));
            result.Add(Location + new Vector3(Size.X, Size.Y, 0f));
            result.Add(Location + new Vector3(Size.X, Size.Y, 0f));
            result.Add(Location + new Vector3(0f, Size.Y, 0f));
            result.Add(Location + new Vector3(0f, Size.Y, 0f));
            result.Add(Location);

            //Add back face
            result.Add(Location + new Vector3(0f, 0f, Size.Z));
            result.Add(Location + new Vector3(Size.X, 0f, Size.Z));
            result.Add(Location + new Vector3(Size.X, 0f, Size.Z));
            result.Add(Location + new Vector3(Size.X, Size.Y, Size.Z));
            result.Add(Location + new Vector3(Size.X, Size.Y, Size.Z));
            result.Add(Location + new Vector3(0f, Size.Y, Size.Z));
            result.Add(Location + new Vector3(0f, Size.Y, Size.Z));
            result.Add(Location + new Vector3(0f, 0f, Size.Z));

            //Add connections
            result.Add(Location);
            result.Add(Location + new Vector3(0f, 0f, Size.Z));
            result.Add(Location + new Vector3(Size.X, 0f, 0f));
            result.Add(Location + new Vector3(Size.X, 0f, Size.Z));
            result.Add(Location + new Vector3(Size.X, Size.Y, 0f));
            result.Add(Location + new Vector3(Size.X, Size.Y, Size.Z));
            result.Add(Location + new Vector3(0f, Size.Y, 0f));
            result.Add(Location + new Vector3(0f, Size.Y, Size.Z));

            return result;
        }

        /// <summary>
        /// Gets the middle center of the box.
        /// </summary>
        public Vector3 GetMiddleCenter()
        {
            return new Vector3(
                Location.X + Size.X / 2f,
                Location.Y + Size.Y / 2f,
                Location.Z + Size.Z / 2f);
        }

        /// <summary>
        /// Gets the bottom center of the box.
        /// </summary>
        public Vector3 GetBottomCenter()
        {
            return new Vector3(
                Location.X + Size.X / 2f,
                Location.Y,
                Location.Z + Size.Z / 2f);
        }

        /// <summary>
        /// Translates the box
        /// </summary>
        public void Translate(Vector3 translateVector)
        {
            Location = Location + translateVector;
        }

        /// <summary>
        /// Transforms this AxisAlignedBox using the given transform matrix.
        /// </summary>
        /// <param name="transformMatrix">The matrix used to transform this box.</param>
        public void Transform(Matrix4 transformMatrix)
        {
            //Build array containing all corners
            Vector3[] allVectors = new Vector3[]
            {
                this.CornerA,
                this.CornerB,
                this.CornerC,
                this.CornerD,
                this.CornerE,
                this.CornerF,
                this.CornerG,
                this.CornerH
            };

            //Transfrom each corner
            for (int loop = 0; loop < 8; loop++)
            {
                allVectors[loop] = Vector3.Transform(allVectors[loop], transformMatrix);
            }

            //Build new AxisAlignedBox around generated corners
            this.Location = allVectors[0];
            this.Size = Vector3.Empty;
            for (int loop = 1; loop < 8; loop++)
            {
                this.MergeWith(allVectors[loop]);
            }
        }

        /// <summary>
        /// Inflates this box using the given value
        /// </summary>
        public void Inflate(float size)
        {
            float halfSize = size / 2f;

            //Update location
            Location.X = Location.X - halfSize;
            Location.Y = Location.Y - halfSize;
            Location.Z = Location.Z - halfSize;

            //Update size
            Size.X = Size.X + size;
            Size.Y = Size.Y + size;
            Size.Z = Size.Z + size;
        }

        /// <summary>
        /// Inflates this box using the given vector
        /// </summary>
        public void Inflate(Vector3 size)
        {
            Vector3 halfSize = new Vector3(size.X / 2f, size.Y / 2f, size.Z / 2f);

            //Update location
            Location.X = Location.X - halfSize.X;
            Location.Y = Location.Y - halfSize.Y;
            Location.Z = Location.Z - halfSize.Z;

            //Update size
            Size.X = Size.X + size.X;
            Size.Y = Size.Y + size.Y;
            Size.Z = Size.Z + size.Z;
        }

        /// <summary>
        /// Merges this box with the given one
        /// </summary>
        public void MergeWith(AxisAlignedBox other)
        {
            Vector3 minimum1 = this.Minimum;
            Vector3 minimum2 = other.Minimum;
            Vector3 maximum1 = this.Maximum;
            Vector3 maximum2 = other.Maximum;

            Vector3 newMinimum = Vector3.Minimize(minimum1, minimum2);
            Vector3 newMaximum = Vector3.Maximize(maximum1, maximum2);

            Location = newMinimum;
            Size = newMaximum - newMinimum;
        }

        /// <summary>
        /// Inflates the given box and returns the result
        /// </summary>
        public static AxisAlignedBox Inflate(AxisAlignedBox box, float size)
        {
            box.Inflate(size);
            return box;
        }

        /// <summary>
        /// Inflates the given box and returns the result
        /// </summary>
        public static AxisAlignedBox Inflate(AxisAlignedBox box, Vector3 size)
        {
            box.Inflate(size);
            return box;
        }

        /// <summary>
        /// Merges the given boxes.
        /// </summary>
        public static AxisAlignedBox Merge(AxisAlignedBox box1, AxisAlignedBox box2)
        {
            box1.MergeWith(box2);
            return box1;
        }

        /// <summary>
        /// Gets the minimum of the box
        /// </summary>
        public Vector3 Minimum
        {
            get { return this.Location; }
        }

        /// <summary>
        /// Gets the maximum of the box
        /// </summary>
        public Vector3 Maximum
        {
            get { return this.Location + this.Size; }
        }

        /// <summary>
        /// Gets the corner A (lower left front).
        /// </summary>
        public Vector3 CornerA
        {
            get { return Location; }
        }

        /// <summary>
        /// Gets the corner B (lower right front).
        /// </summary>
        public Vector3 CornerB
        {
            get { return new Vector3(Location.X + Size.X, Location.Y, Location.Z); }
        }

        /// <summary>
        /// Gets the corner C (lower right back).
        /// </summary>
        public Vector3 CornerC
        {
            get { return new Vector3(Location.X + Size.X, Location.Y, Location.Z + Size.Z); }
        }

        /// <summary>
        /// Gets the corner D (lower left back).
        /// </summary>
        public Vector3 CornerD
        {
            get { return new Vector3(Location.X, Location.Y, Location.Z + Size.Z); }
        }

        /// <summary>
        /// Gets the corner E (upper left front).
        /// </summary>
        public Vector3 CornerE
        {
            get { return new Vector3(Location.X, Location.Y + Size.Y, Location.Z); }
        }

        /// <summary>
        /// Gets the corner F (upper right front).
        /// </summary>
        public Vector3 CornerF
        {
            get { return new Vector3(Location.X + Size.X, Location.Y + Size.Y, Location.Z); }
        }

        /// <summary>
        /// Gets the corner G (upper right back).
        /// </summary>
        public Vector3 CornerG
        {
            get { return new Vector3(Location.X + Size.X, Location.Y + Size.Y, Location.Z + Size.Z); }
        }

        /// <summary>
        /// Gets the corner H (upper left back).
        /// </summary>
        public Vector3 CornerH
        {
            get { return new Vector3(Location.X, Location.Y + Size.Y, Location.Z + Size.Z); }
        }

        /// <summary>
        /// Is this structure empty?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Location.IsEmpty() && Size.IsEmpty();
            }
        }
    }
}
