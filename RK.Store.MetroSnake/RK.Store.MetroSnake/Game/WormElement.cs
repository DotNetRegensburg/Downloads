using System;
using System.Linq;
using System.Collections.Generic;
using RK.Common;
using RK.Common.GraphicsEngine.Animations;
using RK.Common.GraphicsEngine.Objects;

namespace RK.Store.MetroSnake.Game
{
    public class WormElement
    {
        private static int s_wormIDCounter = 0;

        private Queue<WormMoveDirection> m_executedMoveDirections;
        private WormMoveDirection m_lastExecutedMoveDirection;
        private GenericObject m_object3D;
        private Queue<WormMoveDirection> m_outstandingMoveDirections;
        private int m_skipPasses;
        private int m_skipPassesInitial;
        private int m_wormID;
        private int m_animationSpeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WormElement" /> class.
        /// </summary>
        /// <param name="objecct3D">The objecct3 D.</param>
        public WormElement(GenericObject objecct3D, int animationSpeed)
        {
            m_wormID = s_wormIDCounter;
            s_wormIDCounter++;

            m_animationSpeed = animationSpeed;
            m_outstandingMoveDirections = new Queue<WormMoveDirection>();
            m_executedMoveDirections = new Queue<WormMoveDirection>();
            m_object3D = objecct3D;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WormElement" /> class.
        /// </summary>
        /// <param name="object3D">The 3D object to manipulate.</param>
        /// <param name="skipPasses">Total count of passes to skip before executing movement.</param>
        public WormElement(GenericObject object3D, int animationSpeed, int skipPasses)
            : this(object3D, animationSpeed)
        {
            m_skipPasses = skipPasses;
            m_skipPassesInitial = skipPasses;
        }

        /// <summary>
        /// Gets the unique id of this worm element.
        /// </summary>
        public string ElementID
        {
            get { return m_wormID.ToString(); }
        }

        public WormMoveDirection LastExecutedMove
        {
            get
            {
                return m_lastExecutedMoveDirection;
            }
        }

        public GenericObject Object3D
        {
            get { return m_object3D; }
        }

        /// <summary>
        /// Static helper for reversing a single move direction.
        /// </summary>
        /// <param name="direction">The direction to reverse.</param>
        public static WormMoveDirection ReverseMoveDirection(WormMoveDirection direction)
        {
            switch (direction)
            {
                case WormMoveDirection.Down:
                    return WormMoveDirection.Up;

                case WormMoveDirection.Left:
                    return WormMoveDirection.Right;

                case WormMoveDirection.Right:
                    return WormMoveDirection.Left;

                case WormMoveDirection.Up:
                    return WormMoveDirection.Down;
            }
            return WormMoveDirection.None;
        }

        public WormMoveDirection DequeueLastExecutedMove()
        {
            if (m_executedMoveDirections.Count == 0) { return WormMoveDirection.None; }
            return m_executedMoveDirections.Dequeue();
        }

        /// <summary>
        /// Executes one move. Given move direction may not be executed directly, because the difference
        /// beteen this and the previous element is hold by more steps within "outstanding move directions".
        /// </summary>
        /// <param name="moveDirection"></param>
        public void Move(WormMoveDirection moveDirection)
        {
            if (moveDirection == WormMoveDirection.None) { return; }

            m_outstandingMoveDirections.Enqueue(moveDirection);
            if (m_skipPasses <= 0)
            {
                //Get next move animation
                float rotation90Deg = (float)Math.PI / 2f;
                WormMoveDirection nextMoveDirection = m_outstandingMoveDirections.Dequeue();
                
                //Check whether to perform rotation animation
                int passesBeforeAnimation = m_skipPassesInitial / 2;
                WormMoveDirection moveDirectionForComparation = m_lastExecutedMoveDirection;
                WormMoveDirection nextMoveDirectionForAnimation = nextMoveDirection;
                if ((passesBeforeAnimation > 0) &&
                    (m_outstandingMoveDirections.Count >= passesBeforeAnimation))
                {
                    nextMoveDirectionForAnimation = m_outstandingMoveDirections.ElementAt(passesBeforeAnimation - 1);
                    if (passesBeforeAnimation > 1)
                    {
                        moveDirectionForComparation = m_outstandingMoveDirections.ElementAt(passesBeforeAnimation - 2);
                    }
                    else if (passesBeforeAnimation == 1) { moveDirectionForComparation = nextMoveDirection; }
                }
                bool directionChanged = moveDirectionForComparation != nextMoveDirectionForAnimation;

                //Execute next move direction
                switch (nextMoveDirection)
                {
                    case WormMoveDirection.Up:
                        m_object3D.Move(0f, 0.1f);
                        break;

                    case WormMoveDirection.Left:
                        m_object3D.Move(-0.1f, 0f);
                        break;

                    case WormMoveDirection.Down:
                        m_object3D.Move(0f, -0.1f);
                        break;

                    case WormMoveDirection.Right:
                        m_object3D.Move(0.1f, 0f);
                        break;
                }

                //Apply animation
                if (directionChanged)
                {
                    switch (nextMoveDirectionForAnimation)
                    {
                        case WormMoveDirection.Up:
                            m_object3D.BeginAnimationSequence()
                                .RotateHV(new Vector2(0f, 0f), TimeSpan.FromMilliseconds(m_animationSpeed))
                                .Finish();
                            break;

                        case WormMoveDirection.Left:
                            m_object3D.BeginAnimationSequence()
                                .RotateHV(new Vector2(-rotation90Deg, 0f), TimeSpan.FromMilliseconds(m_animationSpeed))
                                .Finish();
                            break;

                        case WormMoveDirection.Down:
                            m_object3D.BeginAnimationSequence()
                                .RotateHV(new Vector2(rotation90Deg * 2f, 0f), TimeSpan.FromMilliseconds(m_animationSpeed))
                                .Finish();
                            break;

                        case WormMoveDirection.Right:
                            m_object3D.BeginAnimationSequence()
                                .RotateHV(new Vector2(rotation90Deg, 0f), TimeSpan.FromMilliseconds(m_animationSpeed))
                                .Finish();
                            break;
                    }
                }

                m_executedMoveDirections.Enqueue(nextMoveDirection);
                m_lastExecutedMoveDirection = nextMoveDirection;
            }
            else
            {
                m_skipPasses--;
            }
        }

        /// <summary>
        /// Gets the hv-rotation for the given move direction.
        /// </summary>
        /// <param name="moveDirection">The move direction.</param>
        public static Vector2 GetRotationForMoveDirection(WormMoveDirection moveDirection)
        {
            float rotation90Deg = (float)Math.PI / 2f;
            switch(moveDirection)
            {
                case WormMoveDirection.Down:
                    return new Vector2(rotation90Deg * 2f, 0f);

                case WormMoveDirection.Left:
                    return new Vector2(-rotation90Deg, 0f);

                case WormMoveDirection.Right:
                    return new Vector2(rotation90Deg, 0f);

                case WormMoveDirection.Up:
                    return new Vector2(0f, 0f);
            }
            return Vector2.Empty;
        }

        /// <summary>
        /// Reverses all contained move directions.
        /// </summary>
        public void ReverseMoveDirections()
        {
            m_lastExecutedMoveDirection = ReverseMoveDirection(m_lastExecutedMoveDirection);
            m_executedMoveDirections = new Queue<WormMoveDirection>(m_executedMoveDirections
                .ConvertAllTo((actDirection) => ReverseMoveDirection(actDirection))
                .Reverse());
            m_outstandingMoveDirections = new Queue<WormMoveDirection>(m_outstandingMoveDirections
                .ConvertAllTo((actDirection) => ReverseMoveDirection(actDirection))
                .Reverse());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "ID = " + this.ElementID;
        }
    }
}