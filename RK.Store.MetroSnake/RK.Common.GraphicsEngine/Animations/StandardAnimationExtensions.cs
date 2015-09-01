using System;

namespace RK.Common.GraphicsEngine.Animations
{
    public static class StandardAnimationExtensions
    {
        /// <summary>
        /// Waits some time before continueing with next animation sequence.
        /// </summary>
        /// <param name="builder">The AnimationSequenceBuilder object.</param>
        /// <param name="duration">Total duration to wait.</param>
        public static AnimationSequenceBuilder Delay(this AnimationSequenceBuilder builder, TimeSpan duration)
        {
            builder.Add(new DelayAnimationSequence(duration));
            return builder;
        }

        /// <summary>
        /// Waits until previous animation steps are finished.
        /// </summary>
        /// <param name="builder">The AnimationSequenceBuilder object.</param>
        public static AnimationSequenceBuilder WaitFinished(this AnimationSequenceBuilder builder)
        {
            builder.Add(new WaitFinishedAnimationSequence());
            return builder;
        }

        public static AnimationSequenceBuilder WaitUnitTimePassed(this AnimationSequenceBuilder builder, TimeSpan waittime)
        {
            builder.Add(new WaitTimePassedAnimationSequence(waittime));
            return builder;
        }

        /// <summary>
        /// Calls the given action.
        /// </summary>
        /// <param name="builder">The AnimationSequenceBuilder object.</param>
        /// <param name="actionToCall">The action to call on this step of the animation.</param>
        public static AnimationSequenceBuilder CallAction(this AnimationSequenceBuilder builder, Action actionToCall)
        {
            if (actionToCall == null) { throw new ArgumentNullException("actionToCall"); }

            builder.Add(new CallActionAnimationSequence(actionToCall));
            return builder;
        }

        /// <summary>
        /// Increases a float value by a given total increase value over the given duration.
        /// </summary>
        /// <param name="builder">The AnimationSequenceBuilder object.</param>
        /// <param name="valueGetter">The value getter.</param>
        /// <param name="valueSetter">The value setter.</param>
        /// <param name="totalIncrease">The value to increase in total.</param>
        /// <param name="duration">Total duration to wait.</param>
        /// <returns></returns>
        public static AnimationSequenceBuilder IncreaseFloat(this AnimationSequenceBuilder builder, Func<float> valueGetter, Action<float> valueSetter, float totalIncrease, TimeSpan duration)
        {
            builder.Add(new IncreaseFloatAnimationSequence(valueGetter, valueSetter, totalIncrease, duration));
            return builder;
        }
    }
}
