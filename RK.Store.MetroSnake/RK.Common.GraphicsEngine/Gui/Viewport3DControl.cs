using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RK.Common.GraphicsEngine.Gui
{
    public class Viewport3DControl : SwapChainBackgroundPanel
    {
        private BackgroundPanelDirectXCanvas m_renderTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport3DControl" /> class.
        /// </summary>
        public Viewport3DControl()
        {
            m_renderTarget = new BackgroundPanelDirectXCanvas(this);
        }
    }
}
