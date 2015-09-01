using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.Util
{
    public class DummyDisposable : IDisposable
    {
        private Action m_disposeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DummyDisposable" /> class.
        /// </summary>
        /// <param name="disposeAction">The dispose action.</param>
        public DummyDisposable(Action disposeAction)
        {
            m_disposeAction = disposeAction;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            m_disposeAction();
        }
    }
}
