using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.GraphicsEngine.Animations
{
    public class WaitTimePassedAnimationSequence : AnimationSequence
    {
        private TimeSpan m_timeToWait;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitFinishedAnimationSequence" /> class.
        /// </summary>
        public WaitTimePassedAnimationSequence(TimeSpan timeToWait)
            : base(AnimationType.FixedTime, timeToWait)
        {
            m_timeToWait = timeToWait;
        }

        /// <summary>
        /// Is this animation a blocking animation?
        /// If true, all following animation have to wait for finish-event.
        /// </summary>
        public override bool IsBlocking
        {
            get { return true; }
        }
    }
}
