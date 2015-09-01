using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Articles.Workflows.CustomExtension
{
    public interface IMainWindow
    {
        /// <summary>
        /// Adds an item to the ListBox.
        /// </summary>
        /// <param name="itemText">The text to add.</param>
        void AddItem(string itemText);
    }
}
