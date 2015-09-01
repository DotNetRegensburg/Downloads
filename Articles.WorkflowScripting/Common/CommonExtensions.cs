using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Util;

namespace Common
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Performs the given action for each item within the given enumeration.
        /// </summary>
        /// <param name="enumeration">Enumeration from wich to get all items.</param>
        /// <param name="actionPerItem">Method to call for each item.</param>
        public static void PerformForEach<T>(this IEnumerable<T> enumeration, Action<T> actionPerItem)
        {
            if (enumeration == null) { throw new ArgumentException("enumeration"); }
            if (actionPerItem == null) { throw new ArgumentException("actionPerItem"); }

            foreach (T actItem in enumeration)
            {
                actionPerItem(actItem);
            }
        }

        /// <summary>
        /// Gets the given integer value as seconds in TimeSpan format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static TimeSpan Seconds(this int value)
        {
            return new TimeSpan(0, 0, value);
        }

        /// <summary>
        /// Gets the given integer value as minutes in TimeSpan format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static TimeSpan Minutes(this int value)
        {
            return new TimeSpan(0, value, 0);
        }

        /// <summary>
        /// Gets the given integer value as hours in TimeSpan format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static TimeSpan Hours(this int value)
        {
            return new TimeSpan(value, 0, 0);
        }

        /// <summary>
        /// Gets the given integer value as milliseconds in TimeSpan format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static TimeSpan Milliseconds(this int value)
        {
            return new TimeSpan(0, 0, 0, 0, value);
        }

        /// <summary>
        /// Gets the given integer value as days in TimeSpan format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static TimeSpan Days(this int value)
        {
            return new TimeSpan(value, 0, 0, 0);
        }

        /// <summary>
        /// Gets the timestamp the given time ago.
        /// </summary>
        public static DateTime Ago(this TimeSpan timeSpan)
        {
            return DateTime.Now.Subtract(timeSpan);
        }

        /// <summary>
        /// Is the given string null or empty?
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Is the given string null, empty or just whitespace?
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
