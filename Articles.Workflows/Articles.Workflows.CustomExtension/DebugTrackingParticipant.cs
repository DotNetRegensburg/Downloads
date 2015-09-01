using System;
using System.Activities.Tracking;
using System.Diagnostics;

namespace Articles.Workflows.CustomExtension
{
    public class DebugTrackingParticipant : TrackingParticipant
    {
        /// <summary>
        /// Tracks the given record.
        /// </summary>
        /// <param name="record">The track to be recorded.</param>
        /// <param name="timeout">Total time for tracking.</param>
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            Debug.WriteLine("[Tracking] " + record.ToString());
        }
    }
}
