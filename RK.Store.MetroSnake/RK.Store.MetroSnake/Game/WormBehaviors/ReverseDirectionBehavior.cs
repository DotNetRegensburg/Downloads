using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RK.Common;
using RK.Common.GraphicsEngine.Animations;

namespace RK.Store.MetroSnake.Game.WormBehaviors
{
    public class ReverseDirectionBehavior : WormBehaviorBase
    {
        private bool m_isFinished;
        private GameViewModel m_gameViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseDirectionBehavior" /> class.
        /// </summary>
        /// <param name="wormElements">The worm elements.</param>
        public ReverseDirectionBehavior(GameViewModel viewModel)
        {
            m_gameViewModel = viewModel;
            m_isFinished = false;

            Vector3 previousHeadPosition = m_gameViewModel.Worm.First().Object3D.Position;
            Vector3 newHeadPosition = m_gameViewModel.Worm.Last().Object3D.Position;

            //Define complete animation for this behavior
            WormElement wormHead = m_gameViewModel.Worm.First();
            WormElement wormEnd = m_gameViewModel.Worm.Last();
            WormMoveDirection wormEndMoveDirection = wormEnd.LastExecutedMove;
            m_gameViewModel.Worm.First().Object3D.BeginAnimationSequence()
                .Scale3D(new Vector3(0.1f, 0.1f, 0.1f), TimeSpan.FromMilliseconds(500.0))
                .WaitFinished()
                .CallAction(() =>
                    {
                        //Change location of all worm elements
                        Vector3 previousElementLocation = wormHead.Object3D.Position;
                        Vector2 previousElementRotation = wormHead.Object3D.RotationHV;
                        for (int loop = 1; loop < m_gameViewModel.Worm.Count; loop++)
                        {
                            Vector3 currentLocationTemp = m_gameViewModel.Worm[loop].Object3D.Position;
                            Vector2 currentRotationTemp = m_gameViewModel.Worm[loop].Object3D.RotationHV;
                            m_gameViewModel.Worm[loop].Object3D.Position = previousElementLocation;
                            m_gameViewModel.Worm[loop].Object3D.RotationHV = previousElementRotation;
                            previousElementLocation = currentLocationTemp;
                            previousElementRotation = currentRotationTemp;
                            if (loop == m_gameViewModel.Worm.Count - 1)
                            {
                                m_gameViewModel.Worm.First().Object3D.Position = previousElementLocation;
                            }
                        }

                        //Reverse the order of the worm
                        m_gameViewModel.Worm.RemoveAt(0);
                        ReverseOrderOfElements(m_gameViewModel.Worm);
                        m_gameViewModel.Worm.Insert(0, wormHead);

                        //Reverses all move directions
                        wormHead.ReverseMoveDirections();
                        foreach (WormElement actElement in m_gameViewModel.Worm)
                        {
                            actElement.ReverseMoveDirections();
                            actElement.Object3D.RotationHV = WormElement.GetRotationForMoveDirection(actElement.LastExecutedMove);
                        }

                        //Apply new move direction for the continuing game
                        m_gameViewModel.LastWormMoveDirection = WormElement.ReverseMoveDirection(wormEndMoveDirection);
                        wormHead.Object3D.RotationHV = WormElement.GetRotationForMoveDirection(m_gameViewModel.LastWormMoveDirection);

                        wormHead.Object3D.Position = previousHeadPosition;
                    })
                .Move3D(newHeadPosition - previousHeadPosition, TimeSpan.FromMilliseconds(500.0))
                .WaitFinished()
                .Scale3D(new Vector3(10f, 10f, 10f), TimeSpan.FromMilliseconds(500.0))
                .Finish(() => m_isFinished = true);
        }

        /// <summary>
        /// Reverses the order of all elements within the given collection.
        /// </summary>
        /// <param name="wormElements">The collection to be reversed.</param>
        public static void ReverseOrderOfElements<ItemType>(ObservableCollection<ItemType> observableCollection)
            where ItemType : class
        {
            if (observableCollection.Count <= 1) { return; }

            ItemType[] itemArray = observableCollection.ToArray();
            for (int loop = 0; loop < itemArray.Length; loop++)
            {
                observableCollection[observableCollection.Count - (loop + 1)] = itemArray[loop];
            }
        }


        public override void Update()
        {

        }

        public override bool IsFinished
        {
            get { return m_isFinished; }
        }

        public override bool AllowsNormalGameLogic
        {
            get { return false; }
        }
    }
}
