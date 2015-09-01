using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Store.MetroSnake.Game.WormBehaviors
{
    public abstract class WormBehaviorBase
    {

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Has the behavior finished finally?
        /// </summary>
        public abstract bool IsFinished { get; }

        public abstract bool AllowsNormalGameLogic { get; }
    }
}
