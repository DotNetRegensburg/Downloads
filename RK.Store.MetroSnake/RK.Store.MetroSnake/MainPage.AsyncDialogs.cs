using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace RK.Store.MetroSnake
{
    public partial class MainPage
    {
        private IAsyncOperation<object> ShowModalDialog(Type dialogType)
        {
            return null;
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        public class AsyncModalDialogOperation : IAsyncOperation<object>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncModalDialogOperation" /> class.
            /// </summary>
            public AsyncModalDialogOperation()
            {

            }

            public object GetResults()
            {
                throw new NotImplementedException();
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }

            public void Close()
            {
                throw new NotImplementedException();
            }

            public Exception ErrorCode
            {
                get { throw new NotImplementedException(); }
            }

            public uint Id
            {
                get { throw new NotImplementedException(); }
            }

            public AsyncStatus Status
            {
                get { throw new NotImplementedException(); }
            }

            public AsyncOperationCompletedHandler<object> Completed
            {
                get;
                set;
            }
        }
    }
}
