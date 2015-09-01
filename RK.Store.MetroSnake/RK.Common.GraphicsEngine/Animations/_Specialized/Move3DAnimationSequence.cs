using System;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Animations
{
    public class Move3DAnimationSequence : AnimationSequence
    {
        //Parameters
        private Vector3 m_moveVector;
        private TimeSpan m_duration;

        //Runtime values
        private Vector3 m_startPosition;
        private SceneSpacialObject m_targetObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Move3DAnimationSequence"/> class.
        /// </summary>
        /// <param name="moveVector">The move vector.</param>
        /// <param name="duration">The duration.</param>
        public Move3DAnimationSequence(Vector3 moveVector, TimeSpan duration)
            : base(AnimationType.FixedTime, duration)
        {
            m_moveVector = moveVector;
            m_duration = duration;
        }

        /// <summary>
        /// Checks the given target object (throws an exception on error).
        /// </summary>
        /// <param name="sceneObject">The object to check.</param>
        protected override void OnCheckTargetObject(SceneObject sceneObject)
        {
            base.OnCheckTargetObject(sceneObject);

            SceneSpacialObject spacialObject = sceneObject as SceneSpacialObject;
            if (spacialObject == null)
            {
                throw new GraphicsEngineException("Invalid object given for animation: It has to be a SceneSpacialObject!");
            }
        }

        /// <summary>
        /// Called when animation starts.
        /// </summary>
        protected override void OnStartAnimation()
        {
            m_targetObject = base.TargetObject as SceneSpacialObject;
            m_startPosition = m_targetObject.Position;
        }

        /// <summary>
        /// Called each time the CurrentTime value gets updated.
        /// </summary>
        protected override void OnCurrentTimeUpdated(UpdateState updateState, AnimationState animationState)
        {
            m_targetObject.Position =
                m_startPosition +
                m_moveVector * ((float)base.CurrentTime.Ticks / (float)base.FixedTime.Ticks);
        }

        /// <summary>
        /// Called when the FixedTime animation has finished.
        /// </summary>
        protected override void OnFixedTimeAnimationFinished()
        {
            m_targetObject.Position = m_startPosition + m_moveVector;

            m_targetObject = null;
            m_startPosition = Vector3.Empty;
        }
    }
}
