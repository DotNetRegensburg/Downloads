using System;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using System.Drawing;

namespace Articles.Workflows.CustomDelayActivity.Activities
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(SampleAsyncSleepActivity))]
    public class SampleAsyncSleepActivity : NativeActivity
    {
        private Variable<Bookmark> m_timerExpiredBookmark;

        /// <summary>
        /// Gets or sets the time to sleep.
        /// </summary>
        [Category("Sleep")]
        [RequiredArgument]
        public InArgument<int> SleepTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleAsyncSleepActivity"/> class.
        /// </summary>
        public SampleAsyncSleepActivity()
        {
            m_timerExpiredBookmark = new Variable<Bookmark>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            //Register local variable
            metadata.AddImplementationVariable(this.m_timerExpiredBookmark);

            //Register extension needed for the timer
            metadata.AddDefaultExtensionProvider(new Func<TimerExtension>(() => new DurableTimerExtension()));
        }

        /// <summary>
        /// Executes the activity.
        /// </summary>
        /// <param name="context">Context of execution.</param>
        protected override void Execute(NativeActivityContext context)
        {
            int sleepTime = context.GetValue(this.SleepTime);
            TimerExtension timerExtension = context.GetExtension<TimerExtension>();
            
            //Create the bookmark
            Bookmark bookmark = context.CreateBookmark("Test");
            context.SetValue(m_timerExpiredBookmark, bookmark);

            //Register sleep time on TimerExtension
            timerExtension.RegisterTimer(TimeSpan.FromMilliseconds(sleepTime), bookmark);
        }

        /// <summary>
        /// Called when activity should cancel.
        /// </summary>
        /// <param name="context">Context of execution.</param>
        protected override void Cancel(NativeActivityContext context)
        {
            base.Cancel(context);

            TimerExtension timerExtension = context.GetExtension<TimerExtension>();
        
            //Get the bookmark and cancel the timer
            Bookmark bookmark = this.m_timerExpiredBookmark.Get(context);
            if (bookmark != null)
            {
                timerExtension.CancelTimer(bookmark);
            }
        }

        /// <summary>
        /// Returns true when this Activity may create a Bookmark.
        /// </summary>
        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
    }
}
