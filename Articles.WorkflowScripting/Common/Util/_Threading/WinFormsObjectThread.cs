using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Common.Util
{
    public class WinFormsObjectThread : ObjectThread
    {
        private Func<Form> m_createMainFormAction;
        private Control m_dummyControl;
        private Form m_mainForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsObjectThread"/> class.
        /// </summary>
        public WinFormsObjectThread(Func<Form> createMainFormAction, string threadName, bool isBackground)
            : base(threadName, isBackground)
        {
            m_createMainFormAction = createMainFormAction;

            //Set heartbeat of this thread
            base.HeartBeat = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsObjectThread"/> class.
        /// </summary>
        public WinFormsObjectThread(Func<Form> createMainFormAction)
            : this(createMainFormAction, "GUI-Thread", false)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsObjectThread"/> class.
        /// </summary>
        public WinFormsObjectThread()
            : this(null)
        {

        }

        /// <summary>
        /// Thread is starting.
        /// </summary>
        protected override void OnStarting(EventArgs eArgs)
        {
            //Create main form before calling starting event
            m_dummyControl = new Control();
            if (m_createMainFormAction != null)
            {
                m_mainForm = m_createMainFormAction();
                m_mainForm.FormClosed += OnMainFormClosed;
                m_mainForm.Show();
            }

            base.OnStarting(eArgs);
        }

        /// <summary>
        /// Called on each tick.
        /// </summary>
        protected override void OnTick(EventArgs eArgs)
        {
            //Waits for next message
            NativeMethods.MSG message = new NativeMethods.MSG();
            if (!NativeMethods.PeekMessage(ref message, NativeMethods.NullHandleRef, 0, 0, 0))
            {
                NativeMethods.WaitMessage();
            }

            //Perform all application events before calling tick event
            Application.DoEvents();

            //Raise tick event
            base.OnTick(eArgs);
        }

        /// <summary>
        /// Thread is stopping.
        /// </summary>
        protected override void OnStopping(EventArgs eArgs)
        {
            base.OnStopping(eArgs);

            m_dummyControl = null;
        }

        /// <summary>
        /// Called when the main form closes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosedEventArgs"/> instance containing the event data.</param>
        private void OnMainFormClosed(object sender, FormClosedEventArgs e)
        {
            m_mainForm.FormClosed -= OnMainFormClosed;

            this.Stop();
        }

        /// <summary>
        /// Gets the main form.
        /// </summary>
        public Form MainForm
        {
            get { return m_mainForm; }
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private static class NativeMethods
        {
            public static readonly HandleRef NullHandleRef = new HandleRef();

            /// <summary>
            /// Waits for next message.
            /// </summary>
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern void WaitMessage();

            /// <summary>
            /// Peeks next message.
            /// </summary>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool PeekMessage([In, Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

            /// <summary>
            /// Standard message structure.
            /// </summary>
            [Serializable, StructLayout(LayoutKind.Sequential)]
            public struct MSG
            {
                public IntPtr hwnd;
                public int message;
                public IntPtr wParam;
                public IntPtr lParam;
                public int time;
                public int pt_x;
                public int pt_y;
            }

            /// <summary>
            /// Standard HandleRef structure
            /// </summary>
            [StructLayout(LayoutKind.Sequential), ComVisible(true)]
            public struct HandleRef
            {
                internal object m_wrapper;
                internal IntPtr m_handle;
            }
        }
    }
}
