#if DESKTOP
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RK.Games.ForkliftRace.Util
{
    public class WpfInputCatcher
    {
        private UIElement m_targetControl;

        private List<Key> m_keysDown;
        private bool m_isMouseInside;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfInputCatcher"/> class.
        /// </summary>
        /// <param name="targetControl">The target control.</param>
        public WpfInputCatcher(UIElement targetControl)
        {
            m_keysDown = new List<Key>();

            m_targetControl = targetControl;
            m_targetControl.MouseEnter += OnTargetControlMouseEnter;
            m_targetControl.MouseLeave += OnTargetControlMouseLeave;
            m_targetControl.KeyDown += OnTargetControlKeyDown;
            m_targetControl.KeyUp += OnTargetControlKeyUp;
        }

        /// <summary>
        /// Is the given key down?
        /// </summary>
        /// <param name="keyToCheck">The key to check.</param>
        public bool IsKeyDown(Key keyToCheck)
        {
            return m_keysDown.Contains(keyToCheck);
        }

        /// <summary>
        /// Called when key is not down anymore.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.IsUp) && (m_keysDown.Contains(e.Key)))
            {
                m_keysDown.Remove(e.Key);
            }
        }

        /// <summary>
        /// Called the first time when a key is down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.IsDown) && (!m_keysDown.Contains(e.Key)))
            {
                m_keysDown.Add(e.Key);
            }
        }

        /// <summary>
        /// Called when the mouse leaves the target control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlMouseLeave(object sender, MouseEventArgs e)
        {
            m_isMouseInside = false;
        }

        /// <summary>
        /// Called when the mouse enters the target control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlMouseEnter(object sender, MouseEventArgs e)
        {
            m_isMouseInside = true;
        }

        /// <summary>
        /// Gets a collection containing all keys currently down.
        /// </summary>
        public IEnumerable<Key> KeysDown
        {
            get { return m_keysDown; }
        }

        /// <summary>
        /// Is the mouse currently inside the target control?
        /// </summary>
        public bool IsMouseInside
        {
            get { return m_isMouseInside; }
        }
    }
}
#endif