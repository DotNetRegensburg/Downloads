using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlExplorer.UnitTest
{
    internal class FakeFileProvider : IFileProvider
    {
        internal static string FileName
        {
            get { return "Test.xml"; }
        }
        
        #region IFileProvider Members

        public string Select(string name, string mask)
        {
            return FileName;
        }

        #endregion
    }
}
