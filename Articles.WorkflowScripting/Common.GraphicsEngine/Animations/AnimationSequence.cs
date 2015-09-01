using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.GraphicsEngine.Drawing3D;

namespace Common.GraphicsEngine.Animations
{
    public abstract class AnimationSequence
    {
        private AnimationType m_animationType;
        private SceneObject m_targetObject;

        //Control members for AnimationType FixedTime.
        private TimeSpan m_fixedTime;
        private TimeSpan m_currentTime;

        private bool m_finished;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSequence"/> class.
        /// </summary>
        public AnimationSequence()
        {
            m_fixedTime = TimeSpan.Zero;
            m_currentTime = TimeSpan.Zero;
            m_animationType = AnimationType.FinishedByEvent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSequence"/> class.
        /// </summary>
        /// <param name="animationType">Type of the animation.</param>
        public AnimationSequence(AnimationType animationType, TimeSpan fixedTime)
            : this()
        {
            m_animationType = animationType;
            m_fixedTime = fixedTime;
        }

        /// <summary>
        /// Check the given object for compatibility.
        /// </summary>
        /// <param name="targetObject">The object to check.</param>
        public void Check(SceneObject targetObject)
        {
            OnCheckTargetObject(targetObject);
            m_targetObject = targetObject;
        }

        /// <summary>
        /// Called for each update step.
        /// </summary>
        public void Update(UpdateState updateState)
        {
            switch (m_animationType)
            {
                    //Update logic for FixedTime animations
                case AnimationType.FixedTime:
                    if (m_fixedTime <= TimeSpan.Zero)
                    {
                        OnStartAnimation();
                        OnFixedTimeAnimationFinished();
                        m_finished = true;
                    }
                    if (m_currentTime == TimeSpan.Zero)
                    {
                        OnStartAnimation();
                    }
                    if (m_currentTime < m_fixedTime)
                    {
                        m_currentTime = m_currentTime.Add(updateState.UpdateTime);
                        if (m_currentTime >= m_fixedTime)
                        {
                            m_currentTime = m_fixedTime;

                            OnCurrentTimeUpdated();
                            OnFixedTimeAnimationFinished();
                            m_finished = true;
                        }
                        else
                        {
                            OnCurrentTimeUpdated();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Called when animation starts.
        /// </summary>
        protected virtual void OnStartAnimation()
        {

        }

        /// <summary>
        /// Called each time the CurrentTime value gets updated.
        /// </summary>
        protected virtual void OnCurrentTimeUpdated()
        {

        }

        /// <summary>
        /// Called when the FixedTime animation has finished.
        /// </summary>
        protected virtual void OnFixedTimeAnimationFinished()
        {

        }

        /// <summary>
        /// Checks the given target object (throws an exception on error).
        /// </summary>
        /// <param name="sceneObject">The object to check.</param>
        protected virtual void OnCheckTargetObject(SceneObject sceneObject)
        {

        }

        /// <summary>
        /// Gets the animation type.
        /// </summary>
        public AnimationType AnimationType
        {
            get { return m_animationType; }
        }

        /// <summary>
        /// Complete duration when in FixedTime mode.
        /// </summary>
        public TimeSpan FixedTime
        {
            get { return m_fixedTime; }
        }

        /// <summary>
        /// Current time in FixedTime mode.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return m_currentTime; }
        }

        /// <summary>
        /// Has this animation finished executing?
        /// </summary>
        public bool Finished
        {
            get { return m_finished; }
        }

        /// <summary>
        /// Is this animation a blocking animation?
        /// If true, all following animation have to wait for finish-event.
        /// </summary>
        public virtual bool IsBlocking
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the target object.
        /// </summary>
        public SceneObject TargetObject
        {
            get { return m_targetObject; }
        }
    }
}
