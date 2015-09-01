#region

using System;
using System.Windows.Forms;
using Microsoft.Office.Tools.Word;
using Action=Microsoft.Office.Tools.Word.Action;
using Office = Microsoft.Office.Core;

#endregion

namespace wdSmartTagBasic
{
    public partial class ThisDocument
    {
        private void ThisDocument_Startup(object sender, EventArgs e)
        {
            SmartTag st =
                new SmartTag("http://mySmartTag#DasisteinTest", "Hallo Smart Tag Welt");
            Action menue1 = new Action("Drück mich!");
            menue1.Click += menue1_Click;
            st.Actions = new[] {menue1};
            st.Terms.Add("BASTA");

            VstoSmartTags.Add(st);
        }

        private void menue1_Click(object sender, ActionEventArgs e)
        {
            MessageBox.Show("juhu");
        }


        private void ThisDocument_Shutdown(object sender, EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisDocument_Startup;
            Shutdown += ThisDocument_Shutdown;
        }

        #endregion
    }
}
    ;