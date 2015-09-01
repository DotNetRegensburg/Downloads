using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlExplorerModule
{
    public interface INodeListProvider
    {
        XmlNodeList Select(string fileName);
    }
}
