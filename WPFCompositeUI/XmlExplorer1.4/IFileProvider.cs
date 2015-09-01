using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlExplorer
{
    public interface IFileProvider
    {
        string Select(string name, string mask);
    }
}
