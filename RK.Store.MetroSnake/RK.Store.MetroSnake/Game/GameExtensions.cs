using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK.Store.MetroSnake.Game
{
    public static class GameExtensions
    {
        /// <summary>
        /// Moves the worm by the given move direction (called from front to back for each element).
        /// </summary>
        /// <param name="wormElements">The collection containing all worm elements.</param>
        /// <param name="moveDiection"></param>
        public static void MoveWorm(this IEnumerable<WormElement> wormElements, WormMoveDirection moveDiection)
        {
            WormMoveDirection currentDirection = moveDiection;
            foreach (WormElement actElement in wormElements)
            {
                actElement.Move(currentDirection);
                currentDirection = actElement.DequeueLastExecutedMove();
            }
        }
    }
}
