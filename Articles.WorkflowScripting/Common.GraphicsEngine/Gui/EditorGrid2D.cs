using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Drawing2D.Resources;
using Common.GraphicsEngine.Drawing2D;

namespace Common.GraphicsEngine.Gui
{
    public class EditorGrid2D
    {
        private BrushResource m_strokeResource;
        private double m_gridSpaceSize;

        /// <summary>
        /// Creates a new object that handles the edito'rs grid in 2D mode.
        /// </summary>
        public EditorGrid2D()
        {
            m_strokeResource = ResourceStorage2D.Current.GetSolidBrush(Color.DarkGray);
            m_gridSpaceSize = 50.0;
        }

        /// <summary>
        /// Renders the grid.
        /// </summary>
        /// <param name="graphics">The graphics object to render with.</param>
        /// <param name="viewProperties">Current view properties.</param>
        public void Render(Graphics2D graphics, EditorViewport2D viewProperties)
        {
            if (m_gridSpaceSize < 0.1) { return; }

            //Get screen dimensions
            double startPointX = viewProperties.TopLeftX;
            double startPointY = viewProperties.TopLeftY;
            double screenWidth = viewProperties.ScreenWidth;
            double screenHeight = viewProperties.ScreenHeight;

            //Calculate current screen size
            double gridSpaceZoomed = m_gridSpaceSize * viewProperties.ZoomFactor;

            ////Get total count of lines in x and y direction
            int linesX = (int)Math.Ceiling(screenWidth / gridSpaceZoomed) + 1;
            int linesY = (int)Math.Ceiling(screenHeight / gridSpaceZoomed) + 1;

            ////Get starting index points
            int startIndexX = (int)Math.Floor(viewProperties.CenterX / m_gridSpaceSize) - (linesX / 2);
            int startIndexY = (int)Math.Floor(viewProperties.CenterY / m_gridSpaceSize) - (linesY / 2);

            //Render all lines
            bool renderNormalLines = gridSpaceZoomed > 3.0;
            for (int actLineIndexX = startIndexX; actLineIndexX <= startIndexX + linesX; actLineIndexX++)
            {
                double actXPos = (screenWidth / 2.0) -(viewProperties.CenterX - (actLineIndexX * m_gridSpaceSize)) * viewProperties.ZoomFactor;
                float strokeSize = actLineIndexX != 0 ? 1f : 2f;

                if ((!renderNormalLines) && (actLineIndexX != 0)) { }
                else
                {
                    graphics.DrawLine(
                        m_strokeResource,
                        new PointF((float)actXPos, 0f),
                        new PointF((float)actXPos, (float)screenHeight),
                        strokeSize);
                }
            }
            for (int actLineIndexY = startIndexY; actLineIndexY <= startIndexY + linesY; actLineIndexY++)
            {
                double actYPos = (screenHeight / 2.0) - (viewProperties.CenterY - (actLineIndexY * m_gridSpaceSize)) * viewProperties.ZoomFactor;//actLineIndexY * gridSpaceZoomed + (screenHeight / 2.0) - viewProperties.CenterY;
                float strokeSize = actLineIndexY != 0 ? 1f : 2f;

                if ((!renderNormalLines) && (actLineIndexY != 0)) { }
                else
                {
                    graphics.DrawLine(
                        m_strokeResource,
                        new PointF(0f, (float)actYPos),
                        new PointF((float)screenWidth, (float)actYPos),
                        strokeSize);
                }
            }
        }

        /// <summary>
        /// Gets or sets the grid's space size.
        /// </summary>
        public double GridSpaceSize
        {
            get { return m_gridSpaceSize; }
            set { m_gridSpaceSize = value; }
        }
    }
}
