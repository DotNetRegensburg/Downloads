using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common.GraphicsEngine.Drawing3D;

namespace RK.Common.GraphicsEngine.Animations
{
    public class CallActionAnimationSequence : AnimationSequence
    {
        private Action m_actionToCall;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallActionAnimationSequence" /> class.
        /// </summary>
        public CallActionAnimationSequence(Action actionToCall)
            : base()
        {
            m_actionToCall = actionToCall;
        }

        /// <summary>
        /// Called each time the CurrentTime value gets updated.
        /// </summary>
        /// <param name="updateState"></param>
        /// <param name="animationState"></param>
        protected override void OnCurrentTimeUpdated(UpdateState updateState, AnimationState animationState)
        {
            m_actionToCall();

            base.NotifyAnimationFinished();
        }
    }
}
