using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//Some namespace mappings
using GDI = System.Drawing;

namespace Common.GraphicsEngine.Gui
{
    public class EditorViewport2D
    {
        private double m_xPosCenter;
        private double m_yPosCenter;
        private double m_zoomFactor;

        private double m_screenWidth;
        private double m_screenHeight;

        private double m_screenWidthZoomed;
        private double m_screenHeightZoomed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewport2D"/> class.
        /// </summary>
        public EditorViewport2D()
        {
            m_xPosCenter = 0.0;
            m_yPosCenter = 0.0;
            m_zoomFactor = 1.0;

            m_screenWidth = 800.0;
            m_screenHeight = 600.0;
            m_screenWidthZoomed = 800.0;
            m_screenHeightZoomed = 600.0;
        }

        /// <summary>
        /// Gets the world coordinate from the given screen coordinate.
        /// </summary>
        public Point GetWorldFromScreenCoordinate(GDI.Point screenCoord)
        {
            Point result = new Point(
                (m_xPosCenter + (((double)screenCoord.X - (m_screenWidth / 2.0)) / m_zoomFactor)),
                (m_yPosCenter + (((double)screenCoord.Y - (m_screenHeight / 2.0)) / m_zoomFactor)));
            return result;
        }

        /// <summary>
        /// Gets the screen coordinate of the given world coordinate.
        /// </summary>
        public GDI.Point GetScreenFromWorldCoordinate(Point worldCoord)
        {
            GDI.Point result = new GDI.Point(
                (int)((m_xPosCenter - worldCoord.X) / m_zoomFactor),
                (int)((m_yPosCenter - worldCoord.Y) / m_zoomFactor));
            return result;
        }

        /// <summary>
        /// Moves the view by given values.
        /// </summary>
        public void Move(double moveX, double moveY)
        {
            UpdateCenter(
                m_xPosCenter + moveX,
                m_yPosCenter + moveY);
        }

        /// <summary>
        /// Updates current center coordinate.
        /// </summary>
        /// <param name="centerX">X Location of the center point.</param>
        /// <param name="centerY">Y Location of the center point.</param>
        public void UpdateCenter(double centerX, double centerY)
        {
            m_xPosCenter = centerX;
            m_yPosCenter = centerY;
        }

        /// <summary>
        /// Updates current screen properties.
        /// </summary>
        /// <param name="screenWidth">Current width of the screen.</param>
        /// <param name="screenHeight">Current height of the screen.</param>
        public void UpdateScreenSize(double screenWidth, double screenHeight)
        {
            m_screenWidth = screenWidth;
            m_screenHeight = screenHeight;
            m_screenWidthZoomed = screenWidth * m_zoomFactor;
            m_screenHeightZoomed = m_screenHeight * m_zoomFactor;
        }

        /// <summary>
        /// Updates current zoom factor.
        /// </summary>
        public void UpdateZoomFactor(double zoomFactor)
        {
            m_zoomFactor = zoomFactor;

            m_screenWidthZoomed = m_screenWidth * m_zoomFactor;
            m_screenHeightZoomed = m_screenHeight * m_zoomFactor;
        }

        /// <summary>
        /// Gets the center x coordinate.
        /// </summary>
        public double CenterX
        {
            get { return m_xPosCenter; }
        }

        /// <summary>
        /// Gets the center y coordinate.
        /// </summary>
        public double CenterY
        {
            get { return m_yPosCenter; }
        }

        /// <summary>
        /// Gets the width of the screen in pixels.
        /// </summary>
        public double ScreenWidth
        {
            get { return m_screenWidth; }
        }

        /// <summary>
        /// Gets the height of the screen in pixels.
        /// </summary>
        public double ScreenHeight
        {
            get { return m_screenHeight; }
        }

        /// <summary>
        /// Gets the zoomed with of the screen in pixels.
        /// </summary>
        public double ScreenWidthZoomed
        {
            get { return m_screenWidthZoomed; }
        }

        /// <summary>
        /// Gets the zoomed height of the screen in pixels.
        /// </summary>
        public double ScreenHeightZoomed
        {
            get { return m_screenHeightZoomed; }
        }

        /// <summary>
        /// Gets the zoom factor.
        /// </summary>
        public double ZoomFactor
        {
            get { return m_zoomFactor; }
        }

        /// <summary>
        /// Gets the coordinate of the top left pixel within zoomed rectangle.
        /// </summary>
        public double TopLeftXZoomed
        {
            get { return m_xPosCenter - (m_screenWidthZoomed / 2.0); }
        }

        /// <summary>
        /// Gets the coordinate of the top left pixel within zoomed rectangle.
        /// </summary>
        public double TopLeftYZoomed
        {
            get { return m_yPosCenter - (m_screenHeightZoomed / 2.0); }
        }

        /// <summary>
        /// Gets the coordinate of the top left pixel.
        /// </summary>
        public double TopLeftX
        {
            get { return m_xPosCenter - (m_screenWidth / 2.0); }
        }

        /// <summary>
        /// Gets the coordinate of the top left pixel.
        /// </summary>
        public double TopLeftY
        {
            get { return m_yPosCenter - (m_screenHeight / 2.0); }
        }
    }
}
