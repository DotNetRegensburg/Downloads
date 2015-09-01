using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Drawing3D.Resources
{
    public interface IRenderableResource
    {
        /// <summary>
        /// Triggers internal update within the resource (e. g. Render to Texture).
        /// </summary>
        /// <param name="updateState">Current state of update process.</param>
        void Update(UpdateState updateState);

        /// <summary>
        /// Triggers internal rendering within the resource (e. g. Render to Texture).
        /// </summary>
        /// <param name="renderState">Current render state.</param>
        void Render(RenderState renderState);

        /// <summary>
        /// Is the resource loaded?
        /// </summary>
        bool IsLoaded { get; }
    }
}
