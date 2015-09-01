using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;

//Some namespace mappings
using D2D = SlimDX.Direct2D;

namespace Common.GraphicsEngine.Drawing2D.Resources
{
    public class StrokeStyleResource
    {
        private D2D.StrokeStyle m_strokeStyle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strokeStyle"></param>
        internal StrokeStyleResource(D2D.StrokeStyle strokeStyle)
        {
            m_strokeStyle = strokeStyle;

            m_strokeStyle = new D2D.StrokeStyle(null, new D2D.StrokeStyleProperties()
            {

            });
        }
    }
}
