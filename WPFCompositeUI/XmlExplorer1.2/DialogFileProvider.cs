using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace XmlExplorer
{
    public class DialogFileProvider : IFileProvider
    {
        #region IFileProvider Members

        public string Select(string name, string mask)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = String.Format("{0}|{1}|All Files|*.*", name, mask)
            };

            string fileName = (dialog.ShowDialog() == true) ? dialog.FileName : null;
            return fileName;
        }

        #endregion
    }
}
