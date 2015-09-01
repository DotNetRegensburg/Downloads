using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using RK.Store.MetroSnake.Game.WormBehaviors;
using RK.Common.Mvvm;
using Windows.UI;
using Windows.UI.Xaml;

namespace RK.Store.MetroSnake.Game
{
    public class GameViewModel : ViewModelBase
    {
        private static readonly SelectorViewItem[] POSSIBLE_VIEW_ITEMS = new SelectorViewItem[]
        {
            new SelectorViewItem(){ StartColor = Colors.Gray, MiddleColor = Colors.LightSteelBlue, EndColor = Colors.LightSlateGray },
            new SelectorViewItem(){ StartColor = Colors.MediumVioletRed, MiddleColor = Colors.BlueViolet, EndColor = Colors.Violet },
            new SelectorViewItem(){ StartColor = Colors.Green, MiddleColor = Colors.ForestGreen, EndColor = Colors.GreenYellow },
            new SelectorViewItem(){ StartColor = Colors.Yellow, MiddleColor = Colors.YellowGreen, EndColor = Colors.LightGoldenrodYellow }
        };

        private int m_points;
        private Random m_randomizer;
        private DispatcherTimer m_timer;
        private WormBehaviorBase m_currentWormBehavior;
        private ObservableCollection<WormElement> m_worm;
        private WormMoveDirection m_lastWormMoveDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel" /> class.
        /// </summary>
        public GameViewModel()
        {
            m_randomizer = new Random(Environment.TickCount);

            m_lastWormMoveDirection = WormMoveDirection.Right;

            m_worm = new ObservableCollection<WormElement>();
        }

        /// <summary>
        /// Tries to turn the worm to the given direction.
        /// </summary>
        /// <param name="newMoveDirection">The new move direction to set.</param>
        public void TryTurnWormTo(WormMoveDirection newMoveDirection)
        {
            switch(newMoveDirection)
            {
                case WormMoveDirection.Down:
                    if (m_lastWormMoveDirection != WormMoveDirection.Up) { this.LastWormMoveDirection = newMoveDirection; }
                    break;

                case WormMoveDirection.Left:
                    if (m_lastWormMoveDirection != WormMoveDirection.Right) { this.LastWormMoveDirection = newMoveDirection; }
                    break;

                case WormMoveDirection.Right:
                    if (m_lastWormMoveDirection != WormMoveDirection.Left) { this.LastWormMoveDirection = newMoveDirection; }
                    break;

                case WormMoveDirection.Up:
                    if (m_lastWormMoveDirection != WormMoveDirection.Down) { this.LastWormMoveDirection = newMoveDirection; }
                    break;
            }
        }

        private SelectorViewItem CreateRandomViewItem()
        {
            return POSSIBLE_VIEW_ITEMS[m_randomizer.Next(0, POSSIBLE_VIEW_ITEMS.Length)];
        }

        /// <summary>
        /// Gets or sets the total score the game has currently.
        /// </summary>
        public int Points
        {
            get { return m_points; }
            set
            {
                if (m_points != value)
                {
                    m_points = value;
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged("PointTitleText");
                }
            }
        }

        ///// <summary>
        ///// Gets the title text for the score.
        ///// </summary>
        //public string PointTitleText
        //{
        //    get { return "Score: " + m_points; }
        //}

        //public string CounterText
        //{
        //    get { return "10"; }
        //}

        /// <summary>
        /// Called when 
        /// </summary>
        public WormBehaviorBase CurrentWormBehavior
        {
            get { return m_currentWormBehavior; }
            set
            {
                if (m_currentWormBehavior != value)
                {
                    m_currentWormBehavior = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the last move direction of the worm.
        /// </summary>
        public WormMoveDirection LastWormMoveDirection
        {
            get { return m_lastWormMoveDirection; }
            set
            {
                if (m_lastWormMoveDirection != value)
                {
                    m_lastWormMoveDirection = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a collection containing all items of the worm.
        /// </summary>
        public ObservableCollection<WormElement> Worm
        {
            get { return m_worm; }
        }
    }
}