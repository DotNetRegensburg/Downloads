using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace RK.Store.MetroSnake.Util
{
    public class UIElementInputCatcher
    {
        private UIElement m_targetControl;

        private List<VirtualKey> m_keysDown;
        private bool m_isMouseInside;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElementInputCatcher"/> class.
        /// </summary>
        /// <param name="targetControl">The target control.</param>
        public UIElementInputCatcher(UIElement targetControl)
        {
            m_keysDown = new List<VirtualKey>();

            m_targetControl = targetControl;
            m_targetControl.KeyDown += OnTargetControlKeyDown;
            m_targetControl.KeyUp += OnTargetControlKeyUp;
        }

        /// <summary>
        /// Clears current input state.
        /// </summary>
        public void Clear()
        {
            m_keysDown.Clear();
        }

        /// <summary>
        /// Removes the given key.
        /// </summary>
        /// <param name="virtualKey"></param>
        public void RemoveKey(VirtualKey virtualKey)
        {
            m_keysDown.Remove(virtualKey);
        }

        /// <summary>
        /// Is the given key down?
        /// </summary>
        /// <param name="keyToCheck">The key to check.</param>
        public bool IsKeyDown(params VirtualKey[] keyToCheck)
        {
            if (keyToCheck.Length == 0) { return false; }
            foreach (VirtualKey actKey in keyToCheck)
            {
                if (m_keysDown.Contains(actKey)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Called when key is not down anymore.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlKeyUp(object sender, KeyRoutedEventArgs e)
        {
            //if ((e.KeyStatus.IsKeyReleased) && (m_keysDown.Contains(e.Key)))
            if (m_keysDown.Contains(e.Key))
            {
                m_keysDown.Remove(e.Key);
            }
        }

        /// <summary>
        /// Called the first time when a key is down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void OnTargetControlKeyDown(object sender, KeyRoutedEventArgs e)
        {
            //if ((e.KeyStatus.WasKeyDown) && (!m_keysDown.Contains(e.Key)))
            if (!m_keysDown.Contains(e.Key))
            {
                m_keysDown.Add(e.Key);
            }
        }

        /// <summary>
        /// Gets a collection containing all keys currently down.
        /// </summary>
        public IEnumerable<VirtualKey> KeysDown
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
