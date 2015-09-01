using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Animations
{
    public class AnimationHandler
    {
        private SceneObject m_targetObject;
        private List<AnimationSequence> m_runningAnimations;
        private ReadOnlyCollection<AnimationSequence> m_runningAnimationsPublic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationHandler"/> class.
        /// </summary>
        internal AnimationHandler(SceneObject targetObject)
        {
            m_targetObject = targetObject;

            m_runningAnimations = new List<AnimationSequence>();
            m_runningAnimationsPublic = new ReadOnlyCollection<AnimationSequence>(m_runningAnimations);
        }

        /// <summary>
        /// Begins a new animation sequence.
        /// </summary>
        public AnimationSequenceBuilder BeginAnimationSequence()
        {
            return new AnimationSequenceBuilder(this);
        }

        /// <summary>
        /// Begins the given AnimationSequence objects.
        /// </summary>
        public void BeginAnimation(IEnumerable<AnimationSequence> animationSequences)
        {
            //Check all given animations
            foreach (AnimationSequence actSequence in animationSequences)
            {
                actSequence.Check(m_targetObject);
                actSequence.Reset();
            }

            m_runningAnimations.AddRange(animationSequences);
        }

        /// <summary>
        /// Updates all animations.
        /// </summary>
        public void Update(UpdateState updateState)
        {
            AnimationState animationState = new AnimationState();
            for(int loop=0 ; loop<m_runningAnimations.Count ; loop++)
            {
                animationState.RunningAnimationsIndex = loop;

                //Get and update current animation
                AnimationSequence actAnimation = m_runningAnimations[loop];
                actAnimation.Update(updateState, animationState);

                //Check for finished state
                if (actAnimation.Finished)
                {
                    m_runningAnimations.RemoveAt(loop);
                    loop--;
                }
                //Cancel update for next animations if this one is blocking
                else if (actAnimation.IsBlocking) { break; }
            }
        }

        /// <summary>
        /// Gets the target object.
        /// </summary>
        public SceneObject TargetObject
        {
            get { return m_targetObject; }
        }

        /// <summary>
        /// Gets a collection containing all running animations.
        /// </summary>
        public ReadOnlyCollection<AnimationSequence> RunningAnimations
        {
            get { return m_runningAnimationsPublic; }
        }
    }
}
