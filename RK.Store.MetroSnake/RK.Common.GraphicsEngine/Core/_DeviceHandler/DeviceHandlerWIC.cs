using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Some namespace mappings
using WIC = SharpDX.WIC;

//Some type mappings
#if WINRT
using WicFactory = SharpDX.WIC.ImagingFactory2;
#endif
#if DESKTOP
using WicFactory = SharpDX.WIC.ImagingFactory;
#endif

namespace RK.Common.GraphicsEngine.Core
{
    public class DeviceHandlerWIC
    {
        private WicFactory m_imagingFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceHandlerWIC" /> class.
        /// </summary>
        public DeviceHandlerWIC()
        {
            m_imagingFactory = new WicFactory();
        }

        /// <summary>
        /// Gets the WIC factory object.
        /// </summary>
        public WicFactory Factory
        {
            get { return m_imagingFactory; }
        }
    }
}
