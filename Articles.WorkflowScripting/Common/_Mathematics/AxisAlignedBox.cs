using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public struct AxisAlignedBox
    {
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
            Size = newMinimum - newMinimum;
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
    }
}
