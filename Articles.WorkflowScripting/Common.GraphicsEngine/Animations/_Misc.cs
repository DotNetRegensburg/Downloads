using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.GraphicsEngine.Animations
{
    public static class StandardAnimationExtensions
    {
        /// <summary>
        /// Waits some time before continueing with next animation sequence.
        /// </summary>
        /// <param name="builder">The AnimationSequenceBuilder object..</param>
        /// <param name="duration">Total duration to wait..</param>
        public static AnimationSequenceBuilder Delay(this AnimationSequenceBuilder builder, TimeSpan duration)
        {
            builder.Add(new DelayAnimationSequence(duration));
            return builder;
        }
    }

    public enum AnimationType
    {
        FixedTime,

        FinishedByEvent
    }
}
