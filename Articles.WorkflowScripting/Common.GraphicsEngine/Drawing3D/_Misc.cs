using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.GraphicsEngine.Drawing3D
{
    public delegate void Rendering3DHandler(object sender, Rendering3DArgs e);
    public delegate void Updating3DHandler(object sender, Updating3DArgs e);
    //public delegate void TextureChangedHandler(object sender, TextureChangedEventArgs e);

    ///// <summary>
    ///// EventArgs class for TextureChangedHandler delegate
    ///// </summary>
    //public class TextureChangedEventArgs : EventArgs
    //{
    //    private RenderState m_renderState;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="TextureChangedEventArgs"/> class.
    //    /// </summary>
    //    /// <param name="renderState">Current render state.</param>
    //    internal TextureChangedEventArgs(RenderState renderState)
    //    {
    //        m_renderState = renderState;
    //    }

    //    /// <summary>
    //    /// Gets current renderstate object.
    //    /// </summary>
    //    public RenderState RenderState
    //    {
    //        get { return m_renderState; }
    //    }
    //}

    public enum RotationType
    {
        /// <summary>
        /// Rotation using euler angles (pitch, yaw and roll).
        /// </summary>
        EulerAngles,

        /// <summary>
        /// Rotation using horizontal and vertical rotation values.
        /// </summary>
        HWAngles
    }

    /// <summary>
    /// EventArgs class for Rendering3DHandler.
    /// </summary>
    public class Rendering3DArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rendering3DArgs"/> class.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        public Rendering3DArgs(RenderState renderState)
        {
            this.RenderState = renderState;
        }

        /// <summary>
        /// Gets the render state.
        /// </summary>
        /// <value>Gets the render state.</value>
        public RenderState RenderState
        {
            get;
            private set;
        }
    }

        /// <summary>
    /// EventArgs class for Updating3DHandler.
    /// </summary>
    public class Updating3DArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Updating3DArgs"/> class.
        /// </summary>
        public Updating3DArgs(UpdateState updateState)
        {
            this.UpdateState = updateState;
        }
        
        /// <summary>
        /// Gets or sets the update state.
        /// </summary>
        public UpdateState UpdateState
        {
            get;
            private set;
        }
    }
}
