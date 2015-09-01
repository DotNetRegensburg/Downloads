using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlExplorer.UnitTest
{
    internal class NullFileProvider : IFileProvider
    {
        #region IFileProvider Members

        public string Select(string name, string mask)
        {
            return null;
        }

        #endregion
    }
}
