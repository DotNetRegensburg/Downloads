using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Articles.Workflow.CustomWorkflowService
{
    public class OwnerWindowWrapper : IWin32Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerWindowWrapper"/> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        public OwnerWindowWrapper(IntPtr handle)
        {
            this.Handle = handle;
        }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr Handle
        {
            get;
            private set;
        }
    }
}
