using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Drawing2D;

namespace Common.GraphicsEngine.Gui
{
    /// <summary>
    /// Describes the type of the canvas.
    /// </summary>
    public enum CanvasType
    {
        /// <summary>
        /// Direct2D rendering is not enabled.
        /// </summary>
        None,

        /// <summary>
        /// Canvas is rendered using hardware.
        /// </summary>
        Hardware,

        /// <summary>
        /// Canvas is rendered using software.
        /// </summary>
        Software
    }

    /// <summary>
    /// Used for Direct2D painting events.
    /// </summary>
    public delegate void Direct2DPaintEventHandler(object sender, Direct2DPaintEventArgs e);

    /// <summary>
    /// Used for Direct2D painting events.
    /// </summary>
    public class Direct2DPaintEventArgs : EventArgs
    {
        private Graphics2D m_graphics;

        /// <summary>
        /// Creates a new Direct2DPaintEventArgs object.
        /// </summary>
        /// <param name="graphics">The Graphics2D object used for drawing.</param>
        internal Direct2DPaintEventArgs(Graphics2D graphics)
        {
            m_graphics = graphics;
        }

        /// <summary>
        /// Gets current graphics object.
        /// </summary>
        public Graphics2D Graphics
        {
            get { return m_graphics; }
        }
    }
}
