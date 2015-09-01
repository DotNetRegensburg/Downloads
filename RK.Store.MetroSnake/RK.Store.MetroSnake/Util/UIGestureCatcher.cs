using System;
using Windows.UI.Input;
using RK.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Foundation;

namespace RK.Store.MetroSnake.Util
{
    public class UIGestureCatcher
    {
        private const double RECOGNIZE_DISTANCE = 40.0;

        //Some very simple gesture events
        public event EventHandler MoveLeft;
        public event EventHandler MoveTop;
        public event EventHandler MoveRight;
        public event EventHandler MoveDown;

        private Point m_lastCapturedPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIGestureCatcher" /> class.
        /// </summary>
        public UIGestureCatcher(UIElement uiElement)
        {
            //Configure manipulation events an register on update event
            uiElement.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            uiElement.ManipulationStarted += OnUIElementManipulationStarted;
            uiElement.ManipulationDelta += OnUIElementManipulationDelta;
        }

        private void OnUIElementManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            m_lastCapturedPoint = new Point(0, 0);
        }

        private void OnUIElementManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            double translationX = args.Cumulative.Translation.X - m_lastCapturedPoint.X;
            double translationY = args.Cumulative.Translation.Y - m_lastCapturedPoint.Y;


            if ((Math.Abs(translationX) > RECOGNIZE_DISTANCE) ||
                (Math.Abs(translationY) > RECOGNIZE_DISTANCE))
            {
                //Handle the case when one coordinat is zero
                if (translationX == 0f)
                {
                    if (translationY < 0f) { MoveTop.Raise(this, EventArgs.Empty); }
                    else { MoveDown.Raise(this, EventArgs.Empty); }

                    m_lastCapturedPoint = args.Cumulative.Translation;
                    return;
                }
                else if (translationY == 0f)
                {
                    if (translationX < 0f) { MoveLeft.Raise(this, EventArgs.Empty); }
                    else { MoveRight.Raise(this, EventArgs.Empty); }

                    m_lastCapturedPoint = args.Cumulative.Translation;
                    return;
                }

                //Handling logic for standard case
                float xToY = (float)translationX / (float)translationY;
                float yToX = (float)translationY / (float)translationX;
                if (Math.Abs(xToY) < 0.4f)
                {
                    if (translationY < 0f) { MoveTop.Raise(this, EventArgs.Empty); }
                    else { MoveDown.Raise(this, EventArgs.Empty); }

                    m_lastCapturedPoint = args.Cumulative.Translation;
                    return;
                }
                else if (Math.Abs(yToX) < 0.4f)
                {
                    if (translationX < 0f) { MoveLeft.Raise(this, EventArgs.Empty); }
                    else { MoveRight.Raise(this, EventArgs.Empty); }

                    m_lastCapturedPoint = args.Cumulative.Translation;
                    return;
                }
            }
        }
    }
}