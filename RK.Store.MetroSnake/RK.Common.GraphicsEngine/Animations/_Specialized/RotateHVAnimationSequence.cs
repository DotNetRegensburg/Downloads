using System;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Animations
{
    public class RotateHVAnimationSequence : AnimationSequence
    {
        private static readonly float ROTATION_90DEG = (float)Math.PI / 2f;
        private static readonly float ROTATION_180DEG = (float)Math.PI;

        //Parameters
        private Vector2 m_targetVector;
        private TimeSpan m_duration;

        //Runtime values
        private Vector2 m_startRotation;
        private SceneSpacialObject m_targetObject;

        public RotateHVAnimationSequence(Vector2 targetVector, TimeSpan duration)
            : base(AnimationType.FixedTime, duration)
        {
            m_targetVector = targetVector;
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
            m_startRotation = m_targetObject.RotationHV;
        }

        /// <summary>
        /// Called each time the CurrentTime value gets updated.
        /// </summary>
        protected override void OnCurrentTimeUpdated(UpdateState updateState, AnimationState animationState)
        {
            double percentagePassed =
                (double)base.CurrentTime.Ticks / (double)m_duration.Ticks;
            Vector2 differenceVector = m_targetVector - m_startRotation;

            if (differenceVector.X > ROTATION_180DEG) { differenceVector.X = -(differenceVector.X - ROTATION_180DEG); }
            if (differenceVector.Y > ROTATION_180DEG) { differenceVector.Y = -(differenceVector.Y - ROTATION_180DEG); }
            if (differenceVector.X < -ROTATION_180DEG) { differenceVector.X = -(differenceVector.X + ROTATION_180DEG); }
            if (differenceVector.Y < -ROTATION_180DEG) { differenceVector.Y = -(differenceVector.Y + ROTATION_180DEG); }

            differenceVector.X = (float)(differenceVector.X * percentagePassed);
            differenceVector.Y = (float)(differenceVector.Y * percentagePassed);

            m_targetObject.RotationHV = m_startRotation + differenceVector;
        }

        /// <summary>
        /// Called when the FixedTime animation has finished.
        /// </summary>
        protected override void OnFixedTimeAnimationFinished()
        {
            m_targetObject.RotationHV = m_targetVector;

            m_targetObject = null;
            m_startRotation = Vector2.Empty;
        }
    }
}
