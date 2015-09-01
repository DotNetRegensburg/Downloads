using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Util
{
    public class ThreadSaveQueue<T>
    {
        private Node m_first;
        private Node m_last;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSaveQueue&lt;T&gt;"/> class.
        /// </summary>
        public ThreadSaveQueue()
        {
            m_first = new Node();
            m_last = m_first;
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            Node newNode = new Node();
            newNode.item = item;
            Node old = null;
            do
            {
                old = m_first;
            }
            while (old != Interlocked.CompareExchange(ref m_first, newNode, old));
            old.next = newNode;
        }

        /// <summary>
        /// Dequeues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Dequeue(out T item)
        {
            Node current;
            do
            {
                current = m_last;
                if (current.next == null)
                {
                    item = default(T);
                    return false;
                }
            }
            while (current != Interlocked.CompareExchange(ref m_last, current.next, current));
            item = current.next.item;
            return true;
        }

        //*********************************************************************
        //*********************************************************************
        //*********************************************************************
        private class Node
        {
            public T item;
            public Node next;
        }
    }
}
