using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.GraphicsEngine.Core;
using Common.GraphicsEngine.Gui;
using Common.GraphicsEngine.Drawing2D.Resources;
using SlimDX;

//Some namespace mappings
using D2D = SlimDX.Direct2D;

namespace Common.GraphicsEngine.Drawing2D
{
    public class ResourceStorage2D
    {
        private static ResourceStorage2D s_current;

        private Dictionary<Color, BrushResource> m_solidBrushes;
        private Direct2DCanvas m_dummyCanvas;

        /// <summary>
        /// Creates a new ResourceStorage2D object.
        /// </summary>
        private ResourceStorage2D()
        {
            //Create resource dictionaries
            m_solidBrushes = new Dictionary<Color, BrushResource>();

            //Initialize dummy canvas for resource creation
            try
            {
                m_dummyCanvas = new Direct2DCanvas();
                m_dummyCanvas.Size = new Size(256, 256);

                m_dummyCanvas.CreateControl();
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a colid brush using the given color.
        /// </summary>
        public BrushResource GetSolidBrush(Color color)
        {
            if (m_solidBrushes.ContainsKey(color)) { return m_solidBrushes[color]; }
            else
            {
                BrushResource result = new BrushResource(color);
                m_solidBrushes[color] = result;
                return result;
            }
        }

        /// <summary>
        /// Gets current instance of the resource storage.
        /// </summary>
        public static ResourceStorage2D Current
        {
            get
            {
                if (s_current == null) { s_current = new ResourceStorage2D(); }
                return s_current;
            }
        }

        /// <summary>
        /// Gets the RenderTarget object that was created for resource creation.
        /// </summary>
        internal D2D.RenderTarget RenderTarget
        {
            get
            {
                if (m_dummyCanvas != null) { return m_dummyCanvas.RenderTarget; }
                else { return null; }
            }
        }
    
    }
}
