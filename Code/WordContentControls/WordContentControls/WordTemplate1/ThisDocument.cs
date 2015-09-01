using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Word;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Outlook;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace WordTemplate1
{
    public partial class ThisDocument
    {
        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
            SetData();

        }

        private void ThisDocument_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void SetData()
        {
            //ComboBox
            cbBetreff.DropDownListEntries.Add("Rechnung", "Rechnung", 1);
            cbBetreff.DropDownListEntries.Add("Kostenvoranschlag", "Kostenvoranschlag", 2);
            cbBetreff.DropDownListEntries.Add("Mahnung", "Mahnung", 3);
            cbBetreff.DropDownListEntries.Add("Zahlungserinnerung", "Zahlungerinnerung", 4);
            cbBetreff.DropDownListEntries.Add("Angebot", "Angebot", 5);
        }



        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisDocument_Startup);
            this.Shutdown += new System.EventHandler(ThisDocument_Shutdown);
        }

        #endregion
    }
}
