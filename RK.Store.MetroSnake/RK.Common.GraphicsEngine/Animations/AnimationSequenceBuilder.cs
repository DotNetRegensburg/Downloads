using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RK.Common.GraphicsEngine.Animations
{
    public class AnimationSequenceBuilder
    {
        private AnimationHandler m_owner;
        private List<AnimationSequence> m_sequenceList;
        private bool m_finished;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSequenceBuilder"/> class.
        /// </summary>
        private AnimationSequenceBuilder()
        {
            m_sequenceList = new List<AnimationSequence>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSequenceBuilder"/> class.
        /// </summary>
        /// <param name="owner">The owner object.</param>
        internal AnimationSequenceBuilder(AnimationHandler owner)
            : this()
        {
            m_owner = owner;
        }

        /// <summary>
        /// Adds an AnimationSequence to the builder.
        /// </summary>
        public void Add(AnimationSequence animationSequence)
        {
            if (m_finished) { throw new GraphicsEngineException("Unable to add a new AnimationSequence to a finished AnimationSequenceBuilder!"); }
            m_sequenceList.Add(animationSequence);
        }

        /// <summary>
        /// Finishes the AnimationSequence and adds it to the AninationHandler it was created with.
        /// </summary>
        public void Finish()
        {
            if (m_owner == null) { throw new GraphicsEngineException("Unable to finish AnimationSequenceBuilder: No default AnimationHandler found!"); }

            m_owner.BeginAnimation(m_sequenceList);
            m_finished = true;
        }

        /// <summary>
        /// Finishes the animation and starts from beginning.
        /// </summary>
        public void FinishAndRewind()
        {
            if (m_owner == null) { throw new GraphicsEngineException("Unable to finish AnimationSequenceBuilder: No default AnimationHandler found!"); }

            this.WaitFinished()
                .CallAction(() => m_owner.BeginAnimation(m_sequenceList));

            m_owner.BeginAnimation(m_sequenceList);
            m_finished = true;
        }

        /// <summary>
        /// Finishes the AnimationSequence and adds it to the AninationHandler it was created with.
        /// </summary>
        /// <param name="actionToCall">The action to be called after animation has finished.</param>
        public void Finish(Action actionToCall)
        {
            if (m_owner == null) { throw new GraphicsEngineException("Unable to finish AnimationSequenceBuilder: No default AnimationHandler found!"); }

            this.WaitFinished()
                .CallAction(actionToCall);

            m_owner.BeginAnimation(m_sequenceList);
            m_finished = true;
        }
    }
}
