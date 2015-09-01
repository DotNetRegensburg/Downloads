using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlExplorerModule
{
    public interface IExplorerViewPresentationModel
    {
        XmlNodeList Nodes { get; }
    }
}
