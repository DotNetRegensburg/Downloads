using System;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Animations
{
    public class Scale3DAnimationSequence : AnimationSequence
    {
        //Parameters
        private Vector3 m_scaleVector;
        private Vector3 m_scaleToPerform;
        private TimeSpan m_duration;

        //Runtime values
        private Vector3 m_startScaling;
        private SceneSpacialObject m_targetObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Move3DAnimationSequence"/> class.
        /// </summary>
        /// <param name="moveVector">The move vector.</param>
        /// <param name="duration">The duration.</param>
        public Scale3DAnimationSequence(Vector3 moveVector, TimeSpan duration)
            : base(AnimationType.FixedTime, duration)
        {
            m_scaleVector = moveVector;
            m_scaleToPerform = new Vector3(0, 0, 0);
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
            m_startScaling = m_targetObject.Scaling;
            m_scaleToPerform = new Vector3(
                m_scaleVector.X * m_startScaling.Y - m_startScaling.X,
                m_scaleVector.Y * m_startScaling.Y - m_startScaling.Y,
                m_scaleVector.Z * m_startScaling.Z - m_startScaling.Z);
        }

        /// <summary>
        /// Called each time the CurrentTime value gets updated.
        /// </summary>
        protected override void OnCurrentTimeUpdated(UpdateState updateState, AnimationState animationState)
        {
            float localScaleFactor = ((float)base.CurrentTime.Ticks / (float)base.FixedTime.Ticks);

            m_targetObject.Scaling = new Vector3(
                m_startScaling.X + m_scaleToPerform.X * localScaleFactor,
                m_startScaling.Y + m_scaleToPerform.Y * localScaleFactor,
                m_startScaling.Z + m_scaleToPerform.Z * localScaleFactor);
        }

        /// <summary>
        /// Called when the FixedTime animation has finished.
        /// </summary>
        protected override void OnFixedTimeAnimationFinished()
        {
            m_targetObject.Scaling = new Vector3(
                m_startScaling.X + m_scaleToPerform.X,
                m_startScaling.Y + m_scaleToPerform.Y,
                m_startScaling.Z + m_scaleToPerform.Z);

            m_targetObject = null;
            m_startScaling = Vector3.Empty;
        }
    }
}
