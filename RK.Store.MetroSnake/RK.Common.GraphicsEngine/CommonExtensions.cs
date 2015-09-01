using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.GraphicsEngine
{
    public static class CommonExtensions
    {
        public static SharpDX.Color4 ToDXColor(this Color4 color)
        {
            return new SharpDX.Color4(color.R, color.G, color.B, color.A); 
        }
    }
}
