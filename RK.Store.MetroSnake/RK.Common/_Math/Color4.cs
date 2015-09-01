using System;
using System.Runtime.InteropServices;

namespace RK.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Color4
    {
        private const float TOLERANCE = 0.0001f;

        public static readonly Color4 Empty = new Color4();

        private float m_alpha;
        private float m_red;
        private float m_green;
        private float m_blue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        public Color4(float red, float green, float blue)
            : this(red, green, blue, 1f)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <param name="alpha">The alpha.</param>
        public Color4(float red, float green, float blue, float alpha)
        {
            m_alpha = alpha;
            m_red = red;
            m_green = green;
            m_blue = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        public Color4(int red, int green, int blue)
            : this(red, green, blue, 255)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <param name="alpha">The alpha.</param>
        public Color4(int red, int green, int blue, int alpha)
        {
            m_alpha = alpha / 255f;
            m_red = red / 255f;
            m_green = green / 255f;
            m_blue = blue / 255f;
        }

#if DESKTOP

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="gdiColor">Color of the GDI.</param>
        public Color4(System.Drawing.Color gdiColor)
            : this(gdiColor.R, gdiColor.G, gdiColor.B, gdiColor.A)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color4" /> struct.
        /// </summary>
        /// <param name="argb">The ARGB.</param>
        public Color4(int argb)
            : this(System.Drawing.Color.FromArgb(argb))
        {

        }

        /// <summary>
        /// Generates a gdi color object 
        /// </summary>
        public System.Drawing.Color ToGdiColor()
        {
            return System.Drawing.Color.FromArgb(
                (int)(255f * m_alpha),
                (int)(255f * m_red),
                (int)(255f * m_green),
                (int)(255f * m_blue));
        }

        /// <summary>
        /// Converts this value to a argb value
        /// </summary>
        public int ToArgb()
        {
            return this.ToGdiColor().ToArgb();
        }
#endif

        /// <summary>
        /// Changes the alpha value to the given one.
        /// </summary>
        /// <param name="newAlpha">The new alpha value to set.</param>
        public Color4 ChangeAlphaTo(float newAlpha)
        {
            this.A = newAlpha;
            return this;
        }

        /// <summary>
        /// Converts current value to R-G-B-A format.
        /// </summary>
        public int ToRgba()
        {
            byte redByte = (byte)((int)(255 * m_red) % 256);
            byte greenByte = (byte)((int)(255 * m_green) % 256);
            byte blueByte = (byte)((int)(255 * m_blue) % 256);
            byte alphaByte = (byte)((int)(255 * m_alpha) % 256);

            return BitConverter.ToInt32(new byte[] { redByte, greenByte, blueByte, alphaByte }, 0);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Color4)
            {
                Color4 other = (Color4)obj;
                return this == other;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// == operator
        /// </summary>
        public static bool operator ==(Color4 left, Color4 right)
        {
            return
                (left.m_red <= right.m_red + TOLERANCE) && (left.m_red >= right.m_red - TOLERANCE) &&
                (left.m_green <= right.m_green + TOLERANCE) && (left.m_green >= right.m_green - TOLERANCE) &&
                (left.m_blue <= right.m_blue + TOLERANCE) && (left.m_blue >= right.m_blue - TOLERANCE) &&
                (left.m_alpha <= right.m_alpha + TOLERANCE) && (left.m_alpha >= right.m_alpha - TOLERANCE);
        }

        /// <summary>
        /// == operator
        /// </summary>
        public static bool operator !=(Color4 left, Color4 right)
        {
            return
                (left.m_red > right.m_red + TOLERANCE) || (left.m_red < right.m_red - TOLERANCE) ||
                (left.m_green > right.m_green + TOLERANCE) || (left.m_green < right.m_green - TOLERANCE) ||
                (left.m_blue > right.m_blue + TOLERANCE) || (left.m_blue < right.m_blue - TOLERANCE) ||
                (left.m_alpha > right.m_alpha + TOLERANCE) || (left.m_alpha < right.m_alpha - TOLERANCE);
        }

        /// <summary>
        /// * operator (Vector3 * float)
        /// </summary>
        public static Color4 operator *(Color4 left, float right)
        {
            return new Color4(left.R * right, left.G * right, left.B * right);
        }

        /// <summary>
        /// Gets or sets the red color component.
        /// </summary>
        public float R
        {
            get { return m_red; }
            set { m_red = value; }
        }

        /// <summary>
        /// Gets or sets the green color component.
        /// </summary>
        public float G
        {
            get { return m_green; }
            set { m_green = value; }
        }

        /// <summary>
        /// Gets or sets the blue color component.
        /// </summary>
        public float B
        {
            get { return m_blue; }
            set { m_blue = value; }
        }

        /// <summary>
        /// Gets or sets the alpha color component.
        /// </summary>
        public float A
        {
            get { return m_alpha; }
            set { m_alpha = value; }
        }
    }
}
