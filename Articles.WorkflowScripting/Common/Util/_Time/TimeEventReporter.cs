using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Util
{
    public class TimeEventReporter
    {
        private TimeSpan m_currentTime;
        private TimeSpan m_eventTime;
        private bool m_autoReset;

        public event EventHandler Triggered;

        /// <summary>
        /// Creates a new TimeEventReporter object.
        /// </summary>
        /// <param name="eventTime">Time period before raising the event.</param>
        public TimeEventReporter(TimeSpan eventTime)
        {
            m_currentTime = TimeSpan.Zero;
            m_eventTime = eventTime;
            m_autoReset = false;
        }

        /// <summary>
        /// Creates a new TimeEventReporter object.
        /// </summary>
        /// <param name="eventTime">Time period before raising the event.</param>
        /// <param name="autoReset">Set to true for cyclic reporting.</param>
        public TimeEventReporter(TimeSpan eventTime, bool autoReset)
        {
            m_currentTime = TimeSpan.Zero;
            m_eventTime = eventTime;
            m_autoReset = autoReset;
        }

        /// <summary>
        /// Updates the event reporter object.
        /// </summary>
        public void Update(TimeSpan updateTime)
        {
            if (m_currentTime < m_eventTime)
            {
                m_currentTime = m_currentTime + updateTime;
                if (m_currentTime >= m_eventTime)
                {
                    RaiseTriggered();
                    if (m_autoReset) { m_currentTime = TimeSpan.Zero; }
                }
            }
        }

        /// <summary>
        /// Resets this object.
        /// </summary>
        public void Reset()
        {
            m_currentTime = TimeSpan.Zero;
        }

        /// <summary>
        /// Raises the Triggered event.
        /// </summary>
        private void RaiseTriggered()
        {
            if (Triggered != null) { Triggered(this, EventArgs.Empty); }
        }

        /// <summary>
        /// Time for one event.
        /// </summary>
        public TimeSpan EventTime
        {
            get { return m_eventTime; }
        }

        /// <summary>
        /// True when cyclic reporting is enablid.
        /// </summary>
        public bool AutoReset
        {
            get { return m_autoReset; }
        }
    }
}
