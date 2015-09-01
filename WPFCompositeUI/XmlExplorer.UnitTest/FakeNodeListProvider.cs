using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlExplorerModule;

namespace XmlExplorer.UnitTest
{
    internal class FakeNodeListProvider : INodeListProvider
    {
        internal static string Xml
        {
            get { return "<test />";  }
        }

        #region INodeListProvider Members

        public XmlNodeList Select(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Xml);
            return doc.SelectNodes("/");
        }

        #endregion
    }
}
