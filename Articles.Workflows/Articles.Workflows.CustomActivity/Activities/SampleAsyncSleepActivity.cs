using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Articles.Workflows.CustomActivity.Activities
{
    [Designer(typeof(SleepActivityDesigner))]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(SampleSleepActivity))]
    public class SampleAsyncSleepActivity : AsyncCodeActivity
    {
        /// <summary>
        /// Gets or sets the time to sleep.
        /// </summary>
        [Category("Sleep")]
        [RequiredArgument]
        public InArgument<int> SleepTime { get; set; }

        /// <summary>
        /// Starts executing the Activity.
        /// </summary>
        /// <param name="context">The context of execution.</param>
        /// <param name="callback">The async callback method.</param>
        /// <param name="state">A custom state object.</param>
        protected override IAsyncResult BeginExecute(
            AsyncCodeActivityContext context, 
            AsyncCallback callback, 
            object state)
        {
            int sleepTime = context.GetValue(this.SleepTime);

            IAsyncResult asyncResult = null;
            asyncResult = Task.Factory.StartNew((stateObject) =>
            {
                Console.WriteLine("Sleeping " + sleepTime + " seconds..");
                Thread.Sleep(sleepTime);
                callback(asyncResult);
            }, state);

            return asyncResult;
        }

        /// <summary>
        /// Ends executing this activity.
        /// </summary>
        protected override void EndExecute(
            AsyncCodeActivityContext context, 
            IAsyncResult result)
        {
            Task asyncTask = result as Task;
            if ((asyncTask != null) && (!asyncTask.IsCompleted))
            {
                asyncTask.Wait();
            }
        }
    }
}
