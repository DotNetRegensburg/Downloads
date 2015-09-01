using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace Articles.Workflows.CustomActivity.Activities
{
    [Designer(typeof(SleepActivityDesigner))]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(SampleSleepActivity))]
    public class SampleSleepActivity : CodeActivity
    {
        /// <summary>
        /// Gets or sets the time to sleep.
        /// </summary>
        [Category("Sleep")]
        [RequiredArgument]
        public InArgument<int> SleepTime { get; set; }

        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">Context of the activity.</param>
        protected override void Execute(CodeActivityContext context)
        {
            int sleepTime = context.GetValue(this.SleepTime);
            if (sleepTime > 0)
            {
                Console.WriteLine("Sleeping " + sleepTime + " seconds..");
                Thread.Sleep(sleepTime);
            }
        }
    }
}
