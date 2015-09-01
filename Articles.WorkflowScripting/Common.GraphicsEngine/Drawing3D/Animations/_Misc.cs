using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Animations;

namespace Common.GraphicsEngine.Drawing3D.Animations
{
    public static class Drawing3DAnimationExtensions
    {
        /// <summary>
        /// Moves current object by the given move vector.
        /// </summary>
        /// <param name="sequenceBuilder">AnimationSequenceBuilder building the animation.</param>
        /// <param name="moveVector">The move vector.</param>
        /// <param name="animationTime">Total time for the animation.</param>
        public static AnimationSequenceBuilder Move3D(this AnimationSequenceBuilder sequenceBuilder, Vector3 moveVector, TimeSpan animationTime)
        {
            sequenceBuilder.Add(
                new Move3DAnimationSequence(moveVector, animationTime));
            return sequenceBuilder;
        }

        /// <summary>
        /// Scales current object by the given move vector.
        /// </summary>
        /// <param name="sequenceBuilder">AnimationSequenceBuilder building the animation.</param>
        /// <param name="moveVector">The scale vector.</param>
        /// <param name="animationTime">Total time for the animation.</param>
        public static AnimationSequenceBuilder Scale3D(this AnimationSequenceBuilder sequenceBuilder, Vector3 scaleVector, TimeSpan animationTime)
        {
            sequenceBuilder.Add(
                new Scale3DAnimationSequence(scaleVector, animationTime));
            return sequenceBuilder;
        }
    }
}
