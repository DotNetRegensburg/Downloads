using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Core;

//Some namespace mappings
using GDI = System.Drawing;

namespace Common.GraphicsEngine.Drawing3D.Resources
{
    public class StringTextureResource : BitmapTextureResource
    {
        private Size m_textureSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTextureResource"/> class.
        /// </summary>
        public StringTextureResource(string name, string text, Font font)
            : this(name, text, font, Color.White, Color.Black, false)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTextureResource"/> class.
        /// </summary>
        public StringTextureResource(string name, string text, Font font, Color backColor, Color foreColor, bool drawBorder)
            : base(name, string.Empty)
        {
            if (font == null) { throw new ArgumentNullException("font"); }
            if (string.IsNullOrEmpty(text)) { throw new ArgumentException("No text given!", "text"); }

            //Measure given string
            SizeF textSize = SizeF.Empty;
            using (Bitmap dummyBitmap = new Bitmap(10, 10))
            {
                using (GDI.Graphics dummyGraphics = GDI.Graphics.FromImage(dummyBitmap))
                {
                    textSize = dummyGraphics.MeasureString(text, font);
                }
            }

            //Generate texture
            Size currentSize = new Size((int)textSize.Width, (int)textSize.Height);
            Size textureSize = GraphicsHelper.AdjustTextureSize(currentSize);
            Bitmap textureBitmap = new Bitmap(textureSize.Width, textureSize.Height);
            using (GDI.Graphics graphics = GDI.Graphics.FromImage(textureBitmap))
            {
                using (Brush backBrush = new SolidBrush(backColor))
                using (Brush foreBrush = new SolidBrush(foreColor))
                {
                    graphics.FillRectangle(backBrush, 0, 0, textureBitmap.Width, textureBitmap.Height);
                    graphics.DrawString(text, font, foreBrush, new PointF(0f, 0f));
                }
            }
            base.SetBitmap(textureBitmap);

            //Remember size of generated texture
            m_textureSize = textureSize;
        }

        /// <summary>
        /// Gets the size of the generated texture.
        /// </summary>
        public Size GeneratedTextureSize
        {
            get { return m_textureSize; }
        }
    }
}
