using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Events;

namespace XmlExplorer.UnitTest
{
    internal class FakeEventAggregator : IEventAggregator
    {
        #region IEventAggregator Members

        public TEventType GetEvent<TEventType>() where TEventType : EventBase
        {
            return Activator.CreateInstance<TEventType>();
        }

        #endregion
    }
}
