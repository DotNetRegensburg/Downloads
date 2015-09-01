using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlExplorerModule
{
    public class FileNodeListProvider : INodeListProvider
    {
        #region INodeListProvider Members

        public XmlNodeList Select(string fileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            return document.SelectNodes("/");
        }

        #endregion
    }
}
