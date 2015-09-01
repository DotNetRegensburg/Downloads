using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Animations
{
    public class DelayAnimationSequence : AnimationSequence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelayAnimationSequence"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public DelayAnimationSequence(TimeSpan duration)
            : base(AnimationType.FixedTime, duration)
        {

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
