using System;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public enum GradientDirection
    {
        LeftToRight,

        TopToBottom,

        Directional,
    }

    //public delegate void LoadTextureEventHandler(object sender, LoadTextureEventArgs e);
    public delegate void TextureChangedHandler(object sender, TextureChangedEventArgs e);

    ///// <summary>
    ///// EventArgs class for LoadTextureEventHandler delegate.
    ///// </summary>
    //public class LoadTextureEventArgs : EventArgs
    //{
    //    private Bitmap m_bitmap;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="LoadTextureEventArgs"/> class.
    //    /// </summary>
    //    internal LoadTextureEventArgs()
    //    {
    //        m_bitmap = null;
    //    }

    //    /// <summary>
    //    /// Gets or sets the source.
    //    /// </summary>
    //    /// <value>The source.</value>
    //    public Bitmap Source
    //    {
    //        get { return m_bitmap; }
    //        set { m_bitmap = value; }
    //    }
    //}

    /// <summary>
    /// EventArgs class for TextureChangedHandler delegate
    /// </summary>
    public class TextureChangedEventArgs : EventArgs
    {
        private RenderState m_renderState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureChangedEventArgs"/> class.
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        internal TextureChangedEventArgs(RenderState renderState)
        {
            m_renderState = renderState;
        }

        /// <summary>
        /// Gets current renderstate object.
        /// </summary>
        public RenderState RenderState
        {
            get { return m_renderState; }
        }
    }

    public enum MaterialApplyMode
    {
        Full,

        OnlyForNewObject
    }
}
