using System;
using System.Collections;
using System.Collections.Generic;

namespace RK.Common.Util
{
    public class WriteProtectedCollection<T> : IEnumerable<T>
    {
        private IList<T> m_target;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteProtectedCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public WriteProtectedCollection(IList<T> target)
        {
            if (target == null) { throw new ArgumentNullException("target"); }
            m_target = target;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_target.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_target.GetEnumerator();
        }

        /// <summary>
        /// Gets the item at the given index.
        /// </summary>
        /// <param name="index">Index of the requested item.</param>
        public T this[int index]
        {
            get { return m_target[index]; }
        }

        /// <summary>
        /// Gets total count of items.
        /// </summary>
        public int Count
        {
            get { return m_target.Count; }
        }
    }
}
