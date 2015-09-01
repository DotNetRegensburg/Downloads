using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Core
{
    /// <summary>
    /// Enumeration containing supported target hardware
    /// </summary>
    public enum TargetHardware
    {
        /// <summary>
        /// Engine targets to DirectX 9 hardware.
        /// </summary>
        DirectX9,

        /// <summary>
        /// Engine targets to DirectX 11 hardware.
        /// </summary>
        DirectX11,

        /// <summary>
        /// Engine targets to a WARP Software renderer.
        /// </summary>
        SoftwareRenderer
    }
}
